// <copyright file="UserOperandList.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

/* TODO: Consider an interface that allows updating elements without growing the list */
/* TODO: Consider a generic implementation of this as it is mosly a duplicate of the Metadata operand list support
         (core difference is the GetOperand() and GetNumOperands calls)
*/

namespace Llvm.NET.Values
{
    /// <summary>Support class to provide read-only list semantics to the operands of a <see cref="User"/> of a method</summary>
    internal class UserOperandList
        : IReadOnlyList<Value>
    {
        /// <inheritdoc/>
        public Value this[ int index ]
        {
            get
            {
                index.ValidateRange( 0, Count - 1, nameof( index ) );

                return GetElement( index );
            }
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                long count = LLVMGetNumOperands( Owner.ValueHandle ) - Offset;
                return ( int )Math.Min( count, int.MaxValue );
            }
        }

        /// <inheritdoc/>
        public IEnumerator<Value> GetEnumerator( )
        {
            for( int i = 0; i < Count; ++i )
            {
                var element = GetElement( i );
                if( element == null )
                {
                    yield break;
                }

                yield return element;
            }
        }

        /// <inheritdoc/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal UserOperandList( User owner )
            : this( owner, 0 )
        {
        }

        internal UserOperandList( User owner, int offset )
        {
            Offset = offset;
            Owner = owner;
        }

        private Value GetElement( int index )
        {
            var handle = LLVMGetOperand( Owner.ValueHandle, checked(( uint )( index + Offset ) ) );
            if( handle == default )
            {
                return null;
            }

            return Value.FromHandle( handle );
        }

        private int Offset;
        private readonly User Owner;

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMValueRef LLVMGetOperand( LLVMValueRef @Val, uint @Index );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMSetOperand( LLVMValueRef @User, uint @Index, LLVMValueRef @Val );

        [DllImport( LibraryPath, EntryPoint = "LLVMGetNumOperands", CallingConvention = CallingConvention.Cdecl )]
        private static extern int LLVMGetNumOperands( LLVMValueRef @Val );
    }
}
