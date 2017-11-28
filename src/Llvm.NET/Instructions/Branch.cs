// <copyright file="Branch.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using JetBrains.Annotations;
using Llvm.NET.Native;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
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
        [property: CanBeNull]
        public Value Condition => IsConditional ? GetOperand<Value>( -3 ) : null;

        /// <summary>Gets the succesor block(s) for this branch</summary>
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

        internal Branch( LLVMValueRef valueRef)
            : base( valueRef )
        {
        }
    }
}
