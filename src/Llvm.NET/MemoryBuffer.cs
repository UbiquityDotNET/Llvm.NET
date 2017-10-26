// <copyright file="MemoryBuffer.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>LLVM MemoryBuffer</summary>
    public sealed class MemoryBuffer
        : IDisposable
    {
        /// <summary>Load a file as an LLVM Memory Buffer</summary>
        /// <param name="path">Path of the file to load into a <see cref="MemoryBuffer"/></param>
        public MemoryBuffer( string path )
        {
            path.ValidateNotNullOrWhiteSpace( nameof( path ) );

            if( LLVMCreateMemoryBufferWithContentsOfFile( path, out BufferHandle_, out string msg ).Failed )
            {
                throw new InternalCodeGeneratorException( msg );
            }
        }

        /// <summary>Gets the size of the buffer</summary>
        public int Size
        {
            get
            {
                if( BufferHandle.Handle == IntPtr.Zero )
                {
                    return 0;
                }

                return LLVMGetBufferSize( BufferHandle ).Pointer.ToInt32();
            }
        }

        public void Dispose( )
        {
            if( BufferHandle.Handle != IntPtr.Zero )
            {
                LLVMDisposeMemoryBuffer( BufferHandle );
                BufferHandle_ = default;
            }
        }

        public byte[] ToArray()
        {
            var bufferStart = LLVMGetBufferStart( BufferHandle );
            var retVal = new byte[ Size ];
            Marshal.Copy( bufferStart, retVal, 0, Size );
            return retVal;
        }

        internal MemoryBuffer( LLVMMemoryBufferRef bufferHandle )
        {
            bufferHandle.Handle.ValidateNotNull( nameof( bufferHandle ) );

            BufferHandle_ = bufferHandle;
        }

        internal LLVMMemoryBufferRef BufferHandle => BufferHandle_;

        // keep as a private field so this is usable as an out parameter in constructor
        // do not write to it directly, treat it as readonly.
        [SuppressMessage( "StyleCop.CSharp.NamingRules"
                        , "SA1310:Field names must not contain underscore"
                        , Justification = "Trailing _ indicates should not be written to directly even internally"
                        )
        ]
        private LLVMMemoryBufferRef BufferHandle_;
    }
}
