// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.InteropServices;
using CppSharp;
using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    internal static class Program
    {
        public static int Main( string[ ] args )
        {
            var diagnostics = new ErrorTrackingDiagnostics( );
            Diagnostics.Implementation = diagnostics;

            if( args.Length < 2 )
            {
                Diagnostics.Error( "USAGE: LlvmBindingsGenerator <llvmRoot> <extensionsRoot> [OutputPath]" );
                return -1;
            }

            string llvmRoot = args[ 0 ];
            string extensionsRoot = args[ 1 ];
            string outputPath = args.Length > 2 ? args[ 2 ] : System.Environment.CurrentDirectory;
            var library = new LibLlvmGeneratorLibrary( CreateConfiguration( ), llvmRoot, extensionsRoot, outputPath );
            Driver.Run( library );
            return diagnostics.ErrorCount;
        }

        // It is hoped that, over time, the generation is flexible enough that LLVM version to version
        // differences are constrained to this configuration only.
        private static GeneratorConfig CreateConfiguration( )
        {
            var config = new GeneratorConfig( )
            {
                /* These functions all return a non-zero value on failure */
                StatusReturningFunctions = new SortedSet<string>( )
                {
                    "LLVMParseIRInContext",
                    "LLVMLinkModules2",
                    "LLVMPrintModuleToFile",
                    "LLVMWriteBitcodeToFile",
                    "LLVMWriteBitcodeToFD",
                    "LLVMWriteBitcodeToFileHandle",
                    "LLVMParseBitcode2",
                    "LLVMGetBitcodeModule2",
                    "LLVMParseBitcodeInContext2",
                    "LLVMGetBitcodeModuleInContext2",
                    "LLVMVerifyModule",
                    "LibLLVMVerifyFunctionEx",
                    "LLVMInitializeNativeTarget",
                    "LLVMInitializeNativeAsmParser",
                    "LLVMInitializeNativeAsmPrinter",
                    "LLVMInitializeNativeDisassembler",
                    "LLVMGetTargetFromTriple",
                    "LLVMTargetMachineEmitToFile",
                    "LLVMTargetMachineEmitToMemoryBuffer",
                    "LLVMCreateExecutionEngineForModule",
                    "LLVMCreateInterpreterForModule",
                    "LLVMCreateJITCompilerForModule",
                    "LLVMCreateMCJITCompilerForModule",
                    "LLVMRemoveModule",
                    "LLVMFindFunction",
                    "LLVMCreateMemoryBufferWithContentsOfFile",
                    "LLVMCreateMemoryBufferWithSTDIN"
                },
                /* Special marshaling information for parameters and return types */
                MarshalingInfo = new List<IMarshalInfo>( )
                {
                    // function parameters
                    new StringMarshalInfo( "LLVMVerifyModule", "OutMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMParseBitcode", "OutMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMParseBitcodeInContext", "OutMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMGetBitcodeModuleInContext", "OutMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMGetBitcodeModule", "OutMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMPrintModuleToFile", "ErrorMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMCreateMemoryBufferWithContentsOfFile", "OutMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMCreateMemoryBufferWithSTDIN", "OutMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMCreateExecutionEngineForModule", "OutError", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMCreateInterpreterForModule", "OutError", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMCreateJITCompilerForModule", "OutError", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMCreateMCJITCompilerForModule", "OutError", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMRemoveModule", "OutError", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMGetTargetFromTriple", "ErrorMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMTargetMachineEmitToFile", "ErrorMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMTargetMachineEmitToMemoryBuffer", "ErrorMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMParseIRInContext", "OutMessage", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMOrcGetMangledSymbol", "MangledSymbol", ParamSemantics.Out, StringDisposal.OrcDisposeMangledSymbol ),
                    new StringMarshalInfo( "LLVMGetInlineAsm", "AsmString", ParamSemantics.In ),
                    new StringMarshalInfo( "LLVMGetInlineAsm", "Constraints", ParamSemantics.In ),
                    new StringMarshalInfo( "LLVMTargetMachineEmitToFile", "Filename", ParamSemantics.In ),
                    new StringMarshalInfo( "LLVMDisasmInstruction", "OutString", ParamSemantics.InOut ),
                    new ArrayMarshalInfo( "LLVMDisasmInstruction", "Bytes" ),
                    new StringMarshalInfo( "LibLLVMVerifyFunctionEx", "OutMessages", ParamSemantics.Out, StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMCopyStringRepOfTargetData", StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LibLLVMMetadataAsString", StringDisposal.DisposeMessage ),
                    new ArrayMarshalInfo( "LLVMDIBuilderCreateEnumerationType", "Elements", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMDIBuilderCreateSubroutineType", "ParameterTypes", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMDIBuilderCreateStructType", "Elements", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMDIBuilderCreateUnionType", "Elements", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMDIBuilderCreateArrayType", "Subscripts", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMDIBuilderCreateVectorType", "Subscripts", UnmanagedType.SysInt ),
                    new PrimitiveTypeMarshalInfo( "LLVMAddInternalizePass", CppSharp.AST.PrimitiveType.Bool, "AllButMain", ParamSemantics.In ),
                    new ArrayMarshalInfo( "LLVMGetCallSiteAttributes", "Attrs", UnmanagedType.SysInt, ParamSemantics.InOut),
                    new ArrayMarshalInfo( "LLVMGetAttributesAtIndex", "Attrs", UnmanagedType.SysInt, ParamSemantics.InOut),
                    new ArrayMarshalInfo( "LLVMAddIncoming", "IncomingValues", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMAddIncoming", "IncomingBlocks", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMGetIntrinsicDeclaration", "ParamTypes", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMIntrinsicGetType", "ParamTypes", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMIntrinsicCopyOverloadedName", "ParamTypes", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMBuildCall2", "Args", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMBuildInBoundsGEP2", "Indices", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMConstGEP", "ConstantIndices", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMBuildGEP2", "Indices", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMDIBuilderCreateExpression", "Addr", UnmanagedType.I8),
                    new ArrayMarshalInfo( "LLVMFunctionType", "ParamTypes", UnmanagedType.SysInt ),
                    new ArrayMarshalInfo( "LLVMConstStructInContext", "ConstantVals", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMConstNamedStruct", "ConstantVals", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMStructTypeInContext", "ElementTypes", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LibLLVMMDNode2", "MDs", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMDIBuilderGetOrCreateTypeArray", "Data", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMDIBuilderGetOrCreateArray", "Data", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMBuildInvoke2", "Args", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMConstInBoundsGEP", "ConstantIndices", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMGetParamTypes", "Dest", UnmanagedType.SysInt, ParamSemantics.InOut),
                    new ArrayMarshalInfo( "LLVMStructSetBody", "ElementTypes", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMGetBasicBlocks", "BasicBlocks", UnmanagedType.SysInt),
                    new ArrayMarshalInfo( "LLVMConstArray", "ConstantVals", UnmanagedType.SysInt),

                    // Function return types
                    new StringMarshalInfo( "LLVMGetDiagInfoDescription", StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMGetStringAttributeKind", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetStringAttributeValue", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetModuleIdentifier", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetSourceFileName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetDataLayoutStr", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetDataLayout", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetTarget", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMModuleFlagEntriesGetKey", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetModuleInlineAsm", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetNamedMetadataName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetDebugLocDirectory", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetDebugLocFilename", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetStructName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetValueName2", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetValueName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetAsString", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetSection", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMIntrinsicGetName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMIntrinsicCopyOverloadedName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetGC", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetMDString", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetBasicBlockName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMDITypeGetName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetTargetName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetTargetDescription", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "lto_get_version", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "lto_get_error_message", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "lto_module_get_target_triple", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "lto_module_get_symbol_name", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "lto_module_get_linkeropts", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "thinlto_module_get_object_file", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetSectionName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetSectionContents", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetSymbolName", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMGetRelocationTypeName", StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMGetRelocationValueString", StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMOptRemarkParserGetErrorMessage", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMOrcGetErrorMsg", StringDisposal.CopyAlias ),
                    new StringMarshalInfo( "LLVMPrintModuleToString", StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMPrintTypeToString", StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMPrintValueToString", StringDisposal.DisposeMessage ),
                    new StringMarshalInfo( "LLVMGetTargetMachineTriple", StringDisposal.DisposeMessage),
                    new StringMarshalInfo( "LLVMGetTargetMachineCPU", StringDisposal.DisposeMessage),
                    new StringMarshalInfo( "LLVMGetTargetMachineFeatureString", StringDisposal.DisposeMessage),
                    new PrimitiveTypeMarshalInfo("LLVMGetBufferStart", CppSharp.AST.PrimitiveType.IntPtr),
                    new StringMarshalInfo("LibLLVMGetMDStringText", StringDisposal.CopyAlias),
                    new StringMarshalInfo("LibLLVMAttributeToString", StringDisposal.DisposeErrorMesage),
                    new StringMarshalInfo("LibLLVMGetModuleSourceFileName", StringDisposal.CopyAlias),
                    new StringMarshalInfo("LibLLVMGetModuleName", StringDisposal.CopyAlias),
                    new StringMarshalInfo("LibLLVMComdatGetName", StringDisposal.DisposeErrorMesage),
                    new StringMarshalInfo("LibLLVMTripleAsString", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LibLLVMTripleGetArchTypeName", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LibLLVMTripleGetSubArchTypeName", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LibLLVMTripleGetVendorTypeName", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LibLLVMTripleGetOsTypeName", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LibLLVMTripleGetEnvironmentTypeName", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LibLLVMTripleGetObjectFormatTypeName", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LLVMNormalizeTargetTriple", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LLVMGetDefaultTargetTriple", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LLVMGetHostCPUName", StringDisposal.DisposeMessage),
                    new StringMarshalInfo("LLVMGetHostCPUFeatures", StringDisposal.DisposeMessage),
                    new PrimitiveTypeMarshalInfo("LLVMHasMetadata", CppSharp.AST.PrimitiveType.Bool),
                },
                /* Functions that are deprecated in LLVM and should be marked obsolte in generation (or by default ommitted completely)*/
                DeprecatedFunctionToMessageMap = new Dictionary<string, string>
                {
                    { "LLVMParseBitCode", "Use LLVMParseBitcod2 instead" },
                    { "LLVMParseBitcodeInContext", "Use LLVMParseBitcodeInContext2 instead" },
                    { "LLVMGetBitcodeModuleInContext", "Use LLVMGetBitcodeModuleInContext2 instead" },
                    { "LVMGetBitcodeModule", "Use LLVMGetBitcodeModule2 instead" },
                    { "LLVMWriteBitcodeToFileHandle", "Use LLVMWriteBitcodeToFD instead" },
                    { "LLVMGetDataLayout", "Use LLVMGetDataLayoutStr instead" },
                    { "LLVMSetModuleInlineAsm", "Use LLVMSetModuleInlineAsm2 instead" },
                    { "LLVMGetValueName", "Use LLVMGetValueName2 instead" },
                    { "LLVMSetValueName", "Use LLVMSetValueName2 instead" },
                    { "LLVMConstInlineAsm", "Use LLVMGetInlineAsm instead" },
                    { "LLVMHasUnnamedAddr", "Use LLVMGetUnnamedAddress instead" },
                    { "LLVMSetUnnamedAddr", "Use LLVMSetUnnamedAddress instead" },
                    { "LLVMBuildInvoke", "Use LLVMBuildInvoke2 instead" },
                    { "LLVMBuildLoad", "Use LLVMBuildLoad2 instead" },
                    { "LLVMBuildGEP", "Use LLVMBuildGEP2 instead" },
                    { "LLVMBuildInBoundsGEP", "Use LLVMBuildInBoundsGEP2 instead" },
                    { "LLVMBuildStructGEP", "Use LLVMBuildStructGEP2 instead" },
                    { "LLVMBuildIntCast", "Use LLVMBuildIntCast2 instead" },
                    { "LLVMBuildCall", "Use LLVMBuildCall2 instead" },
                    { "LLVMCreateFunctionPassManager", "Use LLVMCreateFunctionPassManagerForModule instead" },
                    { "LLVMStartMultithreaded", "This function is deprecated, multi-threading support is a compile-time variable and cannot be changed at run-time" },
                    { "LLVMStopMultithreaded", "This function is deprecated, multi-threading support is a compile-time variable and cannot be changed at run-time" },
                },
                /*Functions that code generation should ignore, but are still exported from LibLLVM*/
                InternalFunctions =
                {
                    /* Disposal methods used and generated in Handle wrappers directly*/
                    { "LLVMDisposeMessage", true },
                    { "LLVMDisposeErrorMessage", true },
                    { "LLVMConsumeError", true },
                    { "LLVMGetErrorMessage", true },
                    { "LLVMCreateMessage", true }, // Not relevant to managed projections
                    { "LLVMOrcDisposeMangledSymbol", true },
                    /* Declared but not present in LibLLVM */
                    { "LLVMConstGEP2", true },         // declared in LLVM headers but never defined [Go, Figure!]
                    { "LLVMConstInBoundsGEP2", true }, // declared in LLVM headers but never defined [Go, Figure!]
                    { "LLVMOptRemarkVersion", true },  // declared in LLVM headers; implemented in tools\opt-remarks, which isn't included in LibLLVM
                    /* LTO/Thin LTO not built into LibLLVM, so these are completely ignored */
                    { "llvm_create_optimizer", true },
                    { "llvm_destroy_optimizer", true },
                    { "llvm_optimize_modules", true },
                    { "llvm_read_object_file", true },
                    { "lto_api_version", true },
                    { "lto_codegen_add_module", true },
                    { "lto_codegen_add_must_preserve_symbol", true },
                    { "lto_codegen_compile", true },
                    { "lto_codegen_compile_optimized", true },
                    { "lto_codegen_compile_to_file", true },
                    { "lto_codegen_create", true },
                    { "lto_codegen_create_in_local_context", true },
                    { "lto_codegen_debug_options", true },
                    { "lto_codegen_dispose", true },
                    { "lto_codegen_optimize", true },
                    { "lto_codegen_set_assembler_args", true },
                    { "lto_codegen_set_assembler_path", true },
                    { "lto_codegen_set_cpu", true },
                    { "lto_codegen_set_debug_model", true },
                    { "lto_codegen_set_diagnostic_handler", true },
                    { "lto_codegen_set_module", true },
                    { "lto_codegen_set_pic_model", true },
                    { "lto_codegen_set_should_embed_uselists", true },
                    { "lto_codegen_set_should_internalize", true },
                    { "lto_codegen_write_merged_modules", true },
                    { "lto_get_error_message", true },
                    { "lto_get_version", true },
                    { "lto_initialize_disassembler", true },
                    { "lto_module_create", true },
                    { "lto_module_create_from_fd", true },
                    { "lto_module_create_from_fd_at_offset", true },
                    { "lto_module_create_from_memory", true },
                    { "lto_module_create_from_memory_with_path", true },
                    { "lto_module_create_in_codegen_context", true },
                    { "lto_module_create_in_local_context", true },
                    { "lto_module_dispose", true },
                    { "lto_module_get_linkeropts", true },
                    { "lto_module_get_num_symbols", true },
                    { "lto_module_get_symbol_attribute", true },
                    { "lto_module_get_symbol_name", true },
                    { "lto_module_get_target_triple", true },
                    { "lto_module_has_objc_category", true },
                    { "lto_module_is_object_file", true },
                    { "lto_module_is_object_file_for_target", true },
                    { "lto_module_is_object_file_in_memory", true },
                    { "lto_module_is_object_file_in_memory_for_target", true },
                    { "lto_module_is_thinlto", true },
                    { "lto_module_set_target_triple", true },
                    { "thinlto_codegen_add_cross_referenced_symbol", true },
                    { "thinlto_codegen_add_module", true },
                    { "thinlto_codegen_add_must_preserve_symbol", true },
                    { "thinlto_codegen_disable_codegen", true },
                    { "thinlto_codegen_dispose", true },
                    { "thinlto_codegen_process", true },
                    { "thinlto_codegen_set_cache_dir", true },
                    { "thinlto_codegen_set_cache_entry_expiration", true },
                    { "thinlto_codegen_set_cache_pruning_interval", true },
                    { "thinlto_codegen_set_cache_size_bytes", true },
                    { "thinlto_codegen_set_cache_size_files", true },
                    { "thinlto_codegen_set_cache_size_megabytes", true },
                    { "thinlto_codegen_set_codegen_only", true },
                    { "thinlto_codegen_set_cpu", true },
                    { "thinlto_codegen_set_final_cache_size_relative_to_available_space", true },
                    { "thinlto_codegen_set_pic_model", true },
                    { "thinlto_codegen_set_savetemps_dir", true },
                    { "thinlto_create_codegen", true },
                    { "thinlto_debug_options", true },
                    { "thinlto_module_get_num_object_files", true },
                    { "thinlto_module_get_num_objects", true },
                    { "thinlto_module_get_object", true },
                    { "thinlto_module_get_object_file", true },
                    { "thinlto_set_generated_objects_dir", true }
                },
                /*Mapping of handle types to the code generation template for the handle*/
                HandleToTemplateMap = new HandleTemplateMap( )
                {
                    new GlobalHandleTemplate( "LLVMMemoryBufferRef","LLVMDisposeMemoryBuffer" ),
                    new GlobalHandleTemplate( "LLVMContextRef" , "LLVMContextDispose", needsAlias: true ),
                    new ContextHandleTemplate( "LLVMModuleRef" ),
                    new ContextHandleTemplate( "LLVMTypeRef" ),
                    new ContextHandleTemplate( "LLVMValueRef" ),
                    new ContextHandleTemplate( "LLVMBasicBlockRef" ),
                    new ContextHandleTemplate( "LLVMMetadataRef" ),
                    new ContextHandleTemplate( "LLVMNamedMDNodeRef" ),
                    new GlobalHandleTemplate( "LLVMBuilderRef", "LLVMDisposeBuilder" ),
                    new GlobalHandleTemplate( "LLVMDIBuilderRef", "LLVMDisposeDIBuilder" ),
                    new GlobalHandleTemplate( "LLVMModuleProviderRef", "LLVMDisposeModuleProvider" ),
                    new GlobalHandleTemplate( "LLVMPassManagerRef", "LLVMDisposePassManager" ),
                    new GlobalHandleTemplate( "LLVMPassRegistryRef", "LLVMPassRegistryDispose" ),
                    new ContextHandleTemplate( "LLVMUseRef" ),
                    new ContextHandleTemplate( "LLVMAttributeRef" ),
                    new ContextHandleTemplate( "LLVMDiagnosticInfoRef" ),
                    new ContextHandleTemplate( "LLVMComdatRef" ),
                    new ContextHandleTemplate( "LLVMJITEventListenerRef" ),
                    new LLVMErrorRefTemplate( ), // this needs special handling as there are two means of disposal (LLVMConsumeError, and LLVMGetErrorMessage)
                    new GlobalHandleTemplate( "LLVMGenericValueRef", "LLVMDisposeGenericValue" ),
                    new GlobalHandleTemplate( "LLVMExecutionEngineRef", "LLVMDisposeExecutionEngine" ),
                    new GlobalHandleTemplate( "LLVMMCJITMemoryManagerRef", "LLVMDisposeMCJITMemoryManager" ),
                    new GlobalHandleTemplate( "LLVMTargetDataRef", "LLVMDisposeTargetData", needsAlias: true ),
                    new ContextHandleTemplate( "LLVMTargetLibraryInfoRef" ),
                    new GlobalHandleTemplate( "LLVMTargetMachineRef", "LLVMDisposeTargetMachine", needsAlias: true ),
                    new ContextHandleTemplate( "LLVMTargetRef" ),
                    new ContextHandleTemplate( "LLVMErrorTypeId" ),
                    new GlobalHandleTemplate( "LLVMObjectFileRef", "LLVMDisposeObjectFile") ,
                    new GlobalHandleTemplate( "LLVMSectionIteratorRef", "LLVMDisposeSectionIterator" ),
                    new GlobalHandleTemplate( "LLVMSymbolIteratorRef", "LLVMDisposeSymbolIterator" ),
                    new GlobalHandleTemplate( "LLVMRelocationIteratorRef", "LLVMDisposeRelocationIterator" ),
                    new GlobalHandleTemplate( "LLVMOptRemarkParserRef", "LLVMOptRemarkParserDispose" ),
                    new GlobalHandleTemplate( "LLVMOrcJITStackRef", "LLVMOrcDisposeInstance" ),
                    new GlobalHandleTemplate( "LLVMPassManagerBuilderRef", "LLVMPassManagerBuilderDispose" ),
                    new GlobalHandleTemplate( "LLVMValueMetadataEntry", "LLVMDisposeValueMetadataEntries" ),
                    new GlobalHandleTemplate( "LLVMModuleFlagEntry", "LLVMDisposeModuleFlagsMetadata" ),
                    new GlobalHandleTemplate( "LLVMDisasmContextRef", "LLVMDisasmDispose" ),
                    new GlobalHandleTemplate( "LibLLVMTripleRef", "LibLLVMDisposeTriple" ),
                    new GlobalHandleTemplate( "LibLLVMValueCacheRef", "LibLLVMDisposeValueCache" ),
                    new ContextHandleTemplate( "LLVMOrcModuleHandle" ),
                    new ContextHandleTemplate( "LibLLVMMDOperandRef" )
                },
                /*Collection of annonymous enum names*/
                AnonymousEnumNames = new Dictionary<string, string>
                {
                    { "LLVMAttributeReturnIndex", "LLVMAttributeIndex" },
                    { "LLVMMDStringMetadataKind", "LLVMMetadataKind" }
                },
                IgnoredHeaders = new SortedSet<string>( )
                {
                    "lto.h",
                    "LinkTimeOptimizer.h",
                    "OptRemarks.h"
                },
                /*
                Set of names of functions that return an alias as the handle, which should not be disposed.
                Basically, these are a weak reference.
                */
                AliasReturningFunctions = new SortedSet<string>( )
                {
                    "LLVMGetTypeContext",
                    "LibLLVMGetNodeContext",
                    "LLVMGetModuleContext",
                    "LLVMGetGlobalContext",
                    "LLVMGetExecutionEngineTargetMachine",
                    "LLVMGetExecutionEngineTargetData",
                }
            };

            // all handle dispose functions are considered internal, but not ignored, so take core of that automatically
            foreach( string disposeFunction in config.HandleToTemplateMap.DisposeFunctionNames )
            {
                config.InternalFunctions.Add( disposeFunction, false );
            }

            return config;
        }
    }
}
