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
check and report any missing or mismatched information. Or possibly automatically
generate the proper code again. (Though that seems unlikely as the type of string is a
major problematic factor.)

## ABI Function Pointers
ABI function pointers are represented as real .NET function pointers with an unmanaged
signature.

### special consideration for handles
LLVM handles are just value types that wrap around a runtime `nint` (basically
a strong typedef for a pointer). Therefore, they are blittable value types and don't
need any significant marshaling support. All LLVM handle managed projections **CANNOT**
appear in the signature of an unmanaged function pointer as there is no way to mark
the marshalling behavior for unmanaged pointers. Implementations of the "callbacks"
MUST handle marshalling of the ABI types manually. Normally, they will leverage a
`GCHandle` as the "context", perform marshalling, and forward the results on to the
managed context object. But implementations are free to deal with things as they need
to (or have to if no context parameter is available).

# General patterns for maintenance of P/Invoke signatures
The general pattern is that the interop APIs now ALL use the built-in interop source
generator via the `LibraryImportAttribute` this generates all the marshalling code at
compile time so the result is AOT compatible. This is leveraged by the types generated
for each LLVM handle type. Specifically, the default marshalling of handles ensures
handle types are marshaled safely with `LibraryImportAttribute`. For the global
handles that is the built-in support for `SafeHandle` derived type handling. For the
context handles that is covered by the `ContextHandleMarshaller<T>` custom marshalling
type. The generated types for all context handles marks the type marshalling with the
`NativeMarshallingAttribute` to ensure it is AOT marshalling ready.

## Arrays
Array marshalling requires some careful annotation and interpretation from the native
code where the type is normally just a pointer. There's nothing in the C/C++ language
that says anything about the SIZE of the data that pointer points to beyond the size
of the type pointed to. (i.e., Is `T*` a pointer to one element of type T, an array of
elements of type `T`? If an array, how many such elements are valid...) Thus,
marshalling to a runtime with stronger type guarantees requires some care. Reading the
docs, and sometimes the source code of the implementation.

### In Arrays
Input arrays are generally rather simple to declare and use the form:
``` C#
void LLVMSomeApi(LLVMHandleOfSomeSort h, [In] UInt32[] elements, int numElements);
```

### Arrays as OUT parameters
Arrays where the implementation is expected to provide a pointer that is allocated and
filled in by the native code use the following pattern:
``` C#
void LLVMSomeApiThatFillsArray(LLVMHandleOfSomeSort h, [Out] UInt32[] elements, int numElements);
```

Arrays where the caller is expected to provide a pointer to valid (allocated, but
likely uninitialized) data that is filled in by native code use the following pattern:
``` C#
void LLVMSomeApiThatFillsArray(LLVMHandleOfSomeSort h, out UInt32[] elements, int numElements);
```

### Return arrays
Return arrays are like an out param except that they are the return type. A problem
with this is that they are **purely** `[Out]` in that the native code must allocate
the return value and cleanup (if any) of that allocation is left to the caller to
perform. 

It is important to note that `[Out] byte[] buf` and `out byte[] buf` are two very
different declarations and have distinct meanings for the marshalling of these
parameters. The first expects the **caller** to allocate the memory and it is 'filled
in' by the callee, the second expects the **callee** to allocate and fill the memory.
The second implies some form of "contract" on the release of the allocated memory. The
normal marshaling has no knowledge of such a thing and does not know how to release it
either. Thus, a custom marshaller is needed for such things.

#### Special support for SafeHandle arrays
The built-in marshalling does NOT support arrays of SafeHandles as in parameters
(retaining ownership [By const ref semantics]) so that is provided as extensions in
the `Ubiquity.NET.InteropHelpers.RefHandleMarshaller` class. 

## Explicit types
All of the P/Invoke API signatures are created with Explicitly sized values for
numerics ***and enumerations***. This ensures there is no confusion on the bit length
of types etc... (For example, [U]Int[64|32|16] is ALWAYS used over any built-in
wrappers to avoid any potential confusion about the size of types between C# and
native C/C++)

### Distinction for real LLVMBOOL vs Status
This library and P/Invoke signatures disambiguate between an actual boolean value
(`LLVMBool`) and what is really a success or failure status code. As with strings, the
only way to tell the difference is to read the docs, or sometimes the source... Thus,
for ALL API signatures an `LLVMStatus` is used for any native code signature `LLVMBool`
that is documented as behaving like a status and NOT a bool. This prevents mass confusion
on the intent and helps keep the API surface cleaner and more self documenting. (Does a
non-zero `LLVMBool` mean it succeeded? Or does it mean there was an error?) Thus all APIs
need a developer to understand the documentation and set the P/Invoke signature to use
`LLVMStatus` for anything that is really a status code and ***NOT*** a proper boolean
(true/false) value. This library has done that work and ALL signatures using the native
`LLVMBool` is manually evaluated and the correct form applied to the managed P/Invoke
signature.

## Enumerated value naming
The normal rules of naming enumerated values are suspended for this low level interop
API. The idea is to keep ALL enumeration names and values as close as possible (if not
identical) to the underlying native code. This helps in supporting the idea that the
documentation for the native code is relevant and easier to consume so that the
documentation for this library focuses ONLY on the interop support provided within it.

## Calling convention
All of the P/Invoke APIs use an explicit
`[UnmanagedCallConv(CallConvs=[typeof(CallConvCdecl)])]` to identify that the APIs use
the standard "C" ABI calling convention. This is defined by the LLVM-C API.

### Future optimization
At some point in the future it might be worth adding the ability to suppress GC
transitions as an optimization. Application of that requires great care and
understanding of the GC and native implementation to ensure it is safe to do. This is
strictly a performance optimization that has NO impact on callers so is left for
future enhancement as applicable.

## Special handling of strings
>[!NOTE]
> All native strings are assumed encoded as UTF8. This is the most common/compatible
> assumption to make. Sadly, LLVM itself is silent on the point.

Since the handle types all have AOT marshalling support (built-in or generated) the
APIs ALL use them directly. Leaving the only "tricky part" of strings. LLVM has come a
long way and unified most string use to one of three forms.

1) Raw pointer aliases as a native string
    1) These are owned by the implementation and assumed invalid after the container
       producing them is destroyed.
    2) Some APIs will return a `char*` but include an out parameter with the length
       of the string.
2) Pointers to strings that require a corresponding call to `LLVMDisposeMessage()`
3) Error message strings that require a corresponding call to `LLVMDisposeErrorMessage()`

#1 is handled by using `LazyEncodedString` which always makes a copy of the native data
These are ALWAYS copied from the native form at point of use. Thus any use of them will
incur a performance hit as the memory is allocated/pinned, copied. This does, however,
mean that there is no ambiguity about the validity or lifetime of an otherwise alias
pointer. The built-in/BCL marshalling assumes that ALL native strings are allocated
via `CoTaskMemAlloc()` and therefore require a release. But that is ***NEVER*** the
case with LLVM and usually not true of interop with arbitrary C/C++ code.

#2 is handled by marking the occurrence with `MarshalUsing(typeof( DisposeMessageMarshaller )`
and a parameter or return signature of `LazyEncodedString`. (Or directly to `string'
if the results of the API is never expected in a call to a native API. Normally this
is only used for some form of `ToString` semantics API)

#3 is handled similar to #2 except that it uses an internal handle type
`ErrorMessageString`. This handle is internal as the ONLY use of such a string is from
an `LLVMErrorRef` and the ownership semantics of such a thing are very unique. The
LLVMErrorRef code will lazy initialize the `ErrorMessage` string to prevent any
construction of the string in the native library until needed. Thus, all of the
complexity is handled within the `LLVMErrorRef` type so that it is fairly easy and
unsurprising to use from upper layers of managed code.

>[!IMPORTANT]
> It is worth stressing the point here that there is NO WAY to know which string type
> to use based on only the header files and API signatures they contain. One ***MUST***
> read the documentation for the API to know (and occasionally dig into the source code,
> as it often isn't documented what the requirements are!). This is one of the greatest
> problems with any form of automated interop code generator. It can only scan the
> headers and knows nothing about the documentation or intended semantics. This is why,
> previously this was all done in a custom YAML file. But as this library and LLVM
> grew that became unmaintainable and downright silly as it was basically just
> describing the marshalling behavior in a foreign language. At best the tool could
> generate anything where there is no potential for ambiguity and leave the rest
> marked in a way a developer could find it. (The implementation in this repo has
> chosen to ignore signatures for generation entirely so it is all maintained
> "by hand" now)

### String lengths
Sadly, the LLVM-C API is across the map on types used for the length of a string in
the `C` APIs. It is any one of `size_t, int, unsigned`. That is, there is no consistent
standard on that point. Thus, the interop code deals with things as `size_t` and
downcasts with `checked()` to ensure conversion failures trigger an exception. Since,
`size_t` is NOT hard defined to any particular size by the language `nuint` is used
as the size of a native pointer is generally equivalent.^1^

#### APIs accepting a length for a String
APIs that accept a string are wrapped at the interop layer using `LazyEncodedString`
where the length is provided by the wrapper so callers don't need to worry about it.
Generally this takes the form like in the following example:
``` C#
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static uint LLVMGetSyncScopeID(LLVMContextRefAlias C, LazyEncodedString Name)
{
    return LLVMGetSyncScopeID(C, Name, Name.NativeStrLen);
}

[LibraryImport( LibraryName )]
[UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
private static unsafe partial uint LLVMGetSyncScopeID(LLVMContextRefAlias C, LazyEncodedString Name, nuint SLen);
```
>[!NOTE]
> The imported API is declared as private because a local `partial` function is NOT
> allowed in the language.

Sometimes an API will return a pointer AND include an out parameter for the length
of a string. These require great care as they do NOT guarantee that the string is
terminated! Only that the length is valid. Thus, the implementation of wrappers for 
such APIs follow a pattern of hiding the length out parameter to produce a
`LazyEncodedString` as in the following example:

``` C#
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static LazyEncodedString LLVMGetStringAttributeKind(LLVMAttributeRef A)
{
    unsafe
    {
        byte* p = LLVMGetStringAttributeKind(A, out uint len);
        return new(new ReadOnlySpan<byte>(p, checked((int)len)));
    }
}

[LibraryImport( LibraryName )]
[UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
private static unsafe partial byte* LLVMGetStringAttributeKind(LLVMAttributeRef A, out uint Length);
```
>[!IMPORTANT]
> It is very tempting to simply ignore the out length and assume the returned pointer
> is for a terminated string. Most of the time it will be, but some times it isn't!
> There are no guarantees either way. Thus, the implementation of this library will
> handle the subtleties and provide a `LazyEncodedString` from the pointer and length.

>[!IMPORTANT]
> It is also tempting to blindly cast the length to the `int` needed for a span and
> that will mostly work - until it doesn't, and then all hell breaks lose with weird
> and difficult to track behavior. By applying a `checked()` to the conversions ANY
> overflows are caught and triggered at the point of the problem. It is unlikely these
> will ever hit as strings that large are generally unmanageable at runtime anyway.

---
^1^ While it is plausible to create an ISO `C` compliant compiler implementation and
OS runtime environment where size_t is larger than a native pointer. Such a compiler
and runtime would have extremely limited use as most code written or found today,
while technically incorrect, will assume `sizeof(size_t) == sizeof(void*)` and simply
would not function correctly on a platform/compiler that did otherwise.
