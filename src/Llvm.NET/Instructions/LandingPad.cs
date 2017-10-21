// <copyright file="LandingPad.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Instructions
{
    public class LandingPad
        : Instruction
    {
        public void AddClause(Value clause)
        {
            if( clause == null )
            {
                throw new ArgumentNullException( nameof( clause ) );
            }

            LLVMAddClause( ValueHandle, clause.ValueHandle);
        }

        public void SetCleanup( bool value ) => LLVMSetCleanup( ValueHandle, value );

        internal LandingPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
