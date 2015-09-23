//===- IRBindings.h - Additional bindings for IR ----------------*- C++ -*-===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// This file defines additional C bindings for the IR component.
//
//===----------------------------------------------------------------------===//

#ifndef LLVM_BINDINGS_LLVM_IRBINDINGS_H
#define LLVM_BINDINGS_LLVM_IRBINDINGS_H

#include "llvm-c/Core.h"
#ifdef __cplusplus
#include "llvm/IR/Metadata.h"
#include "llvm/Support/CBindingWrapping.h"
#endif

#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef struct LLVMOpaqueMetadata *LLVMMetadataRef;

enum LLVMAttrKind
{
    // IR-Level Attributes
    LLVMAttrKindNone,                  ///< No attributes have been set
    LLVMAttrKindAlignment,             ///< Alignment of parameter (5 bits)
                                       ///< stored as log2 of alignment with +1 bias
                                       ///< 0 means unaligned (different from align(1))
    LLVMAttrKindAlwaysInline,          ///< inline=always
    LLVMAttrKindBuiltin,               ///< Callee is recognized as a builtin, despite
                                       ///< nobuiltin attribute on its declaration.
    LLVMAttrKindByVal,                 ///< Pass structure by value
    LLVMAttrKindInAlloca,              ///< Pass structure in an alloca
    LLVMAttrKindCold,                  ///< Marks function as being in a cold path.
    LLVMAttrKindConvergent,            ///< Can only be moved to control-equivalent blocks
    LLVMAttrKindInlineHint,            ///< Source said inlining was desirable
    LLVMAttrKindInReg,                 ///< Force argument to be passed in register
    LLVMAttrKindJumpTable,             ///< Build jump-instruction tables and replace refs.
    LLVMAttrKindMinSize,               ///< Function must be optimized for size first
    LLVMAttrKindNaked,                 ///< Naked function
    LLVMAttrKindNest,                  ///< Nested function static chain
    LLVMAttrKindNoAlias,               ///< Considered to not alias after call
    LLVMAttrKindNoBuiltin,             ///< Callee isn't recognized as a builtin
    LLVMAttrKindNoCapture,             ///< Function creates no aliases of pointer
    LLVMAttrKindNoDuplicate,           ///< Call cannot be duplicated
    LLVMAttrKindNoImplicitFloat,       ///< Disable implicit floating point insts
    LLVMAttrKindNoInline,              ///< inline=never
    LLVMAttrKindNonLazyBind,           ///< Function is called early and/or
                                       ///< often, so lazy binding isn't worthwhile
    LLVMAttrKindNonNull,               ///< Pointer is known to be not null
    LLVMAttrKindDereferenceable,       ///< Pointer is known to be dereferenceable
    LLVMAttrKindDereferenceableOrNull, ///< Pointer is either null or dereferenceable
    LLVMAttrKindNoRedZone,             ///< Disable redzone
    LLVMAttrKindNoReturn,              ///< Mark the function as not returning
    LLVMAttrKindNoUnwind,              ///< Function doesn't unwind stack
    LLVMAttrKindOptimizeForSize,       ///< opt_size
    LLVMAttrKindOptimizeNone,          ///< Function must not be optimized.
    LLVMAttrKindReadNone,              ///< Function does not access memory
    LLVMAttrKindReadOnly,              ///< Function only reads from memory
    LLVMAttrKindArgMemOnly,            ///< Funciton can access memory only using pointers
                                       ///< based on its arguments.
    LLVMAttrKindReturned,              ///< Return value is always equal to this argument
    LLVMAttrKindReturnsTwice,          ///< Function can return twice
    LLVMAttrKindSExt,                  ///< Sign extended before/after call
    LLVMAttrKindStackAlignment,        ///< Alignment of stack for function (3 bits)
                                       ///< stored as log2 of alignment with +1 bias 0
                                       ///< means unaligned (different from
                                       ///< alignstack=(1))
    LLVMAttrKindStackProtect,          ///< Stack protection.
    LLVMAttrKindStackProtectReq,       ///< Stack protection required.
    LLVMAttrKindStackProtectStrong,    ///< Strong Stack protection.
    LLVMAttrKindSafeStack,             ///< Safe Stack protection.
    LLVMAttrKindStructRet,             ///< Hidden pointer to structure to return
    LLVMAttrKindSanitizeAddress,       ///< AddressSanitizer is on.
    LLVMAttrKindSanitizeThread,        ///< ThreadSanitizer is on.
    LLVMAttrKindSanitizeMemory,        ///< MemorySanitizer is on.
    LLVMAttrKindUWTable,               ///< Function must be in a unwind table
    LLVMAttrKindZExt,                  ///< Zero extended before/after call

    EndAttrKinds           ///< Sentinal value useful for loops
};

// These functions duplicate the LLVM*FunctionAttr functions in the stable C
// API. We cannot use the existing functions because they take 32-bit attribute
// values, and the these bindings expose all of the LLVM attributes, some of which
// have values >= 1<<32.
void LLVMAddFunctionAttr2( LLVMValueRef Fn, int index, LLVMAttrKind kind );
LLVMBool LLVMHasFunctionAttr2( LLVMValueRef Fn, int index, LLVMAttrKind kind );
void LLVMRemoveFunctionAttr2( LLVMValueRef Fn, int index, LLVMAttrKind kind );

void LLVMSetFunctionStackAlignment( LLVMValueRef Fn, uint32_t alignment );
uint32_t LLVMGetFunctionStackAlignment( LLVMValueRef Fn );

LLVMMetadataRef LLVMConstantAsMetadata(LLVMValueRef Val);
LLVMMetadataRef LLVMMDString2(LLVMContextRef C, const char *Str, unsigned SLen);
LLVMMetadataRef LLVMMDNode2(LLVMContextRef C, LLVMMetadataRef *MDs, unsigned Count);
LLVMMetadataRef LLVMTemporaryMDNode(LLVMContextRef C, LLVMMetadataRef *MDs, unsigned Count);

char const* LLVMGetMDStringText( LLVMMetadataRef mdstring, unsigned* len );

void LLVMAddNamedMetadataOperand2(LLVMModuleRef M, const char *name, LLVMMetadataRef Val);
void LLVMSetMetadata2(LLVMValueRef Inst, unsigned KindID, LLVMMetadataRef MD);
void LLVMMetadataReplaceAllUsesWith(LLVMMetadataRef MD, LLVMMetadataRef New);
void LLVMSetCurrentDebugLocation2(LLVMBuilderRef Bref, unsigned Line, unsigned Col, LLVMMetadataRef Scope, LLVMMetadataRef InlinedAt);

LLVMBool LLVMIsTemporary( LLVMMetadataRef M );
LLVMBool LLVMIsResolved( LLVMMetadataRef M );
void LLVMMDNodeResolveCycles( LLVMMetadataRef M );

#ifdef __cplusplus
}

namespace llvm {

DEFINE_ISA_CONVERSION_FUNCTIONS(Metadata, LLVMMetadataRef)

inline Metadata **unwrap(LLVMMetadataRef *Vals) {
  return reinterpret_cast<Metadata**>(Vals);
}

}

#endif

#endif
