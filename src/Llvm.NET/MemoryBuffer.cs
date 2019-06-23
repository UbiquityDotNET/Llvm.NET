// -----------------------------------------------------------------------
// <copyright file="MemoryBuffer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Interop;
using Llvm.NET.Properties;
using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET
{
    /// <summary>LLVM MemoryBuffer</summary>
    public sealed class MemoryBuffer
    {
        /// <summary>Initializes a new instance of the <see cref="MemoryBuffer"/> class from a file</summary>
        /// <param name="path">Path of the file to load</param>
        public MemoryBuffer( string path )
        {
            path.ValidateNotNullOrWhiteSpace( nameof( path ) );
            if( LLVMCreateMemoryBufferWithContentsOfFile( path, out LLVMMemoryBufferRef handle, out string msg ).Failed )
            {
                throw new InternalCodeGeneratorException( msg );
            }

            BufferHandle = handle;
        }

        /// <summary>Gets the size of the buffer</summary>
        public int Size => (BufferHandle == default | BufferHandle.IsInvalid) ? 0 : ( int )LLVMGetBufferSize( BufferHandle );

        /// <summary>Gets an array of bytes from the buffer</summary>
        /// <returns>Array of bytes copied from the buffer</returns>
        public byte[] ToArray()
        {
            if( BufferHandle.IsInvalid )
            {
                throw new InvalidOperationException( );
            }

            IntPtr bufferStart = LLVMGetBufferStart( BufferHandle );
            byte[ ] retVal = new byte[ Size ];
            Marshal.Copy( bufferStart, retVal, 0, Size );
            return retVal;
        }

        /// <summary>Create a <see cref="System.ReadOnlySpan{T}"/> for a slice of the buffer</summary>
        /// <param name="start">Starting index for the slice [default = 0]</param>
        /// <param name="length">Length of the slice or -1 to include up to the end of the buffer [default = -1]</param>
        /// <returns>New Span</returns>
        /// <remarks>Creates an efficient means of accessing the raw data of a buffer</remarks>
        public ReadOnlySpan<byte> Slice( int start = 0, int length = -1 )
        {
            if( BufferHandle.IsInvalid )
            {
                throw new InvalidOperationException( );
            }

            if( length == -1 )
            {
                length = Size - start;
            }

            start.ValidateRange( 0, Size - 1, nameof( start ) );
            length.ValidateRange( 0, Size, nameof( length ) );

            if( (start + length) > Size )
            {
                throw new ArgumentException( Resources.start_plus_length_exceeds_size_of_buffer );
            }

            unsafe
            {
                void* startSlice = ( LLVMGetBufferStart( BufferHandle ) + start ).ToPointer( );
                return new ReadOnlySpan<byte>( startSlice, length );
            }
        }

        /// <summary>Detaches the underlying buffer from automatic management</summary>
        /// <remarks>
        /// This is used when passing the memory buffer to an LLVM object (like <see cref="Llvm.NET.ObjectFile.TargetObjectFile"/>
        /// that takes ownership of the underlying buffer. Any use of the buffer after this point results in
        /// an <see cref="InvalidOperationException"/>.
        /// </remarks>
        public void Detach()
        {
            BufferHandle.SetHandleAsInvalid( );
        }

        internal MemoryBuffer( LLVMMemoryBufferRef bufferHandle )
        {
            bufferHandle.ValidateNotDefault( nameof( bufferHandle ) );
            BufferHandle = bufferHandle;
        }

        internal LLVMMemoryBufferRef BufferHandle { get; }
    }
}
