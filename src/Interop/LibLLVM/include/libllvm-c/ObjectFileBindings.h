#ifndef _LIBLLVM_OBJECTILE_BINDINGS_H_
#define _LIBLLVM_OBJECTILE_BINDINGS_H_

#include "llvm-c/Object.h"

#ifdef __cplusplus
extern "C" {
#endif

    LLVMSymbolIteratorRef LibLLVMSymbolIteratorClone( LLVMSymbolIteratorRef ref );
    LLVMSectionIteratorRef LibLLVMSectionIteratorClone( LLVMSectionIteratorRef ref );
    LLVMRelocationIteratorRef LibLLVMRelocationIteratorClone( LLVMRelocationIteratorRef ref );

#ifdef __cplusplus
}
#endif

#endif
