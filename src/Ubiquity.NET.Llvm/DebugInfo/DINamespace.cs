// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug information namespace scope</summary>
    /// <seealso href="xref:llvm_langref#dinamespace"/>
    public class DINamespace
        : DIScope
    {
        /* TODO: non-operand properties
        public bool ExportSymbols => LLVMDINamespaceGetExportSymbols( NativeHandle );
        */

        /// <inheritdoc/>
        public override DIScope? Scope => GetOperand<DIScope>( 1 );

        /// <inheritdoc/>
        public override LazyEncodedString Name => GetOperandString( 2 );

        internal DINamespace( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
