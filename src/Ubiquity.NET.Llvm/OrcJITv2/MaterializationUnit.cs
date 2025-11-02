// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Elements ARE ordered correctly, analyzer has dumb defaults and doesn't allow override of order
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Abstract base class for an LLVM ORC JIT v2 Materialization Unit</summary>
    public abstract class MaterializationUnit
        : DisposableObject
    {
        /// <remarks>
        /// For this class, this is an idempotent method. This allows MOVE semantics for native
        /// code to function and callers remain oblivious. Callers should always call this for
        /// correctness if it was succesfully moved to native code then such a call is a NOP.
        /// </remarks>
        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            if(disposing && !Handle.IsNull)
            {
                Handle.Dispose();
                InvalidateAfterMove();
            }

            base.Dispose( disposing );
        }

        /// <summary>This will set the internal handle to a default state; makeing <see cref="Dispose"/> a NOP</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InvalidateAfterMove( )
        {
            Handle = default;
        }

        internal LLVMOrcMaterializationUnitRef Handle { get; private set; }

        private protected MaterializationUnit( LLVMOrcMaterializationUnitRef h )
        {
            Handle = h;
        }
    }
}
