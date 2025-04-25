// -----------------------------------------------------------------------
// <copyright file="ValueAttributeCollection.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Values
{
    internal class ValueAttributeCollection
        : ICollection<AttributeValue>
    {
        public ValueAttributeCollection( IAttributeAccessor container, FunctionAttributeIndex index )
        {
            Container = container;
            Index = index;
        }

        public int Count => ( int )Container.GetAttributeCountAtIndex( Index );

        public bool IsReadOnly => false;

        public void Add( AttributeValue item )
        {
            Container.AddAttributeAtIndex( Index, item );
        }

        public void Clear( )
        {
            foreach( AttributeValue attrib in this )
            {
                Remove( attrib );
            }
        }

        public bool Contains( AttributeValue item )
        {
            return this.Any( a => a == item );
        }

        public void CopyTo( AttributeValue[ ]? array, int arrayIndex )
        {
            if( array == null )
            {
                return;
            }

            foreach( AttributeValue attribute in this )
            {
                array[ arrayIndex ] = attribute;
                ++arrayIndex;
            }
        }

        public IEnumerator<AttributeValue> GetEnumerator( )
        {
            return Container.GetAttributesAtIndex( Index ).GetEnumerator( );
        }

        public bool Remove( AttributeValue item )
        {
            bool retVal = Contains( item );
            if( item.IsEnum )
            {
                Container.RemoveAttributeAtIndex( Index, item.Id );
            }
            else
            {
                Container.RemoveAttributeAtIndex( Index, item.Name );
            }

            return retVal;
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator( );

        private readonly IAttributeAccessor Container;
        private readonly FunctionAttributeIndex Index;
    }
}
