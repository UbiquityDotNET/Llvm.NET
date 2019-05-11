// -----------------------------------------------------------------------
// <copyright file="IgnoreSystemHeaders.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    // should always be the first pass so that other passes can rely on IsGenerated etc...
    internal class IgnoreSystemHeadersPass
        : TranslationUnitPass
    {
        public IgnoreSystemHeadersPass(ISet<string> ignoredHeaders)
        {
            IgnoredHeaders = ignoredHeaders;
        }

        public IgnoreSystemHeadersPass( )
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
        }

        public override bool VisitTranslationUnit( TranslationUnit unit )
        {
            bool isExplicitlyIgnored = IgnoredHeaders.Contains( unit.FileName );
            if( isExplicitlyIgnored )
            {
                unit.Ignore = true;
            }
            else
            {
                unit.GenerationKind = ( unit.IsCoreHeader( ) || unit.IsExtensionHeader( ) ) ? GenerationKind.Generate : GenerationKind.None;
            }

            return true;
        }

        private readonly ISet<string> IgnoredHeaders;
    }
}
