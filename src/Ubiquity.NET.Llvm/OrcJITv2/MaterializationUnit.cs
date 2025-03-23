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
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                Handle.Dispose();
            }

            base.Dispose(disposing);
        }

        internal LLVMOrcMaterializationUnitRef Handle { get; }

        private protected MaterializationUnit(LLVMOrcMaterializationUnitRef h)
        {
            Handle = h.Move();
        }

        // TODO: Make these an extension to a IReadOnlyDictionary<>
        private protected static IMemoryOwner<LLVMOrcCSymbolMapPair> InitializeNativeCopy(
            [ValidatedNotNull] IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol>> symbols
            )
        {
            ArgumentNullException.ThrowIfNull(symbols);

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            try
            {
                for(; i < symbols.Count; ++i)
                {
                    nativeSpan[ i ] = symbols[ i ].ToABI();
                }
            }
            catch
            {
                // release any addrefs made so far...
                for(; i >=0; --i)
                {
                    symbols[i].Key.DangerousRelease();
                }

                throw;
            }

            return nativeArrayOwner;
        }

        private protected static IMemoryOwner<LLVMOrcCSymbolAliasMapPair> InitializeNativeCopy(
            [ValidatedNotNull] IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, SymbolAliasMapEntry>> symbols
            )
        {
            ArgumentNullException.ThrowIfNull(symbols);

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolAliasMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            try
            {
                for(; i < symbols.Count; ++i)
                {
                    nativeSpan[ i ] = symbols[ i ].ToABI();
                }
            }
            catch
            {
                // release any addrefs made so far...
                for(; i >=0; --i)
                {
                    symbols[i].Key.DangerousRelease();
                }

                throw;
            }

            return nativeArrayOwner;
        }

        private protected static IMemoryOwner<LLVMOrcCSymbolFlagsMapPair> InitializeNativeCopy(
            [ValidatedNotNull] IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols
            )
        {
            ArgumentNullException.ThrowIfNull(symbols);

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolFlagsMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            try
            {
                for(; i < symbols.Count; ++i)
                {
                    nativeSpan[ i ] = symbols[ i ].ToABI();
                }
            }
            catch
            {
                // release any addrefs made so far...
                for(; i >=0; --i)
                {
                    symbols[i].Key.DangerousRelease();
                }

                throw;
            }

            return nativeArrayOwner;
        }
    }
}
