// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

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
            string configPath = Path.Combine( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ), "BindingsConfig.yml");

            try
            {
                // read in the binding configuration from the YAML file
                // It is hoped, that going forward, the YAML file is the only thing that needs to change
                // but either way, it helps keep the declarative part in a more easily edited format.
                var config = new ReadOnlyConfig( YamlConfiguration.ParseFrom( configPath ) );
                var library = new LibLlvmGeneratorLibrary( config, options.LlvmRoot, options.ExtensionsRoot, options.OutputPath );
                Driver.Run( library );
            }
            catch(IOException ioex)
            {
                Diagnostics.Error( ioex.Message );
            }
            catch(YamlDotNet.Core.SyntaxErrorException yamlex)
            {
                // Sadly the yaml exception message includes the location info in a format that doesn't match any standard tooling
                // for parsing error messages, so unpack it to get just the message of interest and re-format
                var matcher = new Regex(@"\(Line\: \d+, Col\: \d+, Idx\: \d+\) - \(Line\: \d+, Col\: \d+, Idx\: \d+\)\: (.*)\Z");
                var result = matcher.Match( yamlex.Message );
                if( result.Success )
                {
                    Diagnostics.Error( "{0}({1},{2},{3},{4}): error CFG001: {5}"
                                     , configPath
                                     , yamlex.Start.Line
                                     , yamlex.Start.Column
                                     , yamlex.End.Line
                                     , yamlex.End.Column
                                     , result.Groups[ 1 ] );
                }
                else
                {
                    // message didn't match expectations, best effort at this point...
                    Diagnostics.Error( yamlex.Message );
                }
            }

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
