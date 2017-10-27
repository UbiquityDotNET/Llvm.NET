// <copyright file="Argument.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

namespace Llvm.NET.Values
{
    /// <summary>An LLVM Value representing an Argument to a function</summary>
    public class Argument
        : Value
    {
        /// <summary>Gets the function this argument belongs to</summary>
        public Function ContainingFunction => FromHandle<Function>( NativeMethods.LLVMGetParamParent( ValueHandle ) );

        /// <summary>Gets the zero based index of the argument</summary>
        public uint Index => NativeMethods.LLVMGetArgumentIndex( ValueHandle );

        /// <summary>Sets the alignment for the argument</summary>
        /// <param name="value">Alignment value for this argument</param>
        /// <returns><see langword="this"/> for Fluent access</returns>
        public Argument SetAlignment( uint value )
        {
            ContainingFunction.AddAttributeAtIndex( FunctionAttributeIndex.Parameter0 + ( int )Index
                                                  , Context.CreateAttribute( AttributeKind.Alignment, value )
                                                  );
            return this;
        }

        /// <summary>Gets the attributes for this argument</summary>
        public ICollection<AttributeValue> Attributes => new ValueAttributeCollection( ContainingFunction, FunctionAttributeIndex.Parameter0 + ( int )Index );

        internal Argument( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }

    /// <summary>Extension methods for Fluent style manipulation of Argument attributes</summary>
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402", Justification = "Fluent extensions considered part of the type")]
    public static class ArgumentExtensions
    {
        /// <summary>Adds attributes to an <see cref="Argument"/></summary>
        /// <param name="self">The <see cref="Argument"/> to add attributes to</param>
        /// <param name="values"><see cref="AttributeKind"/>s to add</param>
        /// <returns>
        /// <paramref name="self"/> for Fluent access
        /// </returns>
        public static Argument AddAttributes( [ValidatedNotNull] this Argument self, [CanBeNull] params AttributeKind[ ] values )
        {
            self.ValidateNotNull( nameof( self ) );

            if( values != null )
            {
                foreach( var kind in values )
                {
                    AttributeValue attrib = self.Context.CreateAttribute( kind );
                    self.Attributes.Add( attrib );
                }
            }

            return self;
        }

        /// <summary>Adds an attribute to an <see cref="Argument"/></summary>
        /// <param name="self">The <see cref="Argument"/> to add attributes to</param>
        /// <param name="kind"><see cref="AttributeKind"/> to add</param>
        /// <returns>
        /// <paramref name="self"/> for Fluent access
        /// </returns>
        public static Argument AddAttribute( [ValidatedNotNull] this Argument self, AttributeKind kind )
        {
            self.ValidateNotNull( nameof( self ) );
            kind.ValidateDefined( nameof( self ) );

            self.Attributes.Add( self.Context.CreateAttribute( kind ) );

            return self;
        }

        /// <summary>Adds an <see cref="AttributeValue"/> to an <see cref="Argument"/></summary>
        /// <param name="self">The <see cref="Argument"/> to add attributes to</param>
        /// <param name="attrib"><see cref="AttributeValue"/> to add</param>
        /// <returns>
        /// <paramref name="self"/> for Fluent access
        /// </returns>
        public static Argument AddAttribute( [ValidatedNotNull] this Argument self, AttributeValue attrib )
        {
            self.ValidateNotNull( nameof( self ) );

            self.Attributes.Add( attrib );

            return self;
        }

        /// <summary>Adds <see cref="AttributeValue"/>s to an <see cref="Argument"/></summary>
        /// <param name="self">The <see cref="Argument"/> to add attributes to</param>
        /// <param name="attributes"><see cref="AttributeValue"/>s to add</param>
        /// <returns>
        /// <paramref name="self"/> for Fluent access
        /// </returns>
        public static Argument AddAttributes( [ValidatedNotNull] this Argument self, params AttributeValue[ ] attributes )
        {
            return AddAttributes( self,  ( IEnumerable<AttributeValue> )attributes );
        }

        /// <summary>Adds <see cref="AttributeValue"/>s to an <see cref="Argument"/></summary>
        /// <param name="self">The <see cref="Argument"/> to add attributes to</param>
        /// <param name="attributes"><see cref="AttributeValue"/>s to add</param>
        /// <returns>
        /// <paramref name="self"/> for Fluent access
        /// </returns>
        public static Argument AddAttributes( [ValidatedNotNull] this Argument self, IEnumerable<AttributeValue> attributes )
        {
            self.ValidateNotNull( nameof( self ) );

            if( attributes != null )
            {
                foreach( var attrib in attributes )
                {
                    self.Attributes.Add( attrib );
                }
            }

            return self;
        }

        /// <summary>Removes an <see cref="AttributeKind"/> from an <see cref="Argument"/></summary>
        /// <param name="self">The <see cref="Argument"/> to add attributes to</param>
        /// <param name="kind"><see cref="AttributeKind"/> to remove</param>
        /// <returns>
        /// <paramref name="self"/> for Fluent access
        /// </returns>
        public static Argument RemoveAttribute( [ValidatedNotNull] this Argument self, AttributeKind kind )
        {
            kind.ValidateDefined( nameof( kind ) );
            if( kind == AttributeKind.None )
            {
                return self;
            }

            return RemoveAttribute( self, kind.GetAttributeName( ) );
        }

        /// <summary>Removes a named attribute from an <see cref="Argument"/></summary>
        /// <param name="self">The <see cref="Argument"/> to add attributes to</param>
        /// <param name="name">Name of the attribute to remove</param>
        /// <returns>
        /// <paramref name="self"/> for Fluent access
        /// </returns>
        public static Argument RemoveAttribute( [ValidatedNotNull] this Argument self, string name )
        {
            self.ValidateNotNull( nameof( self ) );

            self.Attributes.Remove( name );
            return self;
        }
    }
}
