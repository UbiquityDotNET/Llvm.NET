// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace LlvmBindingsGenerator
{
    internal class ErrorTrackingDiagnostics
        : IDiagnostics
    {
        public int ErrorCount { get; private set; }

        public DiagnosticKind Level
        {
            get => InnerDiagnostics.Level;
            set => InnerDiagnostics.Level = value;
        }

        public void Emit( DiagnosticInfo info )
        {
            try
            {
                switch( info.Kind )
                {
                case DiagnosticKind.Debug:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case DiagnosticKind.Message:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case DiagnosticKind.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case DiagnosticKind.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    ++ErrorCount;
                    break;
                }

                InnerDiagnostics.Emit( info );
            }
            finally
            {
                Console.ResetColor( );
            }
        }

        public void PopIndent( )
        {
            InnerDiagnostics.PopIndent( );
        }

        public void PushIndent( int level = 4 )
        {
            InnerDiagnostics.PushIndent( level );
        }

        private readonly ConsoleDiagnostics InnerDiagnostics = new();
    }
}
