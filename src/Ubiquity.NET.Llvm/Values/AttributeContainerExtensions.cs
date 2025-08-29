// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Provides a layer of simplicity and backwards compatibility for manipulating attributes on Values</summary>
    public static class AttributeContainerExtensions
    {
        /// <summary>Determines if a collection of <see cref="AttributeValue"/> contains a given <see cref="UInt32"/></summary>
        /// <param name="self">Collection to test</param>
        /// <param name="id"><see cref="UInt32"/> to search for</param>
        /// <returns><see langword="true"/> if found</returns>
        public static bool Contains( this ICollection<AttributeValue> self, UInt32 id )
        {
            ArgumentNullException.ThrowIfNull( self );
            ArgumentOutOfRangeException.ThrowIfGreaterThan( id, LLVMGetLastEnumAttributeKind() );

            return self.Any( a => a.Id == id );
        }

        /// <summary>Adds a range of values to a collection</summary>
        /// <typeparam name="T">Type of elements in the collection</typeparam>
        /// <param name="self">Collection to Add values to</param>
        /// <param name="range">values to add</param>
        public static void AddRange<T>( this ICollection<T> self, params IEnumerable<T>? range )
        {
            ArgumentNullException.ThrowIfNull( self );
            if(range is null)
            {
                return;
            }

            foreach(var value in range)
            {
                self.Add( value );
            }
        }

        /// <summary>Add a range of attributes to an attribute container</summary>
        /// <param name="self">Attribute container to add attributes to</param>
        /// <param name="range">Range of attributes to operate on</param>
        public static void AddAttributes( this IAttributeContainer self, params IEnumerable<LazyEncodedString>? range )
        {
            ArgumentNullException.ThrowIfNull( self );
            if(range is null)
            {
                return;
            }

            foreach(var attribName in range)
            {
                self.Attributes.Add( self.Context.CreateAttribute( attribName ) );
            }
        }

        /// <summary>Add a range of attributes to an attribute container</summary>
        /// <param name="self">Attribute container to add attributes to</param>
        /// <param name="range">Range of attributes to operate on</param>
        public static void AddAttributes( this IAttributeContainer self, params IEnumerable<AttributeValue>? range )
        {
            ArgumentNullException.ThrowIfNull( self );
            if(range is null)
            {
                return;
            }

            foreach(var value in range)
            {
                self.Attributes.Add( value );
            }
        }

        /// <summary>Adds a single <see cref="AttributeValue"/> to an <see cref="IFunctionAttributeAccessor"/></summary>
        /// <typeparam name="T">Container type</typeparam>
        /// <param name="self">Container to add the attribute to</param>
        /// <param name="index"><see cref="FunctionAttributeIndex"/> to add the attribute to</param>
        /// <param name="attrib">Attribute to add to the container</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static T AddAttribute<T>( this T self, FunctionAttributeIndex index, AttributeValue attrib )
            where T : notnull, IFunctionAttributeAccessor
        {
            ArgumentNullException.ThrowIfNull( self );

            self.AddAttributeAtIndex( index, attrib );

            return self;
        }

        /// <summary>Adds a single <see cref="AttributeValue"/> to an <see cref="IFunctionAttributeAccessor"/></summary>
        /// <typeparam name="T">Accessor type</typeparam>
        /// <param name="self">Accessor to add the attribute to</param>
        /// <param name="index"><see cref="FunctionAttributeIndex"/> to add the attribute to</param>
        /// <param name="name">Name of the attribute to add</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static T AddAttribute<T>( this T self, FunctionAttributeIndex index, LazyEncodedString name )
            where T : notnull, IFunctionAttributeAccessor
        {
            ArgumentNullException.ThrowIfNull( self );
            self.AddAttributeAtIndex( index, self.Context.CreateAttribute( name ) );
            return self;
        }

        /// <summary>Adds <see cref="AttributeValue"/>s to an <see cref="IFunctionAttributeAccessor"/></summary>
        /// <typeparam name="T">Accessor type</typeparam>
        /// <param name="self">Accessor to add the attribute to</param>
        /// <param name="index"><see cref="FunctionAttributeIndex"/> to add the attributes to</param>
        /// <param name="names">Names of a attributes to add to the accessor [All must be an enum with no args OR a string attribute that accepts an empty string]</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static T AddAttributes<T>( this T self, FunctionAttributeIndex index, params IEnumerable<LazyEncodedString>? names )
            where T : notnull, IFunctionAttributeAccessor
        {
            ArgumentNullException.ThrowIfNull( self );

            if(names != null)
            {
                foreach(var attrib in names)
                {
                    self.AddAttribute( index, attrib );
                }
            }

            return self;
        }

        /// <summary>Adds <see cref="AttributeValue"/>s to an <see cref="IFunctionAttributeAccessor"/></summary>
        /// <typeparam name="T">Container type</typeparam>
        /// <param name="self">Container to add the attribute to</param>
        /// <param name="index"><see cref="FunctionAttributeIndex"/> to add the attributes to</param>
        /// <param name="attributes">Attribute to add to the container</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static T AddAttributes<T>( this T self, FunctionAttributeIndex index, params IEnumerable<AttributeValue> attributes )
            where T : notnull, IFunctionAttributeAccessor
        {
            ArgumentNullException.ThrowIfNull( self );

            if(attributes != null)
            {
                foreach(var attrib in attributes)
                {
                    self.AddAttributeAtIndex( index, attrib );
                }
            }

            return self;
        }

        /// <summary>Removes an <see cref="UInt32"/> from an <see cref="IFunctionAttributeAccessor"/></summary>
        /// <typeparam name="T">Container type</typeparam>
        /// <param name="self">Container to remove the attribute from</param>
        /// <param name="index"><see cref="FunctionAttributeIndex"/> to remove the attribute from </param>
        /// <param name="id">Attribute to remove from the container</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static T RemoveAttribute<T>( this T self, FunctionAttributeIndex index, UInt32 id )
            where T : notnull, IFunctionAttributeAccessor
        {
            ArgumentNullException.ThrowIfNull( self );
            self.RemoveAttributeAtIndex( index, id );
            return self;
        }

        /// <summary>Removes a named attribute from an <see cref="IFunctionAttributeAccessor"/></summary>
        /// <typeparam name="T">Container type</typeparam>
        /// <param name="self">Container to remove the attribute from</param>
        /// <param name="index"><see cref="FunctionAttributeIndex"/> to remove the attribute from </param>
        /// <param name="name">Attribute name to remove from the container</param>
        /// <returns><paramref name="self"/> for fluent use</returns>
        public static T RemoveAttribute<T>( this T self, FunctionAttributeIndex index, LazyEncodedString name )
            where T : notnull, IFunctionAttributeAccessor
        {
            ArgumentNullException.ThrowIfNull( self );

            self.RemoveAttributeAtIndex( index, name );
            return self;
        }

        /// <summary>Finds an <see cref="AttributeValue"/> by index and name</summary>
        /// <typeparam name="T">Type of the accessor</typeparam>
        /// <param name="self">Accessor to find the attribute on</param>
        /// <param name="index">Index of the attribute</param>
        /// <param name="name">Name of the attribute</param>
        /// <returns>AttributeValue or <see langword="null"/> if not found.</returns>
        public static AttributeValue? FindAttribute<T>( this T self, FunctionAttributeIndex index, LazyEncodedString name )
            where T : notnull, IFunctionAttributeAccessor
        {
            var attribInfo = AttributeInfo.From(name);
            return attribInfo.ID == 0
                ? null
                : (from attr in self.GetAttributesAtIndex( index )
                   where attr.Id == attribInfo.ID
                   select attr
                  ).FirstOrDefault();
        }
    }
}
