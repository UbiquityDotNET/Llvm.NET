// <copyright file="DITemplateParameter.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Base class for template parameter information</summary>
    /// <seealso cref="DITemplateTypeParameter"/>
    /// <seealso cref="DITemplateValueParameter"/>
    public class DITemplateParameter
        : DINode
    {
        /// <summary>Gets the name of the template parameter</summary>
        public string Name => GetOperand<MDString>( 0 ).ToString( );

        /// <summary>Gets the type of the template parameter</summary>
        public DIType Type => GetOperand<DIType>( 1 );

        internal DITemplateParameter( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
