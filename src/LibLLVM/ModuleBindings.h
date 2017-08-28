#ifndef _MODULE_BINDINGS_H_
#define _MODULE_BINDINGS_H_

#include "llvm-c/Core.h"
#include "IRBindings.h"
#ifdef __cplusplus
#include "llvm\IR\Comdat.h"
#endif

typedef struct OpaqueNameMDNodeRef* LLVMNamedMDNodeRef;
typedef struct OpaqueComdatRef* LLVMComdatRef;

/// See Module::ModFlagBehavior
enum LLVMModFlagBehavior
{
    Error = 1,
    Warning = 2,
    Require = 3,
    Override = 4,
    Append = 5,
    AppendUnique = 6,
    ModFlagBehaviorFirstVal = Error,
    ModFlagBehaviorLastVal = AppendUnique
};

enum LLVMComdatSelectionKind
{
    COMDAT_ANY,
    COMDAT_EXACTMATCH,
    COMDAT_LARGEST,
    COMDAT_NODUPLICATES,
    COMDAT_SAMESIZE
};

#ifdef __cplusplus
extern "C" {
#endif
    void LLVMAddModuleFlag( LLVMModuleRef M
                            , LLVMModFlagBehavior behavior
                            , const char *name
                            , uint32_t value
                            );

    void LLVMAddModuleFlagMetadata( LLVMModuleRef M
                                    , LLVMModFlagBehavior behavior
                                    , const char *name
                                    , LLVMMetadataRef metadataRef
                                    );

    LLVMValueRef LLVMGetOrInsertFunction( LLVMModuleRef module, const char* name, LLVMTypeRef functionType );
    char const* LLVMGetModuleName( LLVMModuleRef module );
    LLVMValueRef LLVMGetGlobalAlias( LLVMModuleRef module, char const* name );

    LLVMNamedMDNodeRef LLVMModuleGetModuleFlagsMetadata( LLVMModuleRef module );
    unsigned LLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode );
    /*MDNode*/ LLVMMetadataRef LLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index );
    LLVMModuleRef LLVMNamedMDNodeGetParentModule( LLVMNamedMDNodeRef namedMDNode );

    // iterating the Comdats is a tricky prospect with a "C" based projection as
    // the Comdat class doesn't have any sort of "Next" method and the iterator
    // for stringmap isn't something that is easily marshaled in a portable manner.
    // Thus, a callback is used to provide the caller with all the elements without
    // requiring the use of unsafe constructs.
    // if the callback returns false the enumeration stops
    typedef LLVMBool( *LLVMComdatIteratorCallback )( LLVMComdatRef comdatRef );
    void LLVMModuleEnumerateComdats( LLVMModuleRef module, LLVMComdatIteratorCallback callback );
    LLVMComdatRef LLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, char const* name, LLVMComdatSelectionKind kind );
    void LLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef );
    void LLVMModuleComdatClear( LLVMModuleRef module );

    // Comdat accessors
    LLVMComdatSelectionKind LLVMComdatGetKind( LLVMComdatRef comdatRef );
    void LLVMComdatSetKind( LLVMComdatRef comdatRef, LLVMComdatSelectionKind kind );
    char const* LLVMComdatGetName( LLVMComdatRef comdatRef );

#ifdef __cplusplus
}

namespace llvm {
    DEFINE_SIMPLE_CONVERSION_FUNCTIONS( Comdat, LLVMComdatRef );
}
#endif

#endif