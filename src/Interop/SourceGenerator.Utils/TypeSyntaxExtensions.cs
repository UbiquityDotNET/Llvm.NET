using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Utils
{
    public static class TypeSyntaxExtensions
    {
        public static bool IsString(this TypeSyntax? t)
        {
            return (t is PredefinedTypeSyntax pts && pts.Keyword.IsKind(SyntaxKind.StringKeyword))
                || (t is QualifiedNameSyntax qns && qns.Left.ToString() == "System" && qns.Right.ToString() == "String");
        }

        public static bool IsVoid(this TypeSyntax? t)
        {
            return t is PredefinedTypeSyntax pts
                 && pts.Keyword.IsKind(SyntaxKind.VoidKeyword);
        }
    }
}
