// -----------------------------------------------------------------------
// <copyright file="ValueAttributeDictionary.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>re-usable implementation of IAttributeDictionary for containers that implement IAttributeAccessor</summary>
    /// <remarks>
    /// This uses the low-level methods of IAttributeAccessor to abstract out the differences in the
    /// LLVM-C API for attributes on CallSites vs. Functions
    /// </remarks>
    internal class ValueAttributeDictionary
        : IAttributeDictionary
    {
        public IContext Context => Container.Context;

        public ICollection<AttributeValue> this[ FunctionAttributeIndex key ]
            => ContainsKey( key )
               ? (ICollection<AttributeValue>)new ValueAttributeCollection( Container, key )
               : throw new KeyNotFoundException();

        public IEnumerable<FunctionAttributeIndex> Keys
            => new ReadOnlyCollection<FunctionAttributeIndex>( [ .. GetValidKeys( ) ] );

        public IEnumerable<ICollection<AttributeValue>> Values
            => new ReadOnlyCollection<ICollection<AttributeValue>>( [ .. this.Select( kvp => kvp.Value ) ] );

        public int Count => GetValidKeys( ).Count( );

        public bool ContainsKey( FunctionAttributeIndex key ) => GetValidKeys( ).Any( k => k == key );

        public IEnumerator<KeyValuePair<FunctionAttributeIndex, ICollection<AttributeValue>>> GetEnumerator( )
        {
            return ( from key in GetValidKeys( )
                     select new KeyValuePair<FunctionAttributeIndex, ICollection<AttributeValue>>( key, this[ key ] )
                   ).GetEnumerator( );
        }

        public bool TryGetValue( FunctionAttributeIndex key, [MaybeNullWhen( false )] out ICollection<AttributeValue> value )
        {
            value = null;
            if( ContainsKey( key ) )
            {
                return false;
            }

            value = new ValueAttributeCollection( Container, key );
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        internal ValueAttributeDictionary( IAttributeAccessor container, Func<Function> functionFetcher )
        {
            Container = container;
            FunctionFetcher = functionFetcher;
        }

        private IEnumerable<FunctionAttributeIndex> GetValidKeys( )
        {
            var endIndex = FunctionAttributeIndex.Parameter0 + FunctionFetcher().Parameters.Count;
            for( var index = FunctionAttributeIndex.Function; index < endIndex; ++index )
            {
                if( Container.GetAttributeCountAtIndex( index ) > 0 )
                {
                    yield return index;
                }
            }
        }

        private readonly Func<Function> FunctionFetcher;
        private readonly IAttributeAccessor Container;
    }
}
