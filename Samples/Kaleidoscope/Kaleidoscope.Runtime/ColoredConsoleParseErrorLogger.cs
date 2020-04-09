// -----------------------------------------------------------------------
// <copyright file="ColoredConsoleParseErrorLogger.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Kaleidoscope.Grammar.AST;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Runtime
{
    public class ColoredConsoleParseErrorLogger
        : IParseErrorLogger
    {
        public void ShowError( ErrorNode node )
        {
            node.ValidateNotNull( nameof( node ) );
            ShowError( node.ToString( ) );
        }

        public void ShowError( string msg )
        {
            if( !string.IsNullOrWhiteSpace( msg ) )
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine( msg );
                Console.ForegroundColor = color;
            }
        }
    }
}
