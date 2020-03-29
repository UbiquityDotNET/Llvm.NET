#include "libllvm-c/ObjectFileBindings.h"

#include "llvm/Object/ObjectFile.h"

using namespace llvm;
using namespace object;

namespace
{
    inline symbol_iterator* unwrap( LLVMSymbolIteratorRef SI )
    {
        return reinterpret_cast< symbol_iterator* >( SI );
    }

    inline LLVMSymbolIteratorRef  wrap( const symbol_iterator* SI )
    {
        return reinterpret_cast< LLVMSymbolIteratorRef > ( const_cast< symbol_iterator* >( SI ) );
    }

    inline section_iterator* unwrap( LLVMSectionIteratorRef SI )
    {
        return reinterpret_cast< section_iterator* >( SI );
    }

    inline LLVMSectionIteratorRef wrap( const section_iterator* SI )
    {
        return reinterpret_cast< LLVMSectionIteratorRef >( const_cast< section_iterator* >( SI ) );
    }

    inline relocation_iterator* unwrap( LLVMRelocationIteratorRef SI )
    {
        return reinterpret_cast< relocation_iterator* >( SI );
    }

    inline LLVMRelocationIteratorRef wrap( const relocation_iterator* SI )
    {
        return reinterpret_cast< LLVMRelocationIteratorRef >( const_cast< relocation_iterator* >( SI ) );
    }
}

extern "C"
{
    LLVMSymbolIteratorRef LibLLVMSymbolIteratorClone( LLVMSymbolIteratorRef ref )
    {
        return wrap( new symbol_iterator( *unwrap( ref ) ) );
    }

    LLVMSectionIteratorRef LibLLVMSectionIteratorClone( LLVMSectionIteratorRef ref )
    {
        return wrap( new section_iterator( *unwrap( ref ) ) );
    }

    LLVMRelocationIteratorRef LibLLVMRelocationIteratorClone( LLVMRelocationIteratorRef ref )
    {
        return wrap( new relocation_iterator( *unwrap( ref ) ) );
    }
}
