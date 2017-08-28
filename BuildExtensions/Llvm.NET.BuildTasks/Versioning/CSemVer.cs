using System;
using JetBrains.Annotations;

namespace Llvm.NET.BuildTasks
{
    public class CSemVer
    {
        public int Major { get; }

        public int Minor { get; }

        public int Patch { get; }

        [CanBeNull]
        public IPrereleaseVersion PrereleaseVersion { get; }

        [CanBeNull]
        public string BuildMetadata { get; }

        public CSemVer( int major, int minor , int patch, [CanBeNull] IPrereleaseVersion preRelVer = null, [CanBeNull] string buildmeta  = null )
        {
            major.ValidateRange( 0, 99999, nameof( major ) );
            minor.ValidateRange( 0, 49999, nameof( minor ) );
            patch.ValidateRange( 0, 9999, nameof( patch ) );
            if( buildmeta != null )
            {
                buildmeta.ValidateLength( 0, 20, nameof( buildmeta ) );
            }

            Major = major;
            Minor = minor;
            Patch = patch;
            PrereleaseVersion = preRelVer;
            BuildMetadata = buildmeta;
        }

        public override string ToString( )
        {
            return ToString( false );
        }

        [NotNull]
        public string ToString( bool fullName )
        {
            var bldr = new System.Text.StringBuilder( );
            bldr.AppendFormat( "{0}.{1}.{2}", Major, Minor, Patch );

            if( PrereleaseVersion != null )
            {
                bldr.Append( PrereleaseVersion.ToString( fullName ) );
            }

            if( BuildMetadata != null )
            {
                bldr.AppendFormat( "+{0}", BuildMetadata );
            }

            return bldr.ToString( );
        }

        [NotNull]
        public Version FileVersion
        {
            get
            {
                ulong orderedNum = OrderedVersion << 1;

                ushort fileRevision = ( ushort )( orderedNum % 65536 );
                ulong rem = ( orderedNum - fileRevision ) / 65536;

                ushort fileBuild = ( ushort )( rem % 65536 );
                rem = ( rem - fileBuild ) / 65536;

                ushort fileMinor = ( ushort )( rem % 65536 );
                rem = ( rem - fileMinor ) / 65536;

                ushort fileMajor = ( ushort )( rem % 65536 );

                return new Version( fileMajor, fileMinor, fileBuild, fileRevision );
            }
        }

        public ulong OrderedVersion
        {
            get
            {
                ulong retVal = ( ( ulong )Major * MulMajor ) + ( ( ulong )Minor * MulMinor ) + ( ( ( ulong )Patch + 1 ) * MulPatch );

                if( PrereleaseVersion != null && PrereleaseVersion.Version.NameIndex > 0 )
                {
                    var preRelVer = PrereleaseVersion.Version;
                    retVal -= MulPatch - 1;
                    retVal += ( ulong )preRelVer.NameIndex * MulName;
                    retVal += preRelVer.Number * MulNum;
                    retVal += preRelVer.Fix;
                }

                return retVal;
            }
        }

        private const ulong MulNum  = 100;
        private const ulong MulName  = MulNum* 100;
        private const ulong MulPatch  = (MulName * 8) + 1;
        private const ulong MulMinor  = MulPatch * 10000;
        private const ulong MulMajor  = MulMinor * 50000;
    }
}
