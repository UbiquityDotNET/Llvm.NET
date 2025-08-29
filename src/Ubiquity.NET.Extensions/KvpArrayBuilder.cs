// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.Extensions
{
    /// <summary>Simple type to support dictionary style creation of an immutable array of KeyValue pairs via <see cref="ImmutableArray{T}.Builder"/></summary>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <remarks>
    /// <para>
    /// This allows creation of an array of pairs without incurring the overhead of allocating and initializing
    /// the hash codes for the keys, if they are not used. (As may be the case if the list is provided to
    /// native interop). The normal behavior of building an <see cref="ImmutableDictionary{TKey, TValue}"/> would
    /// at a minimum incur the cost of allocation and computation of the hash code for each key.
    /// MUCH better performance is achieved by building the array via a builder then converting it into the
    /// final immutable form.
    /// </para>
    /// <para>This implementation is based on a post on StackOverflow by Ian Griffiths (updated for latest language
    /// and runtime version functionality and to produce an <see cref="ImmutableArray{T}"/> ). It provides collection
    /// initialization support for a sequence of <see cref="KeyValuePair{TKey, TValue}"/>.</para>
    /// </remarks>
    /// <seealso href="https://stackoverflow.com/questions/24033629/how-can-i-create-a-new-instance-of-immutabledictionary"/>
    [SuppressMessage( "Design", "CA1010:Generic interface should also be implemented", Justification = "IEnumerable is unused but must exist to support collection initializers" )]
    [SuppressMessage( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "It is the correct suffix; This is NOT a collection it's a builder" )]
    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "Equality not relevant for a builder" )]
    public readonly struct KvpArrayBuilder<TKey, TValue>( )
        : IEnumerable
        where TKey : notnull
    {
        /// <summary>Add a key+value pair into the dictionary</summary>
        /// <param name="key">Key for the entry</param>
        /// <param name="value">Value for the entry</param>
        public void Add( TKey key, TValue value )
        {
            Builder.Add( new( key, value ) );
        }

        /// <summary>Indexer to set the value of an entry</summary>
        /// <param name="key">Key value to set</param>
        /// <value>Value to set for the <paramref name="key"/></value>
        /// <returns>Value type [Ignored for set only support]</returns>
        /// <remarks>
        /// <note type="important">
        /// Since this builder does NOT store or compute hash codes the indexer is a syntactical convenience
        /// wrapper around the <see cref="Add"/> method. It does NOT prevent duplicate entries. If you use
        /// the same <paramref name="key"/> with a different value, then you get TWO entries with that key.
        /// (In some scenarios this may be desirable, but in most it is not.) The important point on this is
        /// that it is ***NOT*** checked here.
        /// </note>
        /// </remarks>
        [SuppressMessage( "Design", "CA1044:Properties should not be write only", Justification = "This type is ONLY for building immutable arrays" )]
        public TValue this[ TKey key ]
        {
            set => Add( key, value );
        }

        /// <inheritdoc cref="ImmutableArray{T}.Builder.ToImmutable"/>
        [MustUseReturnValue]
        public ImmutableArray<KeyValuePair<TKey, TValue>> ToImmutable( )
        {
            return Builder.ToImmutable();
        }

        /// <inheritdoc/>
        /// <exception cref="NotImplementedException">Always; Do not use this method. It exists only to allow compile time initializer syntax.</exception>
        public IEnumerator GetEnumerator( )
        {
            // Only implementing IEnumerable because collection initializer
            // syntax is unavailable if you don't.
            throw new NotImplementedException();
        }

        private readonly ImmutableArray<KeyValuePair<TKey, TValue>>.Builder Builder = ImmutableArray.CreateBuilder<KeyValuePair<TKey, TValue>>();
    }
}
