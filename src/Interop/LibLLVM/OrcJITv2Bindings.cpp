#include "libllvm-c/OrcJITv2Bindings.h"
#include <llvm/Support/Error.h>
#include <llvm/Support/CBindingWrapping.h>
#include <llvm/ExecutionEngine/Orc/Core.h>

using namespace llvm;
using namespace llvm::orc;

namespace
{
    DEFINE_SIMPLE_CONVERSION_FUNCTIONS(ExecutionSession, LLVMOrcExecutionSessionRef)
    DEFINE_SIMPLE_CONVERSION_FUNCTIONS(JITDylib, LLVMOrcJITDylibRef)
}

extern "C"
{
    LLVMErrorRef LibLLVMExecutionSessionRemoveDyLib(LLVMOrcExecutionSessionRef session, LLVMOrcJITDylibRef lib)
    {
        return wrap(unwrap(session)->removeJITDylib(*unwrap(lib)));
    }
}
