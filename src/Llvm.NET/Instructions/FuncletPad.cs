// <copyright file="FuncletPad.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class FuncletPad
        : Instruction
    {
        internal FuncletPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
