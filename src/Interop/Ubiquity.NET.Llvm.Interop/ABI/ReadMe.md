# ABI Interop code
This source was originally generated from the LLVM and LibLLVM extended headers and then
edited by hand to follow new "modern" patterns of interop support. It is now maintained
directly. The source generation was too problematic/complex to generalize to the point
where the output was usable "as-is". Additionally, it used marshalling hints via a custom
YAML configuration file. Ultimately, this file ended up as a foreign language form of the
marshalling attributes in C# code. So it was mostly abandoned. (It is still used to generate
the `EXPOORTS.g.def` file, the type specific handle wrappers, and perform some validations
of the extension code at the native level.)

The generator did have one advantage in that it could read the configuration file AND
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
is no way to mark the marshalling behavior for unmanaged pointers. Implementations of the
"callbacks" MUST handle marshalling of the ABI types manually. Normally, they will
leverage a `GCHandle` as the "context", perform marshalling, and forward the results on
to the managed context object. But implemenations are free to deal with things as they
need to (or have to if no context parameter is available).

# General patterns for maintainance of P/Invoke signatures
The general pattern is that the interop APIs now ALL use the built-in interop source
generator via the `LibraryImportAttribute` this generates all the marshalling code at
compile time so the result is AOT compatible. This is leveraged by the types generated
for each LLVM handle type. Specifically, the default marshalling of handles ensures
ensure handle types are marshalled safely with `LibraryImportAttribute`. For the global
handles that is the built-in support for `SafeHandle` derived type handling. For the
context handles that is coverd by the `ContextHandleMarshaller<T>` custom marshalling type.
The generated types for all context handles marks the type marshalling with the
`NativeMarshallingAttribute` to ensure it is AOT marshalling ready.

## Arrays
Array marshalling requires some careful annotation and interpretation from the native code
where the type is normally just a pointer. There's nothing in the C/C++ language that says
anything about the SIZE of the data that pointer points to beyond the size of the type
pointed to. (i.e., Is `T*` a pointer to one element of type T, an array of elements of type
`T`? If an array, how many such elements are valid...) Thus marshalling to a runtime with
stronger type guarantees requires some care. Reading the docs, and sometimes the source
code of the implementation.

### In Arrays
Input arrays are generally rather simple to declare and use the form:
``` C#
void LLVMSomeApi(LLVMHandleOfSomeSort h, [In] UInt32[] elements, int numElements);
```

### Out arrays
Arrays where the implementation is expected to provide a pointer that is allocated and filled
in by the native code use the following pattern:
``` C#
void LLVMSomeApiThatFillsArray(LLVMHandleOfSomeSort h, [Out] UInt32[] elements, int numElements);
```

Arrays where the caller is expected to provide a pointer to valid (allocatted, but likely
uninitialized) data that is filled in by native code use the following pattern:
``` C#
void LLVMSomeApiThatFillsArray(LLVMHandleOfSomeSort h, out UInt32[] elements, int numElements);
```

### Return arrays
Return arrays are like an out param except that they are the return type. A problem with this
is that they are **purely** `[Out]` in that the native code must allocate the return value
and cleanup (if any) of that return is left to the caller to perform. 

It is important to note that `[Out] byte[] buf` and `out byte[] buf` are two very different
declarations and have distinct meanings for the marshalling of these parameters. The first
expects the **caller** to allocate the memory and it is 'filled in' by the callee, the second
expects the **callee** to allocate and fill the memory. The second implies some form of
"contract" on the reelase of the allocated memory. The marshaller has no knowledge of such
a thing and does not know how to release it either. Thus a custom marshaller is needed for
such things.

#### Special support for SafeHandle arrays
The built-in marshalling does NOT support arrays of SafeHandles as in parameters
(retaining ownwerhsip [By const ref semantics]) so that is provided as extensions in the
`Ubiquity.NET.InteropHelpers.RefHandleMarshaller` class. 

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
really a status code and ***NOT*** a proper boolean (true/false) value.

## Enumerated value naming
The normal rules of naming enumerated values are suspened for this low level interop API.
The idea is to keep ALL enumeration names and values as close as possible (if not identical)
to the underlying native code. This helps in supporting the idea that the documentation
for the native code is relevant and easier to consume so that the documentation for this
library focuses ONLY on the interop support provided within it.

## Calling convention
All of the P/Invoke APIs use an explicit `[UnmanagedCallConv(CallConvs=[typeof(CallConvCdecl)])]`
to identify that the APIs use the standard "C" ABI calling convention.

### Future optimization
Some might at some point in the future add the ability to suppress GC transitions as an
optimization. Application of that requires great care and understanding of the GC and
native implementation to ensure it is safe to do. This is a strictly performance optimization
that has NO impact on callers so is left for future enhancement.

## Special handling of strings
Since the handle types all have AOT marshalling support (built-in or generated) the APIs ALL
use them directly. Leaving the only "tricky part" of strings. LLVM has come a long way and
unified most string use to one of three forms.
1) Raw pointer aliases as a native string (UTF8 encoding is assumed)
    - These are owned by the implementation and assumed invalid after the container producing
      them is destroyed.
2) Pointers to strings that require a correspoinding call to `LLVMDisposeMessage()`
3) Error message strings that require a corresponding call to `LLVMDisposeErrorMessage()`

#1 is handled in one of two ways (eventually everything should converge on the simpler
LazyEncodedString but this API isn't there yet.)
1) by marking the `LibraryImportAttribute`
`StringMarshallingCustomType = typeof( ConstUtf8StringMarshaller )` and a parameter or return
 signature of `string`.
     - Or, more often with a return attribute specifying the `ConstUtf8StringMarshaller` as
       the marshaller for the return type.
     - These are ALWAYS converted to/from native form directly at point of use. Thus any use
       of them will incur a performance hit as the memory is allocated/pinned and copied as
       needed to marshal the string between managed and native forms. This does, however mean
       that there is no ambiguity about the validity or lifetime of an otherwise alias
       pointer. The built-in/BCL marshalling assumes that ALL native strings are allocated
       via `CoTaskMemAlloc()` and therefore require a release. But that is ***NEVER*** the
       case with LLVM and usually not true of interop with arbitrary C/C++ code.

#2 is handled by the `DisposeMessageString` type which is really just a SafeHandle. The
built-in generator supports marshalling handle types so it is really just associating
an otherwise untyped `nint` with a type specific handle to account for automatic
resource cleanup. This type is used directly in the signture of the P/Invoke API. 

#3 is handled similar to #2 except that it uses an internal handle type `ErrorMessageString`.
This handle is internal as the ONLY use of such a string is from an `LLVMErrorRef` and the
ownership semantics of such a thing are complex. The LLVMErrorRef code will lazy initialize
the `ErrorMessage` string to prevent any construction of the string in the native library
until needed. Thus, all of the complexity is handled within the `LLVMErrorRef` type so that
it is fairly easy and unsurprising to use from upper layers of managed code.

> It is worth stressing the point here that there is NO WAY to know which string type to use
 based on only the header files and API signatures they contain. One ***MUST*** read the
 documentation for the API to know (and occasionally dig into the source code as it isn't
 documented what the requirements are!). This is the greatest problem with any form of
 automated interop code generator. It can only scan the headers and knows nothing about the
 documentation or intended semantics. This is why, previously this was all done in a custom
 YAML file. But as this library and LLVM grew that became unmaintainable and downright silly
 as it was basically just describing the marshalling behavior in a foreign language. At best
 the tool could generate anything where there is no potential for ambiguity and leave the
 rest marked in a way a developer could find it. (The implementation in this repo has chosen
 to ignore signatures for generation entirely so it is all maintained "by hand" now)


