#include "llvm/Target/TargetMachine.h"
#include "llvm/Passes/PassBuilder.h"
#include "llvm-c/TargetMachine.h"
#include "llvm-c/Transforms/PassBuilder.h"
#include "llvm-c/Core.h"

using namespace llvm;

// Lifted from llvm's PassBuilderBindings.cpp as it is not in any headers.
// it is theoretically supposed to be an internal detail but they didn't
// provide ANY getter functions for the properties of the class. Thus, this
// set of extensions is to provide that. The shape of this **MUST** match
// what is in LLVM and can only be verified by inspection. The version checks
// below will enforce that.
namespace llvm
{
    /// Helper struct for holding a set of builder options for LLVMRunPasses. This
    /// structure is used to keep LLVMRunPasses backwards compatible with future
    /// versions in case we modify the options the new Pass Manager utilizes.
    class LLVMPassBuilderOptions
    {
    public:
        explicit LLVMPassBuilderOptions(
            bool DebugLogging = false,
            bool VerifyEach = false,
            const char* AAPipeline = nullptr,
            PipelineTuningOptions PTO = PipelineTuningOptions()
            )
            : DebugLogging(DebugLogging),
              VerifyEach(VerifyEach),
              AAPipeline(AAPipeline),
              PTO(PTO)
        {
        }

        bool DebugLogging;
        bool VerifyEach;
        const char* AAPipeline;
        PipelineTuningOptions PTO;
    };
} // namespace llvm

// sanity check to catch any changes in the official LLVM declaration of the class above.
// Since it is NOT declared in a header this **MUST** be validated on any changes.
#if LLVM_VERSION_MAJOR != 20 || LLVM_VERSION_MINOR != 1 || LLVM_VERSION_PATCH != 0
#error "Re-evaluate and match declaration of LLVMPassBuilderOptions; update the version test values above when validated"
#endif

namespace
{
    TargetMachine* unwrap(LLVMTargetMachineRef P)
    {
        return reinterpret_cast<TargetMachine*>(P);
    }

    DEFINE_SIMPLE_CONVERSION_FUNCTIONS(LLVMPassBuilderOptions, LLVMPassBuilderOptionsRef)
}

LLVMBool LibLLVMPassBuilderOptionsGetVerifyEach(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->VerifyEach;
}

LLVMBool LibLLVMPassBuilderOptionsGetDebugLogging(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->DebugLogging;
}

char const* LibLLVMPassBuilderOptionsGetAAPipeline(LLVMPassBuilderOptionsRef Options)
{
    return LLVMCreateMessage(unwrap(Options)->AAPipeline);
}

LLVMBool LibLLVMPassBuilderOptionsGetLoopInterleaving(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.LoopInterleaving;
}

LLVMBool LibLLVMPassBuilderOptionsGetLoopVectorization(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.LoopVectorization;
}

LLVMBool LibLLVMPassBuilderOptionsGetSLPVectorization(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.SLPVectorization;
}

LLVMBool LibLLVMPassBuilderOptionsGetLoopUnrolling(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.LoopUnrolling;
}

LLVMBool LibLLVMPassBuilderOptionsGetForgetAllSCEVInLoopUnroll(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.ForgetAllSCEVInLoopUnroll;
}

uint32_t LibLLVMPassBuilderOptionsGetLicmMssaOptCap(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.LicmMssaOptCap;
}

uint32_t LibLLVMPassBuilderOptionsGetLicmMssaNoAccForPromotionCap(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.LicmMssaNoAccForPromotionCap;
}

LLVMBool LibLLVMPassBuilderOptionsGetCallGraphProfile(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.CallGraphProfile;
}

LLVMBool LibLLVMPassBuilderOptionsGetMergeFunctions(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.MergeFunctions;
}

int32_t LibLLVMPassBuilderOptionsGetInlinerThreshold(LLVMPassBuilderOptionsRef Options)
{
    return unwrap(Options)->PTO.InlinerThreshold;
}
