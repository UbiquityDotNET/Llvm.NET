//===- IRBindings.h - Additional bindings for IR ----------------*- C++ -*-===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// This file defines additional C bindings for the IR component.
//
//===----------------------------------------------------------------------===//

#ifndef LLVM_BINDINGS_LLVM_IRBINDINGS_H
#define LLVM_BINDINGS_LLVM_IRBINDINGS_H

#include <llvm-c/Core.h>
#include <llvm-c/ExecutionEngine.h>

#ifdef __cplusplus
#include <llvm/IR/Metadata.h>
#include <llvm/Support/CBindingWrapping.h>
#endif

#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif

    typedef struct LLVMVersionInfo
    {
        int Major;
        int Minor;
        int Patch;
        char const* VersionString;
    }LLVMVersionInfo;

    void LLVMGetVersionInfo( LLVMVersionInfo* pVersionInfo );

    typedef struct LLVMOpaqueMDOperand* LLVMMDOperandRef;

    LLVMMetadataRef LLVMConstantAsMetadata(LLVMValueRef Val);
    LLVMMetadataRef LLVMMDString2(LLVMContextRef C, const char *Str, unsigned SLen);
    LLVMMetadataRef LLVMMDNode2(LLVMContextRef C, LLVMMetadataRef *MDs, unsigned Count);
    LLVMMetadataRef LLVMTemporaryMDNode(LLVMContextRef C, LLVMMetadataRef *MDs, unsigned Count);

    char const* LLVMGetMDStringText( LLVMMetadataRef mdstring, unsigned* len );

    void LLVMAddNamedMetadataOperand2( LLVMModuleRef M, const char *name, LLVMMetadataRef Val );
    void LLVMSetMetadata2( LLVMValueRef Inst, unsigned KindID, LLVMMetadataRef MD );
    void LLVMMetadataReplaceAllUsesWith( LLVMMetadataRef MD, LLVMMetadataRef New );
    void LLVMSetCurrentDebugLocation2( LLVMBuilderRef Bref, unsigned Line, unsigned Col, LLVMMetadataRef Scope, LLVMMetadataRef InlinedAt );

    LLVMBool LLVMIsTemporary( LLVMMetadataRef M );
    LLVMBool LLVMIsResolved( LLVMMetadataRef M );
    LLVMBool LLVMIsUniqued( LLVMMetadataRef M );
    LLVMBool LLVMIsDistinct( LLVMMetadataRef M );

    void LLVMMDNodeResolveCycles( LLVMMetadataRef M );
    char const* LLVMGetDIFileName( LLVMMetadataRef /*DIFile*/ file );
    char const* LLVMGetDIFileDirectory( LLVMMetadataRef /*DIFile*/ file );

    LLVMMetadataRef LLVMFunctionGetSubprogram( LLVMValueRef function );
    void LLVMFunctionSetSubprogram( LLVMValueRef function, LLVMMetadataRef subprogram );

    LLVMMetadataRef LLVMDIGlobalVarExpGetVariable( LLVMMetadataRef /*DIGlobalVariableExpression*/ metadataHandle );
    void LLVMExecutionEngineClearGlobalMappingsFromModule( LLVMExecutionEngineRef ee, LLVMModuleRef m );
#ifdef __cplusplus
}
#endif

#endif
