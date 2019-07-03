#ifndef _LIBLLVM_DIBUILDERBINDINGS_H_
#include <stdint.h>
#include "llvm-c/Core.h"
#include "llvm-c/DebugInfo.h"

#ifdef __cplusplus
extern "C" {
#endif

/// createEnumerator - Create a single enumerator value.
LLVMMetadataRef LibLLVMDIBuilderCreateEnumeratorValue( LLVMDIBuilderRef D, char const* Name, int64_t Val );

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

/* Same as LLVMDIBuilderCreateAutoVariable except this correctly allows File == nullptr*/
LLVMMetadataRef LibLLVMDIBuilderCreateAutoVariable(
    LLVMDIBuilderRef Builder,
    LLVMMetadataRef Scope,
    const char* Name,
    size_t NameLen,
    LLVMMetadataRef File,
    unsigned LineNo,
    LLVMMetadataRef Ty,
    LLVMBool AlwaysPreserve,
    LLVMDIFlags Flags,
    uint32_t AlignInBits );

/* Same as LLVMDIBuilderCreateParameterVariable except this correctly allows File == nullptr*/
LLVMMetadataRef LibLLVMDIBuilderCreateParameterVariable(
    LLVMDIBuilderRef Builder,
    LLVMMetadataRef Scope,
    const char* Name,
    size_t NameLen,
    unsigned ArgNo,
    LLVMMetadataRef File,
    unsigned LineNo,
    LLVMMetadataRef Ty,
    LLVMBool AlwaysPreserve,
    LLVMDIFlags Flags );

#ifdef __cplusplus
}
#endif

#endif
