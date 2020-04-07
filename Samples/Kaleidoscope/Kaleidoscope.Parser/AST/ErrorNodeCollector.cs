// -----------------------------------------------------------------------
// <copyright file="ErrorNodeCollector.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Kaleidoscope.Grammar.AST
{
    public class ErrorNodeCollector
        : AstVisitorBase<string>
    {
        public ErrorNodeCollector( )
            : base( string.Empty )
        {
        }

        public override string? Visit( ErrorNode errorNode )
        {
            ErrorList.Add( errorNode );
            return DefaultResult;
        }

        public IReadOnlyCollection<ErrorNode> Errors => ErrorList.AsReadOnly( );

        private readonly List<ErrorNode> ErrorList = new List<ErrorNode>();
    }
}
