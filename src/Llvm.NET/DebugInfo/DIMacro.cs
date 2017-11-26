// <copyright file="DIMacro.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Pre-Processor macro</summary>
    public class DIMacro
        : DIMacroNode
    {
        /* TODO: non-operand property
        public uint Line { get; }
        */

        /// <summary>Gets the name of the macro</summary>
        public string Name => GetOperand<MDString>( 0 ).ToString( );

        /// <summary>Gets the value of the property</summary>
        public string Value => GetOperand<MDString>( 1 ).ToString( );

        internal DIMacro( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
