// <copyright file="Metadata.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET.DebugInfo;
using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>Root of the LLVM Metadata hierarchy</summary>
    /// <remarks>In LLVM this is just "Metadata" however that name has the potential
    /// to conflict with the .NET runtime namespace of the same name, so the name
    /// is changed in the .NET bindings to avoid the conflict.</remarks>
    public abstract class LlvmMetadata
    {
        /*TODO:
        MetadataKind Kind { get; }

        */

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
                throw new InvalidOperationException( "Cannot Replace all uses of a null descriptor" );
            }

            NativeMethods.LLVMMetadataReplaceAllUsesWith( MetadataHandle, other.MetadataHandle );
            MetadataHandle = default;
        }

        /// <inheritdoc/>
        public override string ToString( )
        {
            if( MetadataHandle == default )
            {
                return string.Empty;
            }

            return NativeMethods.LLVMMetadataAsString( MetadataHandle );
        }

        internal LLVMMetadataRef MetadataHandle { get; /*protected*/ set; }

        // ideally this would be protected AND internal but C#
        // doesn't have any syntax to allow such a thing on a constructor
        // so it is internal and internal code should ensure it is
        // only ever used by derived type constructors
        /*protected*/ internal LlvmMetadata( LLVMMetadataRef handle )
        {
            if( handle == default )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            MetadataHandle = handle;
        }

        internal static T FromHandle<T>( Context context, LLVMMetadataRef handle )
            where T : LlvmMetadata
        {
            if( handle == default )
            {
                return null;
            }

            return ( T )context.GetNodeFor( handle, StaticFactory );
        }

        /// <summary>Enumeration to define debug information metadata nodes</summary>
        private enum MetadataKind : uint
        {
            MDString,                     // HANDLE_METADATA_LEAF(MDString)
            // ValueAsMetadata,            // HANDLE_METADATA_BRANCH(ValueAsMetadata)
            ConstantAsMetadata,           // HANDLE_METADATA_LEAF(ConstantAsMetadata)
            LocalAsMetadata,              // HANDLE_METADATA_LEAF(LocalAsMetadata)
            DistinctMDOperandPlaceholder, // HANDLE_METADATA_LEAF(DistinctMDOperandPlaceholder)
            // MDNode,                     // HANDLE_MDNODE_BRANCH(MDNode)
            MDTuple,                      // HANDLE_MDNODE_LEAF_UNIQUABLE(MDTuple)
            DILocation,                   // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DILocation)
            DIExpression,                 // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIExpression)
            DIGlobalVariableExpression,   // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIGlobalVariableExpression)
            // DINode,                     // HANDLE_SPECIALIZED_MDNODE_BRANCH(DINode)
            GenericDINode,                // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(GenericDINode)
            DISubrange,                   // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DISubrange)
            DIEnumerator,                 // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIEnumerator)
            // DIScope,                    // HANDLE_SPECIALIZED_MDNODE_BRANCH(DIScope)
            // DIType,                     // HANDLE_SPECIALIZED_MDNODE_BRANCH(DIType)
            DIBasicType,                  // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIBasicType)
            DIDerivedType,                // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIDerivedType)
            DICompositeType,              // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DICompositeType)
            DISubroutineType,             // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DISubroutineType)
            DIFile,                       // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIFile)
            DICompileUnit,                // HANDLE_SPECIALIZED_MDNODE_LEAF(DICompileUnit)
            // DILocalScope,               // HANDLE_SPECIALIZED_MDNODE_BRANCH(DILocalScope)
            DISubprogram,                 // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DISubprogram)
            // DILexicalBlockBase,         // HANDLE_SPECIALIZED_MDNODE_BRANCH(DILexicalBlockBase)
            DILexicalBlock,               // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DILexicalBlock)
            DILexicalBlockFile,           // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DILexicalBlockFile)
            DINamespace,                  // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DINamespace)
            DIModule,                     // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIModule)
            // DITemplateParameter,        // HANDLE_SPECIALIZED_MDNODE_BRANCH(DITemplateParameter)
            DITemplateTypeParameter,      // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DITemplateTypeParameter)
            DITemplateValueParameter,     // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DITemplateValueParameter)
            // DIVariable,                 // HANDLE_SPECIALIZED_MDNODE_BRANCH(DIVariable)
            DIGlobalVariable,             // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIGlobalVariable)
            DILocalVariable,              // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DILocalVariable)
            DIObjCProperty,               // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIObjCProperty)
            DIImportedEntity,             // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIImportedEntity)
            // DIMacroNode,                // HANDLE_SPECIALIZED_MDNODE_BRANCH(DIMacroNode)
            DIMacro,                      // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIMacro)
            DIMacroFile,                  // HANDLE_SPECIALIZED_MDNODE_LEAF_UNIQUABLE(DIMacroFile)
        }

        [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Static factory method" )]
        [SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Static factory method" )]
        private static LlvmMetadata StaticFactory( LLVMMetadataRef handle )
        {
            // use the native kind value to determine the managed type
            // that should wrap this particular handle
            var kind = ( MetadataKind )NativeMethods.LLVMGetMetadataID( handle );
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
}
