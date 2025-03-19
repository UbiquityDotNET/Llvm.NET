// -----------------------------------------------------------------------
// <copyright file="MDNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>LlvmMetadata node for LLVM IR Bitcode modules</summary>
    /// <remarks>
    /// <para>LlvmMetadata nodes may be uniqued, or distinct. Temporary nodes with
    /// support for <see cref="ReplaceAllUsesWith(LlvmMetadata)"/> may be used to
    /// defer uniqueing until the forward references are known.</para>
    /// <para>There is limited support for <see cref="ReplaceAllUsesWith(LlvmMetadata)"/>
    /// at construction. At construction, if any operand is a temporary or otherwise unresolved
    /// uniqued node, the node itself is unresolved. As soon as all operands become resolved
    /// the node will no longer support <see cref="ReplaceAllUsesWith(LlvmMetadata)"/></para>
    /// </remarks>
    public class MDNode
        : LlvmMetadata
    {
        /// <summary>Gets the <see cref="Context"/> this node belongs to</summary>
        public IContext Context
        {
            get
            {
                ThrowIfDeleted( );
                return new ContextAlias(LibLLVMGetNodeContext( Handle ));
            }
        }

        /// <summary>Gets a value indicating whether this node was deleted</summary>
        public bool IsDeleted => Handle == default;

        /// <summary>Gets a value indicating whether this node is a temporary</summary>
        public bool IsTemporary => LibLLVMIsTemporary( Handle );

        /// <summary>Gets a value indicating whether this node is resolved</summary>
        /// <remarks>
        /// <para>If <see cref="IsTemporary"/> is <see langword="true"/>, then this always
        /// returns <see langword="false"/>; if <see cref="IsDistinct"/> is <see langword="true"/>,
        /// this always returns <see langword="true"/>.</para>
        ///
        /// <para>If <see cref="IsUniqued"/> is <see langword="true"/> then this returns <see langword="true"/>
        /// if this node has already dropped RAUW support (because all operands are resolved).</para>
        /// </remarks>
        public bool IsResolved => LibLLVMIsResolved( Handle );

        /// <summary>Gets a value indicating whether this node is uniqued</summary>
        public bool IsUniqued => LibLLVMIsUniqued( Handle );

        /// <summary>Gets a value indicating whether this node is distinct</summary>
        public bool IsDistinct => LibLLVMIsDistinct( Handle );

        /// <summary>Gets the operands for this node, if any</summary>
        public MetadataOperandCollection Operands { get; }

        /// <summary>Replace all uses of this node with a new node</summary>
        /// <param name="other">Node to replace this one with</param>
        public override void ReplaceAllUsesWith( LlvmMetadata other )
        {
            ArgumentNullException.ThrowIfNull( other );

            if( !IsTemporary || IsResolved )
            {
                throw new InvalidOperationException( Resources.Cannot_replace_non_temporary_or_resolved_MDNode );
            }

            if( Handle == default )
            {
                throw new InvalidOperationException( Resources.Cannot_Replace_all_uses_of_a_null_descriptor );
            }

            LLVMMetadataReplaceAllUsesWith( Handle, other.Handle );
        }

        /// <summary>Gets an operand by index as a specific type</summary>
        /// <typeparam name="T">Type of the operand</typeparam>
        /// <param name="index">Index of the operand</param>
        /// <returns>Operand</returns>
        /// <exception cref="InvalidCastException">When the operand is not castable to <typeparamref name="T"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">When the index is out of range for the operands of this node</exception>
        public T? GetOperand<T>( int index )
            where T : LlvmMetadata
        {
            return Operands.GetOperand<T>( index );
        }

        /// <summary>Gets a string operand by index</summary>
        /// <param name="index">Index of the operand</param>
        /// <returns>String value of the operand</returns>
        public string GetOperandString( int index )
            => GetOperand<MDString>( index )?.ToString( ) ?? string.Empty;

        /* TODO: Consider adding these additional properties/methods
        public bool IsTBAAVTableAccess { get; }

        public TempMDNode Clone() {...}

        public void ReplaceOperandWith(unsigned i, LlvmMetadata other) {...}
        public static MDNode Concat(MDNode a, MDNode b) {...}
        public static MDNode Intersect(MDNode a, MDNode b) {...}
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
            Operands = new MetadataOperandCollection( this );
        }

        private void ThrowIfDeleted( )
        {
            if( IsDeleted )
            {
                throw new InvalidOperationException( "Cannot operate on a deleted node" );
            }
        }
    }
}
