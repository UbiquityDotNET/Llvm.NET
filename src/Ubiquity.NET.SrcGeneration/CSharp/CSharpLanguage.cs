// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;

namespace Ubiquity.NET.SrcGeneration.CSharp
{
    /// <summary>Support for generating source files in the C# language</summary>
    public static class CSharpLanguage
    {
        /// <summary>Opening of a scope for C#</summary>
        public const string ScopeOpen = "{";

        /// <summary>Closing of a scope for C#</summary>
        public const string ScopeClose = "}";

        /// <summary>Gets the language keywords used to make identifiers</summary>
        public static ImmutableArray<string> KeyWords { get; }
            = [ // Source: Language spec. §6.4.4 Keywords
                "abstract",
                "as",
                "base",
                "bool",
                "break",
                "byte",
                "case",
                "catch",
                "char",
                "checked",
                "class",
                "const",
                "continue",
                "decimal",
                "default",
                "delegate",
                "do",
                "double",
                "else",
                "enum",
                "event",
                "explicit",
                "extern",
                "false",
                "finally",
                "fixed",
                "float",
                "for",
                "foreach",
                "goto",
                "if",
                "implicit",
                "in",
                "int",
                "interface",
                "internal",
                "is",
                "lock",
                "long",
                "namespace",
                "new",
                "null",
                "object",
                "operator",
                "out",
                "override",
                "params",
                "private",
                "protected",
                "public",
                "readonly",
                "ref",
                "return",
                "sbyte",
                "sealed",
                "short",
                "sizeof",
                "stackalloc",
                "static",
                "string",
                "struct",
                "switch",
                "this",
                "throw",
                "true",
                "try",
                "typeof",
                "uint",
                "ulong",
                "unchecked",
                "unsafe",
                "ushort",
                "using",
                "virtual",
                "void",
                "volatile",
                "while"
                ];

        /// <summary>Makes an identifier (Escaping a language keyword)</summary>
        /// <param name="self">identifier string to convert</param>
        /// <returns>Syntactically valid identifier</returns>
        public static string MakeIdentifier( this string self )
        {
            ArgumentNullException.ThrowIfNull( self );

            return KeyWords.Contains( self )
                    ? $"@{self}"
                    : self.Replace( " ", "_", StringComparison.Ordinal );
        }
    }
}
