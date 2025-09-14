// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Immutable;

namespace LlvmBindingsGenerator.Configuration
{
    internal class GeneratorConfig
        : IGeneratorConfig
    {
        public ImmutableArray<string> IgnoredHeaders { get; }
            = [
                "llvm-c/lto.h".NormalizePathSep(),
                "llvm-c/Remarks.h".NormalizePathSep(),
            ];

        public ImmutableArray<HandleDetails> HandleMap { get; }
            = [

                // Global/Disposable handles, though if an alias is available THAT is not disposable
                new("LLVMMemoryBufferRef", "LLVMDisposeMemoryBuffer"),
                new("LLVMContextRef", "LLVMContextDispose", alias: true),
                new("LLVMModuleRef", "LLVMDisposeModule", alias: true),
                new("LLVMBuilderRef", "LLVMDisposeBuilder"),
                new("LLVMDIBuilderRef", "LLVMDisposeDIBuilder"),
                new("LLVMModuleProviderRef", "LLVMDisposeModuleProvider"),
                new("LLVMPassManagerRef", "LLVMDisposePassManager"),
                new("LLVMGenericValueRef", "LLVMDisposeGenericValue"),
                new("LLVMExecutionEngineRef", "LLVMDisposeExecutionEngine"),
                new("LLVMMCJITMemoryManagerRef", "LLVMDisposeMCJITMemoryManager"),
                new("LLVMTargetDataRef", "LLVMDisposeTargetData", alias: true),
                new("LLVMObjectFileRef", "LLVMDisposeObjectFile"),
                new("LLVMBinaryRef", "LLVMDisposeBinary"),
                new("LLVMSectionIteratorRef", "LLVMDisposeSectionIterator"),
                new("LLVMSymbolIteratorRef", "LLVMDisposeSymbolIterator"),
                new("LLVMRelocationIteratorRef", "LLVMDisposeRelocationIterator"),

                // NOTE: These aren't really a reference to a single entry, they are an array of pointers to entries where
                // the layout of each entry is opaque. LLVM has no name of the array, it just uses a pointer to the opaque
                // entries as the allocated array. The allocation of the entire array is released via the dispose method.
                // This is confusing but allows the generation of a handle for the entire array with dispose. So that things
                // are consistent.
                new("LLVMValueMetadataEntry", "LLVMDisposeValueMetadataEntries"),
                new("LLVMModuleFlagEntry", "LLVMDisposeModuleFlagsMetadata"),

                new("LLVMDisasmContextRef", "LLVMDisasmDispose"),
                new("LLVMTargetMachineRef",  "LLVMDisposeTargetMachine", alias: true),
                new("LLVMOrcSymbolStringPoolEntryRef", "LLVMOrcReleaseSymbolStringPoolEntry", alias: true),
                new("LLVMOperandBundleRef", "LLVMDisposeOperandBundle"),
                new("LLVMTargetMachineOptionsRef", "LLVMDisposeTargetMachineOptions"),
                new("LLVMOrcLLJITBuilderRef", "LLVMOrcDisposeLLJITBuilder"),
                new("LLVMOrcLLJITRef", "LLVMOrcDisposeLLJIT"),
                new("LLVMOrcMaterializationUnitRef", "LLVMOrcDisposeMaterializationUnit"),
                new("LLVMOrcMaterializationResponsibilityRef", "LLVMOrcDisposeMaterializationResponsibility"),
                new("LLVMOrcResourceTrackerRef", "LLVMOrcReleaseResourceTracker"),
                new("LLVMOrcDefinitionGeneratorRef", "LLVMOrcDisposeDefinitionGenerator"),
                new("LLVMOrcThreadSafeContextRef", "LLVMOrcDisposeThreadSafeContext"),
                new("LLVMOrcThreadSafeModuleRef", "LLVMOrcDisposeThreadSafeModule"),
                new("LLVMOrcJITTargetMachineBuilderRef", "LLVMOrcDisposeJITTargetMachineBuilder"),
                new("LLVMOrcObjectLayerRef", "LLVMOrcDisposeObjectLayer", alias: true),
                new("LLVMOrcIndirectStubsManagerRef", "LLVMOrcDisposeIndirectStubsManager"),
                new("LLVMOrcLazyCallThroughManagerRef", "LLVMOrcDisposeLazyCallThroughManager"),
                new("LLVMOrcDumpObjectsRef", "LLVMOrcDisposeDumpObjects"),
                new("LLVMPassBuilderOptionsRef", "LLVMDisposePassBuilderOptions"),
                new("LibLLVMTripleRef", "LibLLVMDisposeTriple"),
                new("LibLLVMValueCacheRef", "LibLLVMDisposeValueCache"),
                new("LibLLVMComdatIteratorRef", "LibLLVMDisposeComdatIterator"),

                // Context handles, NOT Disposable
                new("LLVMTypeRef"),
                new("LLVMValueRef"),
                new("LLVMBasicBlockRef"),
                new("LLVMMetadataRef"),
                new("LLVMNamedMDNodeRef"),
                new("LLVMUseRef"),
                new("LLVMAttributeRef"),
                new("LLVMDiagnosticInfoRef"),
                new("LLVMComdatRef"),
                new("LLVMJITEventListenerRef"),
                new("LLVMTargetLibraryInfoRef"),
                new("LLVMTargetRef"),
                new("LLVMErrorTypeId"),
                new("LibLLVMMDOperandRef"),
                new("LLVMDbgRecordRef"),
                new("LLVMOrcExecutionSessionRef"),
                new("LLVMOrcSymbolStringPoolRef"),
                new("LLVMOrcJITDylibRef"),
                new("LLVMOrcLookupStateRef"),
                new("LLVMOrcIRTransformLayerRef"),
                new("LLVMOrcObjectTransformLayerRef"),
                new("LLVMOrcObjectLinkingLayerRef"),
            ];
    }
}
