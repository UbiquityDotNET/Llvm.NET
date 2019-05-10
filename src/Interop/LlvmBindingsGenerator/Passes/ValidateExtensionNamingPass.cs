// -----------------------------------------------------------------------
// <copyright file="ValidateExtensionNamingPass.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    internal class ValidateExtensionNamingPass
        : TranslationUnitPass
    {
        public ValidateExtensionNamingPass( )
        {
            VisitOptions.VisitClassBases = false;
            VisitOptions.VisitClassProperties = false;
            VisitOptions.VisitClassTemplateSpecializations = false;
            VisitOptions.VisitEventParameters = false;
            VisitOptions.VisitFunctionParameters = false;
            VisitOptions.VisitFunctionReturnType = false;
            VisitOptions.VisitNamespaceEvents = false;
            VisitOptions.VisitNamespaceTemplates = false;
            VisitOptions.VisitNamespaceVariables = false;
            VisitOptions.VisitPropertyAccessors = false;
            VisitOptions.VisitTemplateArguments = false;
        }

        public override bool VisitASTContext( ASTContext context )
        {
            var extensionHeaders = from tu in context.TranslationUnits
                                   where tu.IsExtensionHeader( )
                                   select tu;

            foreach( TranslationUnit translationUnit in extensionHeaders )
            {
                VisitTranslationUnit( translationUnit );
            }

            return true;
        }

        public override bool VisitFunctionDecl( Function function )
        {
            if( !function.Ignore && !function.Name.StartsWith("LibLLVM", System.StringComparison.Ordinal) )
            {
                Diagnostics.Error( "Extension function {0} in {1}, does not use correct prefix", function.Name, function.TranslationUnit.FileName );
            }

            return true;
        }

        public override bool VisitTypedefDecl( TypedefDecl typedef )
        {
            if( !typedef.Name.StartsWith( "LibLLVM", System.StringComparison.Ordinal ) )
            {
                Diagnostics.Error( "Extension typeDef {0} in {1}, does not use correct prefix", typedef.Name, typedef.TranslationUnit.FileName );
            }

            return true;
        }

        public override bool VisitEnumDecl( Enumeration @enum )
        {
            if( !@enum.Name.StartsWith( "LibLLVM", System.StringComparison.Ordinal ) )
            {
                Diagnostics.Error( "Extension enum {0} in {1}, does not use correct prefix", @enum.Name, @enum.TranslationUnit.FileName );
            }

            return true;
        }

        public override bool VisitEnumItemDecl( Enumeration.Item item )
        {
            if( !item.Name.StartsWith( "LibLLVM", System.StringComparison.Ordinal ) )
            {
                Diagnostics.Error( "Extension enum item {0} in {1}, does not use correct prefix", item.Name, item.TranslationUnit.FileName );
            }

            return true;
        }
    }
}
