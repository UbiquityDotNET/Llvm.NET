#include "libllvm-c/LibLlvmOrcJitBindings.h"
#include "OrcCBindingsStack.h"

using namespace llvm;

extern "C"
{
    LLVMErrorRef LibLLVMOrcGetSymbolAddress( LLVMOrcJITStackRef JITStack,
                                             LLVMOrcTargetAddress* RetAddr,
                                             const char* SymbolName,
                                             LLVMBool ExportedSymbolsOnly )
    {
        OrcCBindingsStack& J = *unwrap( JITStack );
        if ( auto Addr = J.findSymbolAddress( SymbolName, !!ExportedSymbolsOnly ) )
        {
            *RetAddr = *Addr;
            return LLVMErrorSuccess;
        }
        else
        {
            return wrap( Addr.takeError( ) );
        }
    }

    LLVMErrorRef LibLLVMOrcGetSymbolAddressIn( LLVMOrcJITStackRef JITStack,
                                               LLVMOrcTargetAddress* RetAddr,
                                               LLVMOrcModuleHandle H,
                                               const char* SymbolName,
                                               LLVMBool ExportedSymbolsOnly )
    {
        OrcCBindingsStack& J = *unwrap( JITStack );
        if ( auto Addr = J.findSymbolAddressIn( H, SymbolName, !!ExportedSymbolsOnly ) )
        {
            *RetAddr = *Addr;
            return LLVMErrorSuccess;
        }
        else
        {
            return wrap( Addr.takeError( ) );
        }
    }
}
