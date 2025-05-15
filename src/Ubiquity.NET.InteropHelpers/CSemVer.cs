using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>CSemVer pre-release information </summary>
    public readonly record struct CSemVerPrerelease
    {
        /// <summary>Initializes a new version of the <see cref="CSemVerPrerelease"/> struct</summary>
        /// <param name="index">Build Index (name) of the pre-release</param>
        /// <param name="number">Pre-release number</param>
        /// <param name="fix">Pre-release fix</param>
        public CSemVerPrerelease( byte index, byte number, byte fix )
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan( index, 7 );
            ArgumentOutOfRangeException.ThrowIfGreaterThan( number, 99 );
            ArgumentOutOfRangeException.ThrowIfGreaterThan( fix, 99 );

            Index = index;
            Number = number;
            Fix = fix;
        }

        /// <summary>Build index</summary>
        public byte Index { get; }

        /// <summary>Build number</summary>
        public byte Number { get; }

        /// <summary>Build fix</summary>
        public byte Fix { get; }

        /// <inheritdoc/>
        public override string ToString( )
        {
            var bldr = new StringBuilder();
            ToString( bldr );
            return bldr.ToString();
        }

        internal void ToString( StringBuilder bldr, bool shortNames = false )
        {
            bldr.Append( CultureInfo.InvariantCulture, $"{(shortNames ? PreReleaseShortNames[ Index ] : PreReleaseNames[ Index ])}" );
            if(Fix > 0)
            {
                bldr.Append( CultureInfo.InvariantCulture, $".{Number}.{Fix}" );
            }
        }

        private static readonly ImmutableArray<string> PreReleaseNames = ["alpha", "beta", "delta", "epsilon", "gamma", "kappa", "prerelease", "rc"];
        private static readonly ImmutableArray<string> PreReleaseShortNames = ["a", "b", "d", "e", "g", "k", "p", "r"];
    }

    /// <summary>CSemVer version information</summary>
    public readonly record struct CSemVer
    {
        /// <summary>Initializes a new instance of the <see cref="CSemVer"/> struct</summary>
        /// <param name="major">Major version number</param>
        /// <param name="minor">Minor version number</param>
        /// <param name="patch">Patch number</param>
        /// <param name="prerelease">Optional pre-release information</param>
        /// <param name="ciBuild">Indicates whether this is a CI build</param>
        public CSemVer( UInt32 major, UInt16 minor, UInt16 patch, CSemVerPrerelease? prerelease = null, bool ciBuild = false )
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan( major, 99999u );
            ArgumentOutOfRangeException.ThrowIfGreaterThan( minor, 49999 );
            ArgumentOutOfRangeException.ThrowIfGreaterThan( patch, 9999 );

            Major = major;
            Minor = minor;
            Patch = patch;
            PreRelease = prerelease;
            IsCIBuild = ciBuild;
        }

        /// <summary>Gets the major part of the version number</summary>
        public UInt32 Major { get; }

        /// <summary>Gets the minor part of the version number</summary>
        public UInt16 Minor { get; }

        /// <summary>Gets the patch part of the version number</summary>
        public UInt16 Patch { get; }

        /// <summary>Gets the optional pre-release information</summary>
        public CSemVerPrerelease? PreRelease { get; }

        /// <summary>Gets a value indicating whether the build is a CI build</summary>
        public bool IsCIBuild { get; }

        /// <inheritdoc/>
        public override string ToString( )
        {
            var bldr = new StringBuilder();
            bldr.Append( CultureInfo.InvariantCulture, $"v{Major}" )
                .Append( CultureInfo.InvariantCulture, $".{Minor}" )
                .Append( CultureInfo.InvariantCulture, $".{Patch}" );

            if(PreRelease.HasValue)
            {
                bldr.Append( '.' );
                PreRelease.Value.ToString( bldr );
            }

            if(IsCIBuild)
            {
                bldr.Append( "--ci" );
            }

            return bldr.ToString();
        }

        /// <summary>Converts a file version form (as a <see cref="UInt64"/>) of a CSemVer into a full <see cref="CSemVer"/></summary>
        /// <param name="fileVersion"></param>
        /// <returns></returns>
        /// <remarks>
        /// <para>A file version is a quad of 4 <see cref="UInt16"/> values. This is convertible to an <see cref="UInt64"/> in the following
        /// pattern:
        /// (bits are numbered with MSB as the highest numeric value [Actual ordering depends on platform endianess])
        ///    bits 48-63: MAJOR
        ///    bits 32-47: MINOR
        ///    bits 16-31: BUILD
        ///    bits 0-15:  REVISION
        ///</para>
        ///<para>A file version cast as a <see cref="UInt64"/> is ***NOT*** the same as an Ordered version number. The file version
        ///includes a "bit" for the status as a CI Build. Thus a "file version" as a <see cref="UInt64"/> is the ordered version shifted
        ///left by one bit and the LSB indicates if it is a CI build</para>
        /// </remarks>
        public static CSemVer FromUInt64( UInt64 fileVersion )
        {
            bool isCIBuild = (fileVersion & 1) == 1;
            UInt64 accumulator = fileVersion >> 1; // Drop the CI bit to get the "ordered" number
            UInt32 realMajor = (UInt32)(accumulator / MulMajor);
            accumulator %= MulMajor;

            UInt16 realMinor = (UInt16)(accumulator / MulMinor);
            accumulator %= MulMinor;

            UInt16 realPatch = (UInt16)(accumulator / MulPatch);
            accumulator %= MulPatch;

            if(accumulator == 0)
            {
                return new CSemVer( realMajor, realMinor, realPatch, null, isCIBuild );
            }

            // reset accumulator for the release value
            accumulator -= 1;
            byte index = (byte)(accumulator / MulName);
            accumulator %= MulName;
            byte number = (byte)(accumulator / MulNum);
            byte fix = (byte)accumulator;

            return new CSemVer( realMajor, realMinor, realPatch, new CSemVerPrerelease( index, number, fix ), isCIBuild );
        }

        private const ulong MulNum = 100;                 // Pre-release Number: [0-99]
        private const ulong MulName = MulNum* 100;        //  Name Value: [0-7] 1 digit
        private const ulong MulPatch = (MulName * 8) + 1; // Patch Value: [0-9999] 4 digits
        private const ulong MulMinor = MulPatch * 10000;  // Minor Value: [0-49999] 4.5 digits
        private const ulong MulMajor = MulMinor * 50000;  // Major Value: [0-99999] 5 digits
    }
}
