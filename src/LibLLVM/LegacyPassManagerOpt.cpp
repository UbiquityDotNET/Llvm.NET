
//===----------- The LLVM Modular Optimizer -------------------------------===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
//
// Optimizations may be specified an arbitrary number of times on the command
// line, They are run in the order specified.
//
//===----------------------------------------------------------------------===//

// ported and modified from opt.cpp in LLVM to allow for use as an API instead of trying to expose all of
// the legacy pass manager support, since the pass management is undergoing a significant transition
// it is best not to build out projectionts to depend on the legacy variant.
#include "llvm/ADT/Triple.h"
#include "llvm/Analysis/CallGraph.h"
#include "llvm/Analysis/CallGraphSCCPass.h"
#include "llvm/Analysis/LoopPass.h"
#include "llvm/Analysis/RegionPass.h"
#include "llvm/Analysis/TargetLibraryInfo.h"
#include "llvm/Analysis/TargetTransformInfo.h"
#include "llvm/Bitcode/BitcodeWriterPass.h"
#include "llvm/CodeGen/CommandFlags.h"
#include "llvm/CodeGen/TargetPassConfig.h"
#include "llvm/IR/DataLayout.h"
#include "llvm/IR/DebugInfo.h"
#include "llvm/IR/IRPrintingPasses.h"
#include "llvm/IR/LLVMContext.h"
#include "llvm/IR/LegacyPassManager.h"
#include "llvm/IR/LegacyPassNameParser.h"
#include "llvm/IR/Module.h"
#include "llvm/IR/Verifier.h"
#include "llvm/IRReader/IRReader.h"
#include "llvm/InitializePasses.h"
#include "llvm/LinkAllIR.h"
#include "llvm/LinkAllPasses.h"
#include "llvm/MC/SubtargetFeature.h"
#include "llvm/Support/Debug.h"
#include "llvm/Support/FileSystem.h"
#include "llvm/Support/Host.h"
#include "llvm/Support/ManagedStatic.h"
#include "llvm/Support/PluginLoader.h"
#include "llvm/Support/PrettyStackTrace.h"
#include "llvm/Support/Signals.h"
#include "llvm/Support/SourceMgr.h"
#include "llvm/Support/SystemUtils.h"
#include "llvm/Support/TargetRegistry.h"
#include "llvm/Support/TargetSelect.h"
#include "llvm/Support/ToolOutputFile.h"
#include "llvm/Support/YAMLTraits.h"
#include "llvm/Target/TargetMachine.h"
#include "llvm/Transforms/Coroutines.h"
#include "llvm/Transforms/IPO/AlwaysInliner.h"
#include "llvm/Transforms/IPO/PassManagerBuilder.h"
#include "llvm/Transforms/Utils/Cloning.h"
#include "llvm-c/TargetMachine.h"
#include <algorithm>
#include <memory>
using namespace llvm;

static TargetMachine *unwrap( LLVMTargetMachineRef P )
{
    return reinterpret_cast< TargetMachine * >( P );
}

// The OptimizationList is automatically populated with registered Passes by the
// PassNameParser.
//
static cl::list<const PassInfo*, bool, PassNameParser>
PassList( cl::desc( "Optimizations available:" ) );

// This flag specifies a textual description of the optimization pass pipeline
// to run over the module. This flag switches opt to use the new pass manager
// infrastructure, completely disabling all of the flags specific to the old
// pass management.
static cl::opt<std::string> PassPipeline(
    "passes",
    cl::desc( "A textual description of the pass pipeline for optimizing" ),
    cl::Hidden );

static cl::opt<bool>
NoVerify( "disable-verify", cl::desc( "Do not run the verifier" ), cl::Hidden );

static cl::opt<bool>
VerifyEach( "verify-each", cl::desc( "Verify after each transform" ) );

static cl::opt<bool>
DisableDITypeMap( "disable-debug-info-type-map",
    cl::desc( "Don't use a uniquing type map for debug info" ) );

static cl::opt<bool>
StripDebug( "strip-debug",
    cl::desc( "Strip debugger symbol info from translation unit" ) );

static cl::opt<bool>
DisableInline( "disable-inlining", cl::desc( "Do not run the inliner pass" ) );

static cl::opt<bool>
DisableOptimizations( "disable-opt",
    cl::desc( "Do not run any optimization passes" ) );
static cl::opt<bool>
StandardLinkOpts( "std-link-opts",
    cl::desc( "Include the standard link time optimizations" ) );

static cl::opt<bool>
OptLevelO0( "O0",
    cl::desc( "Optimization level 0. Similar to clang -O0" ) );

static cl::opt<bool>
OptLevelO1( "O1",
    cl::desc( "Optimization level 1. Similar to clang -O1" ) );

static cl::opt<bool>
OptLevelO2( "O2",
    cl::desc( "Optimization level 2. Similar to clang -O2" ) );

static cl::opt<bool>
OptLevelOs( "Os",
    cl::desc( "Like -O2 with extra optimizations for size. Similar to clang -Os" ) );

static cl::opt<bool>
OptLevelOz( "Oz",
    cl::desc( "Like -Os but reduces code size further. Similar to clang -Oz" ) );

static cl::opt<bool>
OptLevelO3( "O3",
    cl::desc( "Optimization level 3. Similar to clang -O3" ) );

static cl::opt<unsigned>
CodeGenOptLevel( "codegen-opt-level",
    cl::desc( "Override optimization level for codegen hooks" ) );

static cl::opt<std::string>
TargetTriple( "mtriple", cl::desc( "Override target triple for module" ) );

static cl::opt<bool>
UnitAtATime( "funit-at-a-time",
    cl::desc( "Enable IPO. This corresponds to gcc's -funit-at-a-time" ),
    cl::init( true ) );

static cl::opt<bool>
DisableLoopUnrolling( "disable-loop-unrolling",
    cl::desc( "Disable loop unrolling in all relevant passes" ),
    cl::init( false ) );
static cl::opt<bool>
DisableLoopVectorization( "disable-loop-vectorization",
    cl::desc( "Disable the loop vectorization pass" ),
    cl::init( false ) );

static cl::opt<bool>
DisableSLPVectorization( "disable-slp-vectorization",
    cl::desc( "Disable the slp vectorization pass" ),
    cl::init( false ) );

static cl::opt<bool>
DisableSimplifyLibCalls( "disable-simplify-libcalls",
    cl::desc( "Disable simplify-libcalls" ) );

static cl::opt<bool>
AnalyzeOnly( "analyze", cl::desc( "Only perform analysis, no optimization" ) );
static cl::opt<std::string> ClDataLayout( "data-layout",
    cl::desc( "data layout string to use" ),
    cl::value_desc( "layout-string" ),
    cl::init( "" ) );

static cl::opt<bool> PreserveBitcodeUseListOrder(
    "preserve-bc-uselistorder",
    cl::desc( "Preserve use-list order when writing LLVM bitcode." ),
    cl::init( true ), cl::Hidden );

static cl::opt<bool> PreserveAssemblyUseListOrder(
    "preserve-ll-uselistorder",
    cl::desc( "Preserve use-list order when writing LLVM assembly." ),
    cl::init( false ), cl::Hidden );

static cl::opt<bool>
RunTwice( "run-twice",
    cl::desc( "Run all passes twice, re-using the same pass manager." ),
    cl::init( false ), cl::Hidden );

static cl::opt<bool> DiscardValueNames(
    "discard-value-names",
    cl::desc( "Discard names from Value (other than GlobalValue)." ),
    cl::init( false ), cl::Hidden );

static cl::opt<bool> Coroutines(
    "enable-coroutines",
    cl::desc( "Enable coroutine passes." ),
    cl::init( false ), cl::Hidden );

static cl::opt<bool> PassRemarksWithHotness(
    "pass-remarks-with-hotness",
    cl::desc( "With PGO, include profile count in optimization remarks" ),
    cl::Hidden );
static cl::opt<unsigned> PassRemarksHotnessThreshold(
    "pass-remarks-hotness-threshold",
    cl::desc( "Minimum profile count required for an optimization remark to be output" ),
    cl::Hidden );

static cl::opt<std::string>
RemarksFilename( "pass-remarks-output",
    cl::desc( "YAML output filename for pass remarks" ),
    cl::value_desc( "filename" ) );

static inline void addPass( legacy::PassManagerBase &PM, Pass *P ) {
    // Add the pass to the pass manager...
    PM.add( P );

    // If we are verifying all of the intermediate steps, add the verifier...
    if( VerifyEach )
        PM.add( createVerifierPass( ) );
}

/// This routine adds optimization passes based on selected optimization level,
/// OptLevel.
///
/// OptLevel - Optimization Level
static void AddOptimizationPasses( legacy::PassManagerBase &MPM,
    legacy::FunctionPassManager &FPM,
    TargetMachine *TM, unsigned OptLevel,
    unsigned SizeLevel ) {
    if( !NoVerify || VerifyEach )
        FPM.add( createVerifierPass( ) ); // Verify that input is correct

    PassManagerBuilder Builder;
    Builder.OptLevel = OptLevel;
    Builder.SizeLevel = SizeLevel;

    if( DisableInline ) {
        // No inlining pass
    }
    else if( OptLevel > 1 ) {
        Builder.Inliner = createFunctionInliningPass( OptLevel, SizeLevel, false );
    }
    else {
        Builder.Inliner = createAlwaysInlinerLegacyPass( );
    }
    Builder.DisableUnitAtATime = !UnitAtATime;
    Builder.DisableUnrollLoops = ( DisableLoopUnrolling.getNumOccurrences( ) > 0 ) ?
        DisableLoopUnrolling : OptLevel == 0;

    // This is final, unless there is a #pragma vectorize enable
    if( DisableLoopVectorization )
        Builder.LoopVectorize = false;
    // If option wasn't forced via cmd line (-vectorize-loops, -loop-vectorize)
    else if( !Builder.LoopVectorize )
        Builder.LoopVectorize = OptLevel > 1 && SizeLevel < 2;

    // When #pragma vectorize is on for SLP, do the same as above
    Builder.SLPVectorize =
        DisableSLPVectorization ? false : OptLevel > 1 && SizeLevel < 2;

    if( TM )
        TM->adjustPassManager( Builder );

    if( Coroutines )
        addCoroutinePassesToExtensionPoints( Builder );

    Builder.populateFunctionPassManager( FPM );
    Builder.populateModulePassManager( MPM );
}

static void AddStandardLinkPasses( legacy::PassManagerBase &PM ) {
    PassManagerBuilder Builder;
    Builder.VerifyInput = true;
    if( DisableOptimizations )
        Builder.OptLevel = 0;

    if( !DisableInline )
        Builder.Inliner = createFunctionInliningPass( );
    Builder.populateLTOPassManager( PM );
}

//===----------------------------------------------------------------------===//
// CodeGen-related helper functions.
//

static CodeGenOpt::Level GetCodeGenOptLevel( ) {
    if( CodeGenOptLevel.getNumOccurrences( ) )
        return static_cast< CodeGenOpt::Level >( unsigned( CodeGenOptLevel ) );
    if( OptLevelO1 )
        return CodeGenOpt::Less;
    if( OptLevelO2 )
        return CodeGenOpt::Default;
    if( OptLevelO3 )
        return CodeGenOpt::Aggressive;
    return CodeGenOpt::None;
}

// Returns the TargetMachine instance or zero if no triple is provided.
static TargetMachine* GetTargetMachine( Triple TheTriple, StringRef CPUStr,
    StringRef FeaturesStr,
    const TargetOptions &Options ) {
    std::string Error;
    const Target *TheTarget = TargetRegistry::lookupTarget( MArch, TheTriple,
        Error );
    // Some modules don't specify a triple, and this is okay.
    if( !TheTarget ) {
        return nullptr;
    }

    return TheTarget->createTargetMachine( TheTriple.getTriple( ), CPUStr,
        FeaturesStr, Options, getRelocModel( ),
        CMModel, GetCodeGenOptLevel( ) );
}

#ifdef LINK_POLLY_INTO_TOOLS
namespace polly {
    void initializePollyPasses( llvm::PassRegistry &Registry );
}
#endif

void LLVMInitializePassesForLegacyOpt( )
{
    PassRegistry &Registry = *PassRegistry::getPassRegistry( );
    initializeCore( Registry );
    initializeCoroutines( Registry );
    initializeScalarOpts( Registry );
    initializeObjCARCOpts( Registry );
    initializeVectorization( Registry );
    initializeIPO( Registry );
    initializeAnalysis( Registry );
    initializeTransformUtils( Registry );
    initializeInstCombine( Registry );
    initializeInstrumentation( Registry );
    initializeTarget( Registry );
    // For codegen passes, only passes that do IR to IR transformation are
    // supported.
    initializeScalarizeMaskedMemIntrinPass( Registry );
    initializeCodeGenPreparePass( Registry );
    initializeAtomicExpandPass( Registry );
    initializeRewriteSymbolsLegacyPassPass( Registry );
    initializeWinEHPreparePass( Registry );
    initializeDwarfEHPreparePass( Registry );
    initializeSafeStackLegacyPassPass( Registry );
    initializeSjLjEHPreparePass( Registry );
    initializePreISelIntrinsicLoweringLegacyPassPass( Registry );
    initializeGlobalMergePass( Registry );
    initializeInterleavedAccessPass( Registry );
    initializeCountingFunctionInserterPass( Registry );
    initializeUnreachableBlockElimLegacyPassPass( Registry );
    initializeExpandReductionsPass( Registry );
}

void LLVMRunLegacyOptimizer( LLVMModuleRef Mref, LLVMTargetMachineRef TMref ) {

    SMDiagnostic Err;
    LLVMContext Context;

    Context.setDiscardValueNames( DiscardValueNames );
    if( !DisableDITypeMap )
        Context.enableDebugTypeODRUniquing( );

    if( PassRemarksWithHotness )
        Context.setDiagnosticsHotnessRequested( true );

    if( PassRemarksHotnessThreshold )
        Context.setDiagnosticsHotnessThreshold( PassRemarksHotnessThreshold );

    auto M = unwrap( Mref );
    // Strip debug info before running the verifier.
    if( StripDebug )
        StripDebugInfo( *M );

    Triple ModuleTriple( M->getTargetTriple( ) );
    std::string CPUStr, FeaturesStr;
    TargetMachine *TM = unwrap( TMref );
    const TargetOptions Options = InitTargetOptionsFromCodeGenFlags( );

    // Create a PassManager to hold and optimize the collection of passes we are
    // about to build.
    //
    legacy::PassManager Passes;

    // Add an appropriate TargetLibraryInfo pass for the module's triple.
    TargetLibraryInfoImpl TLII( ModuleTriple );

    // The -disable-simplify-libcalls flag actually disables all builtin optzns.
    if( DisableSimplifyLibCalls )
        TLII.disableAllFunctions( );

    Passes.add( new TargetLibraryInfoWrapperPass( TLII ) );

    // Add internal analysis passes from the target machine.
    Passes.add( createTargetTransformInfoWrapperPass( TM ? TM->getTargetIRAnalysis( )
        : TargetIRAnalysis( ) ) );

    std::unique_ptr<legacy::FunctionPassManager> FPasses;
    if( OptLevelO0 || OptLevelO1 || OptLevelO2 || OptLevelOs || OptLevelOz ||
        OptLevelO3 ) {
        FPasses.reset( new legacy::FunctionPassManager( M ) );
        FPasses->add( createTargetTransformInfoWrapperPass(
            TM ? TM->getTargetIRAnalysis( ) : TargetIRAnalysis( ) ) );
    }

    if( TM ) {
        // FIXME: We should dyn_cast this when supported.
        auto &LTM = static_cast< LLVMTargetMachine & >( *TM );
        Pass *TPC = LTM.createPassConfig( Passes );
        Passes.add( TPC );
    }
    // Create a new optimization pass for each one specified on the command line
    for( unsigned i = 0; i < PassList.size( ); ++i ) {
        if( StandardLinkOpts &&
            StandardLinkOpts.getPosition( ) < PassList.getPosition( i ) ) {
            AddStandardLinkPasses( Passes );
            StandardLinkOpts = false;
        }

        if( OptLevelO0 && OptLevelO0.getPosition( ) < PassList.getPosition( i ) ) {
            AddOptimizationPasses( Passes, *FPasses, TM, 0, 0 );
            OptLevelO0 = false;
        }
        if( OptLevelO1 && OptLevelO1.getPosition( ) < PassList.getPosition( i ) ) {
            AddOptimizationPasses( Passes, *FPasses, TM, 1, 0 );
            OptLevelO1 = false;
        }

        if( OptLevelO2 && OptLevelO2.getPosition( ) < PassList.getPosition( i ) ) {
            AddOptimizationPasses( Passes, *FPasses, TM, 2, 0 );
            OptLevelO2 = false;
        }

        if( OptLevelOs && OptLevelOs.getPosition( ) < PassList.getPosition( i ) ) {
            AddOptimizationPasses( Passes, *FPasses, TM, 2, 1 );
            OptLevelOs = false;
        }

        if( OptLevelOz && OptLevelOz.getPosition( ) < PassList.getPosition( i ) ) {
            AddOptimizationPasses( Passes, *FPasses, TM, 2, 2 );
            OptLevelOz = false;
        }

        if( OptLevelO3 && OptLevelO3.getPosition( ) < PassList.getPosition( i ) ) {
            AddOptimizationPasses( Passes, *FPasses, TM, 3, 0 );
            OptLevelO3 = false;
        }

        const PassInfo *PassInf = PassList[ i ];
        Pass *P = nullptr;
        if( PassInf->getNormalCtor( ) )
            P = PassInf->getNormalCtor( )( );
        else {
            //errs( ) << argv[ 0 ] << ": cannot create pass: " << PassInf->getPassName( ) << "\n";
            //TODO: Assert or return error code/message?
            return;
        }
        if( P ) {
            PassKind Kind = P->getPassKind( );
            addPass( Passes, P );
        }
    }

    if( StandardLinkOpts ) {
        AddStandardLinkPasses( Passes );
        StandardLinkOpts = false;
    }

    if( OptLevelO0 )
        AddOptimizationPasses( Passes, *FPasses, TM, 0, 0 );

    if( OptLevelO1 )
        AddOptimizationPasses( Passes, *FPasses, TM, 1, 0 );

    if( OptLevelO2 )
        AddOptimizationPasses( Passes, *FPasses, TM, 2, 0 );

    if( OptLevelOs )
        AddOptimizationPasses( Passes, *FPasses, TM, 2, 1 );

    if( OptLevelOz )
        AddOptimizationPasses( Passes, *FPasses, TM, 2, 2 );

    if( OptLevelO3 )
        AddOptimizationPasses( Passes, *FPasses, TM, 3, 0 );

    if( FPasses ) {
        FPasses->doInitialization( );
        for( Function &F : *M )
            FPasses->run( F );
        FPasses->doFinalization( );
    }

    // Check that the module is well formed on completion of optimization
    if( !NoVerify && !VerifyEach )
        Passes.add( createVerifierPass( ) );

    // Now that we have all of the passes ready, run them.
    Passes.run( *M );
}
