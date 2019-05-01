// -----------------------------------------------------------------------
// <copyright file="MapHandleAliasTypesPass.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    internal class MapHandleAliasTypesPass
        : TranslationUnitPass
    {
        public MapHandleAliasTypesPass( SortedSet<string> aliasReturningFunctions )
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

            AliasReturningFunctions = aliasReturningFunctions;
        }

        public override bool VisitTranslationUnit( TranslationUnit unit )
        {
            var q = from f in unit.Functions
                    where AliasReturningFunctions.Contains( f.Name )
                    select f;

            foreach( var function in q )
            {
                var handleType = ( TypedefType )function.ReturnType.Type;
                var aliasType = new TypedefType( new TypedefDecl( )
                {
                    Name = $"{handleType.Declaration.Name}Alias",
                    QualifiedType = handleType.Declaration.QualifiedType
                } );

                function.ReturnType = new QualifiedType( aliasType, function.ReturnType.Qualifiers );
                var signature = function.FunctionType.Type as FunctionType;
                signature.ReturnType = function.ReturnType;
            }

            return true;
        }

        private readonly SortedSet<string> AliasReturningFunctions;
    }
}
