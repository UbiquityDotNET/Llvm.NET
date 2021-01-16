// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Reflection;

using CommandLine;

using CppSharp;
using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.Configuration.Yaml;

namespace LlvmBindingsGenerator
{
    internal static class Program
    {
        public static int Main( string[ ] args )
        {
            return Parser.Default.ParseArguments<Options>( args ).MapResult( Run, _ => -1 );
        }

        private static int Run( Options options )
        {
            var diagnostics = new ErrorTrackingDiagnostics( )
            {
                Level = options.Diagnostics
            };

            Diagnostics.Implementation = diagnostics;

            // read in the binding configuration from the YAML file
            // It is hoped, that going forward, the YAML file is the only thing that needs to change
            // but either way, it helps keep the declarative part in a more easily edited format.
            string configPath = Path.Combine( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ), "BindingsConfig.yml");
            var config = new ReadOnlyConfig( YamlConfiguration.ParseFrom( configPath ) );
            var library = new LibLlvmGeneratorLibrary( config, options.LlvmRoot, options.ExtensionsRoot, options.OutputPath );
            Driver.Run( library );
            return diagnostics.ErrorCount;
            /* TODO:
            Auto merge the generated docs XML with the Hand edited API Docs as hand merging is tedious and error prone.
                1) delete entries in APIDocs no longer in generated docs
                2) add entries to APIDocs for elements in generated docs but not in API Docs
                3) Leave everything else in APIDocs, intact
            */
        }
    }
}
