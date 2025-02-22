// -----------------------------------------------------------------------
// <copyright file="ContextHandles.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

// This file declares the names of all the context handles.
// The ContextHandle attribute is read by the LLvmHandleGenerator to
// generate the source code for the handles from a template.
namespace Ubiquity.NET.Llvm.Interop
{
    [ContextHandle]
    public readonly partial record struct LibLLVMMDOperandRef;

    [ContextHandle]
    public readonly partial record struct LLVMAttributeRef;

    [ContextHandle]
    public readonly partial record struct LLVMBasicBlockRef;

    [ContextHandle]
    public readonly partial record struct LLVMComdatRef;

    [ContextHandle]
    public readonly partial record struct LLVMDbgRecordRef;

    [ContextHandle]
    public readonly partial record struct LLVMDiagnosticInfoRef;

    [ContextHandle]
    public readonly partial record struct LLVMErrorTypeId;

    [ContextHandle]
    public readonly partial record struct LLVMJITEventListenerRef;

    [ContextHandle]
    public readonly partial record struct LLVMMetadataRef;

    [ContextHandle]
    public readonly partial record struct LLVMNamedMDNodeRef;

    [ContextHandle]
    public readonly partial record struct LLVMOrcExecutionSessionRef;

    [ContextHandle]
    public readonly partial record struct LLVMOrcIRTransformLayerRef;

    [ContextHandle]
    public readonly partial record struct LLVMOrcJITDylibRef;

    [ContextHandle]
    public readonly partial record struct LLVMOrcLookupStateRef;

    [ContextHandle]
    public readonly partial record struct LLVMOrcObjectLinkingLayerRef;

    [ContextHandle]
    public readonly partial record struct LLVMOrcObjectTransformLayerRef;

    [ContextHandle]
    public readonly partial record struct LLVMOrcSymbolStringPoolRef;

    [ContextHandle]
    public readonly partial record struct LLVMTargetLibraryInfoRef;

    [ContextHandle]
    public readonly partial record struct LLVMTargetRef;

    [ContextHandle]
    public readonly partial record struct LLVMTypeRef;

    [ContextHandle]
    public readonly partial record struct LLVMUseRef;

    [ContextHandle]
    public readonly partial record struct LLVMValueRef;
}
