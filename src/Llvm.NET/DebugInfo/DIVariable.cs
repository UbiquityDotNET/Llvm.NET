// <copyright file="DIVariable.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    public class DIVariable
        : DINode
    {
        /* TODO: UInt32 Line => NativeMethods.LLVMDIVariableGetLine( MetadataHandle ); */

        public DIScope Scope => Operands[ 0 ]?.Metadata as DIScope;

        public string Name => ( Operands[ 1 ]?.Metadata as MDString )?.ToString( ) ?? string.Empty;

        public DIFile File => Operands[ 2 ]?.Metadata as DIFile;

        public DIType DIType => Operands[ 3 ]?.Metadata as DIType;

        internal DIVariable( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
