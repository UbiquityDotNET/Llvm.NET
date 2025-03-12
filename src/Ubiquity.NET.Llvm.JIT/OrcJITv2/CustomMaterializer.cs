// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// They are ordered correctly, analyzer has no configurability for the access order and stupid defaults.
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>Delegate to perform action on Materialization</summary>
    /// <param name="r"><see cref="MaterializationResponsibility"/> that serves as the context for this materialization</param>
    public delegate void MaterializationAction(MaterializationResponsibility r);

    /// <summary>Delegate to perform action on discard</summary>
    /// <param name="jitLib">Library the symbols is discarded from</param>
    /// <param name="symbol">Symbol being discarded</param>
    /// <remarks>
    /// This must be a "custom" delegate as the <see cref="JITDyLib"/> is a
    /// ref type that is NOT allowed as a type parameter for <see cref="Action{T1, T2}"/>.
    /// </remarks>
    public delegate void DiscardAction(JITDyLib jitLib, SymbolStringPoolEntry symbol);

    /// <summary>Holds delegates for performing custom materialization for a single materialization unit</summary>
    public sealed class CustomMaterializer
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="CustomMaterializer"/> class.</summary>
        /// <param name="materializeAction">Action to perform to materialize the symbol</param>
        public CustomMaterializer(MaterializationAction materializeAction)
            : this( materializeAction, null, static () => { } )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CustomMaterializer"/> class.</summary>
        /// <param name="materializeAction">Action to perform to materialize the symbol</param>
        /// <param name="destroyAction">Action to perform to release any resources held for this materialization that won't be used</param>
        public CustomMaterializer(MaterializationAction materializeAction, Action destroyAction)
            : this( materializeAction, null, destroyAction )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CustomMaterializer"/> class.</summary>
        /// <param name="materializeAction">Action to perform to materialize the symbol</param>
        /// <param name="discardAction">Action to perform when the JIT discards/replaces a symbol</param>
        /// <param name="destroyAction">Action to perform to release any resources held for this materialization that won't be used</param>
        public CustomMaterializer(MaterializationAction materializeAction, DiscardAction? discardAction, Action destroyAction)
        {
            AllocatedSelf = new( this );
            MaterializeHandler = materializeAction;
            DiscardHandler = discardAction;
            DestroyHandler = destroyAction;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if(!AllocatedSelf.IsInvalid && !AllocatedSelf.IsClosed)
            {
                AllocatedSelf.Dispose();
            }
        }

        internal bool SupportsDiscard => DiscardHandler is not null;

        internal void AddRef()
        {
            bool ignoredButRequired = false;
            AllocatedSelf.DangerousAddRef(ref ignoredButRequired);
        }

        internal unsafe void* GetNativeContext()
        {
            return (void*)AllocatedSelf.DangerousGetHandle();
        }

        internal MaterializationAction MaterializeHandler { get; init; }

        internal DiscardAction? DiscardHandler { get; init; }

        internal Action DestroyHandler { get; init; }

        internal SafeGCHandle AllocatedSelf { get; init; }
    }
}
