// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Values
{
    internal class ValueAttributeCollection
        : ICollection<AttributeValue>
    {
        public ValueAttributeCollection( IFunctionAttributeAccessor container, FunctionAttributeIndex index )
        {
            Container = container;
            Index = index;
        }

        public int Count => (int)Container.GetAttributeCountAtIndex( Index );

        public bool IsReadOnly => false;

        public void Add( AttributeValue item )
        {
            Container.AddAttributeAtIndex( Index, item );
        }

        public void Clear( )
        {
            foreach(AttributeValue attrib in this)
            {
                Remove( attrib );
            }
        }

        public bool Contains( AttributeValue item )
        {
            return this.Any( a => a == item );
        }

        public void CopyTo( AttributeValue[]? array, int arrayIndex )
        {
            if(array == null)
            {
                return;
            }

            foreach(AttributeValue attribute in this)
            {
                array[ arrayIndex ] = attribute;
                ++arrayIndex;
            }
        }

        public IEnumerator<AttributeValue> GetEnumerator( )
        {
            return Container.GetAttributesAtIndex( Index ).GetEnumerator();
        }

        public bool Remove( AttributeValue item )
        {
            bool retVal = Contains( item );
            if(item.IsEnum)
            {
                Container.RemoveAttributeAtIndex( Index, item.Id );
            }
            else
            {
                Container.RemoveAttributeAtIndex( Index, item.Name );
            }

            return retVal;
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();

        private readonly IFunctionAttributeAccessor Container;
        private readonly FunctionAttributeIndex Index;
    }
}
