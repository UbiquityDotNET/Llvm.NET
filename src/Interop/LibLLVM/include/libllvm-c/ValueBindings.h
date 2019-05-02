#ifndef _VALUE_BINDINGS_H_
#define _VALUE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "ModuleBindings.h"

#ifdef __cplusplus
extern "C" {
#endif

    LLVMBool LLVMIsConstantZeroValue( LLVMValueRef valueRef );
    void LLVMRemoveGlobalFromParent( LLVMValueRef valueRef );

    //LLVMValueRef LLVMBuildIntCast2( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LLVMBool isSigned, const char *Name );
    int LLVMGetValueID( LLVMValueRef valueRef);
    LLVMValueRef LLVMGetAliasee( LLVMValueRef Val );
    uint32_t LLVMGetArgumentIndex( LLVMValueRef Val);

    // use: LLVMDisposeMessage() on return
    LLVMComdatRef LLVMGlobalObjectGetComdat( LLVMValueRef Val );
    void LLVMGlobalObjectSetComdat( LLVMValueRef Val, LLVMComdatRef comdatRef );

    void LLVMGlobalVariableAddDebugExpression( LLVMValueRef /*GlobalVariable*/ globalVar, LLVMMetadataRef exp );
    void LLVMFunctionAppendBasicBlock( LLVMValueRef /*Function*/ function, LLVMBasicBlockRef block );
    LLVMValueRef LLVMValueAsMetadataGetValue( LLVMMetadataRef vmd );

    /*
    ValueCache maps Values to binding provided handles as an intptr_t
    This is used to cache mappings between an LLVMValueRef and a binding
    specific type that wraps the LLVMValueRef. The cache handles
    invalidation with callbacks when Values are RAUW or destroyed.
    */
    typedef struct LLVMOpaqueValueCache* LLVMValueCacheRef;

    // Callback function pointers to allow bindings to invalidate their handles
    // This is optional but may be used by garbage collected runtimes to un-protect
    // the object the handle represents so it is collectible or decrement a ref count
    // for ref-counted types, etc...
    // These call backs *MUST NOT* access the cache itself in any way. The actual
    // update to the cache itself has not yet occurred so the cache won't reflect
    // the end state of the update operation
    typedef void ( *LLVMValueCacheItemDeletedCallback )( LLVMValueRef ref, intptr_t handle );
    typedef intptr_t ( *LLVMValueCacheItemReplacedCallback )( LLVMValueRef oldValue, intptr_t handle, LLVMValueRef newValue );

    LLVMValueCacheRef LLVMCreateValueCache( LLVMValueCacheItemDeletedCallback /*MaybeNull*/ deletedCallback, LLVMValueCacheItemReplacedCallback replacedCallback );
    void LLVMDisposeValueCache( LLVMValueCacheRef cacheRef );
    void LLVMValueCacheAdd( LLVMValueCacheRef cacheRef, LLVMValueRef value, intptr_t handle );
    intptr_t LLVMValueCacheLookup( LLVMValueCacheRef cacheRef, LLVMValueRef valueRef );

#ifdef __cplusplus
}
#endif

#endif
