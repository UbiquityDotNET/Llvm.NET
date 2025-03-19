﻿// -----------------------------------------------------------------------
// <copyright file="SIToFP.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction for converting a signed integer value into a floating point value</summary>
    public sealed class SIToFP
        : Cast
    {
        internal SIToFP( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
