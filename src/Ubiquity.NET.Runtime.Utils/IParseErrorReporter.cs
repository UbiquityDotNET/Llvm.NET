// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Interface for a logger of parse errors</summary>
    public interface IParseErrorReporter
    {
        /// <summary>Log errors for a given error node</summary>
        /// <param name="node">Node containing error information to log</param>
        void ReportError( ErrorNode node );

        /// <summary>Log an error message for the parse</summary>
        /// <param name="msg">Message to log for the error</param>
        /// <remarks>
        /// This is normally used only for internal errors where the message is provided
        /// from an Exception. That is ONLY when the actual source context isn't known.
        /// Ideally, the message contains information that can help identify the location
        /// or cause of the error better.
        /// </remarks>
        void ReportError( string msg );
    }

    /// <summary>Utility class to provide extension methods for <see cref="IParseErrorReporter"/></summary>
    public static class ParseErrorReporterExtensions
    {
        /// <summary>Collects and reports all errors in an <see cref="IAstNode"/></summary>
        /// <param name="self">Reporter to use for any errors found</param>
        /// <param name="node">Node to find errors from</param>
        /// <returns><see langword="true"/> if any errors were found; <see langword="false"/> if not</returns>
        public static bool CheckAndReportParseErrors( this IParseErrorReporter self, [NotNullWhen(false)] IAstNode? node )
        {
            ArgumentNullException.ThrowIfNull( self );

            if(node is null)
            {
                return true;
            }

            var errors = node.CollectErrors( );
            if(errors.Length == 0)
            {
                return false;
            }

            foreach(var err in errors)
            {
                self.ReportError( err );
            }

            return true;
        }
    }
}
