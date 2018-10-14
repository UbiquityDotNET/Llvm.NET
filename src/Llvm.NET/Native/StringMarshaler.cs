// <copyright file="StringMarshaler.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Llvm.NET.Native
{
    // Performs string marshaling for various forms of strings used in LLVM interop
    // use with:
    //   // const char* owned by native LLVM, and never disposed by managed callers (just copy to managed string)
    //   [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))]
    //
    //   // const char* allocated in native LLVM, released by managed caller via LLVMDisposeMessage
    //   [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
    //
    //   //  const char* allocated in native LLVM, released by managed caller via LLVMDisposeMangledSymbol
    //   [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="MangledSymbol")]
    [SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via CustomMarshaling" )]
    internal class StringMarshaler
        : ICustomMarshaler
    {
        /// <inheritdoc/>
        public void CleanUpManagedData( object ManagedObj )
        {
        }

        public void CleanUpNativeData( IntPtr pNativeData )
            => NativeDisposer?.Invoke( pNativeData );

        [SuppressMessage( "Design", "CA1024:Use properties where appropriate.", Justification = "Name and signature defined by interface")]
        public int GetNativeDataSize( ) => -1;

        public IntPtr MarshalManagedToNative( object ManagedObj )
            => throw new NotImplementedException( );

        public object MarshalNativeToManaged( IntPtr pNativeData )
            => NormalizeLineEndings( pNativeData );

        public static ICustomMarshaler GetInstance( string cookie )
        {
            switch( cookie.ToUpperInvariant( ) )
            {
            case null:
            case "":
            case "NONE":
                return new StringMarshaler( null );

            case "DISPOSEMESSAGE":
                return new StringMarshaler( LLVMDisposeMessage );

            case "MANGLEDSYMBOL":
                return new StringMarshaler( LLVMOrcDisposeMangledSymbol );

            default:
                throw new ArgumentException( $"'{cookie}' is not a valid option", nameof( cookie ) );
            }
        }

        internal StringMarshaler( Action<IntPtr> nativeDisposer )
        {
            NativeDisposer = nativeDisposer;
        }

        private readonly Action<IntPtr> NativeDisposer;

        // LLVM doesn't use environment/OS specific line endings, so this will
        // normalize the line endings from strings provided by LLVM into the current
        // environment's normal format.
        private static string NormalizeLineEndings( IntPtr llvmString )
        {
            if( llvmString == IntPtr.Zero )
            {
                return string.Empty;
            }

            string str = Marshal.PtrToStringAnsi( llvmString );
            return NormalizeLineEndings( str );
        }

        private static string NormalizeLineEndings( string txt )
        {
            // shortcut optimization for environments that match the LLVM assumption
            if( Environment.NewLine.Length == 1 && Environment.NewLine[ 0 ] == '\n' )
            {
                return txt;
            }

            return LineEndingNormalizingRegEx.Replace( txt, Environment.NewLine );
        }

        private static readonly Regex LineEndingNormalizingRegEx = new Regex( "(\r\n|\n\r|\r|\n)" );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
        private static extern void LLVMOrcDisposeMangledSymbol( IntPtr MangledSymbol );

        [DllImport( NativeMethods.LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMDisposeMessage( IntPtr Message );
    }
}
