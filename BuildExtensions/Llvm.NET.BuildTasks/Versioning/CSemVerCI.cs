namespace Llvm.NET.BuildTasks
{
    internal class CSemVerCI : CSemVer
    {
        public CSemVerCI( CSemVer baseVer, string buildName, string buildIndex )
            : base( baseVer.Major, baseVer.Minor, baseVer.Patch, new CIPreReleaseVersion( buildName, buildIndex ), baseVer.BuildMetadata )
        {
        }

        public CSemVerCI( CSemVer baseVer, string buildName, string buildIndex, string ciMarker )
            : base( baseVer.Major, baseVer.Minor, baseVer.Patch, new CIPreReleaseVersion( buildName, buildIndex, ciMarker ), baseVer.BuildMetadata )
        {
        }
    }
}
