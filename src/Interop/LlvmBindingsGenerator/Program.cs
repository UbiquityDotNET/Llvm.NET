// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace LlvmBindingsGenerator
{
    internal static partial class Program
    {
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "Main function blocks general exceptions from bubbling up, reports them consistently before exiting" )]
        public static int Main( string[ ] args )
        {
            var reporter = new ColoredConsoleReporter(MsgLevel.Information);
            if(!ArgsParsing.TryParse<CmdLineArgs>( args, reporter, out CmdLineArgs? options, out int exitCode ))
            {
                return exitCode;
            }

            // TODO: Reset Command line option to support reporting level from command line (if different)

            var diagnostics = new ErrorTrackingDiagnostics( )
            {
                Level = options.Diagnostics
            };

            // This is how the CppSharpLibrary handles all diagnostics...
            // Instead of passing an interface to the types that need one
            // it uses a static singleton type. [Sigh...]
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
