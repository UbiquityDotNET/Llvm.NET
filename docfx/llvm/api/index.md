# Ubiquity.NET.Llvm
Ubiquity.NET.Llvm is a managed wrapper around an extended LLVM-C API including an Object Oriented model that closely matches 
the underlying LLVM internal object model. This allows for building code generation, JIT and other utilities leveraging LLVM
from .NET applications.

## Guiding principles

  1. Mirror the underlying LLVM model as much as possible while 
  providing a well behaved .NET projection including:
     1. Class names and hierarchies
     2. Object identity and reference equality
     3. [Fluent](https://en.wikipedia.org/wiki/Fluent_interface) APIs when plausible and appropriate
  2. Hide low-level interop details and the raw LLVM-C API.  
  The native model for LLVM is a C++ class hierarchy and not the LLVM-C API used for most
  language/runtime bindings. Ubiquity.NET.Llvm is designed to provide an OO model that faithfully reflects the
  underlying LLVM model while fitting naturally into .NET programming patterns.
  3. Leverage existing LLVM-C APIs underneath whenever possible
     1. Extend only when needed with custom wrappers
  4. FxCop/Code Analysis Clean

## Features
* LLVM Cross target code generation from .NET code
* JIT engine support for creating dynamic domain specific language
  runtimes with JIT support.
* Ahead of time compilation with support for Link time optimization and debug information
* Object model that reflects the underlying LLVM classes

>[!Important]
> It is important to point out that the Ubiquity.NET.Llvm documentation is not a substitute
> for the official LLVM documentation itself. That is, the content here is focused on
> using Ubiquity.NET.Llvm and how it maps to the underlying LLVM. The LLVM documentation is,
> generally speaking, required reading to understand Ubiquity.NET.Llvm. The topics here often
> contain links to the official LLVM documentation to help in further understanding the
> functionality of the library.

## Breaking changes from prior versions
In Version 20.1.0 a number of issues were resolved using newer .NET as well as in the LLVM
design itself that allows for a fundamentally new implementation. While there isn't a LOT of
code that consumers have to change (See the samples and compare against older versions) there
are important factors to consider in the new library:
1) Ownership
    - The previous variants of the library did NOT generally consider ownership carefully. It
      routinely provided types that under some circumstances require disposal, and others did
      not (Alias). This caused problems for the internning of projected types as the behavior
      of the first instance interned was used. (Usually leading to leaks or strange crashes at
      pbscure unrelated times that made testing extremely difficult [Worst case scenario, it
      works fine in all in-house testing but breaks in the field!).
3) No Internning of projected types
    - Projected types are no longer internned, this dramatically increases performance and
      reduces the complexity of maintenance of this library. Generally it should have little
      impact as anything that produces an alias where the type might in other cases require
      the owner to dispose it should now produce an interface that is not disposable. Anything
      the caller owns IS an IDisposable.
        - Move semantics are handled internally where the provided instance is invalidated but
          the Dispose remains a safe NOP. This helps prevent leaks or confusion when transfer is
          unable to complete due to an exception. The caller still owns the resource. Either way,
          Dispose() is called to clean it up, which is either a safe NOP, or an actual release of
          the native resource.
2) Assumption of Reference Equality
    1) In the new library there is NO guarantee of reference equality for reference types.
        - Such types MAY be value equal if they refer to the same underlying native instance.
