// -----------------------------------------------------------------------
// <copyright file="ErrorNodeCollector.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Ubiquity.NET.Runtime.Utils
{
    public class ErrorNodeCollector
        : AstVisitorBase<string>
    {
        public ErrorNodeCollector( )
            : base( string.Empty )
        {
        }
        public override string? Visit( IAstNode node )
        {
            if( node is ErrorNode errNode)
            {
                ErrorList.Add( errNode );
            }

            return base.Visit( node );
        }
        public IReadOnlyCollection<ErrorNode> Errors => ErrorList.AsReadOnly( );

        private readonly List<ErrorNode> ErrorList = [];
    }
}
