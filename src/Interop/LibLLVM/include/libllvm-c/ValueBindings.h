#ifndef _VALUE_BINDINGS_H_
#define _VALUE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "ModuleBindings.h"

#ifdef __cplusplus
extern "C" {
#endif

    typedef enum LibLLVMValueKind
    {
#define HANDLE_VALUE(Name) Name##Kind,
#define HANDLE_MEMORY_VALUE(Name) Name##Kind,
#define HANDLE_INSTRUCTION(Name) Name##Kind,
#include "llvm/IR/Value.def"
#undef HANDLE_VALUE
#undef HANDLE_MEMORY_VALUE
#undef HANDLE_INSTRUCTION

#define HANDLE_INST(N, OPC, CLASS) OPC##Kind = Instruction##Kind + N,
#define HANDLE_USER_INST(N, OPC, CLASS) OPC##Kind = Instruction##Kind + N,
#include "llvm/IR/Instruction.def"
#undef HANDLE_INST
#undef HANDLE_USER_INST

#define HANDLE_CONSTANT_MARKER(MarkerName, ValueName) MarkerName##Kind = ValueName##Kind,
#include "llvm/IR/Value.def"
#undef HANDLE_CONSTANT_MARKER
    }LibLLVMValueKind;

    LLVMBool LibLLVMIsConstantZeroValue( LLVMValueRef valueRef );
    void LibLLVMRemoveGlobalFromParent( LLVMValueRef valueRef );

    LibLLVMValueKind LibLLVMGetValueKind( LLVMValueRef valueRef);
    LLVMValueRef LibLLVMGetAliasee( LLVMValueRef Val );
    uint32_t LibLLVMGetArgumentIndex( LLVMValueRef Val);

    LLVMComdatRef LibLLVMGlobalObjectGetComdat( LLVMValueRef Val );
    void LibLLVMGlobalObjectSetComdat( LLVMValueRef Val, LLVMComdatRef comdatRef );

    void LibLLVMGlobalVariableAddDebugExpression( LLVMValueRef /*GlobalVariable*/ globalVar, LLVMMetadataRef exp );
    void LibLLVMFunctionAppendBasicBlock( LLVMValueRef /*Function*/ function, LLVMBasicBlockRef block );
    LLVMValueRef LibLLVMValueAsMetadataGetValue( LLVMMetadataRef vmd );

    /*
    ValueCache maps Values to binding provided handles as an intptr_t
    This is used to cache mappings between an LLVMValueRef and a binding
    specific type that wraps the LLVMValueRef. The cache handles
    invalidation with callbacks when Values are RAUW or destroyed.
    */
    typedef struct LibLLVMOpaqueValueCache* LibLLVMValueCacheRef;

    // Callback function pointers to allow bindings to invalidate their handles
    // This is optional but may be used by garbage collected runtimes to un-protect
    // the object the handle represents so it is collectible or decrement a ref count
    // for ref-counted types, etc...
    // These call backs *MUST NOT* access the cache itself in any way. The actual
    // update to the cache itself has not yet occurred so the cache won't reflect
    // the end state of the update operation
    typedef void ( *LibLLVMValueCacheItemDeletedCallback )( LLVMValueRef ref, intptr_t handle );
    typedef intptr_t ( *LibLLVMValueCacheItemReplacedCallback )( LLVMValueRef oldValue, intptr_t handle, LLVMValueRef newValue );

    LibLLVMValueCacheRef LibLLVMCreateValueCache( LibLLVMValueCacheItemDeletedCallback /*MaybeNull*/ deletedCallback, LibLLVMValueCacheItemReplacedCallback replacedCallback );
    void LibLLVMDisposeValueCache( LibLLVMValueCacheRef cacheRef );
    void LibLLVMValueCacheAdd( LibLLVMValueCacheRef cacheRef, LLVMValueRef value, intptr_t handle );
    intptr_t LibLLVMValueCacheLookup( LibLLVMValueCacheRef cacheRef, LLVMValueRef valueRef );
#ifdef __cplusplus
}
#endif

#endif
