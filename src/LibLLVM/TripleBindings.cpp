#include <llvm-c/Core.h>
#include <llvm/ADT/Triple.h>
#include <llvm/Support/CBindingWrapping.h>
#include <llvm/Support/TargetParser.h>

#include "TripleBindings.h"

using namespace llvm;

DEFINE_SIMPLE_CONVERSION_FUNCTIONS( Triple, LLVMTripleRef )

LLVMTripleSubArchType MapEnum( ARM::ArchKind from )
{
    switch( from )
    {
    case ARM::ArchKind::ARMV4:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_NoSubArch;
    case ARM::ArchKind::ARMV4T:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v4t;
    case ARM::ArchKind::ARMV5T:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v5;
    case ARM::ArchKind::ARMV5TE:
    case ARM::ArchKind::IWMMXT:
    case ARM::ArchKind::IWMMXT2:
    case ARM::ArchKind::XSCALE:
    case ARM::ArchKind::ARMV5TEJ:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v5te;
    case ARM::ArchKind::ARMV6:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v6;
    case ARM::ArchKind::ARMV6K:
    case ARM::ArchKind::ARMV6KZ:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v6k;
    case ARM::ArchKind::ARMV6T2:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v6t2;
    case ARM::ArchKind::ARMV6M:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v6m;
    case ARM::ArchKind::ARMV7A:
    case ARM::ArchKind::ARMV7R:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v7;
    case ARM::ArchKind::ARMV7K:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v7k;
    case ARM::ArchKind::ARMV7M:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v7m;
    case ARM::ArchKind::ARMV7S:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v7s;
    case ARM::ArchKind::ARMV7EM:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v7em;
    case ARM::ArchKind::ARMV8A:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v8;
    case ARM::ArchKind::ARMV8_1A:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v8_1a;
    case ARM::ArchKind::ARMV8_2A:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v8_2a;
    case ARM::ArchKind::ARMV8MBaseline:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v8m_baseline;
    case ARM::ArchKind::ARMV8MMainline:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_ARMSubArch_v8m_mainline;
    default:
        return LLVMTripleSubArchType::LlvmTripleSubArchType_NoSubArch;
    }
}

ARM::ArchKind MapEnum( LLVMTripleSubArchType from )
{
    switch( from )
    {
    case LlvmTripleSubArchType_ARMSubArch_v8_3a:
        return ARM::ArchKind::ARMV8_3A;

    case LlvmTripleSubArchType_ARMSubArch_v8_2a:
        return ARM::ArchKind::ARMV8_2A;

    case LlvmTripleSubArchType_ARMSubArch_v8_1a:
        return ARM::ArchKind::ARMV8_1A;

    case LlvmTripleSubArchType_ARMSubArch_v8:
        return ARM::ArchKind::ARMV8A;

    case LlvmTripleSubArchType_ARMSubArch_v8r:
        return ARM::ArchKind::ARMV8R;

    case LlvmTripleSubArchType_ARMSubArch_v8m_baseline:
        return ARM::ArchKind::ARMV8MBaseline;

    case LlvmTripleSubArchType_ARMSubArch_v8m_mainline:
        return ARM::ArchKind::ARMV8MMainline;

    case LlvmTripleSubArchType_ARMSubArch_v7:
        return ARM::ArchKind::ARMV7A; // 7A and 7R are mapped to v7 going the other way

    case LlvmTripleSubArchType_ARMSubArch_v7em:
        return ARM::ArchKind::ARMV7EM;

    case LlvmTripleSubArchType_ARMSubArch_v7m:
        return ARM::ArchKind::ARMV7M;

    case LlvmTripleSubArchType_ARMSubArch_v7s:
        return ARM::ArchKind::ARMV7S;

    case LlvmTripleSubArchType_ARMSubArch_v7k:
        return ARM::ArchKind::ARMV7K;

    case LlvmTripleSubArchType_ARMSubArch_v7ve:
        return ARM::ArchKind::ARMV7VE;

    case LlvmTripleSubArchType_ARMSubArch_v6:
        return ARM::ArchKind::ARMV6;

    case LlvmTripleSubArchType_ARMSubArch_v6m:
        return ARM::ArchKind::ARMV6M;

    case LlvmTripleSubArchType_ARMSubArch_v6k:
        return ARM::ArchKind::ARMV6K; // 6K and 6Kz are mapped to 6K going the other way

    case LlvmTripleSubArchType_ARMSubArch_v6t2:
        return ARM::ArchKind::ARMV6T2;

    case LlvmTripleSubArchType_ARMSubArch_v5:
        return ARM::ArchKind::ARMV5T;

    case LlvmTripleSubArchType_ARMSubArch_v5te:
        return ARM::ArchKind::ARMV5TE;

    case LlvmTripleSubArchType_ARMSubArch_v4t:
        return ARM::ArchKind::ARMV4T;

    case LlvmTripleSubArchType_NoSubArch:
    case LlvmTripleSubArchType_KalimbaSubArch_v3:
    case LlvmTripleSubArchType_KalimbaSubArch_v4:
    case LlvmTripleSubArchType_KalimbaSubArch_v5:
    default:
        return ARM::ArchKind::INVALID;
    }
}

char const* LLVMNormalizeTriple( char const* triple )
{
    return LLVMCreateMessage( Triple::normalize( triple ).c_str( ) );
}

LLVMTripleRef LLVMParseTriple( char const* triple )
{
    return wrap( new Triple( Triple::normalize( triple ) ) );
}

void LLVMDisposeTriple( LLVMTripleRef triple )
{
    delete unwrap( triple );
}

LLVMTripleRef LLVMGetHostTriple( )
{
    return LLVMParseTriple( LLVM_HOST_TRIPLE );
}

char const* LLVMTripleAsString( LLVMTripleRef triple, bool normalize )
{
    Triple& llvmTriple = *unwrap( triple );
    auto str = normalize ? llvmTriple.normalize() : llvmTriple.getTriple( );
    return LLVMCreateMessage( str.c_str( ) );
}

LLVMBool LLVMTripleOpEqual( LLVMTripleRef lhs, LLVMTripleRef rhs )
{
    return *unwrap( lhs ) == *unwrap( rhs );
}

LLVMTripleArchType LLVMTripleGetArchType( LLVMTripleRef triple )
{
    return ( LLVMTripleArchType )unwrap( triple )->getArch( );
}

LLVMTripleSubArchType LLVMTripleGetSubArchType( LLVMTripleRef triple )
{
    return ( LLVMTripleSubArchType )unwrap( triple )->getSubArch( );
}

LLVMTripleVendorType LLVMTripleGetVendorType( LLVMTripleRef triple )
{
    return ( LLVMTripleVendorType )unwrap( triple )->getVendor( );
}

LLVMTripleOSType LLVMTripleGetOsType( LLVMTripleRef triple )
{
    return ( LLVMTripleOSType )unwrap( triple )->getOS( );
}

LLVMBool LLVMTripleHasEnvironment( LLVMTripleRef triple )
{
    return unwrap( triple )->hasEnvironment( );
}

LLVMTripleEnvironmentType LLVMTripleGetEnvironmentType( LLVMTripleRef triple )
{
    return ( LLVMTripleEnvironmentType )unwrap( triple )->getEnvironment( );
}

void LLVMTripleGetEnvironmentVersion( LLVMTripleRef triple, unsigned* major, unsigned* minor, unsigned* micro )
{
    unwrap( triple )->getEnvironmentVersion( *major, *minor, *micro );
}

LLVMTripleObjectFormatType LLVMTripleGetObjectFormatType( LLVMTripleRef triple )
{
    return ( LLVMTripleObjectFormatType )unwrap( triple )->getObjectFormat( );
}

char const* LLVMTripleGetArchTypeName( LLVMTripleArchType type )
{
    auto llvmArchType = ( Triple::ArchType )type;
    if( llvmArchType > Triple::ArchType::LastArchType )
        llvmArchType = Triple::ArchType::UnknownArch;

    return LLVMCreateMessage( Triple::getArchTypeName( llvmArchType ).data() );
}

char const* LLVMTripleGetSubArchTypeName( LLVMTripleSubArchType type )
{
    ARM::ArchKind armKind = MapEnum( type );
    if( armKind == ARM::ArchKind::INVALID )
    {
        switch( type )
        {
        case LLVMTripleSubArchType::LlvmTripleSubArchType_KalimbaSubArch_v3:
            return LLVMCreateMessage( "kalimba3" );
        case LLVMTripleSubArchType::LlvmTripleSubArchType_KalimbaSubArch_v4:
            return LLVMCreateMessage( "kalimba4" );
        case LLVMTripleSubArchType::LlvmTripleSubArchType_KalimbaSubArch_v5:
            return LLVMCreateMessage( "kalimba5" );
        default:
            return nullptr;
        }
    }

    return LLVMCreateMessage( ARM::getSubArch( armKind ).begin() );
}

char const* LLVMTripleGetVendorTypeName( LLVMTripleVendorType vendor )
{
    auto llvmVendorType = ( Triple::VendorType )vendor;
    if( llvmVendorType > Triple::VendorType::LastVendorType )
        llvmVendorType = Triple::VendorType::UnknownVendor;

    return LLVMCreateMessage( Triple::getVendorTypeName( llvmVendorType ).data() );
}

char const* LLVMTripleGetOsTypeName( LLVMTripleOSType osType )
{
    auto llvmOsType = ( Triple::OSType )osType;
    if( llvmOsType > Triple::OSType::LastOSType )
        llvmOsType = Triple::OSType::UnknownOS;

    return LLVMCreateMessage( Triple::getOSTypeName( llvmOsType ).data() );
}

char const* LLVMTripleGetEnvironmentTypeName( LLVMTripleEnvironmentType environmentType )
{
    auto llvmEnvironmentType = ( Triple::EnvironmentType )environmentType;
    if( llvmEnvironmentType > Triple::EnvironmentType::LastEnvironmentType )
        llvmEnvironmentType = Triple::EnvironmentType::UnknownEnvironment;

    return LLVMCreateMessage( Triple::getEnvironmentTypeName( llvmEnvironmentType ).data() );
}

char const* LLVMTripleGetObjectFormatTypeName( LLVMTripleObjectFormatType objectFormatType )
{

    auto llvmObjectFormatType = ( Triple::ObjectFormatType )objectFormatType;
    // llvm::Triple doesn't have an equivalent of this for ObjectFormatType
    switch( llvmObjectFormatType )
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

