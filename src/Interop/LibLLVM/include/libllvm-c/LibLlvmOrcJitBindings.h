#ifndef _LIBLLVM_ORCJIT_BINDINGS_H_
#define _LIBLLVM_ORCJIT_BINDINGS_H_

#include <llvm-c/Core.h>
#if LEGACY_REPLACE_WITH_CURRENT_SUPPORT_OR_REMOVE
#include "llvm-c/OrcBindings.h"

#ifdef __cplusplus
extern "C" {
#endif

    LLVMErrorRef LibLLVMOrcGetSymbolAddress( LLVMOrcJITStackRef JITStack,
                                             LLVMOrcTargetAddress* RetAddr,
                                             const char* SymbolName,
                                             LLVMBool ExportedSymbolsOnly );

    LLVMErrorRef LibLLVMOrcGetSymbolAddressIn( LLVMOrcJITStackRef JITStack,
                                               LLVMOrcTargetAddress* RetAddr,
                                               LLVMOrcModuleHandle H,
                                               const char* SymbolName,
                                               LLVMBool ExportedSymbolsOnly );

#ifdef __cplusplus
}
#endif
#endif
#endif
