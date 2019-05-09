#include "libllvm-c/DIBuilderBindings.h"
#include "llvm/IR/DIBuilder.h"

using namespace llvm;

template <typename DIT> DIT* unwrapDI( LLVMMetadataRef Ref )
{
    return (DIT* )( Ref ? unwrap<MDNode>( Ref ) : nullptr );
}

extern "C"
{
    LLVMMetadataRef LLVMDIBuilderCreateEnumeratorValue( LLVMDIBuilderRef Dref, char const* Name, int64_t Val )
    {
        DIBuilder* D = unwrap( Dref );
        DIEnumerator* enumerator = D->createEnumerator( Name, Val );
        return wrap( enumerator );
    }

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
        auto File = unwrapDI<DIFile>( FileRef );

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
