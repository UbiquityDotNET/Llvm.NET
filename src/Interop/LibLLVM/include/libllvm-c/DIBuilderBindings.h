#ifndef _LIBLLVM_DIBUILDERBINDINGS_H_
#include <stdint.h>
#include "llvm-c/Core.h"
#include "llvm-c/DebugInfo.h"

#ifdef __cplusplus
extern "C" {
#endif

enum LibLLVMDwarfSourceLanguage
{
#define HANDLE_DW_LANG(ID, NAME, LOWER_BOUND, VERSION, VENDOR)                 \
  LibLLVMDwarfSourceLanguage##NAME = ID,
#include "llvm/BinaryFormat/Dwarf.def"
#undef HANDLE_DW_LANG
};

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
    LLVMBool DebugInfoForProfiling );

#ifdef __cplusplus
}
#endif

#endif
