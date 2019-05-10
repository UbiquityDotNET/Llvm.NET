//===- IRBindings.cpp - Additional bindings for ir ------------------------===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// This file defines additional C bindings for the ir component.
//
//===----------------------------------------------------------------------===//

#include "libllvm-c/IRBindings.h"
#include "llvm/IR/DebugLoc.h"
#include "llvm/IR/Function.h"
#include "llvm/IR/IRBuilder.h"

using namespace llvm;

extern "C"
{
    void LibLLVMGetVersionInfo( LibLLVMVersionInfo* pVersionInfo )
    {
        *pVersionInfo = { LLVM_VERSION_MAJOR, LLVM_VERSION_MINOR, LLVM_VERSION_PATCH };
    }

    void LibLLVMSetCurrentDebugLocation2( LLVMBuilderRef Bref
                                          , unsigned Line
                                          , unsigned Col
                                          , LLVMMetadataRef Scope
                                          , LLVMMetadataRef InlinedAt
    )
    {
        auto loc = DebugLoc::get( Line
                                  , Col
                                  , Scope ? unwrap<MDNode>( Scope ) : nullptr
                                  , InlinedAt ? unwrap<MDNode>( InlinedAt ) : nullptr
        );
        unwrap( Bref )->SetCurrentDebugLocation( loc );
    }

    unsigned LibLLVMLookupInstrinsicId( char const* name )
    {
        return Function::lookupIntrinsicID( name );
    }
}
