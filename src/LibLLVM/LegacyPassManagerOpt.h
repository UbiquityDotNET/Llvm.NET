#pragma once

#include "llvm-c\Core.h"
#include "llvm-c\TargetMachine.h"


void LLVMInitializePassesForLegacyOpt( );
void LLVMRunLegacyOptimizer( LLVMModuleRef Mref, LLVMTargetMachineRef TMref );
