// -----------------------------------------------------------------------
// <copyright file="IParseErrorLogger.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Kaleidoscope.Grammar.AST;

namespace Kaleidoscope.Runtime
{
    public interface IParseErrorLogger
    {
        void ShowError( ErrorNode node );

        void ShowError( string msg );

        bool CheckAndShowParseErrors( IAstNode node )
        {
            var errors = node.CollectErrors( );
            if( errors.Count == 0 )
            {
                return false;
            }

            foreach( var err in errors )
            {
                ShowError( err );
            }

            return true;
        }
    }
}
