// <copyright file="MemoryBuffer.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>LLVM MemoryBuffer</summary>
    public sealed partial class MemoryBuffer
    {
        internal static class NativeMethods
        {
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMStatus LLVMCreateMemoryBufferWithContentsOfFile( [MarshalAs( UnmanagedType.LPStr )] string Path
                                                                                     , out LLVMMemoryBufferRef OutMemBuf
                                                                                     , [MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]out string OutMessage
                                                                                     );
            /*
            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern LLVMStatus LLVMCreateMemoryBufferWithSTDIN( out LLVMMemoryBufferRef OutMemBuf, out IntPtr OutMessage );


            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMMemoryBufferRef LLVMCreateMemoryBufferWithMemoryRange( [MarshalAs( UnmanagedType.LPStr )] string InputData
                                                                                           , size_t InputDataLength
                                                                                           , [MarshalAs( UnmanagedType.LPStr )] string BufferName
                                                                                           , [MarshalAs( UnmanagedType.Bool )]bool RequiresNullTerminator
                                                                                           );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false )]
            internal static extern LLVMMemoryBufferRef LLVMCreateMemoryBufferWithMemoryRangeCopy( [MarshalAs( UnmanagedType.LPStr )] string InputData
                                                                                               , size_t InputDataLength
                                                                                               , [MarshalAs( UnmanagedType.LPStr )] string BufferName
                                                                                               );
            */

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern IntPtr LLVMGetBufferStart( LLVMMemoryBufferRef MemBuf );

            [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
            internal static extern size_t LLVMGetBufferSize( LLVMMemoryBufferRef MemBuf );
        }
    }
}
