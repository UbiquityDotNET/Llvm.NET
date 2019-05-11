// -----------------------------------------------------------------------
// <copyright file="DeclarationExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal static class DeclarationExtensions
    {
        public static ParsedComment GetParsedComment( this Declaration decl)
        {
            return new ParsedComment( decl.Comment );
        }
    }
}
