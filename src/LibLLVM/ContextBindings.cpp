#include "ContextBindings.h"
#include <llvm\IR\LLVMContext.h>

using namespace llvm;

extern "C"
{
    LLVMBool LLVMContextGetIsODRUniquingDebugTypes( LLVMContextRef context )
    {
        return unwrap( context )->isODRUniquingDebugTypes( );
    }

    void LLVMContextSetIsODRUniquingDebugTypes( LLVMContextRef context, LLVMBool state )
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
