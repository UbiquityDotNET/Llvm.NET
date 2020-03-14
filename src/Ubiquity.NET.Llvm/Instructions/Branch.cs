// -----------------------------------------------------------------------
// <copyright file="Branch.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Branch instruction</summary>
    public class Branch
        : Terminator
    {
        /// <summary>Gets a value indicating whether this branch is unconditional</summary>
        public bool IsUnconitional => Operands.Count == 1;

        /// <summary>Gets a value indicating whether this branch is conditional</summary>
        public bool IsConditional => Operands.Count == 3;

        /// <summary>Gets the condition for the branch, if any</summary>
        public Value? Condition => IsConditional ? GetOperand<Value>( -3 ) : null;

        /// <summary>Gets the successor block(s) for this branch</summary>
        public IReadOnlyList<BasicBlock> Successors
        {
            get
            {
                var retVal = new List<BasicBlock>( );
                if( IsConditional )
                {
                    retVal.Add( GetOperand<BasicBlock>( -2 ) );
                }

                retVal.Add( GetOperand<BasicBlock>( -1 ) );
                return retVal;
            }
        }

        internal Branch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
