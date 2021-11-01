// -----------------------------------------------------------------------
// <copyright file="RemoveDuplicateNames.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Translation unit pass to mark duplicate function names as ignored</summary>
    internal class IgnoreDuplicateNamesPass
        : TranslationUnitPass
    {
        public override bool VisitASTContext( ASTContext context )
        {
            VisitedNames.Clear( );

            foreach( TranslationUnit unit in context.GeneratedUnits( ) )
            {
                VisitTranslationUnit( unit );
            }

            return true;
        }

        public override bool VisitFunctionDecl( Function function )
        {
            if( !base.VisitFunctionDecl( function ) )
            {
                return false;
            }

            if( !VisitedNames.Add( function.Name ) )
            {
                function.Ignore = true;
            }

            return true;
        }

        private readonly HashSet<string> VisitedNames = new HashSet<string>();
    }
}
