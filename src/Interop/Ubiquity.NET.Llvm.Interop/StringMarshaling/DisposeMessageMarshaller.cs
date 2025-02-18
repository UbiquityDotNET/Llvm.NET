// -----------------------------------------------------------------------
// <copyright file="DisposeMessageMarshaller.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Custom string marshalling class for strings using <see cref="LLVMDisposeMessage"/></summary>
    [CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(DisposeMessageMarshaller))]
    [SuppressMessage( "Design", "CA1060:Move pinvokes to native methods class", Justification = "Only called by this class" )]
    public static partial class DisposeMessageMarshaller
    {
        /// <summary>Converts an LLVM string to a managed code string</summary>
        /// <param name="p">native 'char*'</param>
        /// <returns>Managed code string representation of the native string</returns>
        public static unsafe string? ConvertToManaged(byte* p)
        {
            return p is null ? null : AnsiStringMarshaller.ConvertToManaged(p);
        }

        /// <summary>Releases the unmanaged pointer</summary>
        /// <param name="p">unmanaged pointer</param>
        public static unsafe void Free(byte* p)
        {
            LLVMDisposeMessage(p);
        }

        [LibraryImport( NativeMethods.LibraryPath)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        private static unsafe partial void LLVMDisposeMessage( byte* p );
    }
}
