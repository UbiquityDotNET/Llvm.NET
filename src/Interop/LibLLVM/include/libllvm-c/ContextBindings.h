#ifndef _CONTEXT_BINDINGS_H_
#define _CONTEXT_BINDINGS_H_

#include "llvm-c/Core.h"

#ifdef __cplusplus
extern "C" {
#endif

    LLVMBool LibLLVMContextGetIsODRUniquingDebugTypes( LLVMContextRef context );
    void LibLLVMContextSetIsODRUniquingDebugTypes( LLVMContextRef context, LLVMBool state );

#ifdef __cplusplus
}
#endif

#endif
