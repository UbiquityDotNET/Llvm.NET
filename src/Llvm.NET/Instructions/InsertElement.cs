﻿// -----------------------------------------------------------------------
// <copyright file="InsertElement.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to insert an element into a vector type</summary>
    /// <seealso href="xref:llvm_langref#insertelement-instruction">LLVM insertelement Instruction</seealso>
    public class InsertElement
        : Instruction
    {
        internal InsertElement( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
