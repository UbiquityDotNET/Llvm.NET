// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    // NOTE the overloads of InitializeNativeCopy() are all VERY similar.
    // They are NOT generic as that would require some sort of generic interface for the DangerousGetHandle() support and a factory
    // for the resulting pair. That would require either exposing raw interop types as part of the documented API surface
    // of this library, an explicit interface implementation, which would require boxing to get at the method... OR require
    // specifying all the type requirements as generic parameters at the call site...
    //
    // Obviously, attempting to generalize this gets complicated for the implementation AND the call sites. So simple
    // copy/paste turns out to be the simplest "generator" option for these. [Go Figure!]
    //
    // TODO: Make projected structs layout compat with native so a copy isn't needed
    internal static class KvpMapExtensions
    {
        // TODO: Make projected structs layout compat with native so a copy isn't needed
        internal static IMemoryOwner<LLVMOrcCSymbolMapPair> InitializeNativeCopy(
            [NotNull] this IReadOnlyCollection<KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol>> symbols
            )
        {
            ArgumentNullException.ThrowIfNull( symbols );

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            using var enumerator = symbols.GetEnumerator();

            try
            {
                for(; i < symbols.Count; ++i)
                {
                    enumerator.MoveNext();
                    var pair = enumerator.Current;
                    if(pair.Key.IsDisposed)
                    {
                        throw new ArgumentException( "invalid string value", $"{nameof( symbols )}[{i}]" );
                    }

#pragma warning disable IDISP004 // Don't ignore created IDisposable; new Ref count is "moved" to native (recovered below on exception)
                    nativeSpan[ i ] = new( pair.Key.DangerousGetHandle(addRef: true) , pair.Value.ToABI() );
#pragma warning restore IDISP004 // Don't ignore created IDisposable
                }
            }
            catch
            {
                // release any addrefs made so far...
                enumerator.Reset();
                for(; i >= 0; --i)
                {
                    enumerator.MoveNext();
                    using LLVMOrcSymbolStringPoolEntryRef h = enumerator.Current.Key.DangerousGetHandle();
                }

                throw;
            }

            return nativeArrayOwner;
        }

        // TODO: Make projected structs layout compat with native so a copy isn't needed
        internal static IMemoryOwner<LLVMOrcCSymbolAliasMapPair> InitializeNativeCopy(
            [NotNull] this IReadOnlyCollection<KeyValuePair<SymbolStringPoolEntry, SymbolAliasMapEntry>> symbols
            )
        {
            ArgumentNullException.ThrowIfNull( symbols );

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolAliasMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            using var enumerator = symbols.GetEnumerator();

            try
            {
                for(; i < symbols.Count; ++i)
                {
                    enumerator.MoveNext();
                    var pair = enumerator.Current;
                    if(pair.Key.IsDisposed)
                    {
                        throw new ArgumentException( "invalid string value", $"{nameof( symbols )}[{i}]" );
                    }

#pragma warning disable IDISP004 // Don't ignore created IDisposable; new Ref count is "moved" to native (recovered below on exception)
                    nativeSpan[ i ] = new( pair.Key.DangerousGetHandle(addRef: true) , pair.Value.DangerousGetHandle( addRef: true) );
#pragma warning restore IDISP004 // Don't ignore created IDisposable
                }
            }
            catch
            {
                // release any addrefs made so far...
                enumerator.Reset();
                for(; i >= 0; --i)
                {
                    enumerator.MoveNext();
                    using var h = enumerator.Current.Key.DangerousGetHandle();

                    // Not injected - whole point of this handler is to prevent dangling refs in face of exception
                    #pragma warning disable IDISP007 // Don't dispose injected
                        enumerator.Current.Value.Dispose();
                    #pragma warning restore IDISP007 // Don't dispose injected
                }

                throw;
            }

            return nativeArrayOwner;
        }

        // TODO: Make projected structs layout compat with native so a copy isn't needed
        internal static IMemoryOwner<LLVMOrcCSymbolFlagsMapPair> InitializeNativeCopy(
            [NotNull] this IReadOnlyCollection<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols
            )
        {
            ArgumentNullException.ThrowIfNull( symbols );

            var nativeArrayOwner = MemoryPool<LLVMOrcCSymbolFlagsMapPair>.Shared.Rent(symbols.Count);
            var nativeSpan = nativeArrayOwner.Memory.Span;
            int i = 0;
            using var enumerator = symbols.GetEnumerator();

            try
            {
                for(; i < symbols.Count; ++i)
                {
                    enumerator.MoveNext();
                    var pair = enumerator.Current;
                    if(pair.Key.IsDisposed)
                    {
                        throw new ArgumentException( "invalid string value", $"{nameof( symbols )}[{i}]" );
                    }

#pragma warning disable IDISP004 // Don't ignore created IDisposable; new Ref count is "moved" to native (recovered below on exception)
                    nativeSpan[ i ] = new( pair.Key.DangerousGetHandle(addRef: true) , pair.Value.ToABI() );
#pragma warning restore IDISP004 // Don't ignore created IDisposable
                }
            }
            catch
            {
                // release any addrefs made so far...
                enumerator.Reset();
                for(; i >= 0; --i)
                {
                    enumerator.MoveNext();
                    using var h = enumerator.Current.Key.DangerousGetHandle();
                }

                throw;
            }

            return nativeArrayOwner;
        }
    }
}
