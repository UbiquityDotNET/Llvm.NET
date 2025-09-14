// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using CommandLine;

using CppSharp;

namespace LlvmBindingsGenerator
{
    internal static partial class Program
    {
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "MAin function blocks general exceptions from bubbling up, reports them consistently before exiting" )]
        public static int Main( string[ ] args )
        {
            // Parser chokes on an empty args list; force it to a request for help
            if(args is null || args.Length == 0)
            {
                args = ["--help"];
            }

            ParserResult<Options> parsedArgs = Parser.Default.ParseArguments<Options>( args );
            if( parsedArgs.Value == null)
            {
                // NOTE: ParseArguments has already generated the usage message. The default
                //       parser uses Console.Error as the HelpWriter. So, there's no value
                //       in showing or logging errors as it has already done so - just exit.
                return -1;
            }

            Options options = parsedArgs.Value;
            Diagnostics.Message("{0}", options);

            // NOTE: This is NOT a security validation, file/directories could be removed
            // or renamed after this (TOCTOU). This is simply a normal case sanity check to
            // report erroneous inputs. Directory or file not found exceptions may still
            // occur (But are rare and real exceptions when they do happen)
            if (!options.Validate(Console.Error))
            {
                return -1;
            }

            var diagnostics = new ErrorTrackingDiagnostics( )
            {
                Level = options.Diagnostics
            };

            Diagnostics.Implementation = diagnostics;
            try
            {
                var library = new LibLlvmGeneratorLibrary( options );
                Driver.Run( library );
            }
            catch(Exception ex)
            {
                Diagnostics.Error( ex.Message );
            }

            return diagnostics.ErrorCount;
        }
    }
}
