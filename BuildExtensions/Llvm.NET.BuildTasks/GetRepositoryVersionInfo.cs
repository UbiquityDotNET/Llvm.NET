using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Llvm.NET.BuildTasks
{
    public class GetRepositoryVersionInfo
        : Task
    {
        [Required]
        public string BuildVersionXmlFile { get; set; }

        [Required]
        public bool IsAutomatedBuild { get; set; }

        public bool IsPullRequestBuild { get; set; }

        public DateTime BuildTimeStamp { get; set; } = DateTime.UtcNow;

        [Output]
        public string SemVer { get; set; }

        [Output]
        public string NuGetVersion { get; set; }

        [Output]
        public ushort FileVersionMajor { get; set; }

        [Output]
        public ushort FileVersionMinor { get; set; }

        [Output]
        public ushort FileVersionBuild { get; set; }

        [Output]
        public ushort FileVersionRevision { get; set; }

        [Output]
        public ITaskItem[] ExtraProperties { get; set; }

        public override bool Execute( )
        {
            var baseBuildVersionData = BuildVersionData.Load( BuildVersionXmlFile );
            CSemVer fullVersion = baseBuildVersionData.CreateSemVer( IsAutomatedBuild, IsPullRequestBuild, BuildTimeStamp );

            SemVer = fullVersion.ToString( true );
            NuGetVersion = fullVersion.ToString( false );
            FileVersionMajor = ( ushort )fullVersion.FileVersion.Major;
            FileVersionMinor = ( ushort )fullVersion.FileVersion.Minor;
            FileVersionBuild = ( ushort )fullVersion.FileVersion.Build;
            FileVersionRevision = ( ushort )fullVersion.FileVersion.Revision;

            ExtraProperties = ( from kvp in baseBuildVersionData.AdditionalProperties
                                select new TaskItem( "ExtraProperties", new Dictionary<string, string> { { "Name", kvp.Key }, { "Value", kvp.Value } } )
                              ).ToArray( );

            return true;
        }
    }
}
