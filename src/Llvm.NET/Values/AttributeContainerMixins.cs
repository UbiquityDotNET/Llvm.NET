// <copyright file="AttributeContainerMixins.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Ubiquity.ArgValidators;

namespace Llvm.NET.Values
{
    // Provides a layer of simplicity and backwards compatibility for manipulating attributes on Values
    public static class AttributeContainerMixins
    {
        public static bool Contains( [ValidatedNotNull] this ICollection<AttributeValue> self, AttributeKind kind )
        {
            self.ValidateNotNull( nameof( self ) );
            kind.ValidateDefined( nameof( kind ) );

            return self.Any( a => a.Kind == kind );
        }

        public static T AddAttributes<T>( [ValidatedNotNull] this T self, FunctionAttributeIndex index, params AttributeKind[ ] values )
            where T : class, IAttributeContainer
        {
            self.ValidateNotNull( nameof( self ) );

            if( values != null )
            {
                foreach( var kind in values )
                {
                    AttributeValue attrib = self.Context.CreateAttribute( kind );
                    if( self is IAttributeAccessor container )
                    {
                        container.AddAttributeAtIndex( index, attrib );
                    }
                    else
                    {
                        self.Attributes[ index ].Add( attrib );
                    }
                }
            }

            return self;
        }

        public static T AddAttribute<T>( this T self, FunctionAttributeIndex index, AttributeKind kind )
            where T : class, IAttributeContainer
        {
            self.ValidateNotNull( nameof( self ) );

            AttributeValue attrib = self.Context.CreateAttribute( kind );
            if( self is IAttributeAccessor container )
            {
                container.AddAttributeAtIndex( index, attrib );
            }
            else
            {
                self.Attributes[ index ].Add( self.Context.CreateAttribute( kind ) );
            }

            return self;
        }

        public static T AddAttribute<T>( this T self, FunctionAttributeIndex index, AttributeValue attrib )
            where T : class, IAttributeContainer
        {
            self.ValidateNotNull( nameof( self ) );

            if( self is IAttributeAccessor container )
            {
                container.AddAttributeAtIndex( index, attrib );
            }
            else
            {
                self.Attributes[ index ].Add( attrib );
            }

            return self;
        }

        public static T AddAttributes<T>( this T self, FunctionAttributeIndex index, params AttributeValue[ ] attributes )
            where T : class, IAttributeContainer
        {
            return AddAttributes( self, index, ( IEnumerable<AttributeValue> )attributes );
        }

        public static T AddAttributes<T>( this T self, FunctionAttributeIndex index, IEnumerable<AttributeValue> attributes )
            where T : class, IAttributeContainer
        {
            self.ValidateNotNull( nameof( self ) );

            if( attributes != null )
            {
                foreach( var attrib in attributes )
                {
                    if( self is IAttributeAccessor container )
                    {
                        container.AddAttributeAtIndex( index, attrib );
                    }
                    else
                    {
                        self.Attributes[ index ].Add( attrib );
                    }
                }
            }

            return self;
        }

        public static T AddAttributes<T>( this T self, FunctionAttributeIndex index, IAttributeDictionary attributes )
            where T : class, IAttributeContainer
        {
            self.ValidateNotNull( nameof( self ) );
            attributes.ValidateNotNull( nameof( attributes ) );

            return AddAttributes( self, index, attributes[ index ] );
        }

        public static T RemoveAttribute<T>( this T self, FunctionAttributeIndex index, AttributeKind kind )
            where T : class, IAttributeContainer
        {
            self.ValidateNotNull( nameof( self ) );

            if( kind == AttributeKind.None )
            {
                return self;
            }

            if( self is IAttributeAccessor container )
            {
                container.RemoveAttributeAtIndex( index, kind );
            }
            else
            {
                ICollection<AttributeValue> attributes = self.Attributes[ index ];
                AttributeValue attrib = attributes.FirstOrDefault( a => a.Kind == kind );
                if( attrib != default )
                {
                    attributes.Remove( attrib );
                }
            }

            return self;
        }

        public static T RemoveAttribute<T>( this T self, FunctionAttributeIndex index, string name )
            where T : class, IAttributeContainer
        {
            self.ValidateNotNull( nameof( self ) );

            if( self is IAttributeAccessor container )
            {
                container.RemoveAttributeAtIndex( index, name );
            }
            else
            {
                ICollection<AttributeValue> attributes = self.Attributes[ index ];
                AttributeValue attrib = attributes.FirstOrDefault( a => a.Name == name );
                if( attrib != default )
                {
                    attributes.Remove( attrib );
                }
            }

            return self;
        }
    }
}
