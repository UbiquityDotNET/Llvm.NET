// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Support class to provide read only list semantics to the parameters of a method</summary>
    internal class FunctionParameterList
        : IReadOnlyList<Argument>
    {
        public Argument this[ int index ]
        {
            get
            {
                if(index >= Count || index < 0)
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }

                LLVMValueRef valueRef = LLVMGetParam( OwningFunction.Handle, ( uint )index );
                return Value.FromHandle<Argument>( valueRef.ThrowIfInvalid() )!;
            }
        }

        public int Count
        {
            get
            {
                uint count = LLVMCountParams( OwningFunction.Handle );
                return (int)Math.Min( count, int.MaxValue );
            }
        }

        public IEnumerator<Argument> GetEnumerator( )
        {
            for(uint i = 0; i < Count; ++i)
            {
                LLVMValueRef val = LLVMGetParam( OwningFunction.Handle, i );
                yield return Value.FromHandle<Argument>( val.ThrowIfInvalid() )!;
            }
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();

        internal FunctionParameterList( Function owningFunction )
        {
            OwningFunction = owningFunction;
        }

        private readonly Function OwningFunction;
    }
}
