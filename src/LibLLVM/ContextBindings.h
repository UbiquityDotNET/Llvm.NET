#ifndef _CONTEXT_BINDINGS_H_
#define _CONTEXT_BINDINGS_H_

#include "llvm-c/Core.h"

#ifdef __cplusplus
extern "C" {
#endif

    LLVMBool LLVMContextGetIsODRUniquingDebugTypes( LLVMContextRef context );
    void LLVMContextSetIsODRUniquingDebugTypes( LLVMContextRef context, LLVMBool state );

#ifdef __cplusplus
}
#endif

#endif
