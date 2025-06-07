// -----------------------------------------------------------------------
// <copyright file="DIVariable.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug information for a variable</summary>
    public class DIVariable
        : DINode
    {
        /// <summary>Gets the line for the variable</summary>
        public UInt32 Line => LLVMDIVariableGetLine( Handle );

        /// <summary>Gets the Debug information scope for this variable</summary>
        public DIScope? Scope => (DIScope?)LLVMDIVariableGetScope( Handle ).CreateMetadata();

        /// <summary>Gets the Debug information name for this variable</summary>
        public string Name => (Operands[ 1 ] as MDString)?.ToString() ?? string.Empty;

        /// <summary>Gets the Debug information file for this variable</summary>
        public DIFile? File => (DIFile?)LLVMDIVariableGetFile( Handle ).CreateMetadata();

        /// <summary>Gets the Debug information type for this variable</summary>
        public DIType? DIType => GetOperand<DIType>( 3 );

        internal DIVariable( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
