# General Design Notes for Ubiquity.NET.Llvm
## Guiding principles
1) Mirror the underlying LLVM model as much as possible while 
providing a well behaved .NET projection including:
   1) Class names and hierarchies
   1) Object identity and reference equality
   3) [Fluent](https://en.wikipedia.org/wiki/Fluent_interface) APIs when plausible and appropriate
1) Hide low-level interop details and the raw LLVM-C API.  
The native model for LLVM is a C++ class hierarchy and not the LLVM-C API.
Ubiquity.NET.Llvm is designed to provide an OO model that faithfully reflects the
underlying LLVM model while fitting naturally into .NET programming patterns.
   1) While the underlying extended "C" LLVM API is available in the Ubiquity.NET.Llvm.Interop package
      direct use of those APIs is generally discouraged as there are a lot of subtleties for correct
      usage that are automatically mnaged by the Ubiquity.NET.Llvm class library.
1) FxCop Clean
4) Leverage existing LLVM-C APIs underneath whenever possible
   1) Extend only when needed with custom wrappers

# Details
## Interning (Uniquing in LLVM)
Many of the underlying object instances in LLVM are interned/Uniqued. That is,
there will only be one instance of a type with a given value within some scope.

In LLVM the most common scope for uniqueing is the LLVMContext type. In essence
this class is an interning class factory for the LLVM IR system for a given thread.
Most object instances are ultimately owned by the context. LLVM-C APIs provide use
opaque pointers for LLVM objects. This is projected to the low level Ubiquity.NET.Llvm.Interop
namespace as structs that wrap an IntPtr to enforce some type safety. Furthermore,
the LLVM-C API uses the highest level of an object inheritance graph that it can when
declaring the opaque type for the return or arguments of functions. Thus, it is not
possible to know the exact derived type from the base opaque pointer alone.

In addition to opaque pointers an additional challenge exists in mapping such pointers
to projected instances. Any projection is essentially a wrapper around the opaque
pointer. However, when an API returns an opaque pointer the interop layer needs to
determine what to do with it. A naive first approach (actually used in Ubiquity.NET.Llvm early
 boot strapping) is to simply create a new instance of the wrapper type giving it the
opaque pointer to work from. This will work for a while, until the code needs to compare
two instances. Ordinarily reference types are compared with reference equality. However,
if two projected instances share the same opaque pointer then reference equality will fail.
Furthermore for instances that may be enumerated multiple times (i.e. in a tree
traversal or visitor pattern) multiple instances of wrappers for the same underlying
instance would be created, thus wasting memory. 

To solve these problems Ubiquity.NET.Llvm uses an interning approach that maintains a mapping of
the raw opaque pointers to a single instance. Thus whenever an interop API retrieves an
opaque pointer it can look up the wrapper and provide that to the caller. Thus, reference
equality "Just works". If there was no instance then the interning system will create one.
In order to create one it must know the concrete most derived type for the opaque pointer
to construct the wrapper type. Fortunately, LLVM uses a custom type tagging mechanism to
optimize such cases internally (e.g. safe dynamic down casting by keeping a TypeKind value).
The actual implementation in LLVM is not as simplistic as that but for the purposes of
projection that's enough to get to the correct type. Ubiquity.NET.Llvm uses this to manage the
mapping and creation of types.

To keep the interning factory from becoming over blown with type dependencies the actual
construction of new types is performed by the base type and not the factory. Instead the
intern factory for a type will call a delegate provided to create the item if needed.

