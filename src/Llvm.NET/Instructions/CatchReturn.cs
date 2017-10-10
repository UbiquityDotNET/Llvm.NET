// <copyright file="CatchReturn.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class CatchReturn
        : Terminator
    {
        internal CatchReturn( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
