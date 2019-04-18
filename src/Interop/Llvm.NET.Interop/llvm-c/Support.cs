// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 2.17941.31104.49410
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Security;

namespace Llvm.NET.Interop
{
    public static partial class NativeMethods
    {
        /**
         * This function permanently loads the dynamic library at the given path.
         * It is safe to call this function multiple times for the same library.
         *
         * @see sys::DynamicLibrary::LoadLibraryPermanently()
          */
        [SuppressUnmanagedCodeSecurity]
        [DllImport( LibraryPath, CallingConvention=global::System.Runtime.InteropServices.CallingConvention.Cdecl )]
        [return: MarshalAs( UnmanagedType.Bool )]
        public static extern bool LLVMLoadLibraryPermanently( [MarshalAs( UnmanagedType.LPStr )]string Filename );

        /**
         * This function parses the given arguments using the LLVM command line parser.
         * Note that the only stable thing about this function is its signature; you
         * cannot rely on any particular set of command line arguments being interpreted
         * the same way across LLVM versions.
         *
         * @see llvm::cl::ParseCommandLineOptions()
         */
        [SuppressUnmanagedCodeSecurity]
        [DllImport( LibraryPath, CallingConvention=global::System.Runtime.InteropServices.CallingConvention.Cdecl )]
        public static extern void LLVMParseCommandLineOptions( int argc, [MarshalAs( UnmanagedType.LPStr, ArraySubType = UnmanagedType.LPStr )]global::System.String[] argv, [MarshalAs( UnmanagedType.LPStr )]string Overview );

        /**
         * This function will search through all previously loaded dynamic
         * libraries for the symbol \p symbolName. If it is found, the address of
         * that symbol is returned. If not, null is returned.
         *
         * @see sys::DynamicLibrary::SearchForAddressOfSymbol()
         */
        [SuppressUnmanagedCodeSecurity]
        [DllImport( LibraryPath, CallingConvention=global::System.Runtime.InteropServices.CallingConvention.Cdecl )]
        public static extern global::System.IntPtr LLVMSearchForAddressOfSymbol( [MarshalAs( UnmanagedType.LPStr )]string symbolName );

        /**
         * This functions permanently adds the symbol \p symbolName with the
         * value \p symbolValue.  These symbols are searched before any
         * libraries.
         *
         * @see sys::DynamicLibrary::AddSymbol()
         */
        [SuppressUnmanagedCodeSecurity]
        [DllImport( LibraryPath, CallingConvention=global::System.Runtime.InteropServices.CallingConvention.Cdecl )]
        public static extern void LLVMAddSymbol( [MarshalAs( UnmanagedType.LPStr )]string symbolName, global::System.IntPtr symbolValue );

    }
}
