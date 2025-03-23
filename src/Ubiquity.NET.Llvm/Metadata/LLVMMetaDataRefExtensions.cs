// -----------------------------------------------------------------------
// <copyright file="LLVMMetaDataRefExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Metadata
{
    internal static class LLVMMetaDataRefExtensions
    {
        [SuppressMessage( "Maintainability", "CA1502:Avoid excessive complexity", Justification = "This is an internal factory method for mapping to a managed type" )]
        internal static IrMetadata? CreateMetadata(this LLVMMetadataRef handle, [CallerArgumentExpression(nameof(handle))] string? exp = null)
        {
            if( handle.IsNull)
            {
                return null;
            }

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
}
