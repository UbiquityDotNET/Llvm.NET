// -----------------------------------------------------------------------
// <copyright file="MarkFunctionsIgnoredPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Mark functions as internal or ignored</summary>
    /// <remarks>
    /// This pass will mark all Implicit functions from the headers ignored (i.e. an implicit constructor
    /// for a struct). Additionally, it uses a provided collection of name, ignored tuples to
    /// mark functions as either internal of ignored.
    /// </remarks>
    internal class MarkFunctionsInternalPass
        : TranslationUnitPass
    {
        public MarkFunctionsInternalPass( IDictionary<string, bool> internalFunctions )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassFields = false;
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

            InternalFunctions = internalFunctions;
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
            if( function.Ignore )
            {
                return true;
            }

            if( function.IsImplicit )
            {
                function.Ignore = true;
            }

            if( InternalFunctions.TryGetValue( function.Name, out bool isIgnored ) )
            {
                if( isIgnored )
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

        private static IDictionary<string, bool> InternalFunctions;
    }
}
