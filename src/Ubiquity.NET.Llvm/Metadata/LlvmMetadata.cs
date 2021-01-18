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
        /// <summary>Metadata string</summary>
        MDString = LibLLVMMetadataKind.LibLLVMMetadataKind_MDString,

        /// <summary>Constant Value as metadata</summary>
        ConstantAsMetadata = LibLLVMMetadataKind.LibLLVMMetadataKind_ConstantAsMetadata,

        /// <summary>Local value as metadata</summary>
        LocalAsMetadata = LibLLVMMetadataKind.LibLLVMMetadataKind_LocalAsMetadata,

        /// <summary>Distinct metadata place holder</summary>
        DistinctMDOperandPlaceholder = LibLLVMMetadataKind.LibLLVMMetadataKind_DistinctMDOperandPlaceholder,

        /// <summary>Metadata tuple</summary>
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

    /// <summary>Root of the LLVM Metadata hierarchy</summary>
    /// <remarks>In LLVM this is just "Metadata" however that name has the potential
    /// to conflict with the .NET runtime namespace of the same name, so the name
    /// is changed in the .NET bindings to avoid the conflict.</remarks>
    public abstract class LlvmMetadata
    {
        /// <summary>Replace all uses of this descriptor with another</summary>
        /// <param name="other">New descriptor to replace this one with</param>
        public virtual void ReplaceAllUsesWith( LlvmMetadata other )
        {
            if( other == null )
            {
                throw new ArgumentNullException( nameof( other ) );
            }

            if( MetadataHandle == default )
            {
                throw new InvalidOperationException( Resources.Cannot_Replace_all_uses_of_a_null_descriptor );
            }

            LLVMMetadataReplaceAllUsesWith( MetadataHandle, other.MetadataHandle );
            MetadataHandle = default;
        }

        /// <summary>Formats the metadata as a string</summary>
        /// <returns>Metadata as a string</returns>
        public override string ToString( )
        {
            return MetadataHandle == default ? string.Empty : LibLLVMMetadataAsString( MetadataHandle );
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
            : HandleInterningMap<LLVMMetadataRef, LlvmMetadata>
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
                switch( kind )
                {
                case MetadataKind.MDString:
                    return new MDString( handle );

                case MetadataKind.ConstantAsMetadata:
                    return new ConstantAsMetadata( handle );

                case MetadataKind.LocalAsMetadata:
                    return new LocalAsMetadata( handle );

                case MetadataKind.DistinctMDOperandPlaceholder:
                    throw new NotSupportedException( ); // return new DistinctMDOperandPlaceHodler( handle );

                case MetadataKind.MDTuple:
                    return new MDTuple( handle );

                case MetadataKind.DILocation:
                    return new DILocation( handle );

                case MetadataKind.DIExpression:
                    return new DIExpression( handle );

                case MetadataKind.DIGlobalVariableExpression:
                    return new DIGlobalVariableExpression( handle );

                case MetadataKind.GenericDINode:
                    return new GenericDINode( handle );

                case MetadataKind.DISubrange:
                    return new DISubRange( handle );

                case MetadataKind.DIEnumerator:
                    return new DIEnumerator( handle );

                case MetadataKind.DIBasicType:
                    return new DIBasicType( handle );

                case MetadataKind.DIDerivedType:
                    return new DIDerivedType( handle );

                case MetadataKind.DICompositeType:
                    return new DICompositeType( handle );

                case MetadataKind.DISubroutineType:
                    return new DISubroutineType( handle );

                case MetadataKind.DIFile:
                    return new DIFile( handle );

                case MetadataKind.DICompileUnit:
                    return new DICompileUnit( handle );

                case MetadataKind.DISubprogram:
                    return new DISubProgram( handle );

                case MetadataKind.DILexicalBlock:
                    return new DILexicalBlock( handle );

                case MetadataKind.DILexicalBlockFile:
                    return new DILexicalBlockFile( handle );

                case MetadataKind.DINamespace:
                    return new DINamespace( handle );

                case MetadataKind.DIModule:
                    return new DIModule( handle );

                case MetadataKind.DITemplateTypeParameter:
                    return new DITemplateTypeParameter( handle );

                case MetadataKind.DITemplateValueParameter:
                    return new DITemplateValueParameter( handle );

                case MetadataKind.DIGlobalVariable:
                    return new DIGlobalVariable( handle );

                case MetadataKind.DILocalVariable:
                    return new DILocalVariable( handle );

                case MetadataKind.DIObjCProperty:
                    return new DIObjCProperty( handle );

                case MetadataKind.DIImportedEntity:
                    return new DIImportedEntity( handle );

                case MetadataKind.DIMacro:
                    return new DIMacro( handle );

                case MetadataKind.DIMacroFile:
                    return new DIMacroFile( handle );

                default:
#pragma warning disable RECS0083 // Intentional trigger to catch changes in underlying LLVM libs
                    throw new NotImplementedException( );
#pragma warning restore RECS0083
                }
            }
        }

        private protected LlvmMetadata( LLVMMetadataRef handle )
        {
            MetadataHandle = handle;
        }
    }
}
