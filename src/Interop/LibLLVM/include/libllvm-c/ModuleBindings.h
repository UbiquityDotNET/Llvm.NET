#ifndef _MODULE_BINDINGS_H_
#define _MODULE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "llvm-c/Comdat.h"

#ifdef __cplusplus
extern "C" {
#endif
    LLVMValueRef LibLLVMGetOrInsertFunction( LLVMModuleRef module, const char* name, LLVMTypeRef functionType );
    char const* LibLLVMGetModuleSourceFileName( LLVMModuleRef module );
    void LibLLVMSetModuleSourceFileName( LLVMModuleRef module, char const* name );
    char const* LibLLVMGetModuleName( LLVMModuleRef module );
    LLVMValueRef LibLLVMGetGlobalAlias( LLVMModuleRef module, char const* name );

    // iterating the Comdats is a tricky prospect with a "C" based projection as
    // the Comdat class doesn't have any sort of "Next" method and the iterator
    // for stringmap isn't something that is easily marshaled in a portable manner.
    // Thus, a callback is used to provide the caller with all the elements without
    // requiring the use of unsafe and difficult to project constructs.
    // if the callback returns false the enumeration stops
    typedef LLVMBool( *LibLLVMComdatIteratorCallback )( void* context, LLVMComdatRef comdatRef );
    void LibLLVMModuleEnumerateComdats( LLVMModuleRef module, void* context, LibLLVMComdatIteratorCallback callback );
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
