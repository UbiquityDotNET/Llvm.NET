using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Llvm.NET.Native
{
    internal enum NativeStringCleanup
    {
        None,
        DisposeMessage,
    }

    // use with:
    //   [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler))]
    // or
    //   [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(StringMarshaler), MarshalCookie="DisposeMessage")]
    [SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Instantiated via CustomMarshaling" )]
    internal class StringMarshaler
        : ICustomMarshaler
    {
        internal StringMarshaler( Action<IntPtr> nativeDisposer )
        {
            NativeDisposer = nativeDisposer;
        }

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

        private Action<IntPtr> NativeDisposer;

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

        public static ICustomMarshaler GetInstance( string cookie )
        {
            switch(cookie.ToUpperInvariant())
            {
            case null:
            case "":
            case "NONE":
                return new StringMarshaler( null );
            case "DISPOSEMESSAGE":
                return new StringMarshaler( NativeMethods.DisposeMessage );
            default:
                throw new ArgumentException( $"'{cookie}' is not a valid option", nameof( cookie ) );
            }
        }
    }
}
