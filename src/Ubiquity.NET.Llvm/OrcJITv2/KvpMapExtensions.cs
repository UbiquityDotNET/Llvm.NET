// -----------------------------------------------------------------------
// <copyright file="AliasMapEntry.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    // NOTE the overloads of ToABI() and InitializeNativeCopy() are all VERY similar (source identical but NOT IL identical).
    // They are NOT generic as that would require some sort of generic interface for the ToABI() support and a factory
    // for the resulting pair. That would require either exposing raw interop types as part of the documented API surface
    // of this library an explicit interface implementation, which would require boxing to get at the method... OR require
    // specifying all the type requirements as generic parameters at the call site...
    //
    // Obviously, attempting to generalize this gets complicated for the implementation AND the call sites. So simple
    // copy/paste turns out to be the simplest "generator" option for these. [Go Figure!]
    //
    // TODO: Make projected structs layout compat with native so a copy isn't needed
    internal static class KvpMapExtensions
    {
        internal static LLVMOrcCSymbolAliasMapPair ToABI(this KeyValuePair<SymbolStringPoolEntry, SymbolAliasMapEntry> pair)
        {
            return new( pair.Key.ToABI(), pair.Value.ToABI() );
        }

        internal static LLVMOrcCSymbolMapPair ToABI(this KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol> pair)
        {
            return new( pair.Key.ToABI(), pair.Value.ToABI() );
        }

        internal static LLVMOrcCSymbolFlagsMapPair ToABI(this KeyValuePair<SymbolStringPoolEntry, SymbolFlags> pair)
        {
            return new( pair.Key.ToABI(), pair.Value.ToABI() );
        }

        // TODO: Make projected structs layout compat with native so a copy isn't needed
        internal static IMemoryOwner<LLVMOrcCSymbolMapPair> InitializeNativeCopy(
            [ValidatedNotNull] this IReadOnlyCollection<KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol>> symbols
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

        // TODO: Make projected structs layout compat with native so a copy isn't needed
        internal static IMemoryOwner<LLVMOrcCSymbolAliasMapPair> InitializeNativeCopy(
            [ValidatedNotNull] this IReadOnlyCollection<KeyValuePair<SymbolStringPoolEntry, SymbolAliasMapEntry>> symbols
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

        // TODO: Make projected structs layout compat with native so a copy isn't needed
        internal static IMemoryOwner<LLVMOrcCSymbolFlagsMapPair> InitializeNativeCopy(
            [ValidatedNotNull] this IReadOnlyCollection<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols
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
