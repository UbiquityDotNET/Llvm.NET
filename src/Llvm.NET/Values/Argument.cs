using System;
using System.Collections.Generic;

namespace Llvm.NET.Values
{
    /// <summary>An LLVM Value representing an Argument in a function</summary>
    public class Argument
        : Value
    {
        /// <summary>Function this argument belongs to</summary>
        public Function ContainingFunction => new Function( NativeMethods.GetParamParent( ValueHandle ) );
        public uint Index => NativeMethods.GetArgumentIndex( ValueHandle );

        /// <summary>Sets the alignment for the argument</summary>
        /// <param name="value">Alignment value for this argument</param>
        public void SetAlignment( uint value )
        {
            NativeMethods.SetParamAlignment( ValueHandle, value );
        }

        /// <summary>Adds a set of boolean attributes to this argument</summary>
        /// <param name="attributes">Attributes to add</param>
        /// <returns>This argument for use in fluent style coding</returns>
        Argument AddAttributes( params AttributeKind[ ] attributes )
        {
            return AddAttributes( ( IEnumerable<AttributeKind> )attributes );
        }

        /// <summary>Add a collection of attributes to this argument</summary>
        /// <param name="attributes"></param>
        /// <returns>This argument for use in fluent style coding</returns>
        Argument AddAttributes( IEnumerable<AttributeKind> attributes )
        {
            foreach( AttributeKind kind in attributes )
                AddAttribute( kind );
            return this;
        }

        /// <summary>Adds a single boolean attribute to the argument itself</summary>
        /// <param name="kind">Attribute kind to add</param>
        /// <returns>This argument for use in fluent style coding</returns>
        Argument AddAttribute( AttributeKind kind )
        {
            ContainingFunction.AddAttribute( FunctionAttributeIndex.Parameter0 + (int)Index, kind );
            return this;
        }

        internal Argument( LLVMValueRef valueRef )
            : this( valueRef, false )
        {
        }

        internal Argument( LLVMValueRef valueRef, bool preValidated )
            : base( preValidated ? valueRef : ValidateConversion( valueRef, NativeMethods.IsAArgument ) )
        {
        }
    }
}
