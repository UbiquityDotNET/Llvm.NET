#include "libllvm-c/ContextBindings.h"
#include <llvm/IR/LLVMContext.h>

using namespace llvm;

extern "C"
{
    LLVMBool LibLLVMContextGetIsODRUniquingDebugTypes( LLVMContextRef context )
    {
        return unwrap( context )->isODRUniquingDebugTypes( );
    }

    void LibLLVMContextSetIsODRUniquingDebugTypes( LLVMContextRef context, LLVMBool state )
    {
        auto pContext = unwrap( context );
        if ( state )
        {
            pContext->enableDebugTypeODRUniquing( );
        }
        else
        {
            pContext->disableDebugTypeODRUniquing( );
        }
    }
}
