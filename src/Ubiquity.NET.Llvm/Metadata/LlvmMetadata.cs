// -----------------------------------------------------------------------
// <copyright file="LlvmMetadata.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Interop;
using Ubiquity.NET.Llvm.Properties;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Enumeration to define metadata type kind</summary>
    [SuppressMessage( "Design", "CA1027:Mark enums with FlagsAttribute", Justification = "It's not a flags enum, get over it..." )]
    public enum MetadataKind
    {
        /// <summary>LlvmMetadata string</summary>
        MDString = LibLLVMMetadataKind.LibLLVMMetadataKind_MDString,

        /// <summary>Constant Value as metadata</summary>
        ConstantAsMetadata = LibLLVMMetadataKind.LibLLVMMetadataKind_ConstantAsMetadata,

        /// <summary>Local value as metadata</summary>
        LocalAsMetadata = LibLLVMMetadataKind.LibLLVMMetadataKind_LocalAsMetadata,

        /// <summary>Distinct metadata place holder</summary>
        DistinctMDOperandPlaceholder = LibLLVMMetadataKind.LibLLVMMetadataKind_DistinctMDOperandPlaceholder,

        /// <summary>LlvmMetadata tuple</summary>
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

    /// <summary>Root of the LLVM LlvmMetadata hierarchy</summary>
    public abstract class LlvmMetadata
    {
        /// <summary>Replace all uses of this descriptor with another</summary>
        /// <param name="other">New descriptor to replace this one with</param>
        public virtual void ReplaceAllUsesWith( LlvmMetadata other )
        {
            ArgumentNullException.ThrowIfNull( other );

            if( MetadataHandle == default )
            {
                throw new InvalidOperationException( Resources.Cannot_Replace_all_uses_of_a_null_descriptor );
            }

            LLVMMetadataReplaceAllUsesWith( MetadataHandle, other.MetadataHandle );
            MetadataHandle = default;
        }

        /// <summary>Formats the metadata as a string</summary>
        /// <returns>LlvmMetadata as a string</returns>
        public override string ToString( )
        {
            return MetadataHandle == default ? string.Empty : LibLLVMMetadataAsString( MetadataHandle ).ToString();
        }

        /// <summary>Gets a value indicating this metadata's kind</summary>
        public MetadataKind Kind => ( MetadataKind )LibLLVMGetMetadataID( MetadataHandle );

        internal LLVMMetadataRef MetadataHandle { get; /*protected*/ set; }

        internal static T? FromHandle<T>( Context context, LLVMMetadataRef handle )
            where T : LlvmMetadata
        {
            return handle == default ? null : ( T )context.GetNodeFor( handle );
        }

        internal class InterningFactory
            : HandleInterningMapWithContext<LLVMMetadataRef, LlvmMetadata>
        {
            internal InterningFactory( Context context )
                : base( context )
            {
            }

            [SuppressMessage( "Maintainability", "CA1502:Avoid excessive complexity", Justification = "This is an internal factory method for mapping to a managed type" )]
            private protected override LlvmMetadata ItemFactory( LLVMMetadataRef handle )
            {
                // use the native kind value to determine the managed type
                // that should wrap this particular handle
                var kind = ( MetadataKind )LibLLVMGetMetadataID( handle );
                return kind switch
                {
                    MetadataKind.MDString => new MDString( handle ),
                    MetadataKind.ConstantAsMetadata => new ConstantAsMetadata( handle ),
                    MetadataKind.LocalAsMetadata => new LocalAsMetadata( handle ),
                    MetadataKind.DistinctMDOperandPlaceholder => throw new NotSupportedException(), // new DistinctMDOperandPlaceHolder( handle ),
                    MetadataKind.MDTuple => new MDTuple( handle ),
                    MetadataKind.DILocation => new DILocation( handle ),
                    MetadataKind.DIExpression => new DIExpression( handle ),
                    MetadataKind.DIGlobalVariableExpression => new DIGlobalVariableExpression( handle ),
                    MetadataKind.GenericDINode => new GenericDINode( handle ),
                    MetadataKind.DISubrange => new DISubRange( handle ),
                    MetadataKind.DIEnumerator => new DIEnumerator( handle ),
                    MetadataKind.DIBasicType => new DIBasicType( handle ),
                    MetadataKind.DIDerivedType => new DIDerivedType( handle ),
                    MetadataKind.DICompositeType => new DICompositeType( handle ),
                    MetadataKind.DISubroutineType => new DISubroutineType( handle ),
                    MetadataKind.DIFile => new DIFile( handle ),
                    MetadataKind.DICompileUnit => new DICompileUnit( handle ),
                    MetadataKind.DISubprogram => new DISubProgram( handle ),
                    MetadataKind.DILexicalBlock => new DILexicalBlock( handle ),
                    MetadataKind.DILexicalBlockFile => new DILexicalBlockFile( handle ),
                    MetadataKind.DINamespace => new DINamespace( handle ),
                    MetadataKind.DIModule => new DIModule( handle ),
                    MetadataKind.DITemplateTypeParameter => new DITemplateTypeParameter( handle ),
                    MetadataKind.DITemplateValueParameter => new DITemplateValueParameter( handle ),
                    MetadataKind.DIGlobalVariable => new DIGlobalVariable( handle ),
                    MetadataKind.DILocalVariable => new DILocalVariable( handle ),
                    MetadataKind.DIObjCProperty => new DIObjCProperty( handle ),
                    MetadataKind.DIImportedEntity => new DIImportedEntity( handle ),
                    MetadataKind.DIMacro => new DIMacro( handle ),
                    MetadataKind.DIMacroFile => new DIMacroFile( handle ),
                    _ => throw new NotSupportedException(),
                };
            }
        }

        private protected LlvmMetadata( LLVMMetadataRef handle )
        {
            MetadataHandle = handle;
        }
    }
}
