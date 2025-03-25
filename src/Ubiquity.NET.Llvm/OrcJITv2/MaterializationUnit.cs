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
        // NOTE these are all VERY similar but NOT generic as that would require some sort of
        // generic interface for the ToABI() support. That would require either exposing raw
        // interop types as part of the documented API surface of this library OR an explicit
        // interface implementation, which would require boxing to get at the function...
        private protected static IMemoryOwner<LLVMOrcCSymbolMapPair> InitializeNativeCopy(
            [ValidatedNotNull] IReadOnlyDictionary<SymbolStringPoolEntry, EvaluatedSymbol> symbols
            )
        {
            ArgumentNullException.ThrowIfNull(symbols);

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            using var enumerator = symbols.GetEnumerator();
            try
            {
                for(; i < symbols.Count; ++i)
                {
                    enumerator.MoveNext();
                    // NOTE: This will AddRef the handle for the name (Key)
                    //       As the native code will assume ownership of the name
                    nativeSpan[ i ] = enumerator.Current.ToABI();
                }
            }
            catch
            {
                // release any addrefs made so far...
                enumerator.Reset();
                for(; i >=0; --i)
                {
                    enumerator.MoveNext();
                    enumerator.Current.Key.DangerousRelease();
                }

                throw;
            }

            return nativeArrayOwner;
        }

        private protected static IMemoryOwner<LLVMOrcCSymbolAliasMapPair> InitializeNativeCopy(
            [ValidatedNotNull] IReadOnlyDictionary<SymbolStringPoolEntry, SymbolAliasMapEntry> symbols
            )
        {
            ArgumentNullException.ThrowIfNull(symbols);

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolAliasMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            using var enumerator = symbols.GetEnumerator();
            try
            {
                for(; i < symbols.Count; ++i)
                {
                    enumerator.MoveNext();
                    // NOTE: This will AddRef the handle for the name (Key)
                    //       As the native code will assume ownership of the name
                    nativeSpan[ i ] = enumerator.Current.ToABI();
                }
            }
            catch
            {
                // release any addrefs made so far...
                enumerator.Reset();
                for(; i >=0; --i)
                {
                    enumerator.MoveNext();
                    enumerator.Current.Key.DangerousRelease();
                }

                throw;
            }

            return nativeArrayOwner;
        }

        private protected static IMemoryOwner<LLVMOrcCSymbolFlagsMapPair> InitializeNativeCopy(
            [ValidatedNotNull] IReadOnlyCollection<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols
            )
        {
            ArgumentNullException.ThrowIfNull(symbols);

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolFlagsMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            using var enumerator = symbols.GetEnumerator();

            try
            {
                for(; i < symbols.Count; ++i)
                {
                    enumerator.MoveNext();
                    // NOTE: This will AddRef the handle for the name (Key)
                    //       As the native code will assume ownership of the name
                    nativeSpan[ i ] = enumerator.Current.ToABI();
                }
            }
            catch
            {
                // release any addrefs made so far...
                enumerator.Reset();
                for(; i >=0; --i)
                {
                    enumerator.MoveNext();
                    enumerator.Current.Key.DangerousRelease();
                }

                throw;
            }

            return nativeArrayOwner;
        }
    }
}
