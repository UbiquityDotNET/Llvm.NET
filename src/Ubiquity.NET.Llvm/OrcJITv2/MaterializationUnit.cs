// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// Elements ARE ordered correctly, analyzer has dumb defaults and doesn't allow override of order
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Abstract base class for an LLVM ORC JIT v2 Materialization Unit</summary>
    public abstract class MaterializationUnit
        : DisposableObject
    {
        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            if(disposing)
            {
                Handle.Dispose();
            }

            base.Dispose( disposing );
        }

        internal LLVMOrcMaterializationUnitRef Handle { get; }

        private protected MaterializationUnit( LLVMOrcMaterializationUnitRef h )
        {
            Handle = h.Move();
        }
    }
}
