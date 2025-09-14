// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

        private readonly HashSet<string> VisitedNames = [];
    }
}
