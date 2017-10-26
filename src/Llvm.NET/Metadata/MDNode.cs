// <copyright file="MDNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>Metadata node for LLVM IR Bitcode modules</summary>
    /// <remarks>
    /// <para>Metadata nodes may be uniqued, or distinct. Temporary nodes with
    /// support for <see cref="ReplaceAllUsesWith(LlvmMetadata)"/> may be used to
    /// defer uniqueing until the forward references are known.</para>
    /// <para>There is limited support for <see cref="ReplaceAllUsesWith(LlvmMetadata)"/>
    /// at construction. At construction, if any operand is a temporary or otherwise unresolved
    /// uniqued node, the node itself is unresolved. As soon as all operands become resolved
    /// the node will no longer support <see cref="ReplaceAllUsesWith(LlvmMetadata)"/></para>
    /// <para>If an unresolved node is part of a cycle, then <see cref="ResolveCycles"/> must
    /// be called on some member of the cycle once all temporary nodes have been replaced.</para>
    /// </remarks>
    public class MDNode
        : LlvmMetadata
    {
        public Context Context => MetadataHandle.GetContextFor( );

        public bool IsDeleted => MetadataHandle == LLVMMetadataRef.Zero;

        public bool IsTemporary => NativeMethods.LLVMIsTemporary( MetadataHandle );

        public bool IsResolved => NativeMethods.LLVMIsResolved( MetadataHandle );

        public bool IsUniqued => NativeMethods.LLVMIsUniqued( MetadataHandle );

        public bool IsDistinct => NativeMethods.LLVMIsDistinct( MetadataHandle );

        public IReadOnlyList<MDOperand> Operands { get; }

        public void ResolveCycles( ) => NativeMethods.LLVMMDNodeResolveCycles( MetadataHandle );

        public override void ReplaceAllUsesWith( LlvmMetadata other )
        {
            if( other == null )
            {
                throw new ArgumentNullException( nameof( other ) );
            }

            if( !IsTemporary || IsResolved )
            {
                throw new InvalidOperationException( "Cannot replace non temporary or resolved MDNode" );
            }

            if( MetadataHandle.Handle == IntPtr.Zero )
            {
                throw new InvalidOperationException( "Cannot Replace all uses of a null descriptor" );
            }

            NativeMethods.LLVMMDNodeReplaceAllUsesWith( MetadataHandle, other.MetadataHandle );

            // remove current node mapping from the context.
            // It won't be valid for use after clearing the handle
            Context.RemoveDeletedNode( this );
            MetadataHandle = LLVMMetadataRef.Zero;
        }

        /* TODO:
        public bool IsTBAAVTableAccess { get; }

        public TempMDNode Clone() {...}

        public void ReplaceOperandWith(unsigned i, LlvmMetadata other) {...}
        public static MDNode Concat(MDNode a, MDNode b) {...}
        public static MDNode Interesect(MDNode a, MDNode b) {...}
        public static MDNode GetMostGenericTBAA(MDNode a, MDNode b) {...}
        public static MDNode GetMostGenericFPMath(MDNode a, MDNode b) {...}
        public static MDNode GetMostGenericRange(MDNode a, MDNode b) {...}
        public static MDNode GetMostGenericAliasScope(MDNode a, MDNode b) {...}
        public static MDNode GetModtGenericAlignmentOrDereferenceable(MDNode a, MDNode b) {...}

        public static T ReplaceWithPermanent<T>() where T:MDNode
        public static T ReplaceWithUniqued<T>() where T:MDNode
        public static T ReplaceWithDistinct<T>() where T:MDNode
        public static DeleteTemporary(MDNode node) {...}
        */

        internal MDNode( LLVMMetadataRef handle )
            : base( handle )
        {
            Operands = new MDNodeOperandList( this );
        }

        internal static T FromHandle<T>( LLVMMetadataRef handle )
        where T : MDNode
        {
            if( handle.Handle.IsNull( ) )
            {
                return null;
            }

            var context = handle.GetContextFor( );
            return FromHandle<T>( context, handle );
        }
    }
}
