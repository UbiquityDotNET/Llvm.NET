// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
        public IgnoreSystemHeadersPass( ImmutableArray<string> ignoredHeaders )
        {
            IgnoredHeaders = ignoredHeaders;
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

        private readonly ImmutableArray<string> IgnoredHeaders;
    }
}
