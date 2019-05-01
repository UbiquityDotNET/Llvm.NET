#ifndef _LIBLLVM_DIBUILDERBINDINGS_H_
#include <stdint.h>
#include "llvm-c/Core.h"
#include "llvm-c/DebugInfo.h"

#ifdef __cplusplus
extern "C" {
#endif

/// createEnumerator - Create a single enumerator value.
//DIEnumerator createEnumerator( StringRef Name, int64_t Val );
LLVMMetadataRef LLVMDIBuilderCreateEnumeratorValue( LLVMDIBuilderRef D, char const* Name, int64_t Val );

#ifdef __cplusplus
}
#endif

#endif
