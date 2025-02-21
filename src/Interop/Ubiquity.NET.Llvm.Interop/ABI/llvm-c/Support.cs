// -----------------------------------------------------------------------
// <copyright file="Support.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if INCLUDE_LLVM_LIBRARY_SUPPORT_ABI

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    // CONSIDER: These APIs are highly questionable in a managed environment. Should they even be published?
    public static partial class NativeMethods
    {
        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof(AnsiStringMarshaller) )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static unsafe partial bool LLVMLoadLibraryPermanently( string Filename );

        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LLVMParseCommandLineOptions( int argc, [In]string[] argv, string Overview );

        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof(AnsiStringMarshaller) )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial nint LLVMSearchForAddressOfSymbol( string symbolName );

        [LibraryImport( LibraryPath, StringMarshallingCustomType = typeof(AnsiStringMarshaller) )]
        [UnmanagedCallConv( CallConvs = [typeof(CallConvCdecl)] )]
        public static unsafe partial void LLVMAddSymbol( string symbolName, void* symbolValue );
    }
}
#endif
