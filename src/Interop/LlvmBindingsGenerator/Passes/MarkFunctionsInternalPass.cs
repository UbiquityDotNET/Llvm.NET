// -----------------------------------------------------------------------
// <copyright file="MarkFunctionsIgnoredPass.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    internal class MarkFunctionsInternalPass
        : TranslationUnitPass
    {
        public MarkFunctionsInternalPass( IEnumerable<(string Name, bool Ignored)> internalFunctions )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassFields = false;
            VisitOptions.VisitClassMethods = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = false;
            VisitOptions.VisitFunctionReturnType = false;
            VisitOptions.VisitNamespaceEnums = false;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceTypedefs = false;
            VisitOptions.VisitNamespaceVariables = false;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;

            IgnoredFunctions = internalFunctions.ToList( );
        }

        public override bool VisitASTContext( ASTContext context )
        {
            foreach( TranslationUnit unit in context.GeneratedUnits( ) )
            {
                VisitTranslationUnit( unit );
            }

            return true;
        }

        public override bool VisitFunctionDecl( Function function )
        {
            var entry = IgnoredFunctions.FirstOrDefault( e => e.Name == function.Name );
            if( entry != default )
            {
                if( entry.Ignored )
                {
                    function.Ignore = true;
                }
                else
                {
                    function.GenerationKind = GenerationKind.Internal;
                }
            }

            return false;
        }

        private static List<(string Name, bool Ignored)> IgnoredFunctions;
    }
}
