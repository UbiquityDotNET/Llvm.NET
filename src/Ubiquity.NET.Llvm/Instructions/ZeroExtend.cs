﻿// -----------------------------------------------------------------------
// <copyright file="ZeroExtend.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to zero extend a value</summary>
    public class ZeroExtend
        : Cast
    {
        internal ZeroExtend( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
