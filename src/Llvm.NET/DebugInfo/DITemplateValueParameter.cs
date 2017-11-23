// <copyright file="DITemplateValueParameter.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Template Value parameter</summary>
    /// <seealso href="xref:llvm_langref#ditemplatevalueparameter">LLVM DITemplateValueParameter</seealso>
    public class DITemplateValueParameter
        : DITemplateParameter
    {
        /// <summary>Gets the value of the paramter as Metadata</summary>
        /// <typeparam name="T">Metadata type of the value to get</typeparam>
        /// <returns>Value or <see langword="null"/> if the value is not castable to <typeparamref name="T"/></returns>
        public T GetValue<T>( )
            where T : LlvmMetadata
        {
            return GetOperand<T>( 2 );
        }

        internal DITemplateValueParameter( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
