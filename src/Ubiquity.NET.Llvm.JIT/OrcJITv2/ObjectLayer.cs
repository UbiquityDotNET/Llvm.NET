// -----------------------------------------------------------------------
// <copyright file="ObjectLayer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>ORC JIT v2 Object linking layer</summary>
    public sealed class ObjectLayer
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        internal ObjectLayer( LLVMOrcObjectLayerRef h)
        {
            Handle = h;
        }

        internal LLVMOrcObjectLayerRef Handle { get; init; }
    }
}
