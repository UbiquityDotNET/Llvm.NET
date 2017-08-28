//===- NewPMDriver.cpp - Driver for opt with new PM -----------------------===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
/// \file
///
/// This file is just a split of the code that logically belongs in opt.cpp but
/// that includes the new pass manager headers.
///
//===----------------------------------------------------------------------===//

#include "NewOptPassDriver.h"
#include "llvm/ADT/StringRef.h"
#include "llvm/Analysis/AliasAnalysis.h"
#include "llvm/Analysis/CGSCCPassManager.h"
#include "llvm/Transforms/Scalar/LoopPassManager.h"
#include "llvm/Bitcode/BitcodeWriterPass.h"
#include "llvm/IR/Dominators.h"
#include "llvm/IR/IRPrintingPasses.h"
#include "llvm/IR/LLVMContext.h"
#include "llvm/IR/Module.h"
#include "llvm/IR/PassManager.h"
#include "llvm/IR/Verifier.h"
#include "llvm/Passes/PassBuilder.h"
#include "llvm/Support/CommandLine.h"
#include "llvm/Support/ErrorHandling.h"
#include "llvm/Support/ToolOutputFile.h"
#include "llvm/Target/TargetMachine.h"

using namespace llvm;

static TargetMachine *unwrap( LLVMTargetMachineRef P )
{
    return reinterpret_cast<TargetMachine *>( P );
}

static cl::opt<bool>
DebugPM( "debug-pass-manager", cl::Hidden,
    cl::desc( "Print pass management debugging information" ) );

// This flag specifies a textual description of the alias analysis pipeline to
// use when querying for aliasing information. It only works in concert with
// the "passes" flag above.
static cl::opt<std::string>
AAPipeline( "aa-pipeline",
    cl::desc( "A textual description of the alias analysis "
        "pipeline for handling managed aliasing queries" ),
    cl::Hidden );

LLVMBool LLVMRunPassPipeline( LLVMContextRef context
                            , LLVMModuleRef M
                            , LLVMTargetMachineRef TM
                            , char const* passPipeline
                            , LLVMOptVerifierKind VK
                            , bool ShouldPreserveAssemblyUseListOrder
                            , bool ShouldPreserveBitcodeUseListOrder
                            )
 {
    PassBuilder PB( unwrap(TM) );

    // Specially handle the alias analysis manager so that we can register
    // a custom pipeline of AA passes with it.
    AAManager AA;
    if( !PB.parseAAPipeline( AA, AAPipeline ) )
    {
        return false;
    }

    LoopAnalysisManager LAM( DebugPM );
    FunctionAnalysisManager FAM( DebugPM );
    CGSCCAnalysisManager CGAM( DebugPM );
    ModuleAnalysisManager MAM( DebugPM );

    // Register the AA manager first so that our version is the one used.
    FAM.registerPass( [ & ] { return std::move( AA ); } );

    // Register all the basic analyses with the managers.
    PB.registerModuleAnalyses( MAM );
    PB.registerCGSCCAnalyses( CGAM );
    PB.registerFunctionAnalyses( FAM );
    PB.registerLoopAnalyses( LAM );
    PB.crossRegisterProxies( LAM, FAM, CGAM, MAM );

    ModulePassManager MPM( DebugPM );
    if( VK > LLVMOptVerifierKindNone )
    {
        MPM.addPass( VerifierPass( ) );
    }

    if( !PB.parsePassPipeline( MPM, passPipeline, VK == LLVMOptVerifierKindVerifyEachPass, DebugPM ) )
    {
        return false;
    }

    if( VK > LLVMOptVerifierKindNone )
        MPM.addPass( VerifierPass( ) );

    // Now that we have all of the passes ready, run them.
    MPM.run( *unwrap(M), MAM );

    return true;
}
