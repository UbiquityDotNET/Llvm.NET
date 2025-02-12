//===- InstrumentationBindings.h - instrumentation bindings -----*- C++ -*-===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// This file defines C bindings for the Transforms/Instrumentation component.
//
//===----------------------------------------------------------------------===//

#ifndef LLVM_BINDINGS_LLVM_INSTRUMENTATIONBINDINGS_H
#define LLVM_BINDINGS_LLVM_INSTRUMENTATIONBINDINGS_H

#include <llvm-c/Core.h>

#ifdef __cplusplus
extern "C" {
#endif

// FIXME: These bindings shouldn't be language binding-specific and should eventually move to
// a (somewhat) less stable collection of C APIs for use in creating bindings of
// LLVM in other languages.

void LibLLVMAddAddressSanitizerFunctionPass(LLVMPassManagerRef PM);
void LibLLVMAddAddressSanitizerModulePass(LLVMPassManagerRef PM);
void LibLLVMAddThreadSanitizerPass(LLVMPassManagerRef PM);
void LibLLVMAddMemorySanitizerPass(LLVMPassManagerRef PM);
void LibLLVMAddDataFlowSanitizerPass( LLVMPassManagerRef PM, int ABIListFilesNum, const char **ABIListFiles );

//LLVMPassRegistryRef LibLLVMCreatePassRegistry( );
//void LibLLVMPassRegistryDispose( LLVMPassRegistryRef passReg );
//
#ifdef __cplusplus
}
#endif

#endif
