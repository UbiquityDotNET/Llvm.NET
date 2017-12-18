# Marshaling strings in LLVM.NET
LLVM provides strings in several forms and this leads to complexities for
P/Invoke signatures as sometimes the strings require some form of release
and in other cases, they do not. Standard .NET marshaling of strings makes
some assumptions with regard to strings as a return type that make the LLVM
APIs difficult. (e.g. in some LLVM APIs the returned string must be released
via LLVMDisposeMessage() or some other call, while in other cases it is just
a pointer to an internal const string that does not need any release.)

To resolve these issues and make the requirements explicitly clear and consistent
Llvm.NET uses custom marshaling of the strings to mark the exact behavior directly
on the P/Invoke signature so it is both clear and easy to use for the upper layers
(it's just a `System.String`)

The current forms of string marshalling are:

```C#
// const char* owned by native LLVM, and never disposed by managed callers (just copy to managed string)
[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))]

// const char* allocated in native LLVM, released by managed caller via LLVMDisposeMessage
[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]

// const char* allocated in native LLVM, released by managed caller via LLVMDisposeMangledSymbol
[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="MangledSymbol")]

```
