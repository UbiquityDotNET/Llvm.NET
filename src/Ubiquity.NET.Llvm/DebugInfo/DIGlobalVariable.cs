// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug information for a global variable</summary>
    /// <seealso href="xref:llvm_langref#diglobalvariable">LLVM DIGlobalVariable</seealso>
    public class DIGlobalVariable
        : DIVariable
    {
        /* TODO: non-operand properties
            bool IsLocalToUnit {get;}
            bool IsDefinition {get;}
        */

        /// <summary>Gets the display name for the variable</summary>
        public string DisplayName => GetOperandString( 4 );

        /// <summary>Gets the linkage name for the variable</summary>
        public string LinkageName => GetOperandString( 5 );

        /// <summary>Gets the static data member declaration for the variable</summary>
        public DIDerivedType? StaticDataMemberDeclaration => GetOperand<DIDerivedType>( 6 );

        internal DIGlobalVariable( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
