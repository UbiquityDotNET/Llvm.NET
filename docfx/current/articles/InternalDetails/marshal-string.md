# Marshaling strings in Ubiquity.NET.Llvm
LLVM provides strings in several forms and this leads to complexities for
P/Invoke signatures as sometimes the strings require some form of release
and in other cases, they do not. Standard .NET marshaling of strings makes
some assumptions with regard to strings as a return type that make the LLVM
APIs difficult. (e.g. in some LLVM APIs the returned string must be released
via LLVMDisposeMessage() or some other call, while in other cases it is just
a pointer to an internal const string that does not need any release.)

To resolve these issues and make the requirements explicitly clear and consistent
Ubiquity.NET.Llvm.Interop uses custom marshaling of the strings to mark the exact behavior directly
on the P/Invoke signature so it is both clear and easy to use for the upper layers
(it's just a `System.String`)

## Generated String Marshalers
The [LlvmBindingsGenerator](https://github.com/UbiquityDotNET/Llvm.NET/tree/master/src/Interop/LlvmBindingsGenerator)
Creates concrete custom marshalers for every string disposal type supported. To
keep things simple and eliminate redundancies, the generated marshalers all derive from
a common base type CustomStringMarshalerBase.

### Marshaling configuration
LLVMBindingsGenerator supports a flexible configuration to identify which functions require which
form of marshaling. For strings this is an instance of the `StringMarshalInfo`
class
