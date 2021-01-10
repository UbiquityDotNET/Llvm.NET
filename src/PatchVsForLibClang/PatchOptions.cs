// -----------------------------------------------------------------------
// <copyright file="PatchOptions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using CommandLine;

namespace PatchVsForLibClang
{
    [Verb("patch", isDefault: true, HelpText = "patch the specified VS install")]
    public class PatchOptions
    {
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "Not Hungarian notation" )]
        public PatchOptions(string vsInstallPath, bool force)
        {
            VsInstallPath = vsInstallPath;
            Force = force;
        }

        [Value( 0, Required = true, HelpText = "Installation path to Visual Studio. [Normally retrieved from the 'InstallationPath' property of a VS Setup Instance class]" )]
        public string VsInstallPath { get; }

        [Option('F', "Force", Required = false, HelpText = "Force patch, no prompting if patch applies to source [overwrites existing backup]")]
        public bool Force { get; }
    }

    [Verb("diff", HelpText = "Generate diff file", Hidden = true)]
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "All verb Options in one place" )]
    public class GenerateOptions
    {
        public GenerateOptions(string unpatched, string patched)
        {
            Unpatched = unpatched;
            Patched = patched;
        }

        [Value( 0, Required = true, HelpText = "unpatched file for diff" )]
        public string Unpatched { get; }

        [Value( 1, Required = true, HelpText = "patched file for diff" )]
        public string Patched { get; }
    }
}
