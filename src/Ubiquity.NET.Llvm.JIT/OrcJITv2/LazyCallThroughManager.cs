// -----------------------------------------------------------------------
// <copyright file="LazyCallThroughManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Lazy Call Through Manager</summary>
    public class LazyCallThroughManager
    {
        internal LazyCallThroughManager(LLVMOrcLazyCallThroughManagerRef h)
        {
            Handle = h;
        }

        internal LLVMOrcLazyCallThroughManagerRef Handle { get; init; }
    }
}
