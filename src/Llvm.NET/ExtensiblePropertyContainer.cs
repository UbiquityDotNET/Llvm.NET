using System;
using System.Collections.Generic;

namespace Llvm.NET
{
    /// <summary>Common implementation of <see cref="IExtensiblePropertyContainer"/></summary>
    /// <remarks>
    /// This class implements <see cref="IExtensiblePropertyContainer"/> through an
    /// internal <see cref="Dictionary{TKey, TValue}"/>
    /// </remarks>
    public class ExtensiblePropertyContainer
        : IExtensiblePropertyContainer
    {
        /// <inheritdoc/>
        public void AddExtendedPropertyValue( string id, object value )
        {
            lock ( Items )
            {
                if( Items.TryGetValue( id, out object currentValue ) )
                {
                    if( currentValue != null && value != null && currentValue.GetType( ) != value.GetType( ) )
                    {
                        throw new ArgumentException( " Cannot change type of an extended property once set", nameof( value ) );
                    }
                }

                Items[ id ] = value;
            }
        }

        /// <inheritdoc/>
        public bool TryGetExtendedPropertyValue<T>( string id, out T value )
        {
            value = default( T );
            object item;
            lock ( Items )
            {
                if( !Items.TryGetValue( id, out item ) )
                {
                    return false;
                }
            }

            if( !( item is T ) )
            {
                return false;
            }

            value = ( T )item;
            return true;
        }

        private readonly Dictionary<string, object> Items = new Dictionary<string, object>();
    }
}
