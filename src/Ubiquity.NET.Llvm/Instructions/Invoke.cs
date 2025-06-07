// -----------------------------------------------------------------------
// <copyright file="Invoke.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>Instruction to invoke (call) a function with exception handling</summary>
    /// <seealso href="xref:llvm_langref#i-invoke">LLVM invoke Instruction</seealso>
    public sealed class Invoke
        : Terminator
        , IFunctionAttributeAccessor
    {
        /// <summary>Gets the target function of the invocation</summary>
        public Function TargetFunction => FromHandle<Function>( LLVMGetCalledValue( Handle ).ThrowIfInvalid() )!;

        /// <summary>Gets or sets the normal destination for the invoke</summary>
        public BasicBlock NormalDestination
        {
            get => BasicBlock.FromHandle( LLVMGetNormalDest( Handle ).ThrowIfInvalid() )!;
            set => LLVMSetNormalDest( Handle, value.ThrowIfNull().BlockHandle );
        }

        /// <inheritdoc/>
        public void AddAttributeAtIndex( FunctionAttributeIndex index, AttributeValue attrib )
        {
            CallSiteAttributeAccessor.AddAttributeAtIndex( this, index, attrib );
        }

        /// <inheritdoc/>
        public uint GetAttributeCountAtIndex( FunctionAttributeIndex index )
        {
            return CallSiteAttributeAccessor.GetAttributeCountAtIndex( this, index );
        }

        /// <inheritdoc/>
        public IEnumerable<AttributeValue> GetAttributesAtIndex( FunctionAttributeIndex index )
        {
            return CallSiteAttributeAccessor.GetAttributesAtIndex( this, index );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, UInt32 id )
        {
            return CallSiteAttributeAccessor.GetAttributeAtIndex( this, index, id );
        }

        /// <inheritdoc/>
        public AttributeValue GetAttributeAtIndex( FunctionAttributeIndex index, LazyEncodedString name )
        {
            return CallSiteAttributeAccessor.GetAttributeAtIndex( this, index, name );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, UInt32 id )
        {
            CallSiteAttributeAccessor.RemoveAttributeAtIndex( this, index, id );
        }

        /// <inheritdoc/>
        public void RemoveAttributeAtIndex( FunctionAttributeIndex index, LazyEncodedString name )
        {
            CallSiteAttributeAccessor.RemoveAttributeAtIndex( this, index, name );
        }

        internal Invoke( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
