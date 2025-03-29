// -----------------------------------------------------------------------
// <copyright file="IParseErrorLogger.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Interface for a logger of parse errors</summary>
    public interface IParseErrorLogger
    {
        /// <summary>Log errors for a given error node</summary>
        /// <param name="node">Node containing error information to log</param>
        void LogError( ErrorNode node );

        /// <summary>Log an error message for the parse</summary>
        /// <param name="msg">Message to log for the error</param>
        /// <remarks>
        /// This is normally used only for internal errors where the message is provided
        /// from an Exception. That is ONLY when the actual source context isn't known.
        /// Ideally, the message contains information that can help identify the location
        /// or cause of the error better.
        /// </remarks>
        void LogError( string msg );
    }

    /// <summary>Utility class to provide extension methods for <see cref="IParseErrorLogger"/></summary>
    public static class ParseErrorLoggerExtensions
    {
        /// <summary>Collects and logs all errors in an <see cref="IAstNode"/></summary>
        /// <param name="self">Logger to use for logging errors</param>
        /// <param name="node">Node to find errors from</param>
        /// <returns><see langword="true"/> if any errors were found; <see langword="false"/> if not</returns>
        public static bool CheckAndShowParseErrors(this IParseErrorLogger self, IAstNode node )
        {
            ArgumentNullException.ThrowIfNull(self);

            var errors = node.CollectErrors( );
            if( errors.Length == 0 )
            {
                return false;
            }

            foreach( var err in errors )
            {
                self.LogError( err );
            }

            return true;
        }
    }
}
