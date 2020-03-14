# Marshaling LLVMBool

LLVMBool is a typdef in the LLVM-C API that is both simple and problematic. In it's
simplest sense an LLVMBool is a representation of a bi-modal value. However, the
problematic part is that the semantics for the value are different depending on any
given API. That is, in some cases LLVMBool != 0 is a failure case, and others it is
a success! The confusion stems from LLVMBool serving a dual role:
1. A real boolean true/false
2. A status code where 0 == success and non-zero indicates an error

This duality is confusing and can lead to subtle errors in usage of APIs if translated
directly into language projections. This makes hands-off automatic generation of P/Invoke
calls to LLVM either impossible or error prone. Thus, Ubiquity.NET.Llvm uses manually updated P/Invoke
calls that were initially auto generated to get things started but not maintained via any
generation tools. In the case of LLVMBool Ubiquity.NET.Llvm uses distinct types for the different 
semantics and declares the interop signatures with the form appropriate to the function
being called. The two types are LLVMStatus and standard `System.Boolean` or `bool` in C#

## LLVMStatus
This is a status value where 0 == Success and non-zero is a failure or false status.
LLVMStatus is used whenever the 0 == success semantics apply to the API. For example:

```C#
[DllImport( LibraryPath, EntryPoint = "LLVMWriteBitcodeToFD", CallingConvention = CallingConvention.Cdecl )]
internal static extern LLVMStatus LLVMWriteBitcodeToFD( LLVMModuleRef @M, int @FD, int @ShouldClose, int @Unbuffered );
```

## LLVMBool
This is the traditional boolean value where 0==false and non-zero is true and uses the
standard boolean marshaling support for System.Boolean
```C#
[DllImport( LibraryPath, EntryPoint = "LLVMTypeIsSized", CallingConvention = CallingConvention.Cdecl )]
[return: MarshalAs( UnmanagedType.Bool )]
internal static extern bool LLVMTypeIsSized( LLVMTypeRef @Ty );
```

