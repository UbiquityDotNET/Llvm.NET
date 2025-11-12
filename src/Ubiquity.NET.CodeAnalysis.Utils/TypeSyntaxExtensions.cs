// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Utility class to provide extensions for <see cref="TypeSyntax"/></summary>
    public static class TypeSyntaxExtensions
    {
        /// <summary>Tests if <paramref name="self"/> is a string</summary>
        /// <param name="self"><see cref="TypeSyntax"/> to test</param>
        /// <returns><see langword="true"/> if <paramref name="self"/> is a string and <see langword="false"/> if not</returns>
        public static bool IsString(this TypeSyntax? self)
        {
            return (self is PredefinedTypeSyntax pts && pts.Keyword.IsKind(SyntaxKind.StringKeyword))
                || (self is QualifiedNameSyntax qns && qns.Left.ToString() == "System" && qns.Right.ToString() == "String");
        }

        /// <summary>Tests if <paramref name="self"/> is a void</summary>
        /// <param name="self"><see cref="TypeSyntax"/> to test</param>
        /// <returns><see langword="true"/> if <paramref name="self"/> is a void and <see langword="false"/> if not</returns>
        public static bool IsVoid(this TypeSyntax? self)
        {
            return self is PredefinedTypeSyntax pts
            && pts.Keyword.IsKind(SyntaxKind.VoidKeyword);
        }
    }
}
