// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using CommandLine;

using DiffMatchPatch;

[assembly: CLSCompliant(true)]

namespace PatchVsForLibClang
{
    internal class Program
    {
        // example paths
        // With Fix From MS => @"D:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\VC\Tools\MSVC\14.28.29617\include\intrin0.h";
        //      MSVC 16.8.3 => @"D:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Tools\MSVC\14.28.29333\include\intrin0.h";

        // VS 2019 16.9 Preview 2 is the first publicly available version with the fix included
        private static readonly Version MinVersionWithFix = new Version( 14, 28, 2933 );
        private const string MsVCRelativePath = @"VC\Tools\MSVC";
        private const string VersionRelativeRoot = "Include";

        public static int Main( string[ ] args )
        {
            using var x = new Parser( );
            return x.ParseArguments( args, typeof(PatchOptions), typeof(GenerateOptions) )
                    .MapResult( (PatchOptions o) => ApplyPatch( o ),
                                (GenerateOptions o) => GenerateDiffFile(o),
                               _ => -1 );
        }

        private static int ApplyPatch(PatchOptions options)
        {
            if( !Directory.Exists( options.VsInstallPath ) )
            {
                Console.Error.WriteLine( "Specified VS install path does not exist. '{0}'", options.VsInstallPath );
                return -1;
            }

            string msvcPath = Path.Combine( options.VsInstallPath, MsVCRelativePath );
            if( !Directory.Exists( msvcPath ) )
            {
                Console.Error.WriteLine( "Specified VS install path does not contain the MSVC libraries path. '{0}'", msvcPath );
                return -1;
            }

            var versions = GetInstalledMsVcVersions( msvcPath ).ToList( );
            if( versions.Count == 0 )
            {
                Console.Error.WriteLine( "Could not find any installed versions at '{0}'", msvcPath );
                return -1;
            }

            var maxInstalledVersion = versions.Max( );
            if( maxInstalledVersion <= MinVersionWithFix )
            {
                Console.WriteLine( "Installation already contains the fix from MS no files modified" );
                return 0;
            }

            string versionRelativeIncludePath = Path.Combine( msvcPath, maxInstalledVersion.ToString( ), VersionRelativeRoot );
            if( !Directory.Exists( versionRelativeIncludePath ) )
            {
                Console.Error.WriteLine( "Could not find include path '{0}'", versionRelativeIncludePath );
                return -1;
            }

            string intrin0_h_Path = Path.Combine( versionRelativeIncludePath, "intrin0.h" );
            if( !File.Exists( intrin0_h_Path ) )
            {
                Console.Error.WriteLine( "intrin0.h not found. ({0})", intrin0_h_Path );
                return -1;
            }

            // compute patched content before trying backup as it might not apply to this install
            string patchedContent = PatchFile( intrin0_h_Path, Resources.Intrin0_h_diff );
            if( string.IsNullOrWhiteSpace( patchedContent ) )
            {
                Console.WriteLine( "File contents did not match - patches not applied." );
                return 0;
            }

            string backupPath = Path.Combine( versionRelativeIncludePath, "intrin0.h.bak" );
            if( File.Exists( backupPath ) && !options.Force )
            {
                Console.WriteLine( "Backup already exists, no backup will be made" );
            }
            else
            {
                try
                {
                    File.Copy( intrin0_h_Path, backupPath, false );
                }
                catch( IOException ex )
                {
                    Console.Error.WriteLine( "ERROR creating backup: {0}", ex.Message );
                    return -1;
                }
            }

            File.WriteAllText( intrin0_h_Path, patchedContent );
            return 0;
        }

        private static string PatchFile( string source, string diffText )
        {
            string fileText = File.ReadAllText( source );
            var patches = PatchList.Parse( diffText );
            var (patchedText, results) = patches.Apply( fileText );

            // all patches must apply or the source wasn't a proper match
            return results.Aggregate( true, ( l, r ) => l & r ) ? patchedText : string.Empty;
        }

        private static IEnumerable<Version> GetInstalledMsVcVersions( string msvcPath )
        {
            var verPattern = new Regex( @"\d+\.\d+\.\d+" );
            return from dir in Directory.EnumerateDirectories( msvcPath )
                   where verPattern.IsMatch( dir )
                   select Version.Parse( Path.GetFileName( dir ) );
        }

        // While VS team did publish the GIT patch DIFF, that's not usable by the DiffMatchPatch library
        // (or any other I could find on short notice). So this is used to generate the DIFF applied.
        //
        // The resulting intrin0_h.diff is already part of this project so not likely needed, but
        // this is retained as a reference for how it was created)
        private static int GenerateDiffFile( GenerateOptions o )
        {
            string unpatched = File.ReadAllText( o.Unpatched );
            string patched = File.ReadAllText( o.Patched );

            // compute and write the diff file for inclusion
            File.WriteAllText( "intrin0_h.diff", Patch.Compute( unpatched, patched ).ToText( ) );
            return 0;
        }
    }
}
