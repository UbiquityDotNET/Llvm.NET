// -----------------------------------------------------------------------
// <copyright file="Argument.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>An LLVM Value representing an Argument to a function</summary>
    public class Argument
        : Value
        , IAttributeContainer
    {
        /// <summary>Gets the function this argument belongs to</summary>
        public Function ContainingFunction => FromHandle<Function>( LLVMGetParamParent( Handle ).ThrowIfInvalid() )!;

        /// <summary>Gets the zero based index of the argument</summary>
        public uint Index => LibLLVMGetArgumentIndex( Handle );

        /// <summary>Sets the alignment for the argument</summary>
        /// <param name="value">Alignment value for this argument</param>
        /// <returns><see langword="this"/> for Fluent access</returns>
        public Argument SetAlignment( uint value )
        {
            ContainingFunction.AddAttributeAtIndex( FunctionAttributeIndex.Parameter0 + (int)Index
                                                  , Context.CreateAttribute( "align", value )
                                                  );
            return this;
        }

        /// <summary>Gets the attributes for this argument</summary>
        public ICollection<AttributeValue> Attributes
            => new ValueAttributeCollection( ContainingFunction, FunctionAttributeIndex.Parameter0 + (int)Index );

        internal Argument( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
