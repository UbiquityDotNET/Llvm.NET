// <copyright file="Triple.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Native;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Triple to describe a target</summary>
    /// <remarks>
    /// <para>The term 'Triple' is a bit of a misnomer. At some point in the past it
    /// actually consisted of only three parts, but that has changed over the years
    /// without the name itself changing. The triple is normally represented as a
    /// string of 4 components delimited by '-'. Some of the components have
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
        /// <summary>Initializes a new instance of the <see cref="Triple"/> class from a triple string</summary>
        /// <param name="tripleTxt">Triple string to parse</param>
        /// <remarks>
        /// The <paramref name="tripleTxt"/> string is normalized before parsing to allow for
        /// common non-canonical forms of triples.
        /// </remarks>
        public Triple( string tripleTxt )
        {
            TripleHandle = LLVMParseTriple( tripleTxt );
        }

        /// <summary>Retrieves the final string form of the triple</summary>
        /// <returns>Normalized Triple string</returns>
        public override string ToString( ) => LLVMTripleAsString( TripleHandle, true );

        /// <summary>Gets the Architecture of the triple</summary>
        public TripleArchType ArchitectureType => ( TripleArchType )LLVMTripleGetArchType( TripleHandle );

        /// <summary>Gets the Sub Architecture type</summary>
        public TripleSubArchType SubArchitecture => ( TripleSubArchType )LLVMTripleGetSubArchType( TripleHandle );

        /// <summary>Gets the Vendor component of the triple</summary>
        public TripleVendorType VendorType => ( TripleVendorType )LLVMTripleGetVendorType( TripleHandle );

        /// <summary>Gets the OS Type for the triple</summary>
        public TripleOSType OSType => ( TripleOSType )LLVMTripleGetOsType( TripleHandle );

        /// <summary>Gets the environment type for the triple</summary>
        public TripleEnvironmentType EnvironmentType => ( TripleEnvironmentType )LLVMTripleGetEnvironmentType( TripleHandle );

        /// <summary>Gets the object format type for the triple</summary>
        public TripleObjectFormatType ObjectFormatType => ( TripleObjectFormatType )LLVMTripleGetObjectFormatType( TripleHandle );

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
            => LLVMTripleGetArchTypeName( ( LLVMTripleArchType )archType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for an architecture sub type</summary>
        /// <param name="subArchType">Architecture sub type</param>
        /// <returns>String name for the architecture sub type</returns>
        public static string GetCanonicalName( TripleSubArchType subArchType )
            => LLVMTripleGetSubArchTypeName( ( LLVMTripleSubArchType )subArchType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the vendor component of a triple</summary>
        /// <param name="vendorType">Vendor type</param>
        /// <returns>String name for the vendor</returns>
        public static string GetCanonicalName( TripleVendorType vendorType )
            => LLVMTripleGetVendorTypeName( ( LLVMTripleVendorType )vendorType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the OS component of a triple</summary>
        /// <param name="osType">OS type</param>
        /// <returns>String name for the OS</returns>
        public static string GetCanonicalName( TripleOSType osType )
            => LLVMTripleGetOsTypeName( ( LLVMTripleOSType )osType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the environment component of a triple</summary>
        /// <param name="envType">Environment type</param>
        /// <returns>String name for the environment component</returns>
        public static string GetCanonicalName( TripleEnvironmentType envType )
            => LLVMTripleGetEnvironmentTypeName( ( LLVMTripleEnvironmentType )envType ) ?? string.Empty;

        /// <summary>Retrieves the canonical name for the object component of a triple</summary>
        /// <param name="objFormatType">Object type</param>
        /// <returns>String name for the object component</returns>
        public static string GetCanonicalName( TripleObjectFormatType objFormatType )
            => LLVMTripleGetObjectFormatTypeName( ( LLVMTripleObjectFormatType )objFormatType ) ?? string.Empty;

        /// <summary>Equality test for a triple</summary>
        /// <param name="other">triple to compare this triple to</param>
        /// <returns><see langword="true"/> if the two triples are equivalent</returns>
        public bool Equals( Triple other )
        {
            if( other == null )
            {
                return false;
            }

            if( ReferenceEquals( this, other ) )
            {
                return true;
            }

            return LLVMTripleOpEqual( TripleHandle, other.TripleHandle );
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
            unNormalizedTriple.ValidateNotNullOrWhiteSpace( nameof( unNormalizedTriple ) );

            return LLVMNormalizeTriple( unNormalizedTriple );
        }

        /// <summary>Gets the default <see cref="TripleObjectFormatType"/> for a given <see cref="TripleArchType"/> and <see cref="TripleOSType"/></summary>
        /// <param name="arch">Architecture type</param>
        /// <param name="os">Operating system type</param>
        /// <returns>Default object format</returns>
        public static TripleObjectFormatType GetDefaultObjectFormat( TripleArchType arch, TripleOSType os )
        {
            arch.ValidateDefined( nameof( arch ) );
            os.ValidateDefined( nameof( os ) );

            switch( arch )
            {
            case TripleArchType.UnknownArch:
            case TripleArchType.Aarch64:
            case TripleArchType.Arm:
            case TripleArchType.Thumb:
            case TripleArchType.X86:
            case TripleArchType.X86_64:
                if( IsOsDarwin( os ) )
                {
                    return TripleObjectFormatType.MachO;
                }

                if( os == TripleOSType.Win32 )
                {
                    return TripleObjectFormatType.COFF;
                }

                return TripleObjectFormatType.ELF;

            case TripleArchType.Aarch64_be:
            case TripleArchType.AMDGCN:
            case TripleArchType.Amdil:
            case TripleArchType.Amdil64:
            case TripleArchType.Armeb:
            case TripleArchType.Avr:
            case TripleArchType.BPFeb:
            case TripleArchType.BPFel:
            case TripleArchType.Hexagon:
            case TripleArchType.Lanai:
            case TripleArchType.Hsail:
            case TripleArchType.Hsail64:
            case TripleArchType.Kalimba:
            case TripleArchType.Le32:
            case TripleArchType.Le64:
            case TripleArchType.MIPS:
            case TripleArchType.MIPS64:
            case TripleArchType.MIPS64el:
            case TripleArchType.MIPSel:
            case TripleArchType.MSP430:
            case TripleArchType.Nvptx:
            case TripleArchType.Nvptx64:
            case TripleArchType.PPC64le:
            case TripleArchType.R600:
            case TripleArchType.Renderscript32:
            case TripleArchType.Renderscript64:
            case TripleArchType.Shave:
            case TripleArchType.Sparc:
            case TripleArchType.Sparcel:
            case TripleArchType.Sparcv9:
            case TripleArchType.Spir:
            case TripleArchType.Spir64:
            case TripleArchType.SystemZ:
            case TripleArchType.TCE:
            case TripleArchType.Thumbeb:
            case TripleArchType.Wasm32:
            case TripleArchType.Wasm64:
            case TripleArchType.Xcore:
                return TripleObjectFormatType.ELF;

            case TripleArchType.PPC:
            case TripleArchType.PPC64:
                if( IsOsDarwin( os ) )
                {
                    return TripleObjectFormatType.MachO;
                }

                return TripleObjectFormatType.ELF;

            default:
                throw new ArgumentException( "Unsupported Architecture", nameof( arch ) );
            }
        }

        /// <summary>Provides the canonical Architecture form for a given architecture sub architecture pair</summary>
        /// <param name="archType">Architecture type</param>
        /// <param name="subArch">Sub Architecture type</param>
        /// <returns>Canonical <see cref="TripleArchType"/></returns>
        /// <remarks>
        /// Some architectures, particularly ARM variants, have multiple sub-architecture types that
        /// have a canonical form (i.e. Arch=<see cref="TripleArchType.Arm"/>; SubArch=<see cref="TripleSubArchType.ARMSubArch_v7m"/>;
        /// has the Canonical Arch of <see cref="TripleArchType.Thumb"/>). This method retrieves the canonical Arch
        /// for a given architecture,SubArchitecture pair.
        /// </remarks>
        public static TripleArchType GetCanonicalArchForSubArch( TripleArchType archType, TripleSubArchType subArch )
        {
            archType.ValidateDefined( nameof( archType ) );
            subArch.ValidateDefined( nameof( subArch ) );
            switch( archType )
            {
            case TripleArchType.Kalimba:
                switch( subArch )
                {
                case TripleSubArchType.NoSubArch:
                case TripleSubArchType.KalimbaSubArch_v3:
                case TripleSubArchType.KalimbaSubArch_v4:
                case TripleSubArchType.KalimbaSubArch_v5:
                    return TripleArchType.Kalimba;

                default:
                    return TripleArchType.UnknownArch;
                }

            case TripleArchType.Arm:
            case TripleArchType.Armeb:
                switch( subArch )
                {
                case TripleSubArchType.ARMSubArch_v6m:
                    return archType == TripleArchType.Armeb ? TripleArchType.Thumbeb : TripleArchType.Thumb;
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

        /// <summary>Gets a triple for the host LLVM is built for</summary>
        public static Triple HostTriple => new Triple( LLVMGetHostTriple( ) );

        /// <summary>Implicitly converts a triple to a string</summary>
        /// <param name="triple"><see cref="Triple"/> to convert</param>
        public static implicit operator string(Triple triple) => triple.ToString();

        private Triple( LLVMTripleRef handle )
        {
            TripleHandle = handle;
        }

        private static bool IsOsDarwin( TripleOSType osType )
        {
            osType.ValidateDefined( nameof( osType ) );
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

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMTripleRef LLVMParseTriple( [MarshalAs( UnmanagedType.LPStr )] string triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl)]
        private static extern LLVMTripleRef LLVMGetHostTriple( );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static extern bool LLVMTripleOpEqual( LLVMTripleRef lhs, LLVMTripleRef rhs );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMTripleArchType LLVMTripleGetArchType( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMTripleSubArchType LLVMTripleGetSubArchType( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMTripleVendorType LLVMTripleGetVendorType( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMTripleOSType LLVMTripleGetOsType( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.Bool )]
        private static extern bool LLVMTripleHasEnvironment( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMTripleEnvironmentType LLVMTripleGetEnvironmentType( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern void LLVMTripleGetEnvironmentVersion( LLVMTripleRef triple, out UInt32 major, out UInt32 minor, out UInt32 micro );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        private static extern LLVMTripleObjectFormatType LLVMTripleGetObjectFormatType( LLVMTripleRef triple );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        private static extern string LLVMTripleAsString( LLVMTripleRef triple, [MarshalAs( UnmanagedType.U1 )]bool normalize );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        private static extern string LLVMTripleGetArchTypeName( LLVMTripleArchType type );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        private static extern string LLVMTripleGetSubArchTypeName( LLVMTripleSubArchType type );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        private static extern string LLVMTripleGetVendorTypeName( LLVMTripleVendorType vendor );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        private static extern string LLVMTripleGetOsTypeName( LLVMTripleOSType osType );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        private static extern string LLVMTripleGetEnvironmentTypeName( LLVMTripleEnvironmentType environmentType );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        private static extern string LLVMTripleGetObjectFormatTypeName( LLVMTripleObjectFormatType environmentType );

        [DllImport( LibraryPath, CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true )]
        [return: MarshalAs( UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof( StringMarshaler ), MarshalCookie = "DisposeMessage" )]
        private static extern string LLVMNormalizeTriple( [MarshalAs( UnmanagedType.LPStr )] string triple );

        private LLVMTripleRef TripleHandle;
    }
}
