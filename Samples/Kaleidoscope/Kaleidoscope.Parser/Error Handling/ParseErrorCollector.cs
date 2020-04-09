// -----------------------------------------------------------------------
// <copyright file="PareErrorCollector.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Kaleidoscope.Grammar.AST;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar
{
    public class ParseErrorCollector
        : IParseErrorListener
    {
        public void SyntaxError( SyntaxError syntaxError )
        {
            syntaxError.ValidateNotNull( nameof( syntaxError ) );
            Errors.Add( new ErrorNode( syntaxError.Location, syntaxError.ToString( ) ) );
        }

        public IReadOnlyCollection<IAstNode> ErrorNodes => Errors.AsReadOnly( );

        private readonly List<ErrorNode> Errors = new List<ErrorNode>();
    }
}
