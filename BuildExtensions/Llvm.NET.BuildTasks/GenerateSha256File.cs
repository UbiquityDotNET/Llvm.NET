using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Llvm.NET.BuildTasks
{
    public class GenerateSha256File
        : Task
    {
        [Required]
        public ITaskItem[ ] Files { get; set; }

        public override bool Execute( )
        {
            using( var algo = SHA256.Create( ) )
            {
                foreach( var item in Files )
                {
                    string fullPath = item.GetMetadata( "FullPath" );
                    using( var stream = new FileStream( fullPath, FileMode.Open ) )
                    {
                        var fileHash = algo.ComputeHash( stream );
                        var bldr = new StringBuilder( );
                        foreach( byte value in fileHash )
                        {
                            bldr.AppendFormat( "{0:X02}", value );
                        }

                        string hashFileName = fullPath + ".sha256";
                        File.WriteAllText( hashFileName, bldr.ToString( ) );
                        Log.LogMessage( MessageImportance.High, "Generated: {0}", hashFileName );
                    }
                }
            }

            return true;
        }
    }
}
