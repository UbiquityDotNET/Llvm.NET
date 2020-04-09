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
    // caller must release the returned string via LLVMDisposeMessage
    char const* LibLLVMAttributeToString( LLVMAttributeRef attribute );
    LLVMBool LibLLVMIsTypeAttribute( LLVMAttributeRef attribute );
    LLVMTypeRef LibLLVMGetAttributeTypeValue( LLVMAttributeRef attribute );

#ifdef __cplusplus
}
#endif

#endif
