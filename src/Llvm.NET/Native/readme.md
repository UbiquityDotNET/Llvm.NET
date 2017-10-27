# Native Interop

The native interop layer is the lowest layer of LLVM.NET and is generally focused on raw
P/Invoke declarations, enums, structures and handle types for interop with the LibLLVM library.
This is deliberately all marked as internal to keep developers from needing to deal with the
idiosyncracies of the API in the context of a managed runtime.

## Handles
LLVM Handles are value types that wrap a raw pointer owned by the underlying native library and
include equality checks and marshaling support. These are typically named LLVMxxxRef where xxx
is the name of the underlying type the hadle (i.e. LLVMModuleRef )

## String Marshalling
LLVM is written as portable standard C++ and therefore doesn't use string types like BSTR, or
SAFESTRING that the CLR has built-in support for. Occasionally strings returned are just raw
pointers to const string data, which would ordinarily require unsafe constructs to work with
in C# or other .NET languages. Furthermore, the LLVM libraries that do produce strings don't
take platform line endings into account (it just uses \n always). To manage these variations
Llvm.NET uses custom marshalling for strings so that cleanup of allocations is done automatically
as is converion to proper line endings in a central place.

## Enums
The enums in the native layer match the underlying LLVM form in both value and naming. These,
deliberately don't conform to the .NET style rules used throughout the rest of the code. Thus,
they tend to suppress the normal style rules.
