#include <llvm-c/Core.h>
#include <llvm/TargetParser/Triple.h>
#include <llvm/Support/CBindingWrapping.h>
#include <llvm/TargetParser/ARMTargetParser.h>
#include "libllvm-c/TripleBindings.h"

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( Triple, LibLLVMTripleRef )

extern "C"
{
    LibLLVMTripleRef LibLLVMParseTriple( char const* triple )
    {
        return wrap( new Triple( Triple::normalize( triple ) ) );
    }

    void LibLLVMDisposeTriple( LibLLVMTripleRef triple )
    {
        delete unwrap( triple );
    }

    LibLLVMTripleRef LibLLVMGetHostTriple( )
    {
        return LibLLVMParseTriple( LLVM_HOST_TRIPLE );
    }

    char const* LibLLVMTripleAsString( LibLLVMTripleRef triple, LLVMBool normalize )
    {
        Triple& llvmTriple = *unwrap( triple );
        auto str = normalize ? llvmTriple.normalize( ) : llvmTriple.getTriple( );
        return LLVMCreateMessage( str.c_str( ) );
    }

    LLVMBool LibLLVMTripleOpEqual( LibLLVMTripleRef lhs, LibLLVMTripleRef rhs )
    {
        return *unwrap( lhs ) == *unwrap( rhs );
    }

    LibLLVMTripleArchType LibLLVMTripleGetArchType( LibLLVMTripleRef triple )
    {
        return ( LibLLVMTripleArchType )unwrap( triple )->getArch( );
    }

    LibLLVMTripleSubArchType LibLLVMTripleGetSubArchType( LibLLVMTripleRef triple )
    {
        return ( LibLLVMTripleSubArchType )unwrap( triple )->getSubArch( );
    }

    LibLLVMTripleVendorType LibLLVMTripleGetVendorType( LibLLVMTripleRef triple )
    {
        return ( LibLLVMTripleVendorType )unwrap( triple )->getVendor( );
    }

    LibLLVMTripleOSType LibLLVMTripleGetOsType( LibLLVMTripleRef triple )
    {
        return ( LibLLVMTripleOSType )unwrap( triple )->getOS( );
    }

    LLVMBool LibLLVMTripleHasEnvironment( LibLLVMTripleRef triple )
    {
        return unwrap( triple )->hasEnvironment( );
    }

    LibLLVMTripleEnvironmentType LibLLVMTripleGetEnvironmentType( LibLLVMTripleRef triple )
    {
        return ( LibLLVMTripleEnvironmentType )unwrap( triple )->getEnvironment( );
    }

    void LibLLVMTripleGetEnvironmentVersion( LibLLVMTripleRef triple, unsigned* major, unsigned* minor, unsigned* build )
    {
        VersionTuple ver = unwrap( triple )->getEnvironmentVersion( );
         *major = ver.getMajor();
         *minor = ver.getMinor().value_or(0);
         *build = ver.getBuild().value_or(0);
    }

    LibLLVMTripleObjectFormatType LibLLVMTripleGetObjectFormatType( LibLLVMTripleRef triple )
    {
        return ( LibLLVMTripleObjectFormatType )unwrap( triple )->getObjectFormat( );
    }

    char const* LibLLVMTripleGetArchTypeName( LibLLVMTripleArchType type )
    {
        auto llvmArchType = ( Triple::ArchType )type;
        if ( llvmArchType > Triple::ArchType::LastArchType )
        {
            llvmArchType = Triple::ArchType::UnknownArch;
        }

        return LLVMCreateMessage( Triple::getArchTypeName( llvmArchType ).data( ) );
    }

    char const* LibLLVMTripleGetVendorTypeName( LibLLVMTripleVendorType vendor )
    {
        auto llvmVendorType = ( Triple::VendorType )vendor;
        if ( llvmVendorType > Triple::VendorType::LastVendorType )
        {
            llvmVendorType = Triple::VendorType::UnknownVendor;
        }

        return LLVMCreateMessage( Triple::getVendorTypeName( llvmVendorType ).data( ) );
    }

    char const* LibLLVMTripleGetOsTypeName( LibLLVMTripleOSType osType )
    {
        auto llvmOsType = ( Triple::OSType )osType;
        if ( llvmOsType > Triple::OSType::LastOSType )
        {
            llvmOsType = Triple::OSType::UnknownOS;
        }

        return LLVMCreateMessage( Triple::getOSTypeName( llvmOsType ).data( ) );
    }

    char const* LibLLVMTripleGetEnvironmentTypeName( LibLLVMTripleEnvironmentType environmentType )
    {
        auto llvmEnvironmentType = ( Triple::EnvironmentType )environmentType;
        if ( llvmEnvironmentType > Triple::EnvironmentType::LastEnvironmentType )
        {
            llvmEnvironmentType = Triple::EnvironmentType::UnknownEnvironment;
        }

        return LLVMCreateMessage( Triple::getEnvironmentTypeName( llvmEnvironmentType ).data( ) );
    }
}
