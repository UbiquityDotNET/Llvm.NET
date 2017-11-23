#ifndef _VALUE_BINDINGS_H_
#define _VALUE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "DIBuilderBindings.h"
#include "ModuleBindings.h"

#ifdef __cplusplus
extern "C" {
#endif

    LLVMBool LLVMIsConstantZeroValue( LLVMValueRef valueRef );
    void LLVMRemoveGlobalFromParent( LLVMValueRef valueRef );

    LLVMValueRef LLVMBuildIntCast2( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LLVMBool isSigned, const char *Name );
    int LLVMGetValueID( LLVMValueRef valueRef);
    LLVMValueRef LLVMGetAliasee( LLVMValueRef Val );
    uint32_t LLVMGetArgumentIndex( LLVMValueRef Val);

    // use: LLVMDisposeMessage() on return
    LLVMComdatRef LLVMGlobalObjectGetComdat( LLVMValueRef Val );
    void LLVMGlobalObjectSetComdat( LLVMValueRef Val, LLVMComdatRef comdatRef );

    void LLVMGlobalVariableAddDebugExpression( LLVMValueRef /*GlobalVariable*/ globalVar, LLVMMetadataRef exp );
    void LLVMFunctionAppendBasicBlock( LLVMValueRef /*Function*/ function, LLVMBasicBlockRef block );
    LLVMValueRef LLVMValueAsMetadataGetValue( LLVMMetadataRef vmd );

#ifdef __cplusplus
}
#endif

#endif
