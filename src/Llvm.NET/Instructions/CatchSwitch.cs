// <copyright file="CatchSwitch.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class CatchSwitch
        : Instruction
    {
        internal CatchSwitch( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
