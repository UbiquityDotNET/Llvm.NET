using Llvm.NET.Native;
using System;

namespace Llvm.NET
{
    /// <summary>Triple to describe a target</summary>
    /// <remarks>
    /// <para>The term 'Triple' is a bit of a misnomer. At some point in the past it
    /// actually consisted of only three parts, but that has changed over the 
    /// years without the name itself changing. The triple is normally represented
    /// as a string of 4 components delimited by '-'. Some of the components have
    /// sub components as part of the content. The canonical form of a triple is:
    /// <c>{Architecture}{SubArchitecture}-{Vendor}-{OS}-{Environment}{ObjectFormat}</c></para>
    /// <para>
    /// A few shorthand variations are allowed and converted to their full normalized form.
    /// In particular "cygwin" is a shorthand for the OS-Environment tuple "windows-cygnus"
    /// and "mingw" is a shorthand form of "windows-gnu".
    /// </para>
    /// <para>In addition to shorthand allowances, the OS component may optionally include
    /// a trailing version of the form Maj.Min.Micro. If any of the version number parts are
    /// not present, then they default to 0.</para>
    /// <para>
    /// For the environment "androideabi" is allowed and normalized to android (including
    /// an optional version number).
    /// </para>
    /// </remarks>
    public sealed class Triple
        : IEquatable<Triple>
    {
        /// <summary>Constructs a new <see cref="Triple"/> instance from a triple string</summary>
        /// <param name="tripleTxt">Triple string to parse</param>
        /// <remarks>
        /// The <paramref name="tripleTxt"/> string is normalized before parsing to allow for
        /// common non-canonical forms of triples.
        /// </remarks>
        public Triple( string tripleTxt )
        {
            TripleHandle = NativeMethods.ParseTriple( tripleTxt );
        }

        /// <summary>Retrieves the final string form of the triple</summary>
        /// <returns>Normalized Triple string</returns>
        public override string ToString( ) => NativeMethods.MarshalMsg( NativeMethods.TripleAsString( TripleHandle, true ) );

        /// <summary>Architecture of the triple</summary>
        public TripleArchType ArchitectureType => ( TripleArchType )NativeMethods.TripleGetArchType( TripleHandle );

        /// <summary>Sub Architecture type</summary>
        public TripleSubArchType SubArchitecture => ( TripleSubArchType)NativeMethods.TripleGetSubArchType( TripleHandle );

        /// <summary>Vendor component of the triple</summary>
        public TripleVendorType VendorType => ( TripleVendorType ) NativeMethods.TripleGetVendorType( TripleHandle );

        /// <summary>OS Type for the triple</summary>
        public TripleOSType OSType => ( TripleOSType ) NativeMethods.TripleGetOsType( TripleHandle );

        /// <summary>Environment type for the triple</summary>
        public TripleEnvironmentType EnvironmentType => ( TripleEnvironmentType ) NativeMethods.TripleGetEnvironmentType( TripleHandle );

        /// <summary>Object format type for the triple</summary>
        public TripleObjectFormatType ObjectFormatType => ( TripleObjectFormatType ) NativeMethods.TripleGetObjectFormatType( TripleHandle );

        /// <summary>Retrieves the canonical name for an architecture type</summary>
        /// <param name="archType">Architecture type</param>
        /// <returns>String name for the architecture</returns>
        /// <overloads>
        /// Many parts of a triple can take a variety of literal string
        /// forms to allow for common real world triples when parsing.
        /// The GetCanonicalName methods provide the canonical form of
        /// such triple components used in a normalized triple.
        /// </overloads>
        public static string GetCanonicalName( TripleArchType archType )
            => NativeMethods.MarshalMsg( NativeMethods.TripleGetArchTypeName( ( LLVMTripleArchType )archType ) );

        /// <summary>Retrieves the canonical name for an architecture sub type</summary>
        /// <param name="subArchType">Architecture sub type</param>
        /// <returns>String name for the architecture sub type</returns>
        public static string GetCanonicalName( TripleSubArchType subArchType )
            => NativeMethods.MarshalMsg( NativeMethods.TripleGetSubArchTypeName( ( LLVMTripleSubArchType )subArchType ) );

        /// <summary>Retrieves the canonical name for the vendor component of a triple</summary>
        /// <param name="vendorType">Vendor type</param>
        /// <returns>String name for the vendor</returns>
        public static string GetCanonicalName( TripleVendorType vendorType )
            => NativeMethods.MarshalMsg( NativeMethods.TripleGetVendorTypeName( ( LLVMTripleVendorType )vendorType ) );

        /// <summary>Retrieves the canonical name for the OS component of a triple</summary>
        /// <param name="osType">OS type</param>
        /// <returns>String name for the OS</returns>
        public static string GetCanonicalName( TripleOSType osType )
            => NativeMethods.MarshalMsg( NativeMethods.TripleGetOsTypeName( ( LLVMTripleOSType )osType ) );


        /// <summary>Retrieves the canonical name for the environment component of a triple</summary>
        /// <param name="envType">Environment type</param>
        /// <returns>String name for the environment component</returns>
        public static string GetCanonicalName( TripleEnvironmentType envType )
            => NativeMethods.MarshalMsg( NativeMethods.TripleGetEnvironmentTypeName( (LLVMTripleEnvironmentType)envType ) );

        /// <summary>Retrieves the canonical name for the object component of a triple</summary>
        /// <param name="objFormatType">Object type</param>
        /// <returns>String name for the object component</returns>
        public static string GetCanonicalName( TripleObjectFormatType objFormatType )
            => NativeMethods.MarshalMsg( NativeMethods.TripleGetObjectFormatTypeName( ( LLVMTripleObjectFormatType )objFormatType ) );

        /// <summary>Equality test for a triple</summary>
        /// <param name="other">triple to compare this triple to</param>
        /// <returns><see langword="true"/> if the two triples are equivalent</returns>
        public bool Equals( Triple other )
        {
            if( other == null )
                return false;

            if( ReferenceEquals( this, other ) )
                return true;

            return NativeMethods.TripleOpEqual( TripleHandle, other.TripleHandle );
        }

        /// <summary>Equality test for a triple</summary>
        /// <param name="obj">object to compare this triple to</param>
        /// <returns><see langword="true"/> if the two triples are equivalent</returns>
        public override bool Equals( object obj )
        {
            return Equals( obj as Triple );
        }

        /// <inheritdoc/>
        public override int GetHashCode( )
        {
            return ToString( ).GetHashCode( );
        }

        /// <summary>Normalizes a triple string</summary>
        /// <param name="unNormalizedTriple">triple to normalize</param>
        /// <returns>Normalized string</returns>
        public static string Normalize( string unNormalizedTriple )
        {
            return NativeMethods.MarshalMsg( NativeMethods.NormalizeTriple( unNormalizedTriple ) );
        }

        /// <summary>Gets the default <see cref="TripleObjectFormatType"/> for a given <see cref="TripleArchType"/> and <see cref="TripleOSType"/></summary>
        /// <param name="arch">Architecture type</param>
        /// <param name="os">Operating system type</param>
        /// <returns>Default object format</returns>
        public static TripleObjectFormatType GetDefaultObjectFormat( TripleArchType arch, TripleOSType os )
        {
            switch( arch )
            {
            case TripleArchType.UnknownArch:
            case TripleArchType.aarch64:
            case TripleArchType.arm:
            case TripleArchType.thumb:
            case TripleArchType.x86:
            case TripleArchType.x86_64:
                if( IsOsDarwin( os ) )
                    return TripleObjectFormatType.MachO;
                else if( os == TripleOSType.Win32 )
                    return TripleObjectFormatType.COFF;
                return TripleObjectFormatType.ELF;

            case TripleArchType.aarch64_be:
            case TripleArchType.amdgcn:
            case TripleArchType.amdil:
            case TripleArchType.amdil64:
            case TripleArchType.armeb:
            case TripleArchType.avr:
            case TripleArchType.bpfeb:
            case TripleArchType.bpfel:
            case TripleArchType.hexagon:
            case TripleArchType.lanai:
            case TripleArchType.hsail:
            case TripleArchType.hsail64:
            case TripleArchType.kalimba:
            case TripleArchType.le32:
            case TripleArchType.le64:
            case TripleArchType.mips:
            case TripleArchType.mips64:
            case TripleArchType.mips64el:
            case TripleArchType.mipsel:
            case TripleArchType.msp430:
            case TripleArchType.nvptx:
            case TripleArchType.nvptx64:
            case TripleArchType.ppc64le:
            case TripleArchType.r600:
            case TripleArchType.renderscript32:
            case TripleArchType.renderscript64:
            case TripleArchType.shave:
            case TripleArchType.sparc:
            case TripleArchType.sparcel:
            case TripleArchType.sparcv9:
            case TripleArchType.spir:
            case TripleArchType.spir64:
            case TripleArchType.systemz:
            case TripleArchType.tce:
            case TripleArchType.thumbeb:
            case TripleArchType.wasm32:
            case TripleArchType.wasm64:
            case TripleArchType.xcore:
                return TripleObjectFormatType.ELF;

            case TripleArchType.ppc:
            case TripleArchType.ppc64:
                if( IsOsDarwin( os ) )
                    return TripleObjectFormatType.MachO;

                return TripleObjectFormatType.ELF;

            default:
                throw new ArgumentException("Unsupported Architecture", nameof( arch ) );
            }
        }

        /// <summary>Provides the canonical Architecture form for a given architecture sub architecture pair</summary>
        /// <param name="archType">Architecture type</param>
        /// <param name="subArch">Sub Architecture type</param>
        /// <returns>Canonical <see cref="TripleArchType"/></returns>
        /// <remarks>
        /// Some architectures, particularly ARM variants, have multiple sub-architecture types that
        /// have a canonical form (i.e. Arch=<see cref="TripleArchType.arm"/>; SubArch=<see cref="TripleSubArchType.ARMSubArch_v7m"/>;
        /// has the Canonical Arch of <see cref="TripleArchType.thumb"/>). This method retrieves the canonical Arch
        /// for a given architecture,SubArchitecture pair.
        /// </remarks>
        public static TripleArchType GetCanonicalArchForSubArch( TripleArchType archType, TripleSubArchType subArch )
        {
            switch( archType )
            {
            case TripleArchType.kalimba:
                switch( subArch )
                {
                case TripleSubArchType.NoSubArch:
                case TripleSubArchType.KalimbaSubArch_v3:
                case TripleSubArchType.KalimbaSubArch_v4:
                case TripleSubArchType.KalimbaSubArch_v5:
                    return TripleArchType.kalimba;

                default:
                    return TripleArchType.UnknownArch;
                }
            case TripleArchType.arm:
            case TripleArchType.armeb:
                switch( subArch )
                {
                case TripleSubArchType.ARMSubArch_v6m:
                    return archType == TripleArchType.armeb ? TripleArchType.thumbeb : TripleArchType.thumb;
                case TripleSubArchType.KalimbaSubArch_v3:
                case TripleSubArchType.KalimbaSubArch_v4:
                case TripleSubArchType.KalimbaSubArch_v5:
                    return TripleArchType.UnknownArch;

                default:
                    return archType;
                }
            default:
                return archType;
            }
        }

        private static bool IsOsDarwin( TripleOSType osType )
        {
            switch( osType )
            {
            case TripleOSType.Darwin:
            case TripleOSType.MacOSX:
            case TripleOSType.IOS:
            case TripleOSType.TvOS:
            case TripleOSType.WatchOS:
                return true;

            default:
                return false;
            }
        }

        ~Triple()
        {
            TripleHandle.Dispose( );
        }

        Llvm.NET.Native.TripleHandle TripleHandle;
    }
}
