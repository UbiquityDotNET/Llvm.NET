using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGenerator.Utils
{
    public static class SyntaxTokenListExtensions
    {
        public static bool HasExtern(this SyntaxTokenList tokens)
        {
            return tokens.Any(SyntaxKind.ExternKeyword);
        }

        public static bool HasPartialKeyword(this SyntaxTokenList tokens)
        {
            return tokens.Any(SyntaxKind.PartialKeyword);
        }

        public static bool HasStatic(this SyntaxTokenList tokens)
        {
            return tokens.Any(SyntaxKind.StaticKeyword);
        }

        public static bool HasRecordKeyword(this SyntaxTokenList tokens)
        {
            return tokens.Any(SyntaxKind.RecordKeyword);
        }

        public static bool HasReadOnlyKeyword(this SyntaxTokenList tokens)
        {
            return tokens.Any(SyntaxKind.ReadOnlyKeyword);
        }
    }
}