// -----------------------------------------------------------------------
// <copyright file="MetadataAsValue.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Wraps metadata as a <see cref="Value"/></summary>
    public class MetadataAsValue
        : Value
    {
        internal MetadataAsValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }

        internal static LLVMValueRef IsAMetadataAsValue( LLVMValueRef value )
        {
            return value == default
                   ? value
                   : LibLLVMGetValueKind( value ) == LibLLVMValueKind.MetadataAsValueKind ? value : default;
        }

        /*
        //public static implicit operator Metadata( MetadataAsValue self )
        //{
        //    // TODO: Add support to get the metadata ref from the value...
        //    // e.g. call C++ MetadataAsValue.getMetadata()
        //    throw new NotImplementedException();
        //}
        */
    }
}
