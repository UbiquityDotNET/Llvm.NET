// <copyright file="MDNode.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

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
        , IOperandContainer<LlvmMetadata>
    {
        /// <summary>Gets the <see cref="Context"/> this node belongs to</summary>
        public Context Context => MetadataHandle.Context;

        /// <summary>Gets a value indicating whether this node was deleted</summary>
        public bool IsDeleted => MetadataHandle == default;

        /// <summary>Gets a value indicating whether this node is a temprorary</summary>
        public bool IsTemporary => LLVMIsTemporary( MetadataHandle );

        /// <summary>Gets a value indicating whether this node is resolved</summary>
        /// <remarks>
        /// <para>If <see cref="IsTemporary"/> is <see langword="true"/>, then this always
        /// returns <see langword="false"/>; if <see cref="IsDistinct"/> is <see langword="true"/>,
        /// this always returns <see langword="true"/>.</para>
        ///
        /// <para>If <see cref="IsUniqued"/> is <see langword="true"/> then this returns <see langword="true"/>
        /// if this node has already dropped RAUW support (because all operands are resolved).</para>
        ///
        /// <para>As forward declarations are resolved, their containers should get
        /// resolved automatically.  However, if this (or one of its operands) is
        /// involved in a cycle, <see cref="ResolveCycles"/> needs to be called explicitly.</para>
        /// </remarks>
        public bool IsResolved => LLVMIsResolved( MetadataHandle );

        /// <summary>Gets a value indicating whether this node is uniqued</summary>
        public bool IsUniqued => LLVMIsUniqued( MetadataHandle );

        /// <summary>Gets a value indicating whether this node is distinct</summary>
        public bool IsDistinct => LLVMIsDistinct( MetadataHandle );

        /// <summary>Gets the operands for this node, if any</summary>
        public IList<LlvmMetadata> Operands { get; }

        /// <summary>Resolves cycles from this node</summary>
        public void ResolveCycles( ) => LLVMMDNodeResolveCycles( MetadataHandle );

        /// <summary>Replace all uses of this node with a new node</summary>
        /// <param name="other">Node to replace this one with</param>
        public override void ReplaceAllUsesWith( LlvmMetadata other )
        {
            other.ValidateNotNull( nameof( other ) );

            if( !IsTemporary || IsResolved )
            {
                throw new InvalidOperationException( "Cannot replace non temporary or resolved MDNode" );
            }

            if( MetadataHandle == default )
            {
                throw new InvalidOperationException( "Cannot Replace all uses of a null descriptor" );
            }

            LLVMMDNodeReplaceAllUsesWith( MetadataHandle, other.MetadataHandle );

            // remove current node mapping from the context.
            // It won't be valid for use after clearing the handle
            Context.RemoveDeletedNode( this );
            MetadataHandle = default;
        }

        /// <summary>Gets an operand by index as a specific type</summary>
        /// <typeparam name="T">Type of the operand</typeparam>
        /// <param name="index">Index of the operand</param>
        /// <returns>Operand or <see langword="null"/> if the operand isn't casable to <typeparamref name="T"/></returns>
        public T GetOperand<T>( int index )
            where T : LlvmMetadata
        {
            return Operands[ index ] as T;
        }

        /// <summary>Gets a string operand by index</summary>
        /// <param name="index">Index of the operand</param>
        /// <returns>String value of the operand</returns>
        public string GetOperandString( int index )
            => GetOperand<MDString>( index )?.ToString( ) ?? string.Empty;

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

        long IOperandContainer<LlvmMetadata>.Count => LLVMMDNodeGetNumOperands( MetadataHandle );

        LlvmMetadata IOperandContainer<LlvmMetadata>.this[ int index ]
        {
            get => FromHandle<LlvmMetadata>( Context, LLVMGetOperandNode( LLVMMDNodeGetOperand( MetadataHandle, ( uint )index ) ) );
            set => LLVMMDNodeReplaceOperand( MetadataHandle, ( uint )index, value.MetadataHandle );
        }

        internal MDNode( LLVMMetadataRef handle )
            : base( handle )
        {
            Operands = new OperandList<LlvmMetadata>( this );
        }

        internal static T FromHandle<T>( LLVMMetadataRef handle )
        where T : MDNode
        {
            if( handle == default )
            {
                return null;
            }

            var context = handle.Context;
            return FromHandle<T>( context, handle );
        }

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern UInt32 LLVMMDNodeGetNumOperands( LLVMMetadataRef /*MDNode*/ node );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern LLVMMDOperandRef LLVMMDNodeGetOperand( LLVMMetadataRef /*MDNode*/ node, UInt32 index );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl )]
        private static extern void LLVMMDNodeReplaceOperand( LLVMMetadataRef /* MDNode */ node, UInt32 index, LLVMMetadataRef operand );
    }
}
