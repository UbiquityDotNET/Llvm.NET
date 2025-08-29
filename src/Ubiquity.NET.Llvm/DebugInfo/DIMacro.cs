// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Pre-Processor macro</summary>
    public class DIMacro
        : DIMacroNode
    {
        /* TODO: non-operand property
        public uint Line { get; }
        */

        /// <summary>Gets the name of the macro</summary>
        public string Name => GetOperandString( 0 );

        /// <summary>Gets the value of the property</summary>
        public string Value => GetOperandString( 1 );

        internal DIMacro( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
