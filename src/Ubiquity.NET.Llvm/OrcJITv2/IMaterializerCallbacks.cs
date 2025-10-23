// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Interface for a type that receives call backs for materialization (usually a type derived from <see cref="CustomMaterializationUnit"/>)</summary>
    public interface IMaterializerCallbacks
    {
        /// <summary>Materializes all symbols in this unit except those that were previously discarded</summary>
        /// <param name="r"><see cref="MaterializationResponsibility"/> that serves as the context for this materialization</param>
        void Materialize( MaterializationResponsibility r );

        /// <summary>Discards a symbol overwridden by the JIT (Before materialization)</summary>
        /// <param name="jitLib">Library the symbols is discarded from</param>
        /// <param name="symbol">Symbol being discarded</param>
        void Discard( ref readonly JITDyLib jitLib, SymbolStringPoolEntry symbol );

        /// <summary>Destroys the materializer callback context</summary>
        public void Destroy();
    }
}
