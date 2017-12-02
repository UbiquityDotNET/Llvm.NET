#include <type_traits>
#include <llvm/IR/Module.h>
#include "ModuleBindings.h"
#include "IRBindings.h"

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( NamedMDNode, LLVMNamedMDNodeRef )

extern "C"
{
    void LLVMAddModuleFlag( LLVMModuleRef M
                            , LLVMModFlagBehavior behavior
                            , const char *name
                            , uint32_t value
                            )
    {
        unwrap( M )->addModuleFlag( (Module::ModFlagBehavior)behavior, name, value );
    }

    void LLVMAddModuleFlagMetadata( LLVMModuleRef M
                                   , LLVMModFlagBehavior behavior
                                   , const char *name
                                   , LLVMMetadataRef metadataRef
                                   )
    {
        unwrap( M )->addModuleFlag( ( Module::ModFlagBehavior )behavior, name, unwrap( metadataRef ) );
    }

    LLVMValueRef LLVMGetOrInsertFunction( LLVMModuleRef module, const char* name, LLVMTypeRef functionType )
    {
        auto pModule = unwrap( module );
        auto pSignature = cast< FunctionType >( unwrap( functionType ) );
        return wrap( pModule->getOrInsertFunction( name, pSignature ) );
    }

    char const* LLVMGetModuleSourceFileName( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        return pModule->getSourceFileName( ).c_str( );
    }

    void LLVMSetModuleSourceFileName( LLVMModuleRef module, char const* name )
    {
        auto pModule = unwrap( module );
        pModule->setSourceFileName( name );
    }

    char const* LLVMGetModuleName( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        return pModule->getModuleIdentifier( ).c_str( );
    }

    LLVMValueRef LLVMGetGlobalAlias( LLVMModuleRef module, char const* name )
    {
        auto pModule = unwrap( module );
        return wrap( pModule->getNamedAlias( name ) );
    }

    LLVMNamedMDNodeRef LLVMModuleGetModuleFlagsMetadata( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        return wrap( pModule->getModuleFlagsMetadata( ) );
    }

    char const* LLVMNamedMDNodeGetName( LLVMNamedMDNodeRef namedMDNode )
    {
        return unwrap( namedMDNode )->getName().data();
    }

    unsigned LLVMNamedMDNodeGetNumOperands( LLVMNamedMDNodeRef namedMDNode )
    {
        auto pMDNode = unwrap( namedMDNode );
        return pMDNode->getNumOperands( );
    }

    LLVMMetadataRef LLVMNamedMDNodeGetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index )
    {
        auto pMDNode = unwrap( namedMDNode );
        if( index >= pMDNode->getNumOperands( ) )
            return nullptr;

        return wrap( pMDNode->getOperand( index ) );
    }

    void LLVMNamedMDNodeSetOperand( LLVMNamedMDNodeRef namedMDNode, unsigned index, LLVMMetadataRef /*MDNode*/ node )
    {
        auto pMDNode = unwrap( namedMDNode );
        if ( index >= pMDNode->getNumOperands( ) )
            return;

        pMDNode->setOperand( index, unwrap<MDNode>( node ) );
    }

    void LLVMNamedMDNodeAddOperand( LLVMNamedMDNodeRef namedMDNode, LLVMMetadataRef /*MDNode*/ node )
    {
        auto pMDNode = unwrap( namedMDNode );
        pMDNode->addOperand( unwrap<MDNode>( node ) );
    }

    LLVMModuleRef LLVMNamedMDNodeGetParentModule( LLVMNamedMDNodeRef namedMDNode )
    {
        auto pMDNode = unwrap( namedMDNode );
        return wrap( pMDNode->getParent( ) );
    }

    void LLVMNamedMDNodeClearOperands( LLVMNamedMDNodeRef namedMDNode )
    {
        unwrap( namedMDNode )->clearOperands( );
    }

    void LLVMNamedMDNodeEraseFromParent( LLVMNamedMDNodeRef namedMDNode )
    {
        unwrap( namedMDNode )->eraseFromParent( );
    }

    LLVMComdatRef LLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, char const* name, LLVMComdatSelectionKind kind )
    {
        auto pModule = unwrap( module );
        auto pComdat = pModule->getOrInsertComdat( name );
        pComdat->setSelectionKind( ( Comdat::SelectionKind ) kind );
        return wrap( pComdat );
    }

    void LLVMModuleEnumerateComdats( LLVMModuleRef module, LLVMComdatIteratorCallback callback )
    {
        auto pModule = unwrap( module );
        for( auto&& entry : pModule->getComdatSymbolTable( ) )
        {
            if( !callback( wrap( &entry.second ) ) )
                break;
        }
    }

    void LLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef )
    {
        auto pModule = unwrap( module );
        auto pComdat = unwrap( comdatRef );
        pModule->getComdatSymbolTable( ).erase( pComdat->getName( ) );
    }

    void LLVMModuleComdatClear( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        pModule->getComdatSymbolTable( ).clear( );
    }

    LLVMComdatSelectionKind LLVMComdatGetKind( LLVMComdatRef comdatRef )
    {
        Comdat const& comdat = *unwrap( comdatRef );
        return ( LLVMComdatSelectionKind )comdat.getSelectionKind( );
    }

    void LLVMComdatSetKind( LLVMComdatRef comdatRef, LLVMComdatSelectionKind kind )
    {
        Comdat& comdat = *unwrap( comdatRef );
        comdat.setSelectionKind( ( Comdat::SelectionKind )kind );
    }

    char const* LLVMComdatGetName( LLVMComdatRef comdatRef )
    {
        Comdat const& comdat = *unwrap( comdatRef );
        return LLVMCreateMessage( comdat.getName( ).str( ).c_str( ) );
    }
}
