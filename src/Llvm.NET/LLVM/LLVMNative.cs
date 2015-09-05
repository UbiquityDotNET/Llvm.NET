﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Llvm.NET
{
    // add implicit conversions to/from C# bool for convenience
    internal partial struct LLVMBool
    {
        // sometimes LLVMBool values are actually success/failure codes
        // and thus 0 actually means success and not "false".
        public bool Succeeded => Value == 0;
        public bool Failed => !Succeeded;

        public static implicit operator LLVMBool( bool value ) => new LLVMBool( value ? 1 : 0 );
        public static implicit operator bool( LLVMBool value ) => value.Value != 0;
    }

    internal partial struct LLVMMetadataRef
    {
        internal static LLVMMetadataRef Zero = new LLVMMetadataRef( IntPtr.Zero );
    }

    internal static partial class LLVMNative
    {
        internal static ValueKind GetValueKind( LLVMValueRef valueRef ) => ( ValueKind )GetValueID( valueRef );

        internal static string MarshalMsg( IntPtr msg )
        {
            var retVal = string.Empty;
            if( msg != IntPtr.Zero )
            {
                try
                {
                    retVal = NormalizeLineEndings( msg );
                }
                finally
                {
                    DisposeMessage( msg );
                }
            }
            return retVal;
        }

        static LLVMNative()
        {
            try
            {
                // force loading the appropriate architecture specific 
                // DLL before any use of the wrapped inter-op APIs to 
                // allow building this library as ANYCPU
                var handle = NativeMethods.LoadWin32Library( libraryPath, "LibLLVM" );
            }
            catch( Win32Exception )
            {
                // fallback to standard library search paths to allow building
                // CPU specific variants with only one DLL without needing
                // conditional compilation on this library
                NativeMethods.LoadWin32Library( libraryPath, null );
            }
        }

        // LLVM doesn't honor environment/OS specific default line endings, so this will
        // normalize the line endings from strings provided by LLVM into the current
        // environment's format.
        internal static string NormalizeLineEndings( IntPtr llvmString )
        {
            if( llvmString == IntPtr.Zero )
                return string.Empty;

            var str = Marshal.PtrToStringAnsi( llvmString );
            return NormalizeLineEndings( str );
        }

        internal static string NormalizeLineEndings( IntPtr llvmString, int len )
        {
            if( llvmString == IntPtr.Zero )
                return string.Empty;

            var str = Marshal.PtrToStringAnsi( llvmString, len );
            return NormalizeLineEndings( str );
        }

        private static string NormalizeLineEndings( string txt )
        {
            // shortcut optimization for environments that match the LLVM assumption
            if( Environment.NewLine.Length == 1 && Environment.NewLine[ 0 ] == '\n' )
                return txt;

            return LineEndingNormalizingRegEx.Replace( txt, Environment.NewLine );
        }

        private static readonly Regex LineEndingNormalizingRegEx = new Regex( "(\r\n|\n\n|\n\r|\r|\n)" );
    }
}
