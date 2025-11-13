// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Utility class for extensions to <see cref="SyntaxTokenList"/></summary>
    public static class SyntaxTokenListExtensions
    {
        /// <summary>Gets a value indicating whether <paramref name="self"/> has the "extern" keyword</summary>
        /// <param name="self"><see cref="SyntaxTokenList"/> to test</param>
        /// <returns><see cref="true"/> if the keyword is found <see cref="false"/> if not</returns>
        public static bool HasExtern(this SyntaxTokenList self)
        {
            return self.Any(SyntaxKind.ExternKeyword);
        }

        /// <summary>Gets a value indicating whether <paramref name="self"/> has the "partial" keyword</summary>
        /// <inheritdoc cref="HasExtern(SyntaxTokenList)"/>
        public static bool HasPartialKeyword(this SyntaxTokenList self)
        {
            return self.Any(SyntaxKind.PartialKeyword);
        }

        /// <summary>Gets a value indicating whether <paramref name="self"/> has the "static" keyword</summary>
        /// <inheritdoc cref="HasExtern(SyntaxTokenList)"/>
        public static bool HasStatic(this SyntaxTokenList self)
        {
            return self.Any(SyntaxKind.StaticKeyword);
        }
    }
}
