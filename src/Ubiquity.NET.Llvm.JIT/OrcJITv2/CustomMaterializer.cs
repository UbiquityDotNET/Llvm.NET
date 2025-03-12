// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Holds delegates for performing custom materialization for a single materialization unit</summary>
    internal sealed class CustomMaterializer
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="CustomMaterializer"/> class.</summary>
        /// <param name="materializeAction">Action to perform to materialize the symbol</param>
        /// <param name="discardAction">Action to perform when the JIT discards/replaces a symbol</param>
        /// <param name="dataOwner">[Optional] Owner of data used by <paramref name="materializeAction"/> that is disposed once materialization is complete</param>
        /// <remarks>
        /// The use of <see cref="IDisposable"/> for release of the data is to allow for early disposal of resources if the <paramref name="materializeAction"/>
        /// is never called. If provided the <see cref="IDisposable.Dispose"/> method is ALWAYS called on completion of materialization that, is the
        /// sequence is <paramref name="materializeAction"/> then <see cref="IDisposable.Dispose"/> is called if the <paramref name="dataOwner"/> is
        /// provided. This allows finer control over the lifetime of data used by a materializer even if the materialization itself is never called.
        /// </remarks>
        public CustomMaterializer(MaterializationAction materializeAction, DiscardAction? discardAction, IDisposable? dataOwner)
        {
            AllocatedSelf = new( this );
            MaterializeHandler = materializeAction;
            DiscardHandler = discardAction;
            DataOwner = dataOwner;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if(!AllocatedSelf.IsInvalid && !AllocatedSelf.IsClosed)
            {
                // Decrements the ref count on the handle
                // might not actually destroy anything
                AllocatedSelf.Dispose();

                // IFF the allocated handle reaches ref count == 0
                // dispose the data as well.
                if (AllocatedSelf.IsClosed)
                {
                    DataOwner?.Dispose();
                }
            }
        }

        internal bool SupportsDiscard => DiscardHandler is not null;

        internal unsafe void* AddRefAndGetNativeContext()
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

        private readonly IDisposable? DataOwner;
    }
}
