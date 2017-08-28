using System.Net;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Llvm.NET.BuildTasks
{
    public class DownloadFile
        : Task
    {
        [Required]
        public string SourceUrl { get; set; }

        [Required]
        public string DestinationPath { get; set; }

        public override bool Execute( )
        {
            var client = new WebClient( );
            client.DownloadFile( SourceUrl, DestinationPath );
            return true;
        }
    }
}
