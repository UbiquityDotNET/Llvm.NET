---
uid: Ubiquity.NET.Llvm
remarks: *content
---
This is the root namespace of all of the library support, it contains the entirety of the wrapped
projection of LLVM for .NET consumers. There are several core items in this namespace as well as
distinct child namespaces for various purposes.

## Ownership and IDisposable
When dealing with native interop the concept of ownership is of critical importance. The underlying
resources are NOT controlled by a Garbage collector, and therefore require care to avoid access violations
and other app crash scenarios. This library aims to make that much easier by using IDisposable for these
scenarios. It is ***HIGHLY** recommended to use the [IDisposableAnalyzers](https://www.nuget.org/packages/IDisposableAnalyzers/)
in ANY project that consumes this library. (It was/is used internally to find and fix issues across the
library that were tedious to identify otherwise).

### Ownership transfer (move semantics)
Sometimes an API will transfer ownership to a containing type or native code in general. In C++ terminology
that is known as 'move semantics' and typically handled with `std::move()` but .NET and C# have no such
concept. To make life easier and keep usage of disposable types consistent, when a method follows the move
semantics it should be documented as such and, more importantly, it will set the value provided as invalid
BUT calling `Dispose()` is still a NOP. This keeps usage consistent even if ownership is transferred.
Attempting to use an instance after it is transferred will result in an `ObjectDisposedException`.

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

## Unowned references (alias)
For an unowned referenve to an underlying resource an interface is defined such as [IModule](xref:Ubiquity.NET.Llvm.IModule).
When a property returns an interface only it is not Disposable and ownership remains with the source.
Care is required on the part of a consumer to not store that instance anywhere and treat it as if it was a
`ref struct` (That is, only held on the stack). While the GC is free to clean up such an instance at any time
this prevents attempts to use the interface after the containing object is destroyed.

## Equality
In prior releases of this library a complex scheme of interning projection wrappers was used to support
reference equality. When you had an instance of class 'foo' you could just compare it to any other using reference
equality. For any two that refered to the same native instance they'd be the same object. While this had convenience
for the user it had a multitude of hidden flaws. The biggest is the concept of owership. [See discussion above]. If
objects are interned then you would end up with whatever instance was first created, ignoring the ownership completely.
If the first instance was an unowned alias, then it would leak as nothing owns it... If it was NOT an alias, then,
when retrieved from interning when an alias is needed to be the result, you could end up with premature disposal...
It was all confusing on whether you are supposed to call Dispose() or not. (Exact opposite of recommended best practice
for IDisposable).

Thus, this version of the library eliminates the confusion and complexity by use of objects that are disposable,
interfaces and a usage pattern that ensures Dispose() is idempotent and a NOP when already disposed. In the current
release no interning is performed, and instead wrapping types implement [`IEquatable<T>`](xref:System.IEquatable`1)
to allow value equality to compare the underlying native handle and resolve them as the same underlying instance or
not.