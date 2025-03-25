#include "libllvm-c/DataLayoutBindings.h"
#include <llvm/IR/DataLayout.h>
#include <llvm/Support/Error.h>

using namespace llvm;

extern "C"
{
    LLVMErrorRef LibLLVMParseDataLayout(char const* layoutString, size_t strLen, /*out*/ LLVMTargetDataRef* outRetVal)
    {
        auto expectedResult = DataLayout::parse(StringRef(layoutString, strLen));
        *outRetVal = expectedResult ? wrap(new DataLayout(*expectedResult)) : nullptr;
        return expectedResult ? 0 : wrap(expectedResult.takeError());
    }

    char const* LibLLVMGetDataLayoutString(LLVMTargetDataRef dataLayout, /*out*/ size_t* outLen)
    {
        std::string const& StringRep = unwrap(dataLayout)->getStringRepresentation();
        *outLen = StringRep.length();
        return StringRep.data();
    }
}
