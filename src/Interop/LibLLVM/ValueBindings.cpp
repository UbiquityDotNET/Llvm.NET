#include "libllvm-c/ValueBindings.h"
#include <llvm/IR/Constant.h>
#include <llvm/IR/Comdat.h>
#include <llvm/IR/Module.h>
#include <llvm/IR/GlobalVariable.h>
#include <llvm/IR/GlobalObject.h>
#include <llvm/IR/GlobalAlias.h>
#include <llvm/IR/IRBuilder.h>
#include <llvm/Support/CBindingWrapping.h>

#include "ValueCache.h"


using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( ValueCache, LibLLVMValueCacheRef );

extern "C"
{
    uint32_t LibLLVMGetArgumentIndex( LLVMValueRef valueRef )
    {
        auto pArgument = unwrap<Argument>( valueRef );
        return pArgument->getArgNo( );
    }

    LLVMBool LibLLVMIsConstantZeroValue( LLVMValueRef valueRef )
    {
        auto pConstant = dyn_cast< Constant >( unwrap( valueRef ) );
        if ( pConstant == nullptr )
            return 0;

        return pConstant->isZeroValue( ) ? 1 : 0;
    }

    void LibLLVMRemoveGlobalFromParent( LLVMValueRef valueRef )
    {
        auto pGlobal = dyn_cast< GlobalVariable >( unwrap( valueRef ) );
        if ( pGlobal == nullptr )
            return;

        pGlobal->removeFromParent( );
    }

    LibLLVMValueKind LibLLVMGetValueKind( LLVMValueRef valueRef )
    {
        return static_cast< LibLLVMValueKind >( unwrap( valueRef )->getValueID( ) );
    }

    LLVMValueRef LibLLVMGetAliasee( LLVMValueRef Val )
    {
        auto pAlias = unwrap<GlobalAlias>( Val );
        return wrap( pAlias->getAliasee( ) );
    }

    void LibLLVMGlobalVariableAddDebugExpression( LLVMValueRef /*GlobalVariable*/ globalVar, LLVMMetadataRef exp )
    {
        auto gv = unwrap<GlobalVariable>( globalVar );
        gv->addDebugInfo( unwrap<DIGlobalVariableExpression>( exp ) );
    }

    void LibLLVMFunctionAppendBasicBlock( LLVMValueRef /*Function*/ function, LLVMBasicBlockRef block )
    {
        auto pFunction = unwrap<Function>( function );
        pFunction->insert(pFunction->end(), unwrap( block ) );
    }

    LLVMValueRef LibLLVMValueAsMetadataGetValue( LLVMMetadataRef vmd )
    {
        return wrap( unwrap<ValueAsMetadata>( vmd )->getValue( ) );
    }

    LibLLVMValueCacheRef LibLLVMCreateValueCache( void* ctx, LibLLVMValueCacheItemDeletedCallback /*MaybeNull*/ deletedCallback, LibLLVMValueCacheItemReplacedCallback replacedCallback )
    {
        return wrap( new ValueCache(ctx, deletedCallback, replacedCallback ) );
    }

    void LibLLVMDisposeValueCache( LibLLVMValueCacheRef cacheRef )
    {
        delete unwrap( cacheRef );
    }

    void LibLLVMValueCacheAdd( LibLLVMValueCacheRef cacheRef, LLVMValueRef valueRef, intptr_t handle )
    {
        auto& cache = *unwrap( cacheRef );
        cache[unwrap( valueRef )] = handle;
    }

    intptr_t LibLLVMValueCacheLookup( LibLLVMValueCacheRef cacheRef, LLVMValueRef valueRef )
    {
        return unwrap( cacheRef )->lookup( unwrap( valueRef ) );
    }

    LLVMBool LibLLVMIsConstantCString( LLVMValueRef C )
    {
        return unwrap<ConstantDataSequential>( C )->isCString( );
    }

    uint32_t LibLLVMGetConstantDataSequentialElementCount( LLVMValueRef C )
    {
        return unwrap<ConstantDataSequential>( C )->getNumElements( );
    }

    const char* LibLLVMGetConstantDataSequentialRawData( LLVMValueRef C, size_t* Length )
    {
        StringRef Str = unwrap<ConstantDataSequential>( C )->getRawDataValues( );
        *Length = Str.size( );
        return Str.data( );
    }
}
