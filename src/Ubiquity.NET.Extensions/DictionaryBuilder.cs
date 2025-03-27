// -----------------------------------------------------------------------
// <copyright file="DictionaryBuilder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Ubiquity.NET.Extensions
{
    /// <summary>Simple type to support direct creation of an immutable array via <see cref="ImmutableDictionary{TKey, TValue}.Builder"/></summary>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <remarks>
    /// <para>
    /// For unknown reasons <see cref="ImmutableDictionary{TKey, TValue}"/> does not have an equivalent
    /// to <see cref="CollectionBuilderAttribute"/> (Perhaps because there is none for a dictionary?)
    /// the normal behavior of initializing a mutable dictionary and then converting it to immutable
    /// has sub-par performance as it needs to allocate the mutable form, add the members, then allocate
    /// the immutable form and COPY all the members. This includes the overhead of building and maintaining
    /// the hash codes for all the keys - just to copy it and destroy it... MUCH better performance is achieved
    /// by building the immutable array directly then converting it into the final immutable form.
    /// </para>
    /// <para>This implementation is based on a post on StackOverflow by Ian Griffiths (updated for latest language
    /// and runtime version functionality). It provides collection initialization support for
    /// <see cref="ImmutableDictionary{TKey, TValue}"/>.</para>
    /// </remarks>
    /// <seealso href="https://stackoverflow.com/questions/24033629/how-can-i-create-a-new-instance-of-immutabledictionary"/>
    [SuppressMessage( "Design", "CA1010:Generic interface should also be implemented", Justification = "IEnumerable is unused but must exist to support collection initializers" )]
    [SuppressMessage( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "It is the correct suffix" )]
    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "Equality not relevant for a builder" )]
    public readonly struct DictionaryBuilder<TKey, TValue>()
        : IEnumerable
        where TKey : notnull
    {
        /// <summary>Add a key+value pair into the dictionary</summary>
        /// <param name="key">Key for the entry</param>
        /// <param name="value">Value for the entry</param>
        public void Add(TKey key, TValue value)
        {
            Builder.Add(key, value);
        }

        /// <summary>Indexer to set the value of an entry</summary>
        /// <param name="key">Key value to set</param>
        /// <value>Value to set for the <paramref name="key"/></value>
        /// <returns>Value type [Ignored for set only support]</returns>
        [SuppressMessage( "Design", "CA1044:Properties should not be write only", Justification = "This type is ONLY for building immutable dictionaries" )]
        public TValue this[TKey key]
        {
            set => Builder[ key ] = value;
        }

        /// <inheritdoc cref="ImmutableDictionary{TKey, TValue}.Builder.ToImmutable"/>
        [MustUseReturnValue]
        public ImmutableDictionary<TKey, TValue> ToImmutable()
        {
            return Builder.ToImmutable();
        }

        /// <inheritdoc/>
        public IEnumerator GetEnumerator()
        {
            // Only implementing IEnumerable because collection initializer
            // syntax is unavailable if you don't.
            throw new NotImplementedException();
        }

        private readonly ImmutableDictionary<TKey, TValue>.Builder Builder = ImmutableDictionary.CreateBuilder<TKey, TValue>();
    }
}
