# Ubiquity.NET.Llvm
Ubiquity.NET.Llvm is a managed wrapper around an extended LLVM-C API providing an Object
Oriented (OO) model that closely matches the underlying LLVM internal object model. This
allows for building code generation, JIT and other utilities leveraging LLVM from .NET
applications.

## Guiding principles

  1. Mirror the underlying LLVM model as much as possible while providing a well behaved
    .NET projection including:
     1. Class names and hierarchies
     2. Object identity and equality
     3. [Fluent](https://en.wikipedia.org/wiki/Fluent_interface) APIs when plausible and
        appropriate.
  2. Hide low-level interop details and the raw LLVM-C API.
      - The native model for LLVM is a C++ class hierarchy and not the LLVM-C API used for
        most language/runtime bindings. `Ubiquity.NET.Llvm` is designed to provide an OO
        model that faithfully reflects the underlying LLVM model while fitting naturally
        into .NET programming patterns.
  3. Leverage existing LLVM-C APIs underneath whenever possible
     1. Extend only when needed with custom wrappers
  4. StyleCop/Code Analysis Clean

## Features
* LLVM Cross target code generation from .NET code
* JIT engine support for creating dynamic Domain Specific Language (DSL) runtimes with JIT
  support.
* Ahead of Time (AOT) compilation with support for Link time optimization and debug
  information.
* Object model that reflects the underlying LLVM classes

>[!Important]
> It is important to point out that the `Ubiquity.NET.Llvm` documentation is not a
> substitute for the official LLVM documentation itself. That is, the content here is
> focused on using `Ubiquity.NET.Llvm` and how it maps to the underlying LLVM. The LLVM
> documentation is, generally speaking, required reading to understand `Ubiquity.NET.Llvm`.
> The topics here often contain links to the official LLVM documentation to help in
> further understanding the functionality of the library.

## Breaking changes from prior versions
In Version 20.1.x a number of issues were resolved using newer .NET as well as in the LLVM
design itself that allows for a fundamentally new implementation. While there isn't a LOT of
code that consumers have to change (See the samples and compare against older versions)
there are important factors to consider in the new library:
1) Ownership
    - The previous variants of the library did NOT generally consider ownership carefully.
      It routinely provided types that under some circumstances require disposal, and others
      did not (Alias). This caused problems for the interning of projected types as the
      behavior of the first instance interned was used. (Usually leading to leaks or strange
      crashes at obscure unrelated times that made testing extremely difficult [Worst case
      scenario, it works fine in all in-house testing but breaks in the field!]).
3) No Interning of projected types
    - Projected types are no longer interned, this dramatically increases performance and
      reduces the complexity to maintain this library. Generally it should have little
      impact as anything that produces an alias where the type might in other cases require
      the owner to dispose it should now produce an interface that is not disposable.
      Anything the caller owns IS an `IDisposable`.
        - Move semantics are handled internally where the provided instance is invalidated
          but the Dispose remains a safe NOP. This helps prevent leaks or confusion when
          transfer is unable to complete due to an exception. The caller still owns the
          resource. Either way, `Dispose()` is called to clean it up, which is either a
          safe NOP, or an actual release of the native resource of transfer didn't complete.
2) Assumption of Reference Equality
    1) In the new library there is NO guarantee of reference equality for reference types.
        - Such types MAY be value equal if they refer to the same underlying native instance.
    2) Reference equality only considers the MANAGED wrapper instances and NOT the LLVM
       handles or the contents of the object they refer to.

### Ownership and IDisposable
When dealing with native interop the concept of ownership is of critical importance. The
underlying resources are NOT controlled by a Garbage Collector (GC), and therefore require
care to avoid access violations and other app crash scenarios. This library aims to make
that much easier by using IDisposable for these scenarios. It is ***HIGHLY*** recommended to
use the [IDisposableAnalyzers](https://www.nuget.org/packages/IDisposableAnalyzers/) in ANY
project that consumes this library. (It was/is used internally to find and fix issues across
the library that were tedious to identify otherwise). The down side of this is that there is
no standard pattern for move semantics (e.g., when there is a transfer of ownership
responsibility).

#### Ownership transfer (move semantics)
Sometimes an API will transfer ownership to a containing type or native code in general. In
C++ terminology that is known as 'move semantics' and typically handled with `std::move()`
but .NET and C# have no such concept. To make life easier and keep usage of disposable types
consistent, when a method follows the move semantics it should be documented as such.
Furthermore, and more importantly, it will set the value provided as invalid BUT calling
`Dispose()` is still a NOP. This keeps usage consistent even if ownership is transferred.
Attempting to use an instance after it is transferred will result in an
`ObjectDisposedException`.

Example from [OrcV2VeryLazy](xref:orcjitv2-very-lazy) sample application
``` C#
// ownership of this Materialization Unit (MU) is "moved" to the JITDyLib in the
// call to Define. Applying a "using" ensures it is released even if an exception
// occurs that prevents completion of the transfer. When transfer completes the
// MU is marked as disposed but a call to Dispose() is a safe NOP. Thus, this handles
// all conditions consistently
using var fooMu = new CustomMaterializationUnit("FooMU", Materialize, fooSym);
jit.MainLib.Define(fooMu);
```

### Unowned references (alias)
For an unowned reference to an underlying resource an interface is defined such as
[IModule](xref:Ubiquity.NET.Llvm.IModule). When a property returns an interface only it is
not Disposable and ownership remains with the source. Care is required on the part of a
consumer to not store that instance anywhere and treat it as if it was a `ref struct`
(That is, only held on the stack). While the GC is free to clean up such an instance at any
time this prevents attempts to use the interface after the containing object is destroyed.

### Equality
In prior releases of this library a complex scheme of interning projection wrappers was used
to support reference equality. When you had an instance of class 'foo' you could just
compare it to any other using reference equality. For any two that referred to the same
native instance they'd be the same object. While this had convenience for the user it had a
multitude of hidden flaws. The biggest is the concept of ownership. [See discussion above].
If objects are interned then you would end up with whatever instance was first created,
ignoring the ownership completely. If the first instance was an unowned alias, then it would
leak as nothing owns it... If it was NOT an alias, then, when retrieved from interning when
an alias is needed to be the result, you could end up with premature disposal...
It was all confusing on whether you are supposed to call Dispose() or not. (Exact opposite
of recommended best practice for IDisposable).

Thus, this version of the library eliminates the confusion and complexity by use of objects
that are disposable, interfaces and a usage pattern that ensures `Dispose()` is idempotent
and a NOP when already disposed or transferred. In the current release no interning is
performed, and instead wrapping types implement [`IEquatable<T>`](xref:System.IEquatable`1)
to allow value equality to compare the underlying native handle and resolve them as the same
underlying instance or not.
