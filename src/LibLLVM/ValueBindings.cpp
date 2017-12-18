#include "ValueBindings.h"
#include <llvm\IR\Constant.h>
#include <llvm\IR\Comdat.h>
#include <llvm\IR\Module.h>
#include <llvm\IR\GlobalVariable.h>
#include <llvm\IR\GlobalObject.h>
#include <llvm\IR\GlobalAlias.h>
#include <llvm\IR\IRBuilder.h>
#include <llvm\Support\CBindingWrapping.h>

#include "ValueCache.h"


using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( ValueCache, LLVMValueCacheRef );

extern "C"
{
    LLVMComdatRef LLVMGlobalObjectGetComdat( LLVMValueRef Val )
    {
        auto pGlobalObj = dyn_cast< GlobalObject >( unwrap( Val ) );
        return wrap( pGlobalObj->getComdat( ) );
    }

    void LLVMGlobalObjectSetComdat( LLVMValueRef Val, LLVMComdatRef comdatRef )
    {
        auto pGlobalObj = dyn_cast< GlobalObject >( unwrap( Val ) );
        pGlobalObj->setComdat( unwrap( comdatRef ) );
    }

    uint32_t LLVMGetArgumentIndex( LLVMValueRef valueRef )
    {
        auto pArgument = unwrap<Argument>( valueRef );
        return pArgument->getArgNo( );
    }

    LLVMBool LLVMIsConstantZeroValue( LLVMValueRef valueRef )
    {
        auto pConstant = dyn_cast< Constant >( unwrap( valueRef ) );
        if( pConstant == nullptr )
            return 0;

        return pConstant->isZeroValue( ) ? 1 : 0;
    }

    void LLVMRemoveGlobalFromParent( LLVMValueRef valueRef )
    {
        auto pGlobal = dyn_cast< GlobalVariable >( unwrap( valueRef ) );
        if( pGlobal == nullptr )
            return;

        pGlobal->removeFromParent( );
    }

    LLVMValueRef LLVMBuildIntCast2( LLVMBuilderRef B, LLVMValueRef Val, LLVMTypeRef DestTy, LLVMBool isSigned, const char *Name )
    {
        return wrap( unwrap( B )->CreateIntCast( unwrap( Val ), unwrap( DestTy ), isSigned, Name ) );
    }

    int LLVMGetValueID( LLVMValueRef valueRef )
    {
        return unwrap( valueRef )->getValueID( );
    }

    LLVMValueRef LLVMGetAliasee( LLVMValueRef Val )
    {
        auto pAlias = unwrap<GlobalAlias>( Val );
        return wrap( pAlias->getAliasee( ) );
    }

    void LLVMGlobalVariableAddDebugExpression( LLVMValueRef /*GlobalVariable*/ globalVar, LLVMMetadataRef exp )
    {
        auto gv = unwrap<GlobalVariable>( globalVar );
        gv->addDebugInfo( unwrap<DIGlobalVariableExpression>( exp ));
    }

    void LLVMFunctionAppendBasicBlock( LLVMValueRef /*Function*/ function, LLVMBasicBlockRef block )
    {
        unwrap<Function>( function )->getBasicBlockList( ).push_back( unwrap( block ) );
    }

    LLVMValueRef LLVMValueAsMetadataGetValue( LLVMMetadataRef vmd )
    {
        return wrap( unwrap<ValueAsMetadata>( vmd )->getValue( ) );
    }

    LLVMValueCacheRef LLVMCreateValueCache( LLVMValueCacheItemDeletedCallback /*MaybeNull*/ deletedCallback, LLVMValueCacheItemReplacedCallback replacedCallback )
    {
        return wrap( new ValueCache( deletedCallback, replacedCallback ) );
    }

    void LLVMDisposeValueCache( LLVMValueCacheRef cacheRef )
    {
        delete unwrap( cacheRef );
    }

    void LLVMValueCacheAdd( LLVMValueCacheRef cacheRef, LLVMValueRef valueRef, intptr_t handle )
    {
        auto& cache = *unwrap( cacheRef );
        cache[unwrap( valueRef )] = handle;
    }

    intptr_t LLVMValueCacheLookup( LLVMValueCacheRef cacheRef, LLVMValueRef valueRef )
    {
        return unwrap( cacheRef )->lookup( unwrap( valueRef ) );
    }
}
