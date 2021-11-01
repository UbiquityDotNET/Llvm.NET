// -----------------------------------------------------------------------
// <copyright file="MapHandleAliasTypesPass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using CppSharp.AST;
using CppSharp.Passes;

using LlvmBindingsGenerator.Configuration;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Translation unit pass to map handle aliases to the projected handle type</summary>
    /// <remarks>
    /// Reads the Function Bindings information from the YAML configuration to determine the
    /// handle mapping for any "alias" type handles (e.g. a projected handle that is not owned
    /// by the projection - so it is an alias) These, are projected into specific types with the
    /// "Alias" suffix so it is clear that it is only an alias.
    /// </remarks>
    internal class MapHandleAliasTypesPass
        : TranslationUnitPass
    {
        public MapHandleAliasTypesPass( IGeneratorConfig config )
        {
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
