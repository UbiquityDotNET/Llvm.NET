// <copyright file="DICompileUnit.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug Information Compile Unit, which acts as the containing parent for debug information in a module</summary>
    /// <seealso href="xref:llvm_langref#dicompileunit">LLVM DICompileUnit</seealso>
    public class DICompileUnit
        : DIScope
    {
        /* non-operand properties
        SourceLanguage SourcLanguage {get;}
        bool IsOptimized {get;}
        uint RunTimeVersion {get;}
        ?? EmissionKind { get; }
        */

        /// <summary>Gets the name of the producer of thie unit</summary>
        public string Producer => GetOperand<MDString>( 1 ).ToString( );

        /// <summary>Gets the compliation flags for this unit</summary>
        public string Flags => GetOperand<MDString>( 2 ).ToString( );

        /// <summary>Gets the split debug file name for this unit</summary>
        public string SplitDebugFileName => GetOperand<MDString>( 3 ).ToString( );

        /// <summary>Gets the enum types in this unit</summary>
        public DICompositeTypeArray EnumTypes => new DICompositeTypeArray( GetOperand<MDTuple>(4) );

        /// <summary>Gets the retained types for this unit</summary>
        public DIScopeArray RetainedTypes => new DIScopeArray( GetOperand<MDTuple>( 5 ) );

        /// <summary>Gets the global variable expressions for this unit</summary>
        public DIGlobalVariableExpressionArray GlovalVariables => new DIGlobalVariableExpressionArray( GetOperand<MDTuple>( 6 ) );

        /// <summary>Gets the imported entities for this unit</summary>
        public DIImportedEntityArray ImportedEntities => new DIImportedEntityArray( GetOperand<MDTuple>( 7 ) );

        /// <summary>Gets the macro information for the compile unit</summary>
        public DIMacroNodeArray Macros => new DIMacroNodeArray( GetOperand<MDTuple>( 8 ) );

        /// <summary>Initializes a new instance of the <see cref="DICompileUnit"/> class from a native <see cref="LLVMMetadataRef"/></summary>
        /// <param name="handle">native reference to wrap</param>
        internal DICompileUnit( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
