cmake_minimum_required(VERSION 3.0.0)
project(LibLLVM VERSION 0.1.0)

# message("CMAKE version is " ${CMAKE_VERSION})

configure_file(
        "../../../llvm/include/llvm/Config/config.h.cmake"
        "LLVM/Config/config.h")
configure_file(
        "../../../llvm/include/llvm/Config/llvm-config.h.cmake"
        "LLVM/Config/llvm-config.h")
configure_file(
        "../../../llvm/include/llvm/Config/abi-breaking.h.cmake"
        "LLVM/Config/abi-breaking.h")
configure_file(
        "../../../llvm/include/llvm/Config/AsmParsers.def.in"
        "LLVM/Config/AsmParsers.def")
configure_file(
        "../../../llvm/include/llvm/Config/AsmPrinters.def.in"
        "LLVM/Config/AsmPrinters.def")
configure_file(
        "../../../llvm/include/llvm/Config/Disassemblers.def.in"
        "LLVM/Config/Disassemblers.def")
configure_file(
        "../../../llvm/include/llvm/Config/Targets.def.in"
        "LLVM/Config/Targets.def")

include(CTest)
enable_testing()

link_directories(BEFORE "../../../llvm/x64-Release/Release/lib")

add_library(LibLLVM SHARED "")

target_include_directories(LibLLVM PRIVATE "../../../llvm/x64-Release/include")
target_include_directories(LibLLVM PRIVATE "../../../llvm/include")
target_include_directories(LibLLVM PRIVATE "../../../llvm/include/llvm/ExecutionEngine/Orc")
target_include_directories(LibLLVM PRIVATE "include")
target_include_directories(LibLLVM PRIVATE "${PROJECT_BINARY_DIR}")

add_compile_definitions("NDEBUG" "_USRDLL" "LIBLLVM_EXPORTS")
add_compile_definitions("_ITERATOR_DEBUG_LEVEL=0")

target_sources(LibLLVM PRIVATE 
    "AnalysisBindings" 
    "AttributeBindings" 
    "ContextBindings" 
    "DIBuilderBindings"
    "ObjectFileBindings"
    "InlinedExports" 
    "IRBindings" 
#   "LegacyPassManagerOpt" 
    "LibLlvmOrcJitBindings"
    "MetadataBindings" 
    "PassManagerBindings" 
    "ModuleBindings" 
#   "NewOptPassDriver" 
    "TripleBindings" 
    "ValueBindings")

target_link_libraries(LibLLVM PRIVATE
        "LLVMAArch64AsmParser"
        "LLVMAArch64CodeGen"
        "LLVMAArch64Desc"
        "LLVMAArch64Disassembler"
        "LLVMAArch64Info"
        "LLVMAArch64Utils"
        "LLVMAggressiveInstCombine"
        "LLVMAMDGPUAsmParser"
        "LLVMAMDGPUCodeGen"
        "LLVMAMDGPUDesc"
        "LLVMAMDGPUDisassembler"
        "LLVMAMDGPUInfo"
        "LLVMAMDGPUUtils"
        "LLVMAnalysis"
        "LLVMARMAsmParser"
        "LLVMARMCodeGen"
        "LLVMARMDesc"
        "LLVMARMDisassembler"
        "LLVMARMInfo"
        "LLVMARMUtils"
        "LLVMAsmParser"
        "LLVMAsmPrinter"
        "LLVMBinaryFormat"
        "LLVMBitReader"
        "LLVMBitstreamReader"
        "LLVMBitWriter"
        "LLVMBPFAsmParser"
        "LLVMBPFCodeGen"
        "LLVMBPFDesc"
        "LLVMBPFDisassembler"
        "LLVMBPFInfo"
        "LLVMCFGuard"
        "LLVMCodeGen"
        "LLVMCore"
        "LLVMCoroutines"
        "LLVMCoverage"
        "LLVMDebugInfoCodeView"
        "LLVMDebugInfoDWARF"
        "LLVMDebugInfoGSYM"
        "LLVMDebugInfoMSF"
        "LLVMDebugInfoPDB"
        "LLVMDemangle"
        "LLVMDlltoolDriver"
        "LLVMDWARFLinker"
        "LLVMExecutionEngine"
        "LLVMFrontendOpenMP"
        "LLVMFuzzMutate"
        "LLVMGlobalISel"
        "LLVMHexagonAsmParser"
        "LLVMHexagonCodeGen"
        "LLVMHexagonDesc"
        "LLVMHexagonDisassembler"
        "LLVMHexagonInfo"
        "LLVMInstCombine"
        "LLVMInstrumentation"
        "LLVMInterpreter"
        "LLVMipo"
        "LLVMIRReader"
        "LLVMJITLink"
        "LLVMLanaiAsmParser"
        "LLVMLanaiCodeGen"
        "LLVMLanaiDesc"
        "LLVMLanaiDisassembler"
        "LLVMLanaiInfo"
        "LLVMLibDriver"
        "LLVMLineEditor"
        "LLVMLinker"
        "LLVMLTO"
        "LLVMMC"
        "LLVMMCA"
        "LLVMMCDisassembler"
        "LLVMMCJIT"
        "LLVMMCParser"
        "LLVMMipsAsmParser"
        "LLVMMipsCodeGen"
        "LLVMMipsDesc"
        "LLVMMipsDisassembler"
        "LLVMMipsInfo"
        "LLVMMIRParser"
        "LLVMMSP430AsmParser"
        "LLVMMSP430CodeGen"
        "LLVMMSP430Desc"
        "LLVMMSP430Disassembler"
        "LLVMMSP430Info"
        "LLVMNVPTXCodeGen"
        "LLVMNVPTXDesc"
        "LLVMNVPTXInfo"
        "LLVMObjCARCOpts"
        "LLVMObject"
        "LLVMObjectYAML"
        "LLVMOption"
        "LLVMOrcError"
        "LLVMOrcJIT"
        "LLVMPasses"
        "LLVMPowerPCAsmParser"
        "LLVMPowerPCCodeGen"
        "LLVMPowerPCDesc"
        "LLVMPowerPCDisassembler"
        "LLVMPowerPCInfo"
        "LLVMProfileData"
        "LLVMRemarks"
        "LLVMRISCVAsmParser"
        "LLVMRISCVCodeGen"
        "LLVMRISCVDesc"
        "LLVMRISCVDisassembler"
        "LLVMRISCVInfo"
        "LLVMRISCVUtils"
        "LLVMRuntimeDyld"
        "LLVMScalarOpts"
        "LLVMSelectionDAG"
        "LLVMSparcAsmParser"
        "LLVMSparcCodeGen"
        "LLVMSparcDesc"
        "LLVMSparcDisassembler"
        "LLVMSparcInfo"
        "LLVMSupport"
        "LLVMSymbolize"
        "LLVMSystemZAsmParser"
        "LLVMSystemZCodeGen"
        "LLVMSystemZDesc"
        "LLVMSystemZDisassembler"
        "LLVMSystemZInfo"
        "LLVMTableGen"
        "LLVMTableGenGlobalISel"
        "LLVMTarget"
        "LLVMTextAPI"
        "LLVMTransformUtils"
        "LLVMVectorize"
        "LLVMWebAssemblyAsmParser"
        "LLVMWebAssemblyCodeGen"
        "LLVMWebAssemblyDesc"
        "LLVMWebAssemblyDisassembler"
        "LLVMWebAssemblyInfo"
        "LLVMWindowsManifest"
        "LLVMX86AsmParser"
        "LLVMX86CodeGen"
        "LLVMX86Desc"
        "LLVMX86Disassembler"
        "LLVMX86Info"
        "LLVMX86Utils"
        "LLVMXCoreCodeGen"
        "LLVMXCoreDesc"
        "LLVMXCoreDisassembler"
        "LLVMXCoreInfo"
        "LLVMXRay"
    )

set(CPACK_PROJECT_NAME ${PROJECT_NAME})
set(CPACK_PROJECT_VERSION ${PROJECT_VERSION})
include(CPack)
