#include <type_traits>
#include <llvm/IR/Module.h>
#include "libllvm-c/ModuleBindings.h"

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( NamedMDNode, LLVMNamedMDNodeRef )

extern "C"
{
    LLVMValueRef LibLLVMGetOrInsertFunction( LLVMModuleRef module, const char* name, LLVMTypeRef functionType )
    {
        auto pModule = unwrap( module );
        auto pSignature = cast< FunctionType >( unwrap( functionType ) );
        return wrap( pModule->getOrInsertFunction( name, pSignature ).getCallee() );
    }

    char const* LibLLVMGetModuleSourceFileName( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        return pModule->getSourceFileName( ).c_str( );
    }

    void LibLLVMSetModuleSourceFileName( LLVMModuleRef module, char const* name )
    {
        auto pModule = unwrap( module );
        pModule->setSourceFileName( name );
    }

    char const* LibLLVMGetModuleName( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        return pModule->getModuleIdentifier( ).c_str( );
    }

    LLVMValueRef LibLLVMGetGlobalAlias( LLVMModuleRef module, char const* name )
    {
        auto pModule = unwrap( module );
        return wrap( pModule->getNamedAlias( name ) );
    }

    LLVMComdatRef LibLLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, char const* name, LLVMComdatSelectionKind kind )
    {
        auto pModule = unwrap( module );
        auto pComdat = pModule->getOrInsertComdat( name );
        pComdat->setSelectionKind( ( Comdat::SelectionKind ) kind );
        return wrap( pComdat );
    }

    void LibLLVMModuleEnumerateComdats( LLVMModuleRef module, LibLLVMComdatIteratorCallback callback )
    {
        auto pModule = unwrap( module );
        for( auto&& entry : pModule->getComdatSymbolTable( ) )
        {
            if( !callback( wrap( &entry.second ) ) )
                break;
        }
    }

    void LibLLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef )
    {
        auto pModule = unwrap( module );
        auto pComdat = unwrap( comdatRef );
        pModule->getComdatSymbolTable( ).erase( pComdat->getName( ) );
    }

    void LibLLVMModuleComdatClear( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        pModule->getComdatSymbolTable( ).clear( );
    }

    char const* LibLLVMComdatGetName( LLVMComdatRef comdatRef )
    {
        Comdat const& comdat = *unwrap( comdatRef );
        return LLVMCreateMessage( comdat.getName( ).str( ).c_str( ) );
    }

    LLVMValueRef LibLLVMModuleGetFirstGlobalAlias( LLVMModuleRef M )
    {
        Module *Mod = unwrap( M );
        Module::alias_iterator I = Mod->alias_begin( );
        if ( I == Mod->alias_end( ) )
            return nullptr;

        return wrap( &*I );
    }

    LLVMValueRef LibLLVMModuleGetNextGlobalAlias( LLVMValueRef valueRef )
    {
        GlobalAlias *pGA = unwrap<GlobalAlias>( valueRef );
        Module::alias_iterator I( pGA );
        if ( ++I == pGA->getParent( )->alias_end( ) )
            return nullptr;

        return wrap( &*I );
    }
}
