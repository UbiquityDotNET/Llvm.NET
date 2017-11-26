# Native Interop

The native interop layer is the lowest layer of LLVM.NET and is generally focused on raw
P/Invoke declarations, enums, structures and handle types for interop with the LibLLVM library.
This is deliberately all marked as internal to keep developers from needing to deal with the
idiosyncracies of the API in the context of a managed runtime.

Originally this was all one "LLVM" static class with all P/Invoke declarations in a single file
generated from the LLVM headers. This was helpful to get over the tedious stage of mapping all
the P/Invoke calls. However, the generated signatures were of low quality. They were functional
for many cases but string marshalling was generally wrong for any strings provided by LLVM. (see
below for more information on string marshalling). Thus, they were updated manually and the
"generated" moniker was misleading at best. In addition many new/extended APIs were added to
extend the LLVM-C APIs for more full support in language projections. This, created two such
"generated" files with a plethora of P/Invoke delcarations, which makes maintanance more difficult.
Thus, the plan is to move the P/Invoke declarations to the classes that use them and eliminate
the large "generated" files. This, is an ongoing process but, when finished will help keep things
better organized and more flexible.

## Handles
LLVM-C Handles are essentially opaque pointers. Llvm.NET represents these as .NET Value types that
include equality checks and marshaling support. These are typically named LLVMxxxRef where xxx
is the name of the underlying type of the handle (i.e. LLVMModuleRef ). See the [handles readme](Handles\readme.md)
for more information. 

## String Marshalling
LLVM is written as portable standard C++ and therefore doesn't use string types like BSTR, or
SAFESTRING that the CLR has built-in support for. Occasionally strings returned are just raw
pointers to const string data, which would ordinarily require unsafe constructs to work with
in C# or other .NET languages. Furthermore, the LLVM libraries that work with strings don't
take platform line endings into account (it just uses \n always). To manage these variations
Llvm.NET uses custom marshalling for strings so that cleanup of allocations is done automatically
as is converion to proper line endings - all in a central place.

## Enums
The enums in the native layer match the underlying LLVM form in both value and naming. These,
deliberately don't conform to the .NET style rules used throughout the rest of the code. Thus,
they tend to suppress the normal style rules. Whenever enumerations are required in the public API
a new .NET style conformant enumeration is created.
