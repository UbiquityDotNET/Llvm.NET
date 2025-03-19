#include <type_traits>
#include <llvm/IR/Module.h>
#include "libllvm-c/ModuleBindings.h"

using namespace llvm;

namespace
{
    // Template for wrapping a C++ iterator in a "HANDLE" that aids in projection
    // to .NET's IEnumerator<T> This allows a C interop with a reference to an
    // instance of this template as an iterator handle.
    template<typename iterator_t, typename handle_t>
    class NativeMapIteratorWrapper
    {
    public:
        NativeMapIteratorWrapper(iterator_t b, iterator_t e)
            : begin(b), current(b), end(e)
        {
        }

        LLVMBool MoveNext()
        {
            if (current == end)
            {
                return 0;
            }

            current++;
            return 1;
        }

        handle_t Current()
        {
            if (current == end)
            {
                return nullptr;
            }

            return wrap(&current->second);
        }

        void Reset()
        {
            current = begin;
        }

    private:
        iterator_t begin;
        iterator_t current;
        iterator_t end;
    };

    // iterator for comdats in a module
    using ModuleComdatIterator = NativeMapIteratorWrapper<Module::ComdatSymTabType::const_iterator, LLVMComdatRef>;

    DEFINE_SIMPLE_CONVERSION_FUNCTIONS(NamedMDNode, LLVMNamedMDNodeRef)
    DEFINE_SIMPLE_CONVERSION_FUNCTIONS(ModuleComdatIterator, LibLLVMComdatIteratorRef);
}

extern "C"
{
    LibLLVMComdatIteratorRef LibLLVMModuleBeginComdats(LLVMModuleRef module)
    {
        Module* pModule = unwrap(module);
        Module::ComdatSymTabType const& comdatSymTab = pModule->getComdatSymbolTable();
        return wrap(new ModuleComdatIterator(std::begin(comdatSymTab), std::end(comdatSymTab)));
    }

    LLVMComdatRef LibLLVMModuleGetComdat(LLVMModuleRef module, char const* name)
    {
        Module::ComdatSymTabType const& comdatSymTab = unwrap(module)->getComdatSymbolTable();
        auto it = comdatSymTab.find(StringRef(name));
        return (it == std::end(comdatSymTab)) ? nullptr : wrap(&it->second);
    }

    uint32_t LibLLVMModuleGetNumComdats(LLVMModuleRef module)
    {
        return unwrap(module)->getComdatSymbolTable().getNumItems();
    }

    LLVMBool LibLLVMMoveNextComdat(LibLLVMComdatIteratorRef it)
    {
        return unwrap(it)->MoveNext();
    }

    LLVMComdatRef LibLLVMCurrentComdat(LibLLVMComdatIteratorRef it)
    {
        return unwrap(it)->Current();
    }

    void LibLLVMModuleComdatIteratorReset(LibLLVMComdatIteratorRef it)
    {
        unwrap(it)->Reset();
    }

    void LibLLVMDisposeComdatIterator(LibLLVMComdatIteratorRef it)
    {
        delete unwrap(it);
    }

    LLVMValueRef LibLLVMGetOrInsertFunction( LLVMModuleRef module, const char* name, LLVMTypeRef functionType )
    {
        auto pModule = unwrap( module );
        auto pSignature = cast< FunctionType >( unwrap( functionType ) );
        return wrap( pModule->getOrInsertFunction( name, pSignature ).getCallee() );
    }

    char const* LibLLVMGetModuleSourceFileName( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        return pModule->getSourceFileName( ).c_str( );
    }

    void LibLLVMSetModuleSourceFileName( LLVMModuleRef module, char const* name )
    {
        auto pModule = unwrap( module );
        pModule->setSourceFileName( name );
    }

    char const* LibLLVMGetModuleName( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        return pModule->getModuleIdentifier( ).c_str( );
    }

    LLVMValueRef LibLLVMGetGlobalAlias( LLVMModuleRef module, char const* name )
    {
        auto pModule = unwrap( module );
        return wrap( pModule->getNamedAlias( name ) );
    }

    LLVMComdatRef LibLLVMModuleInsertOrUpdateComdat( LLVMModuleRef module, char const* name, LLVMComdatSelectionKind kind )
    {
        Module* pModule = unwrap( module );
        Comdat* pComdat = pModule->getOrInsertComdat( name );
        pComdat->setSelectionKind( ( Comdat::SelectionKind ) kind );
        return wrap( pComdat );
    }

    void LibLLVMModuleComdatRemove( LLVMModuleRef module, LLVMComdatRef comdatRef )
    {
        auto pModule = unwrap( module );
        auto pComdat = unwrap( comdatRef );
        pModule->getComdatSymbolTable( ).erase( pComdat->getName( ) );
    }

    void LibLLVMModuleComdatClear( LLVMModuleRef module )
    {
        auto pModule = unwrap( module );
        pModule->getComdatSymbolTable( ).clear( );
    }

    char const* LibLLVMComdatGetName( LLVMComdatRef comdatRef )
    {
        Comdat const& comdat = *unwrap( comdatRef );
        return LLVMCreateMessage( comdat.getName( ).str( ).c_str( ) );
    }

    LLVMValueRef LibLLVMModuleGetFirstGlobalAlias( LLVMModuleRef M )
    {
        Module *Mod = unwrap( M );
        Module::alias_iterator I = Mod->alias_begin( );
        if ( I == Mod->alias_end( ) )
            return nullptr;

        return wrap( &*I );
    }

    LLVMValueRef LibLLVMModuleGetNextGlobalAlias( LLVMValueRef valueRef )
    {
        GlobalAlias *pGA = unwrap<GlobalAlias>( valueRef );
        Module::alias_iterator I( pGA );
        if ( ++I == pGA->getParent( )->alias_end( ) )
            return nullptr;

        return wrap( &*I );
    }
}
