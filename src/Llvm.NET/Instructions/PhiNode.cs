// <copyright file="PhiNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Instructions
{
    public class PhiNode
        : Instruction
    {
        public void AddIncoming( Value value, BasicBlock srcBlock )
        {
            AddIncoming( Tuple.Create( value, srcBlock ) );
        }

        public void AddIncoming( Tuple<Value, BasicBlock> firstIncoming, params Tuple<Value, BasicBlock>[ ] additionalIncoming )
        {
            if( firstIncoming == null )
            {
                throw new ArgumentNullException( nameof( firstIncoming ) );
            }

            if( additionalIncoming == null )
            {
                throw new ArgumentNullException( nameof( additionalIncoming ) );
            }

            LLVMValueRef[ ] llvmValues = new LLVMValueRef[ additionalIncoming.Length + 1 ];
            llvmValues[ 0 ] = firstIncoming.Item1.ValueHandle;
            for( int i = 0; i < additionalIncoming.Length; ++i )
            {
                llvmValues[ i + i ] = additionalIncoming[ i ].Item1.ValueHandle;
            }

            LLVMBasicBlockRef[ ] llvmBlocks = new LLVMBasicBlockRef[ additionalIncoming.Length + 1 ];
            llvmBlocks[ 0 ] = firstIncoming.Item2.BlockHandle;
            for( int i = 0; i < additionalIncoming.Length; ++i )
            {
                llvmBlocks[ i + i ] = additionalIncoming[ i ].Item2.BlockHandle;
            }

            LLVMAddIncoming( ValueHandle, out llvmValues[ 0 ], out llvmBlocks[ 0 ], ( uint )llvmValues.Length );
        }

        internal PhiNode( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
