// -----------------------------------------------------------------------
// <copyright file="MemoryBuffer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm
{
    /// <summary>LLVM MemoryBuffer</summary>
    public sealed class MemoryBuffer
    {
        /// <summary>Initializes a new instance of the <see cref="MemoryBuffer"/> class from a file</summary>
        /// <param name="path">Path of the file to load</param>
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "msg is Disposed IFF it is valid to begin with" )]
        public MemoryBuffer( string path )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( path );
            if( LLVMCreateMemoryBufferWithContentsOfFile( path, out LLVMMemoryBufferRef handle, out DisposeMessageString msg ).Failed )
            {
                string errMsg = msg.ToString() ?? string.Empty;
                msg.Dispose();
                throw new InternalCodeGeneratorException( errMsg );
            }

            BufferHandle = handle;
        }

        /// <summary>Initializes a new instance of the <see cref="MemoryBuffer"/> class from a byte array</summary>
        /// <param name="data">Array of bytes to copy into the memory buffer</param>
        /// <param name="name">Name of the buffer (for diagnostics)</param>
        /// <remarks>
        /// This constructor makes a copy of the data array as a <see cref="MemoryBuffer"/> the memory in the buffer
        /// is unmanaged memory usable by the LLVM native code. It is released in the Dispose method
        /// </remarks>
        public MemoryBuffer( byte[] data, string? name = null)
        {
            ArgumentNullException.ThrowIfNull( data );
            BufferHandle = LLVMCreateMemoryBufferWithMemoryRangeCopy( data, data.Length, name ?? string.Empty )
                          .ThrowIfInvalid( );
        }

        /// <summary>Gets the size of the buffer</summary>
        public int Size => BufferHandle.IsInvalid ? 0 : ( int )LLVMGetBufferSize( BufferHandle );

        /// <summary>Gets an array of bytes from the buffer</summary>
        /// <returns>Array of bytes copied from the buffer</returns>
        public byte[ ] ToArray( )
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

        /// <summary>Implicit convert to a <see cref="ReadOnlySpan{T}"/></summary>
        /// <param name="buffer">Buffer to convert</param>
        /// <remarks>This is a simple wrapper around calling <see cref="Slice(int, int)"/> with default parameters</remarks>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "Named alternate exists - Slice()" )]
        public static implicit operator ReadOnlySpan<byte>( MemoryBuffer buffer ) => buffer.ThrowIfNull().Slice( 0, -1 );

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

            start.ThrowIfOutOfRange( 0, Size - 1 );
            length.ThrowIfOutOfRange( 0, Size );

            if( ( start + length ) > Size )
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
        /// This is used when passing the memory buffer to an LLVM object (like <see cref="Ubiquity.NET.Llvm.ObjectFile.TargetBinary"/>
        /// that takes ownership of the underlying buffer. Any use of the buffer after this point results in
        /// an <see cref="InvalidOperationException"/>.
        /// </remarks>
        public void Detach( )
        {
            BufferHandle.SetHandleAsInvalid( );
        }

        internal MemoryBuffer( LLVMMemoryBufferRef bufferHandle )
        {
            bufferHandle.ThrowIfInvalid();
            BufferHandle = bufferHandle;
        }

        internal LLVMMemoryBufferRef BufferHandle { get; }
    }
}
