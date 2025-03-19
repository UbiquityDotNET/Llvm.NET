#ifndef _MODULE_BINDINGS_H_
#define _MODULE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "llvm-c/Comdat.h"

#ifdef __cplusplus
extern "C" {
#endif
    typedef struct LLVMOpaqueComdatIterator* LibLLVMComdatIteratorRef;

    uint32_t LibLLVMModuleGetNumComdats(LLVMModuleRef module);
    LLVMComdatRef LibLLVMModuleGetComdat(LLVMModuleRef module, char const* name);
    LibLLVMComdatIteratorRef LibLLVMModuleBeginComdats(LLVMModuleRef module);
    LLVMComdatRef LibLLVMCurrentComdat(LibLLVMComdatIteratorRef it);
    LLVMBool LibLLVMMoveNextComdat(LibLLVMComdatIteratorRef it);
    void LibLLVMModuleComdatIteratorReset(LibLLVMComdatIteratorRef it);
    void LibLLVMDisposeComdatIterator(LibLLVMComdatIteratorRef it);

    LLVMValueRef LibLLVMGetOrInsertFunction( LLVMModuleRef module, const char* name, LLVMTypeRef functionType );
    char const* LibLLVMGetModuleSourceFileName( LLVMModuleRef module );
    void LibLLVMSetModuleSourceFileName( LLVMModuleRef module, char const* name );
    char const* LibLLVMGetModuleName( LLVMModuleRef module );
    LLVMValueRef LibLLVMGetGlobalAlias( LLVMModuleRef module, char const* name );

    LLVMComdatRef LibLLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, char const* name, LLVMComdatSelectionKind kind );
    void LibLLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef );
    void LibLLVMModuleComdatClear( LLVMModuleRef module );

    // caller must free returned string via LLVMDisposeMessage()
    char const* LibLLVMComdatGetName( LLVMComdatRef comdatRef );

    // Alias enumeration
    LLVMValueRef LibLLVMModuleGetFirstGlobalAlias( LLVMModuleRef M );
    LLVMValueRef LibLLVMModuleGetNextGlobalAlias( LLVMValueRef /*GlobalAlias*/ valueRef );
#ifdef __cplusplus
}
#endif

#endif
