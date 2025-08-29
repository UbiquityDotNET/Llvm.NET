// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Performs custom marshalling of handle arrays as in parameters</summary>
    /// <remarks>
    /// <para>Sadly, the built-in support for safe handles doesn't include arrays of the elements
    /// as `in` parameters while still retaining ownership (That is, `ref` semantics). Worse,
    /// the documentation for source generator custom marshallers (especially for arrays)
    /// is so poor that it wasn't plausible to implement this support as a custom marshaller.</para>
    /// <para>Instead these APIs are declared to simplify and control the marshalling as safely
    /// as possible. Callers must use either <see cref="WithNativePointer{THandle}(THandle[], RefHandleMarshaller.VoidOp)"/>
    /// or <see cref="WithNativePointer{THandle, TRetVal}(THandle[], RefHandleMarshaller.ReturningOp{TRetVal})"/>
    /// to allocate, build, call an operation delegate, and then release the native array
    /// [optionally returning a result]. That is, the hard and tedious work of allocating,
    /// copying the managed array and pinning the array for native consumption is ALL handled
    /// in the methods provided by this class.</para>
    /// </remarks>
    public static class RefHandleMarshaller
    {
        // TODO: Support APIs that need to accept more than one such array...

        /// <summary>Delegate for an operation with marshaled memory</summary>
        /// <typeparam name="TRetVal">Return value of the operation</typeparam>
        /// <param name="nativeArrayPtr">Pointer to a pinned array of raw handles</param>
        /// <param name="size">Number of handles in the array</param>
        /// <returns>Result of the operation</returns>
        public unsafe delegate TRetVal ReturningOp<TRetVal>( nint* nativeArrayPtr, int size );

        /// <summary>Delegate for an operation with marshaled memory</summary>
        /// <param name="nativeArrayPtr">Pointer to a pinned array of raw handles</param>
        /// <param name="size">Number of handles in the array</param>
        public unsafe delegate void VoidOp( nint* nativeArrayPtr, int size );

        /// <summary>Marshals an array SafeHandle to a native pointer (as an array of nint) </summary>
        /// <typeparam name="THandle"><see cref="SafeHandle"/> type to marshal</typeparam>
        /// <typeparam name="TRetVal">Return type of the operation</typeparam>
        /// <param name="managedArray">Managed array of handles to marshal (by reference)</param>
        /// <param name="op">Operation to perform with the native array.</param>
        /// <returns>Value returned from <paramref name="op"/></returns>
        /// <remarks>
        /// A native nint holding array is allocated, then the managed array's handle values are
        /// copied to it (Without any AddRefs etc... the managed array OWNS the handles) before
        /// pinning the memory for the native handles and calling <paramref name="op"/>
        /// </remarks>
        public static TRetVal WithNativePointer<THandle, TRetVal>( this THandle[] managedArray, ReturningOp<TRetVal> op )
            where THandle : SafeHandle
        {
            ArgumentNullException.ThrowIfNull( managedArray );
            ArgumentNullException.ThrowIfNull( op );

            unsafe
            {
                using var nativeArray = AllocateNativeSpace(managedArray.Length);
                FillNative( nativeArray.Memory, managedArray );

                using var pinnedHandle = nativeArray.Memory.Pin();
                return op( (nint*)pinnedHandle.Pointer, managedArray.Length );
            }
        }

        /// <summary>Marshals an array SafeHandle to a native pointer (as an array of nint) </summary>
        /// <typeparam name="THandle"><see cref="SafeHandle"/> type to marshal</typeparam>
        /// <param name="managedArray">Managed array of handles to marshal (by reference)</param>
        /// <param name="op">Operation to perform with the native array.</param>
        /// <inheritdoc cref="WithNativePointer{THandle, TRetVal}(THandle[], ReturningOp{TRetVal})" path="/remarks"/>
        public static void WithNativePointer<THandle>( this THandle[] managedArray, VoidOp op )
            where THandle : SafeHandle
        {
            ArgumentNullException.ThrowIfNull( managedArray );
            ArgumentNullException.ThrowIfNull( op );

            unsafe
            {
                using var nativeArray = AllocateNativeSpace(managedArray.Length);
                FillNative( nativeArray.Memory, managedArray );

                using var pinnedHandle = nativeArray.Memory.Pin();
                op( (nint*)pinnedHandle.Pointer, managedArray.Length );
            }
        }

        private static IMemoryOwner<nint> AllocateNativeSpace( int len )
        {
            return MemoryPool<nint>.Shared.Rent( Unsafe.SizeOf<nint>() * len );
        }

        private static void FillNative<T>( Memory<nint> nativeSpace, T[] managedArray )
            where T : SafeHandle
        {
            ArgumentNullException.ThrowIfNull( managedArray );
            ArgumentOutOfRangeException.ThrowIfNotEqual( nativeSpace.Length, managedArray.Length );

            for(int i = 0; i < managedArray.Length; ++i)
            {
                nativeSpace.Span[ i ] = managedArray[ i ].DangerousGetHandle();
            }
        }
    }
}
