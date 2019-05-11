// <copyright file="FunctionParameterList.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>Support class to provide read only list semantics to the parameters of a method</summary>
    internal class FunctionParameterList
        : IReadOnlyList<Argument>
    {
        public Argument this[ int index ]
        {
            get
            {
                if( index >= Count || index < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }

                return Value.FromHandle<Argument>( LLVMGetParam( OwningFunction.ValueHandle, ( uint )index ) );
            }
        }

        public int Count
        {
            get
            {
                uint count = LLVMCountParams( OwningFunction.ValueHandle );
                return ( int )Math.Min( count, int.MaxValue );
            }
        }

        public IEnumerator<Argument> GetEnumerator( )
        {
            for( uint i = 0; i < Count; ++i )
            {
                LLVMValueRef val = LLVMGetParam( OwningFunction.ValueHandle, i );
                if( val == default )
                {
                    yield break;
                }

                yield return Value.FromHandle<Argument>( val );
            }
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal FunctionParameterList( IrFunction owningFunction )
        {
            OwningFunction = owningFunction;
        }

        private readonly IrFunction OwningFunction;
    }
}
