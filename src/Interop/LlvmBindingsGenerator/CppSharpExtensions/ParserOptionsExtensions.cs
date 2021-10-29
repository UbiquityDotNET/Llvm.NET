// -----------------------------------------------------------------------
// <copyright file="ParserOptionsExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp;
using CppSharp.Parser;

namespace LlvmBindingsGenerator.CppSharpExtensions
{
    internal static class ParserOptionsExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "Not Hungarian" )]
        public static void SetupMSVC2( this ParserOptions options, VisualStudioVersion vsVersion )
        {
            options.MicrosoftMode = true;
            Version clangVersion = MSVCToolchain.GetCLVersion(vsVersion);
            options.ToolSetToUse = (clangVersion.Major * 10000000) + (clangVersion.Minor * 100000);
            options.NoStandardIncludes = true;
            options.NoBuiltinIncludes = true;
            options.AddSystemIncludeDirs( options.BuiltinsDir );
            vsVersion = MSVCToolchain.FindVSVersion( vsVersion );
            foreach( string systemInclude in MSVCToolchain.GetSystemIncludes( vsVersion ) )
            {
                options.AddSystemIncludeDirs( systemInclude );
            }

            if( !options.LanguageVersion.HasValue )
            {
                options.LanguageVersion = LanguageVersion.CPP17;
            }

            options.AddArguments( "-fms-extensions" );
            options.AddArguments( "-fms-compatibility" );
            options.AddArguments( "-fdelayed-template-parsing" );
        }
    }
}
