// -----------------------------------------------------------------------
// <copyright file="JITDyLib.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Class for an LLVM ORC JIT v2 Dynamic Library</summary>
    public class JITDyLib
    {
        internal JITDyLib(LLVMOrcJITDylibRef h)
        {
            Handle = h;
        }

        internal LLVMOrcJITDylibRef Handle { get; init; }
    }
}
