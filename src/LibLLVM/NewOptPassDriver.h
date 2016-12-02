//===- NewPMDriver.h - Function to drive opt with the new PM ----*- C++ -*-===//
//
//                     The LLVM Compiler Infrastructure
//
// This file is distributed under the University of Illinois Open Source
// License. See LICENSE.TXT for details.
//
//===----------------------------------------------------------------------===//
/// \file
///
/// A single function which is called to drive the opt behavior for the new
/// PassManager.
///
/// This is is a clone of the NewPMDriver support in the LLVM OPT tool, it is
/// modified slightly to support use in a language binding to use the simpler
/// one function call optimization.
///
//===----------------------------------------------------------------------===//
#pragma once

#include "llvm-c\Core.h"
#include "llvm-c\TargetMachine.h"

enum LLVMOptVerifierKind
{
    LLVMOptVerifierKindNone,
    LLVMOptVerifierKindVerifyInAndOut,
    LLVMOptVerifierKindVerifyEachPass
};

/// \brief Driver function to run the new pass manager over a module.
///
LLVMBool LLVMRunPassPipeline( LLVMContextRef context
                            , LLVMModuleRef M
                            , LLVMTargetMachineRef TM
                            , char const* passPipeline
                            , LLVMOptVerifierKind VK
                            , bool ShouldPreserveAssemblyUseListOrder
                            , bool ShouldPreserveBitcodeUseListOrder
                            );
