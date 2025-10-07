// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Linq;

using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;

namespace LlvmBindingsGenerator.Passes
{
    /// <summary>Translation unit pass to validate that names of all functions in the extension headers have correct prefix</summary>
    /// <remarks>
    /// All extended C APIs should have the 'LibLLVM' prefix so that they are clearly marked as an extension and there isn't
    /// any conflict with the official LLVM-C API.
    /// </remarks>
    internal class ValidateExtensionNamingPass
        : TranslationUnitPass
    {
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
            if( !function.Ignore && !function.Name.StartsWith( "LibLLVM", System.StringComparison.Ordinal ) )
            {
                Diagnostics.Error( "Extension function {0} in {1}, does not use correct prefix", function.Name, function.TranslationUnit.FileName );
            }

            return true;
        }

        public override bool VisitTypedefDecl( TypedefDecl typedef )
        {
            if( !typedef.Ignore && !typedef.Name.StartsWith( "LibLLVM", System.StringComparison.Ordinal ) )
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
