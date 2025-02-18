#include "libllvm-c/DIBuilderBindings.h"
#include "llvm/IR/DIBuilder.h"
#include "llvm/IR/DebugLoc.h"
#include "llvm/IR/DebugInfoMetadata.h"
#include <llvm/Support/CBindingWrapping.h>

using namespace llvm;

namespace
{
    template <typename DIT>
    DIT* unwrap_maybenull( LLVMMetadataRef Ref )
    {
        return (DIT* )( Ref ? unwrap<MDNode>( Ref ) : nullptr );
    }
}

extern "C"
{
    LLVMMetadataRef LibLLVMDIBuilderCreateCompileUnit(
        LLVMDIBuilderRef Builder,
        LibLLVMDwarfSourceLanguage Lang,
        LLVMMetadataRef FileRef,
        const char* Producer,
        size_t ProducerLen,
        LLVMBool isOptimized,
        const char* Flags,
        size_t FlagsLen,
        unsigned RuntimeVer,
        const char* SplitName,
        size_t SplitNameLen,
        LLVMDWARFEmissionKind Kind,
        unsigned DWOId,
        LLVMBool SplitDebugInlining,
        LLVMBool DebugInfoForProfiling )
    {
        auto File = unwrap_maybenull<DIFile>( FileRef );

        return wrap( unwrap( Builder )->createCompileUnit(
            Lang,
            File,
            StringRef( Producer, ProducerLen ),
            isOptimized,
            StringRef( Flags, FlagsLen ),
            RuntimeVer,
            StringRef( SplitName, SplitNameLen ),
            static_cast< DICompileUnit::DebugEmissionKind >( Kind ),
            DWOId,
            SplitDebugInlining, DebugInfoForProfiling )
        );
    }
}
