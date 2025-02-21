#ifndef _VALUE_BINDINGS_H_
#define _VALUE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "ModuleBindings.h"

#ifdef __cplusplus
extern "C" {
#endif

    // ordering matters, all distinct values are generated first, then any derived values (e.g. foo = bar + 1), to ensure the
    // values match expectations of underlying C++ code and don't alter the sequencing as C++ numbers enum values without an
    // initializer as automatic +1 of the previous value, thus sticking the derived values in at arbitrary locations in the
    // declaration order would reset the values.

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

    // Detect if a ConstantDataSequential is a C string (i8 sequence terminated with \0 and no embedded \0)
    LLVMBool LibLLVMIsConstantCString(LLVMValueRef C);

    // Retrieve the number of elements in a ConstantDataSequential
    uint32_t LibLLVMGetConstantDataSequentialElementCount( LLVMValueRef C );

    // retrieve the contents of a ConstantDataSequential
    // NOTE: The returned pointer does not gurantee a terminating '\0' the 'OUT'
    //       Length does contain the valid length of the data.
    const char* LibLLVMGetConstantDataSequentialRawData( LLVMValueRef C, size_t* Length );
#ifdef __cplusplus
}
#endif

#endif
