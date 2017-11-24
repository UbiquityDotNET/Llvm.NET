// <copyright file="DINamespace.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information namespace scope</summary>
    /// <seealso href="xref:llvm_langref#dinamespace"/>
    public class DINamespace
        : DIScope
    {
        /* non-operand properties
        public bool ExportSymbols => LLVMDINamespaceGetExportSymbols( MetadataHandle );
        */

        /// <inheritdoc/>
        public override DIScope Scope => GetOperand<DIScope>( 1 );

        /// <inheritdoc/>
        public override string Name => GetOperand<MDString>( 2 ).ToString( );

        internal DINamespace( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
