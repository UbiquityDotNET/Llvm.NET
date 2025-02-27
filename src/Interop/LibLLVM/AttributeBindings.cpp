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

    char const* LibLLVMGetEnumAttributeKindName(LLVMAttributeRef attribute)
    {
        auto attr = unwrap(attribute);
        if(!attr.hasKindAsEnum())
        {
            return nullptr;
        }

        StringRef strName = Attribute::getNameFromAttrKind(attr.getKindAsEnum());

        // Make a copy of the StringRef that is compatible with LLVMDisposeMessage
        // While the str() method is there, it creates a COPY of the string to produce
        // an std::string which would then be allocated and copied again to form the
        // result. This skips the intermediate operations and leverages a single
        // allocation and copy for the result.
        auto pRetVal = (char*)malloc(strName.size()+1);
        return pRetVal == nullptr
             ? nullptr
             : strncpy(pRetVal, strName.data(), strName.size());
    }
}
