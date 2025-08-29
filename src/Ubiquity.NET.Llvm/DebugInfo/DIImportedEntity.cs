// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Debug information from an imported entity</summary>
    /// <seealso href="xref:llvm_langref#diimportedentity">LLVM DIImportedEntity</seealso>
    public class DIImportedEntity
        : DINode
    {
        /*
        uint Line {get;}
        */

        /// <summary>Gets the <see cref="DIScope"/> for the imported entity</summary>
        public DIScope Scope => GetOperand<DIScope>( 0 )!;

        /// <summary>Gets the entity imported</summary>
        public DINode Entity => GetOperand<DINode>( 1 )!;

        /// <summary>Gets the name of the node</summary>
        public string Name => GetOperandString( 2 );

        /// <summary>Gets the <see cref="DIFile"/> for the imported entity</summary>
        public DIFile File => GetOperand<DIFile>( 3 )!;

        internal DIImportedEntity( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
