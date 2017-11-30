﻿// <copyright file="Select.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>Select instruction</summary>
    public class Select
        : Instruction
    {
        internal Select( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
