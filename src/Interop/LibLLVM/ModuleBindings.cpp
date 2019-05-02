#include <type_traits>
#include <llvm/IR/Module.h>
#include "libllvm-c/ModuleBindings.h"
//#include <llvm-c/comdat.h>

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( NamedMDNode, LLVMNamedMDNodeRef )

extern "C"
{
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

    char const* LLVMComdatGetName( LLVMComdatRef comdatRef )
    {
        Comdat const& comdat = *unwrap( comdatRef );
        return LLVMCreateMessage( comdat.getName( ).str( ).c_str( ) );
    }

    LLVMValueRef LLVMModuleGetFirstGlobalAlias( LLVMModuleRef M )
    {
        Module *Mod = unwrap( M );
        Module::alias_iterator I = Mod->alias_begin( );
        if ( I == Mod->alias_end( ) )
            return nullptr;

        return wrap( &*I );
    }

    LLVMValueRef LLVMModuleGetNextGlobalAlias( LLVMValueRef valueRef )
    {
        GlobalAlias *pGA = unwrap<GlobalAlias>( valueRef );
        Module::alias_iterator I( pGA );
        if ( ++I == pGA->getParent( )->alias_end( ) )
            return nullptr;

        return wrap( &*I );
    }
}
