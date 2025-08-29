// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug information for a function signature</summary>
    /// <seealso href="xref:llvm_langref#disubroutinetype"/>
    public class DISubroutineType
        : DIType
    {
        /* TODO: non-operand properties
            CallingConvention CallingConvention {get;}
        */

        /// <summary>Gets the types for the sub routine</summary>
        public DITypeArray TypeArray => new( GetOperand<MDTuple>( 3 ) );

        internal DISubroutineType( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
