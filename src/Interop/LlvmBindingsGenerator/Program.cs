// -----------------------------------------------------------------------
// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CppSharp;
using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    internal static class Program
    {
        public static int Main( string[ ] args )
        {
            if(args.Length < 2 )
            {
                Diagnostics.Error( "USAGE: LlvmBindingsGenerator <llvmRoot> <extensionsRoot> [OutputPath]" );
                return -1;
            }

            string llvmRoot = args[ 0 ];
            string extensionsRoot = args[ 1 ];
            string outputPath = args.Length > 2 ? args[ 2 ] : System.Environment.CurrentDirectory;
            var library = new LibLlvmGeneratorLibrary( CreateConfiguration(), llvmRoot, extensionsRoot, outputPath );
            ConsoleDriver.Run( library );
            return 0;
        }

        // it is hoped that, over time, the generation is flexible enough that LLVM version to version
        // differences are constrained to this configuration only.
        private static GeneratorConfig CreateConfiguration()
        {
            return new GeneratorConfig( )
            {
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
                    "LLVMVerifyFunction",
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
                },
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
                    new ByteArrayMarshalInfo( "LLVMDisasmInstruction", "Bytes" ),
                    new StringMarshalInfo( "LLVMVerifyFunctionEx", "OutMessages", ParamSemantics.Out, StringDisposal.DisposeMessage ),

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
                    new StringMarshalInfo( "LLVMGetBufferStart", StringDisposal.CopyAlias ),
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
                },
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
                IgnoredFunctions = new List<string>
                {
                    "LLVMCreateMessage",
                    "LLVMDisposeMessage",
                    "LLVMDisposeErrorMessage",
                    "LLVMConsumeError",
                    "LLVMConstGEP2",         // declared in LLVM headers but never defined [Go, Figure!]
                    "LLVMConstInBoundsGEP2", // declared in LLVM headers but never defined [Go, Figure!]
                    "LLVMOptRemarkVersion",  // declared in LLVM headers; implemented in tools\opt-remarks, which isn't included in LibLLVM
                    /* LTO/Thin LTO not built into LibLLVM */
                    "llvm_create_optimizer",
                    "llvm_destroy_optimizer",
                    "llvm_optimize_modules",
                    "llvm_read_object_file",
                    "lto_api_version",
                    "lto_codegen_add_module",
                    "lto_codegen_add_must_preserve_symbol",
                    "lto_codegen_compile",
                    "lto_codegen_compile_optimized",
                    "lto_codegen_compile_to_file",
                    "lto_codegen_create",
                    "lto_codegen_create_in_local_context",
                    "lto_codegen_debug_options",
                    "lto_codegen_dispose",
                    "lto_codegen_optimize",
                    "lto_codegen_set_assembler_args",
                    "lto_codegen_set_assembler_path",
                    "lto_codegen_set_cpu",
                    "lto_codegen_set_debug_model",
                    "lto_codegen_set_diagnostic_handler",
                    "lto_codegen_set_module",
                    "lto_codegen_set_pic_model",
                    "lto_codegen_set_should_embed_uselists",
                    "lto_codegen_set_should_internalize",
                    "lto_codegen_write_merged_modules",
                    "lto_get_error_message",
                    "lto_get_version",
                    "lto_initialize_disassembler",
                    "lto_module_create",
                    "lto_module_create_from_fd",
                    "lto_module_create_from_fd_at_offset",
                    "lto_module_create_from_memory",
                    "lto_module_create_from_memory_with_path",
                    "lto_module_create_in_codegen_context",
                    "lto_module_create_in_local_context",
                    "lto_module_dispose",
                    "lto_module_get_linkeropts",
                    "lto_module_get_num_symbols",
                    "lto_module_get_symbol_attribute",
                    "lto_module_get_symbol_name",
                    "lto_module_get_target_triple",
                    "lto_module_has_objc_category",
                    "lto_module_is_object_file",
                    "lto_module_is_object_file_for_target",
                    "lto_module_is_object_file_in_memory",
                    "lto_module_is_object_file_in_memory_for_target",
                    "lto_module_is_thinlto",
                    "lto_module_set_target_triple",
                    "thinlto_codegen_add_cross_referenced_symbol",
                    "thinlto_codegen_add_module",
                    "thinlto_codegen_add_must_preserve_symbol",
                    "thinlto_codegen_disable_codegen",
                    "thinlto_codegen_dispose",
                    "thinlto_codegen_process",
                    "thinlto_codegen_set_cache_dir",
                    "thinlto_codegen_set_cache_entry_expiration",
                    "thinlto_codegen_set_cache_pruning_interval",
                    "thinlto_codegen_set_cache_size_bytes",
                    "thinlto_codegen_set_cache_size_files",
                    "thinlto_codegen_set_cache_size_megabytes",
                    "thinlto_codegen_set_codegen_only",
                    "thinlto_codegen_set_cpu",
                    "thinlto_codegen_set_final_cache_size_relative_to_available_space",
                    "thinlto_codegen_set_pic_model",
                    "thinlto_codegen_set_savetemps_dir",
                    "thinlto_create_codegen",
                    "thinlto_debug_options",
                    "thinlto_module_get_num_object_files",
                    "thinlto_module_get_num_objects",
                    "thinlto_module_get_object",
                    "thinlto_module_get_object_file",
                    "thinlto_set_generated_objects_dir"
                },
                HandleToTemplateMap = new HandleTemplateMap( )
                {
                    new GlobalHandleTemplate( "LLVMMemoryBufferRef","LLVMDisposeMemoryBuffer"),
                    new GlobalHandleTemplate( "LLVMContextRef" , "LLVMContextDispose" ),
                    new ContextHandleTemplate( "LLVMModuleRef" ),
                    new ContextHandleTemplate( "LLVMTypeRef" ),
                    new ContextHandleTemplate( "LLVMValueRef" ),
                    new ContextHandleTemplate( "LLVMBasicBlockRef" ),
                    new ContextHandleTemplate( "LLVMMetadataRef" ),
                    new ContextHandleTemplate( "LLVMNamedMDNodeRef" ),
                    new GlobalHandleTemplate( "LLVMBuilderRef", "LLVMDisposeBuilder" ),
                    new GlobalHandleTemplate( "LLVMDIBuilderRef", "LLVMDisposeBuilder" ),
                    new GlobalHandleTemplate( "LLVMModuleProviderRef", "LLVMDisposeModuleProvider" ),
                    new GlobalHandleTemplate( "LLVMPassManagerRef", "LLVMDisposePassManager" ),
                    new GlobalHandleTemplate( "LLVMPassRegistryRef", "LLVMPassRegistryDispose" ),
                    new ContextHandleTemplate( "LLVMUseRef" ),
                    new ContextHandleTemplate( "LLVMAttributeRef" ),
                    new ContextHandleTemplate( "LLVMDiagnosticInfoRef" ),
                    new ContextHandleTemplate( "LLVMComdatRef" ),
                    new ContextHandleTemplate("LLVMJITEventListenerRef" ),
                    new LLVMErrorRefTemplate( ), // this needs special handling as there are two means of disposal (LLVMConsumeError, and LLVMGetErrorMessage)
                    new GlobalHandleTemplate( "LLVMGenericValueRef", "LLVMDisposeGenericValue" ),
                    new GlobalHandleTemplate( "LLVMExecutionEngineRef", "LLVMDisposeExecutionEngine" ),
                    new GlobalHandleTemplate( "LLVMMCJITMemoryManagerRef", "LLVMDisposeMCJITMemoryManager" ),
                    new GlobalHandleTemplate( "LLVMTargetDataRef", "LLVMDisposeTargetData" ),
                    new ContextHandleTemplate( "LLVMTargetLibraryInfoRef" ),
                    new GlobalHandleTemplate( "LLVMTargetMachineRef", "LLVMDisposeTargetMachine" ),
                    new ContextHandleTemplate( "LLVMTargetRef" ),
                    new ContextHandleTemplate( "LLVMErrorTypeId"),
                    new GlobalHandleTemplate( "llvm_lto_t", "llvm_destroy_optimizer"),
                    new GlobalHandleTemplate( "lto_module_t", "lto_module_dispose" ),
                    new GlobalHandleTemplate( "lto_code_gen_t", "lto_codegen_dispose" ),
                    new GlobalHandleTemplate( "thinlto_code_gen_t", "thinlto_codegen_dispose" ),
                    new GlobalHandleTemplate( "LLVMObjectFileRef", "LLVMDisposeObjectFile"),
                    new GlobalHandleTemplate( "LLVMSectionIteratorRef", "LLVMDisposeSectionIterator"),
                    new GlobalHandleTemplate( "LLVMSymbolIteratorRef", "LLVMDisposeSymbolIterator"),
                    new GlobalHandleTemplate( "LLVMRelocationIteratorRef", "LLVMDisposeRelocationIterator"),
                    new GlobalHandleTemplate( "LLVMOptRemarkParserRef", "LLVMOptRemarkParserDispose"),
                    new GlobalHandleTemplate( "LLVMOrcJITStackRef", "LLVMOrcDisposeInstance"),
                    new GlobalHandleTemplate( "LLVMPassManagerBuilderRef", "LLVMPassManagerBuilderDispose"),
                    new GlobalHandleTemplate( "LLVMValueMetadataEntry", "LLVMDisposeValueMetadataEntries"),
                    new GlobalHandleTemplate( "LLVMModuleFlagEntry", "LLVMDisposeModuleFlagsMetadata"),
                    new GlobalHandleTemplate( "LLVMDisasmContextRef", "LLVMDisasmDispose"),
                    new GlobalHandleTemplate( "LLVMTripleRef", "LLVMDisposeTriple" ),
                    new GlobalHandleTemplate( "LLVMValueCacheRef", "LLVMDisposeValueCache")
                },
                AnonymousEnumNames = new Dictionary<string, string>
                {
                    { "LLVMAttributeReturnIndex", "LLVMAttributeIndex" },
                    { "LLVMMDStringMetadataKind", "LLVMMetadataKind" }
                }
            };
        }
    }
}
