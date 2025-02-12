#include "libllvm-c/AttributeBindings.h"
#include "llvm/IR/Attributes.h"
#include "llvm/IR/LLVMContext.h"
#include "llvm/IR/Value.h"
#include "llvm/IR/Function.h"
#include "llvm/Support/CBindingWrapping.h"
#include <type_traits>

using namespace llvm;

extern "C"
{
    char const* LibLLVMAttributeToString( LLVMAttributeRef attribute )
    {
        return LLVMCreateMessage( unwrap( attribute ).getAsString( ).c_str( ) );
    }

    LLVMBool LibLLVMIsTypeAttribute( LLVMAttributeRef attribute )
    {
        return unwrap( attribute ).isTypeAttribute( );
    }

    LLVMTypeRef LibLLVMGetAttributeTypeValue( LLVMAttributeRef attribute )
    {
        return wrap(unwrap( attribute ).getValueAsType( ));
    }
}
