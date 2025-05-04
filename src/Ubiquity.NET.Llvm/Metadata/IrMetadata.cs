// -----------------------------------------------------------------------
// <copyright file="IrMetadata.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Enumeration to define metadata type kind</summary>
    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "It's not a flags enum, get over it..." )]
    public enum MetadataKind
    {
        /// <summary>IrMetadata string</summary>
        MDString = LibLLVMMetadataKind.LibLLVMMetadataKind_MDString,

        /// <summary>Constant Value as metadata</summary>
        ConstantAsMetadata = LibLLVMMetadataKind.LibLLVMMetadataKind_ConstantAsMetadata,

        /// <summary>Local value as metadata</summary>
        LocalAsMetadata = LibLLVMMetadataKind.LibLLVMMetadataKind_LocalAsMetadata,

        /// <summary>Distinct metadata place holder</summary>
        DistinctMDOperandPlaceholder = LibLLVMMetadataKind.LibLLVMMetadataKind_DistinctMDOperandPlaceholder,

        /// <summary>IrMetadata tuple</summary>
        MDTuple = LibLLVMMetadataKind.LibLLVMMetadataKind_MDTuple,

        /// <summary>Debug info location</summary>
        DILocation = LibLLVMMetadataKind.LibLLVMMetadataKind_DILocation,

        /// <summary>Debug info expression</summary>
        DIExpression = LibLLVMMetadataKind.LibLLVMMetadataKind_DIExpression,

        /// <summary>Debug info global variable expression</summary>
        DIGlobalVariableExpression = LibLLVMMetadataKind.LibLLVMMetadataKind_DIGlobalVariableExpression,

        /// <summary>Generic Debug info node</summary>
        GenericDINode = LibLLVMMetadataKind.LibLLVMMetadataKind_GenericDINode,

        /// <summary>Debug info sub range</summary>
        DISubrange = LibLLVMMetadataKind.LibLLVMMetadataKind_DISubrange,

        /// <summary>Debug info enumerator</summary>
        DIEnumerator = LibLLVMMetadataKind.LibLLVMMetadataKind_DIEnumerator,

        /// <summary>Debug info basic type</summary>
        DIBasicType = LibLLVMMetadataKind.LibLLVMMetadataKind_DIBasicType,

        /// <summary>Debug info derived type</summary>
        DIDerivedType = LibLLVMMetadataKind.LibLLVMMetadataKind_DIDerivedType,

        /// <summary>Debug info composite type</summary>
        DICompositeType = LibLLVMMetadataKind.LibLLVMMetadataKind_DICompositeType,

        /// <summary>Debug info subroutine type</summary>
        DISubroutineType = LibLLVMMetadataKind.LibLLVMMetadataKind_DISubroutineType,

        /// <summary>Debug info file reference</summary>
        DIFile = LibLLVMMetadataKind.LibLLVMMetadataKind_DIFile,

        /// <summary>Debug info Compilation Unit</summary>
        DICompileUnit = LibLLVMMetadataKind.LibLLVMMetadataKind_DICompileUnit,

        /// <summary>Debug info sub program</summary>
        DISubprogram = LibLLVMMetadataKind.LibLLVMMetadataKind_DISubprogram,

        /// <summary>Debug info lexical block</summary>
        DILexicalBlock = LibLLVMMetadataKind.LibLLVMMetadataKind_DILexicalBlock,

        /// <summary>Debug info lexical block file</summary>
        DILexicalBlockFile = LibLLVMMetadataKind.LibLLVMMetadataKind_DILexicalBlockFile,

        /// <summary>Debug info namespace</summary>
        DINamespace = LibLLVMMetadataKind.LibLLVMMetadataKind_DINamespace,

        /// <summary>Debug info fro a module</summary>
        DIModule = LibLLVMMetadataKind.LibLLVMMetadataKind_DIModule,

        /// <summary>Debug info for a template type parameter</summary>
        DITemplateTypeParameter = LibLLVMMetadataKind.LibLLVMMetadataKind_DITemplateTypeParameter,

        /// <summary>Debug info for a template value parameter</summary>
        DITemplateValueParameter = LibLLVMMetadataKind.LibLLVMMetadataKind_DITemplateValueParameter,

        /// <summary>Debug info for a global variable</summary>
        DIGlobalVariable = LibLLVMMetadataKind.LibLLVMMetadataKind_DIGlobalVariable,

        /// <summary>Debug info for a local variable</summary>
        DILocalVariable = LibLLVMMetadataKind.LibLLVMMetadataKind_DILocalVariable,

        /// <summary>Debug info for an Objective C style property</summary>
        DIObjCProperty = LibLLVMMetadataKind.LibLLVMMetadataKind_DIObjCProperty,

        /// <summary>Debug info for an imported entity</summary>
        DIImportedEntity = LibLLVMMetadataKind.LibLLVMMetadataKind_DIImportedEntity,

        /// <summary>Debug info for a macro</summary>
        DIMacro = LibLLVMMetadataKind.LibLLVMMetadataKind_DIMacro,

        /// <summary>Debug info for a macro file</summary>
        DIMacroFile = LibLLVMMetadataKind.LibLLVMMetadataKind_DIMacroFile,
    }

    /// <summary>Root of the LLVM IR Metadata hierarchy</summary>
    public abstract class IrMetadata
        : IEquatable<IrMetadata>
    {
        /// <summary>Replace all uses of this descriptor with another</summary>
        /// <param name="other">New descriptor to replace this one with</param>
        public virtual void ReplaceAllUsesWith( IrMetadata other )
        {
            ArgumentNullException.ThrowIfNull( other );

            if( Handle == default )
            {
                throw new InvalidOperationException( Resources.Cannot_Replace_all_uses_of_a_null_descriptor );
            }

            LLVMMetadataReplaceAllUsesWith( Handle, other.Handle );
        }

        /// <summary>Formats the metadata as a string</summary>
        /// <returns>IrMetadata as a string</returns>
        public override string ToString( )
        {
            return Handle.IsNull ? string.Empty : LibLLVMMetadataAsString( Handle );
        }

        /// <inheritdoc/>
        public bool Equals(IrMetadata? other) => other is not null && Handle.Equals(other.Handle);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals( obj as IrMetadata );

        /// <inheritdoc/>
        public override int GetHashCode() => Handle.GetHashCode();

        /// <summary>Gets a value indicating this metadata's kind</summary>
        public MetadataKind Kind => ( MetadataKind )LibLLVMGetMetadataID( Handle );

        /// <summary>Gets the internal native handle</summary>
        protected internal LLVMMetadataRef Handle { get; }

        private protected IrMetadata( LLVMMetadataRef handle )
        {
            Handle = handle;
        }
    }
}
