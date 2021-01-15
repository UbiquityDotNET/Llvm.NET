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
#include "llvm/LinkAllIR.h"

using namespace llvm;

extern "C"
{
    void LibLLVMGetVersionInfo( LibLLVMVersionInfo* pVersionInfo )
    {
        *pVersionInfo = { LLVM_VERSION_MAJOR, LLVM_VERSION_MINOR, LLVM_VERSION_PATCH };
    }

    LLVMBool LibLLVMHasUnwindDest( LLVMValueRef Invoke )
    {
        if ( CleanupReturnInst* CRI = dyn_cast< CleanupReturnInst >( unwrap( Invoke ) ) )
        {
            return CRI->hasUnwindDest( );
        }
        else if ( CatchSwitchInst* CSI = dyn_cast< CatchSwitchInst >( unwrap( Invoke ) ) )
        {
            return CSI->hasUnwindDest( );
        }
        return 0;
    }

}
