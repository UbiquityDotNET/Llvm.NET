﻿// -----------------------------------------------------------------------
// <copyright file="PointerToInt.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to cast a pointer to an integer value</summary>
    public class PointerToInt
        : Cast
    {
        internal PointerToInt( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
