#if SUPPORT_OBJECTLINKING_LAYER
// -----------------------------------------------------------------------
// <copyright file="ObjectLayer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
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
            Handle = h.Move();
        }

        internal LLVMOrcObjectLayerRef Handle { get; init; }
    }
}
#endif
