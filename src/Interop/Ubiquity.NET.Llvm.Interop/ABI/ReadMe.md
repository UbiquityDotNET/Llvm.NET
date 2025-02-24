# ABI Interop code
This source was originally generated from the LLVM and LibLLVM extended headers and then
edited by hand to follow new "modern" patterns of interop support. It is now maintained
directly. The source generation was too problematic/complex to generalize to the point
where the output was usable "as-is". Additionally, it used marshalling hints via a custom
YAML configuration file. Ultimately, this file ended up as a foreign language form of the
marshalling attributes in C# code. So it was mostly abandoned. (It is still used to generate
the exports and perform some validations of the extension code at the native level and
to generate the otherwise tedious handle types.)

The generator had one advantage in that it could read the configuration file AND
validate that the functions listed in it were still in the actual headers (though
it failed to identify any additional elements in the headers NOT in the config file!).
There may yet be some way to salvage some parts of the generator to perform a sanity
check and report any missing or mismatched information. Or possibly automatiacly generate
the proper code again. (Though that seems unlikely as the type of string is a major
problematatic factor.)

## ABI Function Pointers
ABI function pointers are represented as real .NET function pointers with an unmanaged
signature.

### special consideration for handles
LLVM Context handles are  just value types that wrap around a runtime nint (basically a
strong typedef for a pointer). Therefore, they are blittable value types and don't need
any significant marshaling support. Global handles, however, are reference types derived
from `SafeHandle` as they need special release semantics. All LLVM handle managed
projections **CANNOT** appear in the signature of an unmanaged function pointer as there
is way to mark the marshalling behavior for unmanaged pointers. Implementations of the
"callbacks" MUST handle marshalling of the ABI types manually. Normally, they will
leverage a `GCHandle` as the "context", perform marshalling, and forward the results on
to the managed context object. But implemenations are free to deal with things as they
prefer.

# General patterns for maintainance of P/Invoke signatures
The general pattern is that the interop APIs now ALL use the built-in interop source
generator via the `LibraryImportAttribute` this generates all the marshalling code at
compile time so the results is AOT compatible. This is leveraged by the types generated
for each LLVM handle type. Specifically, the handle types have a proper marshalling type
identified that is compatible with the needs of `LibraryImportAttribute`. For the global
handles that is the built-in support for `SafeHandle` derived type handling. For the
context handles that is coverd by the `ContextHandleMarshaller<T>` custom marshalling type.
The generated types for all context handles marks the type marshalling with the
`NativeMarshallingAttribute` to ensure it is AOT marshalling ready.

## Explicit types
All of the P/Invoke API signatures are created with Explicitly sized values for numerics
***and enumerations***. This ensures there is no confusion on the bit length of types etc...
(For example, [U]Int[64|32|16] is ALWAYS used over any built-in wrappers to avoid any
potential confusion about the size of types between C# and native C/C++)

### Distinction for real LLVMBOOL vs Status
This library and P/Invoke signatures disambiguates between an actual boolean value
(`LLVMBool`) and what is really a success or failure status code. As with strings, the
only way to tell the difference is to read the docs... Thus, for ALL API signatures an
LLVMStatus is used for any native code signature `LLVMBool` that is documented as behaving
like a status and NOT a bool. This prevents mass confusion on the intent and helps keep
the API surface cleaner and more self documenting. (Does a non-zero `LLVMBool` mean it
succeeded? Or does it mean there was an error?) Thus all APIs need a developer to understand
the documentation and set the P/Invoke signature to use `LLVMStatus` for anything that is
really a status code and ***NOT*** a boolean value.

## Enumerated value naming
The normal rules of naming enumerated values are suspened for this low level interop API.
The idea is to keep ALL enumeration names and values as close as possible (if not identical)
to the underlying native code. This helps in supporting the idea that the documentation
for the native code is relevant and easier to consume so that the documentation for this
library focuses ONLY on the interop support provided within it.

## Calling convention
All of the P/Invoke APIs use an explicit `[UnmanagedCallConv(CallConvs=[typeof(CallConvCdecl)])]`
to identify that the APIs use the standard "C" ABI calling convention.

## Special handling of strings
Since the handle types all have AOT marshalling support (built-in or generated) the APIs ALL
use them directly. Leaving the only "tricky part" of strings. LLVM has come a long way and
unified most string use to one of three forms.
1) Raw pointer aliases as an ANSI string
    - These are owned by the implementation and assumed invalid after the container producing
      them is destroyed.
2) Pointers to strings that require a correspoinding call to `LLVMDisposeMessage()`
3) Error message strings that require a corresponding call to `LLVMDisposeErrorMessage()`

#1 is handled by marking the `LibraryImportAttribute` with `StringMarshallingCustomType = typeof( AnsiStringMarshaller )`
and a parameter or return signature of `string`. These are ALWAYS converted to/from native
form directly at point of use. Thus any use of them will incur a performance hit as the
memory is allocated/pinned and copied as needed to marshal the string between managed and
native forms. This does, however mean that there is no ambiguity about the validity or
lifetime of an otherwise alias pointer.

#2 is handled by the `DisposeMessageString` type which is really just a SafeHandle. The
built-in generator supports marshalling handle types so it is really just associating
an otherwise untyped `nint` with a type specific handle to account for automatic
resource cleanup. This type is used directly in the signture of the P/Invoke API. 

#3 is handled similar to #2 except that it uses and internal handle type `ErrorMessageString`.
This handle is internal as the ONLY use of such a string is from an `LLVMErrorRef` and the
ownership semantics of such a thing are complex. The LlvmErrorREf code will lazy initialize
the `ErrorMessage` string to prevent any construction of the string in the native library
until needed. Thus, all of the complexity is handled within the `LLVMErrorRef` type so that
it is fairly easy and unsurprising to use from upper layers of managed code.

> It is worth stressing the point here that there is NO WAY to know which string type to use
 based on only the header files and API signatures they contain. One ***MUST*** read the
 documentation for the API to know (and occasionally dig into the source code as it isn't
 documented what the requirements are!). This is the greatest problem with any form of automated
 interop code generator. It can only scan the headers and knows nothing about the documentation.
 This is why, previously this was all done in a custom YAML file. But as this library and LLVM
 grew that became unmaintainable and downright silly as it was basically just describing
 the marshalling behavior. At best the tool could generate anything where there is no
 potential for ambiguity and leave the rest marked in a way a developer could find it.
 (The implementation in this repo has chosen to ignore signatures for generation entirely
 so it is all maintained "by hand")


