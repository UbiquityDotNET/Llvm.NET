// -----------------------------------------------------------------------
// <copyright file="Argument.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using JetBrains.Annotations;
using Llvm.NET.Interop;
using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>An LLVM Value representing an Argument to a function</summary>
    public class Argument
        : Value
    {
        /// <summary>Gets the function this argument belongs to</summary>
        public IrFunction ContainingFunction => FromHandle<IrFunction>( LLVMGetParamParent( ValueHandle ) );

        /// <summary>Gets the zero based index of the argument</summary>
        public uint Index => LibLLVMGetArgumentIndex( ValueHandle );

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

        /// <summary>Adds attributes to an <see cref="Argument"/></summary>
        /// <param name="values"><see cref="AttributeKind"/>s to add</param>
        /// <returns>This Argument for Fluent use</returns>
        public Argument AddAttributes( [CanBeNull] params AttributeKind[ ] values )
        {
            if( values != null )
            {
                foreach( var kind in values )
                {
                    AttributeValue attrib = Context.CreateAttribute( kind );
                    Attributes.Add( attrib );
                }
            }

            return this;
        }

        /// <summary>Adds an attribute to an <see cref="Argument"/></summary>
        /// <param name="kind"><see cref="AttributeKind"/> to add</param>
        /// <returns>
        /// This Argument for Fluent access
        /// </returns>
        public Argument AddAttribute( AttributeKind kind )
        {
            kind.ValidateDefined( nameof( kind ) );

            Attributes.Add( Context.CreateAttribute( kind ) );

            return this;
        }

        /// <summary>Adds <see cref="AttributeValue"/>s to an <see cref="Argument"/></summary>
        /// <param name="attributes"><see cref="AttributeValue"/>s to add</param>
        /// <returns>This Argument for fluent usage</returns>
        public Argument AddAttributes( params AttributeValue[ ] attributes )
        {
            return AddAttributes( ( IEnumerable<AttributeValue> )attributes );
        }

        /// <summary>Adds <see cref="AttributeValue"/>s to an <see cref="Argument"/></summary>
        /// <param name="attributes"><see cref="AttributeValue"/>s to add</param>
        /// <returns>This Argument for fluent usage</returns>
        public Argument AddAttributes( IEnumerable<AttributeValue> attributes )
        {
            if( attributes != null )
            {
                foreach( var attrib in attributes )
                {
                    Attributes.Add( attrib );
                }
            }

            return this;
        }

        /// <summary>Removes an <see cref="AttributeKind"/> from an <see cref="Argument"/></summary>
        /// <param name="kind"><see cref="AttributeKind"/> to remove</param>
        /// <returns>This Argument for fluent usage</returns>
        public Argument RemoveAttribute( AttributeKind kind )
        {
            kind.ValidateDefined( nameof( kind ) );
            if( kind == AttributeKind.None )
            {
                return this;
            }

            return RemoveAttribute( kind.GetAttributeName( ) );
        }

        /// <summary>Removes a named attribute from an <see cref="Argument"/></summary>
        /// <param name="name">Name of the attribute to remove</param>
        /// <returns>This Argument for fluent usage</returns>
        public Argument RemoveAttribute( string name )
        {
            Attributes.Remove( name );
            return this;
        }

        internal Argument( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
