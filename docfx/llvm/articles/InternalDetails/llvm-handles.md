---
title: LLVM-C Handle Wrappers
---

## LLVM-C Handle wrappers
?Handles? for LLVM are just opaque pointers. They generally come in one of three forms.

  1. Context owned  
     Where there is always a well known owner that ultimately is responsible for
     disposing/releasing the resource.
  2. Global resources  
     Where there is no parent child ownership relationship and callers must manually release
     the resource.
  3. An unowned alias to a global resource  
     This occurs when a child of a global resource contains a reference to the parent. In such
     a case the handle should be considered like an alias and not disposed.

The Handle implementations in Ubiquity.NET.Llvm follow consistent patterns for implementing
each form of handle. All handle types are generated from the native C++ headers. Thus they are
a source only NuGet package built along with the native extended C API library. The generated
sources are not useful outside of the `Ubiquity.NET.Llvm.Interop` as they use classes within
that as a base class. Ultimately, the handles are reduced to two forms:
1) Requires caller to release them
    - Lifetime of the thing the handle refers to is controlled by the caller
    - Release is implemented by standard .NET pattern with [IDisposable](xref:System.IDisposable)
2) Does NOT require any dispose
    - Lifetime of the thing the handle refers to is controlled by the container

>[!NOTE]
> The use of code generation for the handles in a different repo is a bit fragile as the
> generated handles are derived from and depend on support in a different consuming repository.
> This is a result of the historical split of the native code libraries. The build of that,
> takes a MUCH longer time AND requires distinct runners for each RID supported. While there is
> thinking about how to unify these repositories that isn't done yet as the focus is on getting
> the support for LLVM20.x and especially the JIT support. [It's been a long run with LLVM10 as
> the only option.]

### Contextual handles and Aliases
These handles are never manually released or disposed, though releasing their containers will
make them invalid. The general pattern for implementing such handles is to use a generated
struct that is marked as implementing the `IContextHandle<THandle>` interface. This interface
is ONLY used during marshalling where the concreted type `THandle` is known and therefore does
NOT require any boxing. The struct is essentially a strongly typed alias for an nint value.
Contiguous sequences of these handles are re-interpret castable to a sequence of nint. (The
interop support uses this for efficient marshalling of arrays.)

### Global Handles
Global handles require the caller to explicitly release the resources. In
Ubiquity.NET.Llvm.Interop these are managed with the .NET [SafeHandle](xref:System.Runtime.InteropServices.SafeHandle)
types through an LLVM interop specific derived type `GlobalHandleBase`. Since these types are
derived from a `SafeHandle` they are cleaned up with the standard .NET [IDisposable](xref:System.IDisposable).

All resource handles in `Ubiquity.NET.Llvm,Interop` requiring explicit release are handled consistently
using the generated handle types as a distinct type derived from `GlobalHandleBase`

Global handles that also have an alias include a declaration of the alias type and allow
conversion to the unowned form of the handle.

### Global Alias handles
Global alias handles are a specialized form of global handles where they do not participate in
ownership control/release. These are commonly used when a child of a global container exposes
a property that references the parent container. In such cases the reference retrieved from the
child shouldn't be used to destroy the parent when no longer used. 

In Ubiquity.NET.Llvm.Interop this is represented as an unowned context handle, that is alias
handles are the same as a context handle. There is no way to convert from an unowned alias to
an owned global handle (The other way around is allowed and supported)

