---
title: LLVM-C Handle Wrappers
---

## LLVM-C Handle wrappers
"Handles" for LLVM are just opaque pointers. They generally come in one of three forms.

  1. Context owned  
     Where there is always a well known owner that ultimately is responsible for
     disposing/releasing the resource.
  2. Global resources  
     Where there is no parent child ownership relationship and callers must manually
     release the resource.
  3. An unowned alias to a resource  
     This occurs when a child of a resource contains a reference to the parent. In
     such a case the handle should be considered like an alias and not disposed.

The Handle implementations in `Ubiquity.NET.Llvm` follow consistent patterns for
implementing each form of handle. All handle types are generated from the native C++ headers
contained in the `Ubiquity.NET.LibLLVM` package. Ultimately, the handles are reduced to two
forms:
1) Requires caller to release them
    - Case 1 & 2 [previously discussed](#llvm-c-handle-wrappers).
    - Lifetime of the thing the handle refers to is controlled by the caller
    - Release is implemented by standard .NET pattern with [IDisposable](xref:System.IDisposable)
2) Does NOT require any dispose
    - Case 3 [previously discussed](#llvm-c-handle-wrappers).
    - Lifetime of the thing the handle refers to is controlled by the container

>[!NOTE]
> The generated sources are not useful outside of the `Ubiquity.NET.Llvm.Interop` as they
> use classes within that as a base class. These are generated manually via the
> `Generate-HandleWrappers.ps1` script with the sources checked in to the repository. This
> is done once for any updates to the `Ubiquity.NET.LibLLVM` package to ensure the handles
> are kept up to date with the underlying native library.

### Contextual handles and Aliases
These handles are never manually released or disposed, though releasing their containers
will make them invalid. The general pattern for implementing such handles is to use a
generated struct that is marked as implementing the `IContextHandle<THandle>` interface.
This interface is ONLY used during marshalling where the concrete type `THandle` is known
and therefore does NOT require any boxing. The struct is essentially a strongly typed alias
for an nint value. Contiguous sequences of these handles are re-interpret castable to a
sequence of nint. (The interop support uses this for efficient marshalling of arrays.)

### Global Handles
Global handles require the caller to explicitly release the resources. These types all
implement `IDisposable` even though they are value types. This ensures a consistency of the
destruction at the API level. It's ALWAYS done via a call to the `Dispose()` method. Since
these handles are value types they are immutable and the `Dispose()` method is not, and
cannot be idempotent. This is generally handled in wrapper classes that ARE mutable and
replace the wrapped handle with a default value on `Dispose()` or when "moved" (usually to
native code) IFF, the wrapper supports "move" semantics then the `Dispose()` call is
idempotent. Calling Dispose() may be a NOP. This ensures that applications need not worry
about move semantics and just call `Dispose()` [Usually implicitly via a `using` expression]
Thus, even if an exception occurred and the move didn't complete, the resource is properly
disposed of.

All resource handles in `Ubiquity.NET.Llvm,Interop` requiring explicit release are handled
consistently using the generated handle types as a distinct type implementing `IDisposable`

Global handles that also have an alias include a declaration of the alias type and allow
conversion to the unowned form of the handle.

### Global Alias handles
Global alias handles are a specialized form of global handles where they do not participate
in ownership control/release. These are commonly used when a child of a global container
exposes a property that references the parent container. In such cases the reference
retrieved from the child shouldn't be used to destroy the parent when no longer used.

In `Ubiquity.NET.Llvm.Interop` this is represented as an unowned context handle, that is
alias handles are the same as a context handle. There is no way to convert from an unowned
alias to an owned global handle (Though the other way around is allowed and supported
implicitly)

