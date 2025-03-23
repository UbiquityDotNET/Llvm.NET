// -----------------------------------------------------------------------
// <copyright file="ObjectLayer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#if FUTURE_DEVELOPMENT_AREA

namespace Ubiquity.NET.Llvm.OrcJITv2
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
#endif
