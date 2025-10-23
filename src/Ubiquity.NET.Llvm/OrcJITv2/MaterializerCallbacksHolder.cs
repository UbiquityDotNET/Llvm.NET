// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Holds delegates for performing custom materialization for a single materialization unit</summary>
    /// <remarks>
    /// Instances of this type serve as the native context for callbacks. This is an internal holder of delegates
    /// instead of an interface to allow for inline functions and lambdas etc.. for each action. This is not
    /// possible if only an interface is used.
    /// </remarks>
    internal sealed class MaterializerCallbacksHolder
        : IMaterializerCallbacks
    {
        /// <summary>Initializes a new instance of the <see cref="MaterializerCallbacksHolder"/> class.</summary>
        /// <param name="materializeAction">Action to perform to materialize the symbol</param>
        /// <param name="discardAction">Action to perform when the JIT discards/replaces a symbol</param>
        public MaterializerCallbacksHolder( MaterializationAction materializeAction, DiscardAction? discardAction )
        {
            MaterializeHandler = materializeAction;
            DiscardHandler = discardAction;
        }

        public void Destroy( )
        {
        }

        public void Discard( ref readonly JITDyLib jitLib, SymbolStringPoolEntry symbol )
        {
            if(DiscardHandler is not null)
            {
                DiscardHandler(in jitLib, symbol);
            }
        }

        public void Materialize( MaterializationResponsibility r )
        {
            MaterializeHandler(r);
        }

        internal MaterializationAction MaterializeHandler { get; private set; }

        internal DiscardAction? DiscardHandler { get; private set; }
    }
}
