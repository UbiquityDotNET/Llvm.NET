// <copyright file="DIImportedEntity.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug information forn an imported entity</summary>
    /// <seealso href="xref:llvm_langref#diimportedentity">LLVM DIImportedEntity</seealso>
    public class DIImportedEntity
        : DINode
    {
        /*
        uint Line {get;}
        */

        /// <summary>Gets the <see cref="DIScope"/> for the imported entity</summary>
        public DIScope Scope => GetOperand<DIScope>( 0 );

        /// <summary>Gets the entity imported</summary>
        public DINode Entity => GetOperand<DINode>( 1 );

        /// <summary>Gets the name of the node</summary>
        public string Name => GetOperandString( 2 );

        /// <summary>Gets the <see cref="DIFile"/> for the imported entity</summary>
        public DIFile File => GetOperand<DIFile>( 3 );

        internal DIImportedEntity( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
