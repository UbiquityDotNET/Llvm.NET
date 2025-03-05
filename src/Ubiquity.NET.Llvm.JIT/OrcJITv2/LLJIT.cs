// -----------------------------------------------------------------------
// <copyright file="LLJIT.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>ORC v2 LLJIT instance</summary>
    public sealed class LLJIT
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        /// <summary>Gets the Execution session for this JIT</summary>
        public ExecutionSession Session => new(LLVMOrcLLJITGetExecutionSession(Handle));

        internal LLJIT(LLVMOrcLLJITRef h)
        {
            Handle = h;
        }

        private readonly LLVMOrcLLJITRef Handle;
    }
}
