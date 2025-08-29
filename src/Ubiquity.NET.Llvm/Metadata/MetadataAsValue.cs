// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Wraps metadata as a <see cref="Value"/></summary>
    public class MetadataAsValue
        : Value
    {
        /// <summary>Gets a list of the operands for the IrMetadata</summary>
        public ValueOperandListCollection<Value> Operands { get; }

        internal MetadataAsValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
            Operands = new ValueOperandListCollection<Value>( this );
        }

        internal static LLVMValueRef IsAMetadataAsValue( LLVMValueRef value )
        {
            return value == default
                   ? value
                   : LibLLVMGetValueKind( value ) == LibLLVMValueKind.MetadataAsValueKind ? value : default;
        }

        /*
        //public static implicit operator IrMetadata( MetadataAsValue self )
        //{
        //    // TODO: Add support to get the metadata ref from the value...
        //    // e.g. call C++ MetadataAsValue.getMetadata()
        //    throw new NotImplementedException();
        //}
        */
    }
}
