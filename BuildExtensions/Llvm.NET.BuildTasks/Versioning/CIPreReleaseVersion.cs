using JetBrains.Annotations;

namespace Llvm.NET.BuildTasks
{
    // CSemVer-CI style pre-release version with an extension
    // to enable choosing the pre-release marker 'ci'. CSemVer-CI
    // specifies a pre-release version in the form:
    // '--ci-CIBuildName.CIBuildIndex'
    // This class allows substituting alternate values for the
    // 'ci' to enable additional scenarios. These are essentially
    // a different namespace for regular builds that allows for
    // local builds, PR Validation (auto buddy) builds, and official
    // CI builds to have distinct versions with the local builds by
    // choosing two letter markers with appropriate ASCII lexical
    // sort ordering. (e.g. local built versions should have the
    // highest precedence, then auto buddy builds, then CIbuilds,
    // and official releases)
    internal class CIPreReleaseVersion
        : IPrereleaseVersion
    {
        public const string DefaultCIMarker = "ci";

        public CIPreReleaseVersion( [NotNull] string buildName, [NotNull] string buildIndex )
            : this( buildName, buildIndex, DefaultCIMarker )
        {
        }

        public CIPreReleaseVersion( [NotNull] string buildName, [NotNull] string buildIndex, [NotNull] string ciMarker )
        {
            buildName.ValidatePattern( @"[a-zA-z0-9\-]+", nameof( buildName ) );
            buildIndex.ValidatePattern( @"[a-zA-z0-9\-]+", nameof( buildIndex ) );
            ciMarker.ValidatePattern( @"[a-z]{2}", nameof( ciMarker ) );

            BuildName = buildName;
            BuildIndex = buildIndex;
            CIMarker = ciMarker;
        }

        public string BuildName { get; }

        public string BuildIndex { get; }

        public string CIMarker { get; }

        public (int NameIndex, byte Number, byte Fix) Version => (NameIndex: -1, Number: 0, Fix: 0);

        public override string ToString( )
        {
            return $"--{CIMarker}-{BuildName}.{BuildIndex}";
        }

        public string ToString( bool useFullPreRelNames )
        {
            return ToString( );
        }
    }
}
