#include "AttributeBindings.h"
#include "llvm\IR\Attributes.h"
#include "llvm\IR\LLVMContext.h"
#include "llvm\IR\Value.h"
#include "llvm\IR\Function.h"
#include "llvm\IR\CallSite.h"
#include "llvm\Support\CBindingWrapping.h"
#include <type_traits>

using namespace llvm;

extern "C"
{
    char const* LLVMAttributeToString( LLVMAttributeRef attribute )
    {
        return LLVMCreateMessage( unwrap( attribute ).getAsString( ).c_str( ) );
    }

}
