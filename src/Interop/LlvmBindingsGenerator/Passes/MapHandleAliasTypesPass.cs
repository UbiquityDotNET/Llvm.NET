// -----------------------------------------------------------------------
// <copyright file="MapHandleAliasTypesPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;

using LlvmBindingsGenerator.Configuration;

namespace LlvmBindingsGenerator.Passes
{
    internal class MapHandleAliasTypesPass
        : TranslationUnitPass
    {
        public MapHandleAliasTypesPass( IGeneratorConfig config )
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

            var aliasFuncs = from f in config.FunctionBindings.Values
                             where f.ReturnTransform != null && f.ReturnTransform.IsAlias && f.IsProjected
                             select f.Name;

            AliasReturningFunctions = new SortedSet<string>( aliasFuncs );
        }

        public override bool VisitTranslationUnit( TranslationUnit unit )
        {
            var unitFunctions = from func in unit.Functions
                                where AliasReturningFunctions.Contains( func.Name )
                                select func;

            foreach( var function in unitFunctions )
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
