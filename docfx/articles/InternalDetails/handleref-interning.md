# Interning LLVM handle to Managed wrappers
Many of the underlying object instances in LLVM are interned/uniqued. That is,
there will only be one instance of a type with a given value within some scope.

In LLVM the most common scope for uniqueing is the [Context](xref:Llvm.NET.Context) type.
In essence this class is an interning class factory for the LLVM IR system for a given
thread. Most object instances are ultimately owned by the context or a 
[BitcodeModule](xref:Llvm.NET.BitcodeModule). LLVM-C APIs use opaque pointers for LLVM
objects. This is projected to the low level Llvm.NET.Native namespace as structs that wrap
an IntPtr to enforce some type safety. Furthermore, the LLVM-C API uses the highest level
of an object inheritance graph that it can when declaring the opaque type for the return
or arguments of functions. Thus, it is not possible to know the exact derived type from
the base opaque pointer alone.

In addition to opaque pointers an additional challenge exists in mapping such pointers
to projected instances. Any projection is essentially a wrapper around the opaque
pointer. However, when an API returns an opaque pointer the interop layer needs to
determine what to do with it. A naive first approach (actually used in Llvm.NET early
 versions) is to simply create a new instance of the wrapper type giving it the
opaque pointer to work from. This will work for a while, until the code needs to compare
two instances. Ordinarily reference types are compared with reference equality. However,
if two projected instances are the same then reference equality will fail. While it is
plausible to add equality checks via IEquatable, they are not without problems, particularly
with respect to computing the hash code when the type is fully mutable. Furthermore, for
instances that may be enumerated multiple times (i.e. in a tree traversal or visitor pattern)
multiple instances of wrappers for the same underlying instance would be created, thus wasting
memory.

To solve these problems Llvm.NET uses an interning approach that mirrors the underlying LLVM
implementation by maintaining a mapping of the raw opaque pointers to a single managed instance. This
means that whenever an interop API retrieves an opaque pointer it can look up the wrapper and
provide that to the caller. Thus, reference equality "Just works". If there was no instance then
the interning system will create one. In order to create one it must know the concrete most
derived type for the opaque pointer to construct the wrapper type. The LLVM-C API generally defines
a reference handle type only for the top most base class rather than a distinct handle for each
derived type. Thus the mapping must know how to determine the correct derived type for the
projected wrapper. Fortunately, LLVM uses a custom type tagging mechanism to optimize such cases
internally (e.g. safe dynamic down casting by keeping a TypeKind value). While, the actual
implementation in LLVM is not as simplistic as that the details aren't relevant to the projection.
The projection wrapping can use the LLVM support to determine the correct type. Llvm.NET uses
this to manage the mapping and creation of types and consumers can remain blissfully ignorant
of these details.

## LLVM Internal implementation of interning
Internally Llvm.NET uses a common Generic type LLvm.NET.HandleInterningMap<THandle, TMappedType> to
handle the interning. This type accepts a factory function and optional disposer function to
abstract the desired projected type factory and support for disposing items where the lifetime
is not implicitly managed by some container. 
