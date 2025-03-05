// -----------------------------------------------------------------------
// <copyright file="PareErrorCollector.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Kaleidoscope.Grammar.AST;

namespace Kaleidoscope.Grammar
{
    public class ParseErrorCollector
        : IParseErrorListener
    {
        public void SyntaxError( SyntaxError syntaxError )
        {
            ArgumentNullException.ThrowIfNull( syntaxError );
            Errors.Add( new ErrorNode( syntaxError.Location, syntaxError.ToString( ) ) );
        }

        public IReadOnlyCollection<IAstNode> ErrorNodes => Errors.AsReadOnly( );

        private readonly List<ErrorNode> Errors = new();
    }
}
