// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Instructions
{
    /// <summary>CallBr instruction</summary>
    /// <seealso href="xref:llvm_langref#i-callbr"/>
    public sealed class CallBr
        : Instruction
        , IFunctionAttributeAccessor
    {
        /// <summary>Gets the target function of the call</summary>
        public Function TargetFunction
            => FromHandle<Function>( LLVMGetCalledValue( Handle ).ThrowIfInvalid() )!;

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

        internal CallBr( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
