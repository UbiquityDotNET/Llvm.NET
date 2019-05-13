// -----------------------------------------------------------------------
// <copyright file="DIMacroFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
