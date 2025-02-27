//===- AttributeBindings.h - Additional bindings for IR ----------------*- C++ -*-===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// This file defines additional C bindings for LLVM Attributes IR Attributes.
//
//===----------------------------------------------------------------------===//

#ifndef LLVM_BINDINGS_LLVM_ATTRIBUTEBINDINGS_H
#define LLVM_BINDINGS_LLVM_ATTRIBUTEBINDINGS_H

#include <llvm-c/Core.h>
#include <llvm-c/Types.h>

#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif
    typedef enum LibLLVMAttrKind {
        // IR-Level Attributes
        None,                  ///< No attributes have been set
#define GET_ATTR_ENUM
#include "llvm/IR/Attributes.inc"
        EndAttrKinds,          ///< Sentinel value useful for loops
        EmptyKey,              ///< Use as Empty key for DenseMap of AttrKind
        TombstoneKey,          ///< Use as Tombstone key for DenseMap of AttrKind
    } LibLLVMAttrKind;

    // caller must release the returned string via LLVMDisposeMessage
    char const* LibLLVMAttributeToString( LLVMAttributeRef attribute );
    char const* LibLLVMGetEnumAttributeKindName(LLVMAttributeRef attribute);
#ifdef __cplusplus
}
#endif

#endif
