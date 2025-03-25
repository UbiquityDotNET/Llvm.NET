#ifndef LLVM_DATALAYOUT_BINDINGS_H
#define LLVM_DATALAYOUT_BINDINGS_H
#include <llvm-c/Types.h>
#include <llvm-c/Error.h>
#include <llvm-c/Target.h>

LLVM_C_EXTERN_C_BEGIN
    // Safe version of parsing a layout string - returns an error ref if the string is not valid
    // LLVMCreateTargetDataLayout() would just crash/abort the app...
    LLVMErrorRef LibLLVMParseDataLayout(char const* layoutString, size_t strLen, /*out*/ LLVMTargetDataRef* outRetVal);
    char const* LibLLVMGetDataLayoutString(LLVMTargetDataRef dataLayout, /*out*/ size_t* outLen);
LLVM_C_EXTERN_C_END

#endif
