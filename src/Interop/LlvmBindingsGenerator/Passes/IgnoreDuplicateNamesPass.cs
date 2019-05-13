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
    internal class IgnoreDuplicateNamesPass
        : TranslationUnitPass
    {
        public IgnoreDuplicateNamesPass( )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassFields = false;
            VisitOptions.VisitClassMethods = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = false;
            VisitOptions.VisitFunctionReturnType = false;
            VisitOptions.VisitNamespaceEnums = true;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceTypedefs = true;
            VisitOptions.VisitNamespaceVariables = true;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;
        }

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
