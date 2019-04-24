// <copyright file="DIMacroFile.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Macro file included by a unit</summary>
    public class DIMacroFile
        : DIMacroNode
    {
        /// <summary>Gets the file information for this macro file</summary>
        public DIFile File => GetOperand<DIFile>( 0 );

        /// <summary>Gets the elements of this macro file</summary>
        public DIMacroNodeArray Elements => new DIMacroNodeArray( GetOperand<MDTuple>( 1 ) );

        internal DIMacroFile( LLVMMetadataRef handle)
            : base( handle )
        {
        }
    }
}
