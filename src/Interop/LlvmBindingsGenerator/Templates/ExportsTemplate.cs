// -----------------------------------------------------------------------
// <copyright file="ExportsTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class ExportsTemplate
        : ICodeGenTemplate
    {
        public ExportsTemplate(ASTContext ast)
        {
            Ast = ast;
        }

        public Version ToolVersion => GetType( ).Assembly.GetName( ).Version;

        public string FileExtension => "DEF";

        public string Generate( )
        {
            return TransformText( );
        }

        public IEnumerable<Function> InlinedFunctions
            => from tu in Ast.GeneratedUnits( )
               from func in tu.Functions
               where func.IsInline && !func.Ignore
               select func;

        public IEnumerable<Function> ExtensionFunctions
            => from tu in Ast.GeneratedUnits( )
               where tu.IsExtensionHeader( )
               from func in tu.Functions
               where !func.IsInline && !func.Ignore
               select func;

        public IEnumerable<Function> LlvmFunctions
            => from tu in Ast.GeneratedUnits()
               where tu.IsCoreHeader()
               from func in tu.Functions
               where !func.IsInline && !func.Ignore
               select func;

        private readonly ASTContext Ast;
    }
}
