#include "libllvm-c/DIBuilderBindings.h"
#include "llvm/IR/DIBuilder.h"
#include "llvm/IR/DebugLoc.h"
#include "llvm/IR/DebugInfoMetadata.h"
#include <llvm/Support/CBindingWrapping.h>

using namespace llvm;

namespace
{
    template <typename DIT> DIT* unwrap_maybenull( LLVMMetadataRef Ref )
    {
        return (DIT* )( Ref ? unwrap<MDNode>( Ref ) : nullptr );
    }

    //static DINode::DIFlags map_from_llvmDIFlags( LLVMDIFlags Flags )
    //{
    //    return static_cast< DINode::DIFlags >( Flags );
    //}

    //static LLVMDIFlags map_to_llvmDIFlags( DINode::DIFlags Flags )
    //{
    //    return static_cast< LLVMDIFlags >( Flags );
    //}
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

    //LLVMMetadataRef LibLLVMDIBuilderCreateAutoVariable(
    //    LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, const char* Name,
    //    size_t NameLen, LLVMMetadataRef File, unsigned LineNo, LLVMMetadataRef Ty,
    //    LLVMBool AlwaysPreserve, LLVMDIFlags Flags, uint32_t AlignInBits )
    //{
    //    return wrap( unwrap( Builder )->createAutoVariable(
    //        unwrap<DIScope>( Scope ), { Name, NameLen }, unwrap_maybenull<DIFile>( File ),
    //        LineNo, unwrap<DIType>( Ty ), AlwaysPreserve,
    //        map_from_llvmDIFlags( Flags ), AlignInBits ) );
    //}

    //LLVMMetadataRef LibLLVMDIBuilderCreateParameterVariable(
    //    LLVMDIBuilderRef Builder, LLVMMetadataRef Scope, const char* Name,
    //    size_t NameLen, unsigned ArgNo, LLVMMetadataRef File, unsigned LineNo,
    //    LLVMMetadataRef Ty, LLVMBool AlwaysPreserve, LLVMDIFlags Flags )
    //{
    //    return wrap( unwrap( Builder )->createParameterVariable(
    //        unwrap<DIScope>( Scope ), { Name, NameLen }, ArgNo, unwrap_maybenull<DIFile>( File ),
    //        LineNo, unwrap<DIType>( Ty ), AlwaysPreserve,
    //        map_from_llvmDIFlags( Flags ) ) );
    //}
}
