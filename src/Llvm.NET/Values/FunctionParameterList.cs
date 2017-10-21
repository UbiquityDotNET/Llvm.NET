// <copyright file="FunctionParameterList.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET.Native;

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

                return Value.FromHandle<Argument>( NativeMethods.LLVMGetParam( OwningFunction.ValueHandle, ( uint )index ) );
            }
        }

        public int Count
        {
            get
            {
                uint count = NativeMethods.LLVMCountParams( OwningFunction.ValueHandle );
                return ( int )Math.Min( count, int.MaxValue );
            }
        }

        public IEnumerator<Argument> GetEnumerator( )
        {
            for( uint i = 0; i < Count; ++i )
            {
                LLVMValueRef val = NativeMethods.LLVMGetParam( OwningFunction.ValueHandle, i );
                if( val.Pointer == IntPtr.Zero )
                {
                    yield break;
                }

                yield return Value.FromHandle<Argument>( val );
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal FunctionParameterList( Function owningFunction )
        {
            OwningFunction = owningFunction;
        }

        private Function OwningFunction;
    }
}
