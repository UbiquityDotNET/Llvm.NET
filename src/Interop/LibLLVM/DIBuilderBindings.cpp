#include "libllvm-c/DIBuilderBindings.h"
#include "llvm/IR/DIBuilder.h"

using namespace llvm;

extern "C"
{
    LLVMMetadataRef LLVMDIBuilderCreateEnumeratorValue( LLVMDIBuilderRef Dref, char const* Name, int64_t Val )
    {
        DIBuilder* D = unwrap( Dref );
        DIEnumerator* enumerator = D->createEnumerator( Name, Val );
        return wrap( enumerator );
    }
}
