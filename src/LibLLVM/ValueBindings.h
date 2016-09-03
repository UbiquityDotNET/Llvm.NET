#ifndef _VALUE_BINDINGS_H_
#define _VALUE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "DIBuilderBindings.h"

#ifdef __cplusplus
extern "C" {
#endif

    LLVMBool LLVMIsConstantZeroValue( LLVMValueRef valueRef );
    void LLVMRemoveGlobalFromParent( LLVMValueRef valueRef );
    
    LLVMValueRef LLVMBuildIntCast2( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LLVMBool isSigned, const char *Name );
    int LLVMGetValueID( LLVMValueRef valueRef);
    LLVMValueRef LLVMMetadataAsValue( LLVMContextRef context, LLVMMetadataRef metadataRef );
    LLVMValueRef LLVMGetAliasee( LLVMValueRef Val );
    uint32_t LLVMGetArgumentIndex( LLVMValueRef Val);


#ifdef __cplusplus
}
#endif

#endif