// <copyright file="MetadataAsValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    public class MetadataAsValue : Value
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
