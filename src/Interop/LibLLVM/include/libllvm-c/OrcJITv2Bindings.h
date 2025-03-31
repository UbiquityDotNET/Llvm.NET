#ifndef _LIBLLVM_ORCJITV2_BINDINGS_H_
#define _LIBLLVM_ORCJITV2_BINDINGS_H_

#include "llvm-c/Orc.h"

LLVM_C_EXTERN_C_BEGIN
    LLVMErrorRef LibLLVMExecutionSessionRemoveDyLib(LLVMOrcExecutionSessionRef session, LLVMOrcJITDylibRef lib);
LLVM_C_EXTERN_C_END
#endif
