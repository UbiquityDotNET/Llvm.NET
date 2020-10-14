#include <llvm-c/Core.h>
#include <llvm/ADT/Triple.h>
#include <llvm/Support/CBindingWrapping.h>
#include <llvm/Support/TargetParser.h>
#include <llvm/Config/llvm-config.h>

#include "libllvm-c/TripleBindings.h"

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( Triple, LibLLVMTripleRef )

// cribbed from internals of Triple::SubArchType parseSubArch()
LibLLVMTripleSubArchType MapEnum( ARM::ArchKind from )
{
    switch ( from )
    {
    case ARM::ArchKind::ARMV4:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_NoSubArch;
    case ARM::ArchKind::ARMV4T:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v4t;
    case ARM::ArchKind::ARMV5T:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v5;
    case ARM::ArchKind::ARMV5TE:
    case ARM::ArchKind::IWMMXT:
    case ARM::ArchKind::IWMMXT2:
    case ARM::ArchKind::XSCALE:
    case ARM::ArchKind::ARMV5TEJ:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v5te;
    case ARM::ArchKind::ARMV6:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v6;
    case ARM::ArchKind::ARMV6K:
    case ARM::ArchKind::ARMV6KZ:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v6k;
    case ARM::ArchKind::ARMV6T2:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v6t2;
    case ARM::ArchKind::ARMV6M:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v6m;
    case ARM::ArchKind::ARMV7A:
    case ARM::ArchKind::ARMV7R:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v7;
    case ARM::ArchKind::ARMV7VE:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v7ve;
    case ARM::ArchKind::ARMV7K:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v7k;
    case ARM::ArchKind::ARMV7M:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v7m;
    case ARM::ArchKind::ARMV7S:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v7s;
    case ARM::ArchKind::ARMV7EM:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v7em;
    case ARM::ArchKind::ARMV8A:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8;
    case ARM::ArchKind::ARMV8_1A:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8_1a;
    case ARM::ArchKind::ARMV8_2A:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8_2a;
    case ARM::ArchKind::ARMV8_3A:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8_3a;
    case ARM::ArchKind::ARMV8_4A:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8_4a;
    case ARM::ArchKind::ARMV8_5A:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8_5a;
    case ARM::ArchKind::ARMV8R:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8r;
    case ARM::ArchKind::ARMV8MBaseline:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8m_baseline;
    case ARM::ArchKind::ARMV8MMainline:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8m_mainline;
    case ARM::ArchKind::ARMV8_1MMainline:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_ARMSubArch_v8_1m_mainline;
    default:
        return LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_NoSubArch;
    }
}

// Created by effectively inverting the mapping function above
ARM::ArchKind MapEnum( LibLLVMTripleSubArchType from )
{
    switch ( from )
    {
    case LibLLVMTripleSubArchType_ARMSubArch_v8_5a:
        return ARM::ArchKind::ARMV8_5A;

    case LibLLVMTripleSubArchType_ARMSubArch_v8_4a:
        return ARM::ArchKind::ARMV8_4A;

    case LibLLVMTripleSubArchType_ARMSubArch_v8_3a:
        return ARM::ArchKind::ARMV8_3A;

    case LibLLVMTripleSubArchType_ARMSubArch_v8_2a:
        return ARM::ArchKind::ARMV8_2A;

    case LibLLVMTripleSubArchType_ARMSubArch_v8_1a:
        return ARM::ArchKind::ARMV8_1A;

    case LibLLVMTripleSubArchType_ARMSubArch_v8:
        return ARM::ArchKind::ARMV8A;

    case LibLLVMTripleSubArchType_ARMSubArch_v8r:
        return ARM::ArchKind::ARMV8R;

    case LibLLVMTripleSubArchType_ARMSubArch_v8m_baseline:
        return ARM::ArchKind::ARMV8MBaseline;

    case LibLLVMTripleSubArchType_ARMSubArch_v8m_mainline:
        return ARM::ArchKind::ARMV8MMainline;

    case LibLLVMTripleSubArchType_ARMSubArch_v8_1m_mainline:
        return ARM::ArchKind::ARMV8_1MMainline;

    case LibLLVMTripleSubArchType_ARMSubArch_v7:
        return ARM::ArchKind::ARMV7A; // 7A and 7R are mapped to v7 going the other way

    case LibLLVMTripleSubArchType_ARMSubArch_v7em:
        return ARM::ArchKind::ARMV7EM;

    case LibLLVMTripleSubArchType_ARMSubArch_v7m:
        return ARM::ArchKind::ARMV7M;

    case LibLLVMTripleSubArchType_ARMSubArch_v7s:
        return ARM::ArchKind::ARMV7S;

    case LibLLVMTripleSubArchType_ARMSubArch_v7k:
        return ARM::ArchKind::ARMV7K;

    case LibLLVMTripleSubArchType_ARMSubArch_v7ve:
        return ARM::ArchKind::ARMV7VE;

    case LibLLVMTripleSubArchType_ARMSubArch_v6:
        return ARM::ArchKind::ARMV6;

    case LibLLVMTripleSubArchType_ARMSubArch_v6m:
        return ARM::ArchKind::ARMV6M;

    case LibLLVMTripleSubArchType_ARMSubArch_v6k:
        return ARM::ArchKind::ARMV6K; // 6K and 6Kz are mapped to 6K going the other way

    case LibLLVMTripleSubArchType_ARMSubArch_v6t2:
        return ARM::ArchKind::ARMV6T2;

    case LibLLVMTripleSubArchType_ARMSubArch_v5:
        return ARM::ArchKind::ARMV5T;

    case LibLLVMTripleSubArchType_ARMSubArch_v5te:
        return ARM::ArchKind::ARMV5TE;

    case LibLLVMTripleSubArchType_ARMSubArch_v4t:
        return ARM::ArchKind::ARMV4T;

    case LibLLVMTripleSubArchType_NoSubArch:
    case LibLLVMTripleSubArchType_KalimbaSubArch_v3:
    case LibLLVMTripleSubArchType_KalimbaSubArch_v4:
    case LibLLVMTripleSubArchType_KalimbaSubArch_v5:
    default:
        return ARM::ArchKind::INVALID;
    }
}
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

    char const* LibLLVMTripleAsString( LibLLVMTripleRef triple, bool normalize )
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

    void LibLLVMTripleGetEnvironmentVersion( LibLLVMTripleRef triple, unsigned* major, unsigned* minor, unsigned* micro )
    {
        unwrap( triple )->getEnvironmentVersion( *major, *minor, *micro );
    }

    LibLLVMTripleObjectFormatType LibLLVMTripleGetObjectFormatType( LibLLVMTripleRef triple )
    {
        return ( LibLLVMTripleObjectFormatType )unwrap( triple )->getObjectFormat( );
    }

    char const* LibLLVMTripleGetArchTypeName( LibLLVMTripleArchType type )
    {
        auto llvmArchType = ( Triple::ArchType )type;
        if ( llvmArchType > Triple::ArchType::LastArchType )
            llvmArchType = Triple::ArchType::UnknownArch;

        return LLVMCreateMessage( Triple::getArchTypeName( llvmArchType ).data( ) );
    }

    char const* LibLLVMTripleGetSubArchTypeName( LibLLVMTripleSubArchType subArchType )
    {
        // try for an ARM sub type first...
        ARM::ArchKind armKind = MapEnum( subArchType );
        if ( armKind == ARM::ArchKind::INVALID )
        {
            switch ( subArchType )
            {
            case LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_KalimbaSubArch_v3:
                return LLVMCreateMessage( "kalimba3" );
            case LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_KalimbaSubArch_v4:
                return LLVMCreateMessage( "kalimba4" );
            case LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_KalimbaSubArch_v5:
                return LLVMCreateMessage( "kalimba5" );
            case LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_MipsSubArch_r6:
                return LLVMCreateMessage( "mipsr6" );
            case LibLLVMTripleSubArchType::LibLLVMTripleSubArchType_PPCSubArch_spe:
                return LLVMCreateMessage( "powerpcspe" );
            default:
                return nullptr;
            }
        }

        return LLVMCreateMessage( ARM::getSubArch( armKind ).begin( ) );
    }

    char const* LibLLVMTripleGetVendorTypeName( LibLLVMTripleVendorType vendor )
    {
        auto llvmVendorType = ( Triple::VendorType )vendor;
        if ( llvmVendorType > Triple::VendorType::LastVendorType )
            llvmVendorType = Triple::VendorType::UnknownVendor;

        return LLVMCreateMessage( Triple::getVendorTypeName( llvmVendorType ).data( ) );
    }

    char const* LibLLVMTripleGetOsTypeName( LibLLVMTripleOSType osType )
    {
        auto llvmOsType = ( Triple::OSType )osType;
        if ( llvmOsType > Triple::OSType::LastOSType )
            llvmOsType = Triple::OSType::UnknownOS;

        return LLVMCreateMessage( Triple::getOSTypeName( llvmOsType ).data( ) );
    }

    char const* LibLLVMTripleGetEnvironmentTypeName( LibLLVMTripleEnvironmentType environmentType )
    {
        auto llvmEnvironmentType = ( Triple::EnvironmentType )environmentType;
        if ( llvmEnvironmentType > Triple::EnvironmentType::LastEnvironmentType )
            llvmEnvironmentType = Triple::EnvironmentType::UnknownEnvironment;

        return LLVMCreateMessage( Triple::getEnvironmentTypeName( llvmEnvironmentType ).data( ) );
    }

    char const* LibLLVMTripleGetObjectFormatTypeName( LibLLVMTripleObjectFormatType objectFormatType )
    {
        auto llvmObjectFormatType = ( Triple::ObjectFormatType )objectFormatType;

        // llvm::Triple doesn't have an equivalent of this for ObjectFormatType
        switch ( llvmObjectFormatType )
        {
        case Triple::ObjectFormatType::COFF:
            return LLVMCreateMessage( "coff" );
        case Triple::ObjectFormatType::ELF:
            return LLVMCreateMessage( "elf" );
        case Triple::ObjectFormatType::MachO:
            return LLVMCreateMessage( "macho" );
        default:
            return LLVMCreateMessage( "" );
        }
    }
}
