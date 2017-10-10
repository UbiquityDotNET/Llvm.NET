// <copyright file="CatchPad.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class CatchPad
        : FuncletPad
    {
        internal CatchPad( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
