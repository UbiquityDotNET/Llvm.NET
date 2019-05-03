#ifndef _MODULE_BINDINGS_H_
#define _MODULE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "llvm-c/Comdat.h"

#ifdef __cplusplus
extern "C" {
#endif
    LLVMValueRef LLVMGetOrInsertFunction( LLVMModuleRef module, const char* name, LLVMTypeRef functionType );
    char const* LLVMGetModuleSourceFileName( LLVMModuleRef module );
    void LLVMSetModuleSourceFileName( LLVMModuleRef module, char const* name );
    char const* LLVMGetModuleName( LLVMModuleRef module );
    LLVMValueRef LLVMGetGlobalAlias( LLVMModuleRef module, char const* name );

    // iterating the Comdats is a tricky prospect with a "C" based projection as
    // the Comdat class doesn't have any sort of "Next" method and the iterator
    // for stringmap isn't something that is easily marshaled in a portable manner.
    // Thus, a callback is used to provide the caller with all the elements without
    // requiring the use of unsafe and difficult to project constructs.
    // if the callback returns false the enumeration stops
    typedef LLVMBool( *LLVMComdatIteratorCallback )( LLVMComdatRef comdatRef );
    void LLVMModuleEnumerateComdats( LLVMModuleRef module, LLVMComdatIteratorCallback callback );
    LLVMComdatRef LLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, char const* name, LLVMComdatSelectionKind kind );
    void LLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef );
    void LLVMModuleComdatClear( LLVMModuleRef module );

    // caller must free returned string via LLVMDisposeMessage()
    char const* LLVMComdatGetName( LLVMComdatRef comdatRef );

    // Alias enumeration
    LLVMValueRef LLVMModuleGetFirstGlobalAlias( LLVMModuleRef M );
    LLVMValueRef LLVMModuleGetNextGlobalAlias( LLVMValueRef /*GlobalAlias*/ valueRef );
#ifdef __cplusplus
}
#endif

#endif
