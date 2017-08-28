using System;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Llvm.NET.BuildTasks
{
    internal class OfficialPreRelease
        : IPrereleaseVersion
    {
        public OfficialPreRelease( [NotNull] string preRelName, int preRelNumber = 0, int preRelFix = 0 )
            : this( GetPreReleaseIndex( preRelName, nameof( preRelName ) ), preRelNumber, preRelFix )
        {
        }

        public OfficialPreRelease( int preRelNameIndex, int preRelNumber = 0, int preRelFix = 0 )
        {
            preRelNameIndex.ValidateRange( 0, 7, nameof( preRelNameIndex ) );
            preRelNumber.ValidateRange( 0, 99, nameof( preRelNumber ) );
            preRelFix.ValidateRange( 0, 99, nameof( preRelFix ) );

            Version = (NameIndex: preRelNameIndex, Number: (byte)preRelNumber, Fix: (byte)preRelFix);
        }

        public int PreReleaseIndex { get; }

        public int PreReleaseNumber { get; }

        public int PreReleaseFix { get; }

        public string PreReleaseName => PreReleaseNames[ PreReleaseIndex ];

        public (int NameIndex, byte Number, byte Fix) Version { get; }

        public override string ToString( )
        {
            return ToString( false );
        }

        public string ToString( bool useFullPreRelNames )
        {
            var bldr = new StringBuilder( "-" );
            bldr.Append( PreReleaseName );
            if( PreReleaseNumber > 0 )
            {
                bldr.AppendFormat( ".{0}", PreReleaseNumber );
                if( PreReleaseFix > 0 )
                {
                    bldr.AppendFormat( ".{0}", PreReleaseFix );
                }
            }

            return bldr.ToString( );
        }

        private static int GetPreReleaseIndex( [NotNull] string preRelName, [InvokerParameterNameAttribute] string paramName )
        {
            preRelName.ValidateNotNullOrWhiteSpace( paramName );
            var q = from name in PreReleaseNames.Select( ( n, i ) => (Name: n, Index: i) )
                    where 0 == string.Compare( name.Name, preRelName, StringComparison.OrdinalIgnoreCase )
                    select name;

            var nameIndex = q.FirstOrDefault( );
            return nameIndex.Name == null ? -1 : nameIndex.Index;
        }

        private static readonly string[] PreReleaseNames = { "alpha", "beta", "delta", "epsilon", "gamma", "kappa", "prerelease", "rc" };
    }
}
