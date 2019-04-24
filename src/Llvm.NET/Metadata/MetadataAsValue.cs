// <copyright file="MetadataAsValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;
using Llvm.NET.Values;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET
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
            if( value == default )
            {
                return value;
            }

            return LLVMGetValueIdAsKind( value ) == ValueKind.MetadataAsValue ? value : default;
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
