// -----------------------------------------------------------------------
// <copyright file="MarkDeprecatedFunctionsAsObsoletePass.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using CppSharp.AST;
using CppSharp.Passes;

using LlvmBindingsGenerator.Configuration;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Translation unit pass to mark any deprecated functions as Obsolete</summary>
    internal class MarkDeprecatedFunctionsAsObsoletePass
        : TranslationUnitPass
    {
        public MarkDeprecatedFunctionsAsObsoletePass( IGeneratorConfig config, bool ignoreObsolete = false )
        {
            Map = ( from f in config.FunctionBindings.Values
                    where f.IsObsolete && f.IsProjected
                    select f
                  ).ToDictionary( f => f.Name, f => f.DeprecationMessage );

            IgnoreObsoleteFunctions = ignoreObsolete;
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
            if( Map.TryGetValue( function.Name, out string msg ) )
            {
                if( IgnoreObsoleteFunctions )
                {
                    function.Ignore = true;
                    function.GenerationKind = GenerationKind.None;
                }
                else
                {
                    function.Attributes.Add( new TargetedAttribute( typeof( ObsoleteAttribute ), $"\"{msg}\"" ) );
                }
            }

            return false;
        }

        private readonly bool IgnoreObsoleteFunctions;
        private readonly IReadOnlyDictionary<string, string> Map;
    }
}
