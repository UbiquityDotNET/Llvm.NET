﻿// <copyright file="UserOperandList.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET.Native;

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
                if( index >= Count || index < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }

                return Value.FromHandle( NativeMethods.LLVMGetOperand( Owner.ValueHandle, ( uint )index ) );
            }
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                int count = NativeMethods.LLVMGetNumOperands( Owner.ValueHandle );
                return Math.Min( count, int.MaxValue );
            }
        }

        /// <inheritdoc/>
        public IEnumerator<Value> GetEnumerator( )
        {
            for( uint i = 0; i < Count; ++i )
            {
                LLVMValueRef val = NativeMethods.LLVMGetOperand( Owner.ValueHandle, i );
                if( val == default )
                {
                    yield break;
                }

                yield return Value.FromHandle( val );
            }
        }

        /// <inheritdoc/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal UserOperandList( User owner )
        {
            Owner = owner;
        }

        private readonly User Owner;
    }
}
