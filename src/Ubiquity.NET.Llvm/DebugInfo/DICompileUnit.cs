// -----------------------------------------------------------------------
// <copyright file="DICompileUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Defines the amount of debug information to emit</summary>
    public enum DwarfEmissionKind
    {
        /// <summary>No debug information</summary>
        None = 0,

        /// <summary>Full Debug information</summary>
        Full,

        /// <summary>Emit line tables only</summary>
        LineTablesOnly,

        /// <summary>Emit Debug directives only</summary>
        DebugDirectivesOnly,
    }

    /// <summary>Debug Information Compile Unit, which acts as the containing parent for debug information in a module</summary>
    /// <seealso href="xref:llvm_langref#dicompileunit">LLVM DICompileUnit</seealso>
    public class DICompileUnit
        : DIScope
    {
        /* TODO: non-operand properties
        SourceLanguage SourceLanguage {get;}
        bool IsOptimized {get;}
        uint RunTimeVersion {get;}
        ?? EmissionKind { get; }
        */

        /// <summary>Gets the emission kind for this compile unit</summary>
        public DwarfEmissionKind EmissionKind => (DwarfEmissionKind)LibLLVMDiCompileUnitGetEmissionKind(Handle);

        /// <summary>Gets the name of the producer of this unit</summary>
        public LazyEncodedString Producer => GetOperandString( 1 );

        /// <summary>Gets the compilation flags for this unit</summary>
        public LazyEncodedString Flags => GetOperandString( 2 );

        /// <summary>Gets the split debug file name for this unit</summary>
        public LazyEncodedString SplitDebugFileName => GetOperandString( 3 );

        /// <summary>Gets the enum types in this unit</summary>
        public DICompositeTypeArray EnumTypes => new( GetOperand<MDTuple>( 4 ) );

        /// <summary>Gets the retained types for this unit</summary>
        public DIScopeArray RetainedTypes => new( GetOperand<MDTuple>( 5 ) );

        /// <summary>Gets the global variable expressions for this unit</summary>
        public DIGlobalVariableExpressionArray GlobalVariables => new( GetOperand<MDTuple>( 6 ) );

        /// <summary>Gets the imported entities for this unit</summary>
        public DIImportedEntityArray ImportedEntities => new( GetOperand<MDTuple>( 7 ) );

        /// <summary>Gets the macro information for the compile unit</summary>
        public DIMacroNodeArray Macros => new( GetOperand<MDTuple>( 8 ) );

        /// <summary>Initializes a new instance of the <see cref="DICompileUnit"/> class from a native <see cref="LLVMMetadataRef"/></summary>
        /// <param name="handle">native reference to wrap</param>
        internal DICompileUnit( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
