// -----------------------------------------------------------------------
// <copyright file="VAArg.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction to load an argument of a specified type from a variadic argument list</summary>
    public class VaArg
        : UnaryInstruction
    {
        internal VaArg( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
