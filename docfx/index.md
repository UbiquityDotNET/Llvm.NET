![Logo](DragonSharp.png)

# Llvm.NET
Llvm.NET is a managed wrapper around an extended LLVM-C API including an Object Oriented model that closely matches 
the underlying LLVM internal object model. This allows for building code generation and other utilities
leveraging LLVM from .NET applications.

For more details see the full [API documentation](api/index.md)

## Guiding principles

  1. Mirror the underlying LLVM model as much as possible while 
  providing a well behaved .NET projection including:
     1. Class names and hierarchies
     2. Object identity and reference equality
     3. [Fluent](https://en.wikipedia.org/wiki/Fluent_interface) APIs when plausible and appropriate
  2. Hide low-level interop details and the raw LLVM-C API.  
  The native model for LLVM is a C++ class hierarchy and not the LLVM-C API used for most
  language/runtime bindings. Llvm.NET is designed to provide an OO model that faithfully reflects the
  underlying LLVM model while fitting naturally into .NET programming patterns.
  3. Leverage existing LLVM-C APIs underneath whenever possible
     1. Extend only when needed with custom wrappers
  4. FxCop/Code Analysis Clean
  
### Interning
Many of the underlying object instances in LLVM are interned/Uniqued. That is,
there will only be one instance of a type with a given value within some scope.

In LLVM the most common scope for uniqueing is the [Context](xref:Llvm.NET.Context) type.
In essence this class is an interning class factory for the LLVM IR system for a given
thread. Most object instances are ultimately owned by the context. LLVM-C APIs provide
use opaque pointers for LLVM objects. This is projected to the low level Llvm.NET.Native
namespace as structs that wrap an IntPtr to enforce some type safety. Furthermore,
the LLVM-C API uses the highest level of an object inheritance graph that it can when
declaring the opaque type for the return or arguments of functions. Thus, it is not
possible to know the exact derived type from the base opaque pointer alone.

In addition to opaque pointers an additional challenge exists in mapping such pointers
to projected instances. Any projection is essentially a wrapper around the opaque
pointer. However, when an API returns an opaque pointer the interop layer needs to
determine what to do with it. A naive first approach (actually used in Llvm.NET early
 boot strapping) is to simply create a new instance of the wrapper type giving it the
opaque pointer to work from. This will work for a while, until the code needs to compare
two instances. Ordinarily reference types are compared with reference equality. However,
if two projected instances are the same then reference equality will fail.
Furthermore, for instances that may be enumerated multiple times (i.e. in a tree
traversal or visitor pattern) multiple instances of wrappers for the same underlying
instance would be created, thus wasting memory.

To solve these problems Llvm.NET uses an interning approach that maintains a mapping of
the raw opaque pointers to a single instance. This means that whenever an interop API
retrieves an opaque pointer it can look up the wrapper and provide that to the caller.
Thus, reference equality "Just works". If there was no instance then the interning system
will create one. In order to create one it must know the concrete most derived type for
the opaque pointer to construct the wrapper type. Fortunately, LLVM uses a custom type
tagging mechanism to optimize such cases internally (e.g. safe dynamic down casting by
keeping a TypeKind value). The actual implementation in LLVM is not as simplistic as that
but for the purposes of projection that's enough to get to the correct type. Llvm.NET uses
this to manage the mapping and creation of types and consumers can remain blissfully ignorant
of these details.
