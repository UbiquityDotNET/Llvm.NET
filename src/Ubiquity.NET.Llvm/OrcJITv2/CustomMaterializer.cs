// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Holds delegates for performing custom materialization for a single materialization unit</summary>
    internal sealed class CustomMaterializer
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="CustomMaterializer"/> class.</summary>
        /// <param name="materializeAction">Action to perform to materialize the symbol</param>
        /// <param name="discardAction">Action to perform when the JIT discards/replaces a symbol</param>
        public CustomMaterializer( MaterializationAction materializeAction, DiscardAction? discardAction )
        {
            AllocatedSelf = new( this );
            MaterializeHandler = materializeAction;
            DiscardHandler = discardAction;
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            if(!AllocatedSelf.IsInvalid && !AllocatedSelf.IsClosed)
            {
                // Decrements the ref count on the handle
                // might not actually destroy anything
                AllocatedSelf.Dispose();
            }
        }

        internal bool SupportsDiscard => DiscardHandler is not null;

        internal unsafe nint AddRefAndGetNativeContext( )
        {
            return AllocatedSelf.AddRefAndGetNativeContext();
        }

        internal MaterializationAction MaterializeHandler { get; init; }

        internal DiscardAction? DiscardHandler { get; init; }

        // This is the key to ref counted behavior to hold this instance (and anything it references)
        // alive for the GC. The "ownership" of the refcount is handed to native code while the
        // calling code is free to no longer reference this instance as it holds an allocated
        // GCHandle for itself and THAT is kept alive by a ref count that is "owned" by native code.
        private SafeGCHandle AllocatedSelf { get; init; }
    }
}
