//===- InstrumentationBindings.cpp - instrumentation bindings -------------===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// This file defines C bindings for the instrumentation component.
//
//===----------------------------------------------------------------------===//

#include "libllvm-c/PassManagerBindings.h"

#include <llvm-c/Core.h>
#include <llvm/IR/LegacyPassManager.h>
#include <llvm/IR/Module.h>
#include <llvm/Transforms/Instrumentation.h>
#include <llvm/PassRegistry.h>
#include <llvm/LinkAllPasses.h>
#include "llvm/Transforms/Instrumentation/AddressSanitizer.h"
#include <llvm/Transforms/Instrumentation/MemorySanitizer.h>
#include <llvm/Transforms/Instrumentation/ThreadSanitizer.h>

using namespace llvm;

extern "C"
{
    LLVMPassRegistryRef LibLLVMCreatePassRegistry( )
    {
        return wrap( new PassRegistry( ) );
    }

    void LibLLVMPassRegistryDispose( LLVMPassRegistryRef passReg )
    {
        delete unwrap( passReg );
    }

    void LibLLVMAddAddressSanitizerFunctionPass( LLVMPassManagerRef PM )
    {
        unwrap( PM )->add( createAddressSanitizerFunctionPass( ) );
    }

    void LibLLVMAddAddressSanitizerModulePass( LLVMPassManagerRef PM )
    {
        unwrap( PM )->add( createModuleAddressSanitizerLegacyPassPass( ) );
    }

    void LibLLVMAddThreadSanitizerPass( LLVMPassManagerRef PM )
    {
        unwrap( PM )->add( createThreadSanitizerLegacyPassPass( ) );
    }

    void LibLLVMAddMemorySanitizerPass( LLVMPassManagerRef PM )
    {
        unwrap( PM )->add( createMemorySanitizerLegacyPassPass( ) );
    }

    void LibLLVMAddDataFlowSanitizerPass( LLVMPassManagerRef PM, int ABIListFilesNum, const char **ABIListFiles )
    {
        std::vector<std::string> ABIListFilesVec;
        for( int i = 0; i != ABIListFilesNum; ++i )
        {
            ABIListFilesVec.push_back( ABIListFiles[ i ] );
        }

        unwrap( PM )->add( createDataFlowSanitizerPass( ABIListFilesVec ) );
    }
}
