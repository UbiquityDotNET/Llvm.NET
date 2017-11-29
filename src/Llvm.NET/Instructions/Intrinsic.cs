// <copyright file="Intrinsic.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    /// <summary>base class for calls to LLVM intrinsic functions</summary>
    public class Intrinsic
        : CallInstruction
    {
        internal Intrinsic( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }

        internal const string DoNothingName = "llvm.donothing";
        internal const string DebugTrapName = "llvm.debugtrap";
    }
}
