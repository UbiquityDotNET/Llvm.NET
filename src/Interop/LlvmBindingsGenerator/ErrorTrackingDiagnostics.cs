// -----------------------------------------------------------------------
// <copyright file="ErrorTrackingDiagnostics.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp;

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
            if(info.Kind == DiagnosticKind.Error )
            {
                ++ErrorCount;
            }

            InnerDiagnostics.Emit( info );
        }

        public void PopIndent( )
        {
            InnerDiagnostics.PopIndent( );
        }

        public void PushIndent( int level = 4 )
        {
            InnerDiagnostics.PushIndent( level );
        }

        private readonly IDiagnostics InnerDiagnostics = new ConsoleDiagnostics();
    }
}
