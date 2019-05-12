// -----------------------------------------------------------------------
// <copyright file="ResumeInstruction.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Resume instruction</summary>
    public class ResumeInstruction
        : Terminator
    {
        internal ResumeInstruction( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
