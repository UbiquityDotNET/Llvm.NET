// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Base class for template parameter information</summary>
    /// <seealso cref="DITemplateTypeParameter"/>
    /// <seealso cref="DITemplateValueParameter"/>
    public class DITemplateParameter
        : DINode
    {
        /// <summary>Gets the name of the template parameter</summary>
        public string Name => GetOperandString( 0 );

        /// <summary>Gets the type of the template parameter</summary>
        public DIType? Type => GetOperand<DIType>( 1 );

        internal DITemplateParameter( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
