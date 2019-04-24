// <copyright file="NamedMDNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Wraps an LLVM NamedMDNode</summary>
    /// <remarks>Despite its name a NamedMDNode is not itself an MDNode. It is owned directly by a
    /// a <see cref="BitcodeModule"/> and contains a list of <see cref="MDNode"/> operands.</remarks>
    public partial class NamedMDNode
    {
        /// <summary>Gets the name of the node</summary>
        public string Name => LLVMNamedMDNodeGetName( NativeHandle );

        /// <summary>Gets the operands for the node</summary>
        public IList<MDNode> Operands { get; }

        /// <summary>Gets the module that owns this node</summary>
        public BitcodeModule ParentModule => BitcodeModule.FromHandle( LLVMNamedMDNodeGetParentModule( NativeHandle ) );

        /// <summary>Erases this node from its parent</summary>
        public void EraseFromParent() => LLVMNamedMDNodeEraseFromParent( NativeHandle );

        internal NamedMDNode( LLVMNamedMDNodeRef nativeNode )
        {
            NativeHandle = nativeNode;
            Operands = new OperandIterator( this );
        }

        private readonly LLVMNamedMDNodeRef NativeHandle;
    }
}
