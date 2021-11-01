// -----------------------------------------------------------------------
// <copyright file="IgnoreSystemHeaders.cs" company="Ubiquity.NET Contributors">
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
    /// <summary>Translation unit pass to mark system headers as ignored</summary>
    /// <remarks>
    /// should always be the first pass so that other passes can rely on IsGenerated
    /// properly to correctly ignore the header.
    /// </remarks>
    internal class IgnoreSystemHeadersPass
        : TranslationUnitPass
    {
        public IgnoreSystemHeadersPass( IReadOnlyCollection<IncludeRef> ignoredHeaders )
        {
            IgnoredHeaders = from entry in ignoredHeaders
                             select entry.Path;
        }

        public override bool VisitTranslationUnit( TranslationUnit unit )
        {
            if( unit.IncludePath == null || !unit.IsValid || unit.IsInvalid )
            {
                Diagnostics.Debug("Translation Unit '{0}' is invalid - marked to ignore.", unit);
                unit.GenerationKind = GenerationKind.None;
                return true;
            }

            bool isExplicitlyIgnored = IgnoredHeaders.Contains( unit.FileRelativePath );
            if( isExplicitlyIgnored )
            {
                unit.Ignore = true;
                unit.GenerationKind = GenerationKind.None;
                Diagnostics.Debug("Translation unit '{0}' is explicitly ignored", unit);
            }
            else
            {
                unit.GenerationKind = ( unit.IsCoreHeader() || unit.IsExtensionHeader() ) ? GenerationKind.Generate : GenerationKind.None;
                Diagnostics.Debug("Translation unit '{0}' GenerationKind == {1}", unit, unit.GenerationKind);
            }

            return true;
        }

        private readonly IEnumerable<string> IgnoredHeaders;
    }
}
