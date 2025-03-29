// -----------------------------------------------------------------------
// <copyright file="ColoredConsoleParseErrorLogger.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Implementation of <see cref="IParseErrorLogger"/> that uses colorized console output</summary>
    public class ColoredConsoleParseErrorLogger
        : IParseErrorLogger
    {
        /// <inheritdoc/>
        public void LogError( ErrorNode node )
        {
            ArgumentNullException.ThrowIfNull( node );
            LogError( node.ToString( ) );
        }

        /// <inheritdoc/>
        public void LogError( string msg )
        {
            if( !string.IsNullOrWhiteSpace( msg ) )
            {
                var color = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine( msg );
                }
                finally
                {
                    Console.ForegroundColor = color;
                }
            }
        }
    }
}
