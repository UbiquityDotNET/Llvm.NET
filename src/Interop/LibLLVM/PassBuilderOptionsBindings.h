#ifndef _LIBLLVM_PASSBUILDEROPTIONS_BINDINGS_H
#define _LIBLLVM_PASSBUILDEROPTIONS_BINDINGS_H

#include "llvm-c/Transforms/PassBuilder.h"

LLVM_C_EXTERN_C_BEGIN

    LLVMBool LibLLVMPassBuilderOptionsGetVerifyEach(LLVMPassBuilderOptionsRef Options);
    LLVMBool LibLLVMPassBuilderOptionsGetDebugLogging(LLVMPassBuilderOptionsRef Options);
    // result is a simple alias; DO NOT dispose of it in any way.
    char const* LibLLVMPassBuilderOptionsGetAAPipeline(LLVMPassBuilderOptionsRef Options);
    LLVMBool LibLLVMPassBuilderOptionsGetLoopInterleaving(LLVMPassBuilderOptionsRef Options);
    LLVMBool LibLLVMPassBuilderOptionsGetLoopVectorization(LLVMPassBuilderOptionsRef Options);
    LLVMBool LibLLVMPassBuilderOptionsGetSLPVectorization(LLVMPassBuilderOptionsRef Options);
    LLVMBool LibLLVMPassBuilderOptionsGetLoopUnrolling(LLVMPassBuilderOptionsRef Options);
    LLVMBool LibLLVMPassBuilderOptionsGetForgetAllSCEVInLoopUnroll(LLVMPassBuilderOptionsRef Options);
    uint32_t LibLLVMPassBuilderOptionsGetLicmMssaOptCap(LLVMPassBuilderOptionsRef Options);
    uint32_t LibLLVMPassBuilderOptionsGetLicmMssaNoAccForPromotionCap(LLVMPassBuilderOptionsRef Options);
    LLVMBool LibLLVMPassBuilderOptionsGetCallGraphProfile(LLVMPassBuilderOptionsRef Options);
    LLVMBool LibLLVMPassBuilderOptionsGetMergeFunctions(LLVMPassBuilderOptionsRef Options);
    int32_t LibLLVMPassBuilderOptionsGetInlinerThreshold(LLVMPassBuilderOptionsRef Options);

LLVM_C_EXTERN_C_END

#endif

