cmake_minimum_required(VERSION 3.0.0)
project(Ubiquity.NET.LibLlvm VERSION 0.1.0)

if(POLICY CMP0076)
  cmake_policy(SET CMP0076 NEW)
endif()

message("CMAKE version is " ${CMAKE_VERSION})
message("Host system name is " ${CMAKE_HOST_SYSTEM_NAME})
message("Host system processor is " ${CMAKE_HOST_SYSTEM_PROCESSOR})
message("C++ compiler is " ${CMAKE_CXX_COMPILER})
message("C++ compiler ID is " ${CMAKE_CXX_COMPILER_ID})

message(INFO "*** build config: ${CMAKE_BUILD_TYPE}")

set(CMAKE_CXX_STANDARD 14 CACHE STRING "C++ standard to conform to")
set(CMAKE_CXX_STANDARD_REQUIRED YES)
set(CMAKE_CXX_EXTENSIONS NO)

set(CMAKE_VERBOSE_MAKEFILE TRUE)
set(LLVM_VERSION_STRING "10.0.0")
set(LLVM_VERSION_MAJOR "10")
set(LLVM_VERSION_MINOR "0")
set(LLVM_VERSION_PATCH "0")

include(CTest)
enable_testing()

add_library(Ubiquity.NET.LibLlvm SHARED "")

target_include_directories(Ubiquity.NET.LibLlvm PRIVATE "../../../llvm/include")
target_include_directories(Ubiquity.NET.LibLlvm PRIVATE "../../../llvm/lib/ExecutionEngine/Orc")
target_include_directories(Ubiquity.NET.LibLlvm PRIVATE "include")
target_include_directories(Ubiquity.NET.LibLlvm PRIVATE ".")

target_link_directories(Ubiquity.NET.LibLlvm BEFORE PRIVATE "../../../llvm/lib")

add_compile_definitions("NDEBUG" "_USRDLL" "LIBLLVM_EXPORTS" "LibLLVM_EXPORTS")
add_compile_definitions("_ITERATOR_DEBUG_LEVEL=0")

if (NOT WIN32)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fno-rtti -pthread")
endif()

target_sources(Ubiquity.NET.LibLlvm PUBLIC 
    "InlinedExports"
    "EXPORTS.g.DEF")

target_sources(Ubiquity.NET.LibLlvm PRIVATE 
    "AnalysisBindings" 
    "AttributeBindings" 
    "ContextBindings" 
    "DIBuilderBindings"
    "ObjectFileBindings"
    "IRBindings" 
    "LibLlvmOrcJitBindings"
    "MetadataBindings" 
    "PassManagerBindings" 
    "ModuleBindings" 
    "TripleBindings" 
    "ValueBindings")

if (APPLE)
    target_link_libraries(Ubiquity.NET.LibLlvm PRIVATE "-Wl,-all_load")
elseif (UNIX)
    target_link_libraries(Ubiquity.NET.LibLlvm PRIVATE "-Wl,--whole-archive")
endif()

target_link_libraries(Ubiquity.NET.LibLlvm PRIVATE
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

if (APPLE)
    target_link_libraries(Ubiquity.NET.LibLlvm PRIVATE "z" "edit" "xml2")
elseif (UNIX)
    target_link_libraries(Ubiquity.NET.LibLlvm PRIVATE "-Wl,--no-whole-archive")
endif()

if (NOT WIN32)
    target_link_libraries(Ubiquity.NET.LibLlvm PRIVATE "ncurses" "pthread" "dl")
endif()


set(CPACK_PROJECT_NAME ${PROJECT_NAME})
set(CPACK_PROJECT_VERSION ${PROJECT_VERSION})
include(CPack)
