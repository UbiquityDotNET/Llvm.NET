using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Utils
{
    public static class MethodDeclarationSyntaxExtensions
    {
        // LibraryImportAttribute - intentionally NOT reported as a P/Invoke
        // It uses different qualifiers and, technically, is NOT a P/Invoke
        // signature (It's a generated marshalling function with a nested private
        // P/Invoke using NO marshaling)
        public static bool IsPInvoke(this MethodDeclarationSyntax methodDecl)
        {
            return methodDecl.IsStatic()
                && methodDecl.IsExtern()
                && methodDecl.HasAttribute("System.Runtime.InteropServices.DllImportAttribute");
        }

        public static bool IsStatic(this MemberDeclarationSyntax? methodDecl)
        {
            return methodDecl is not null && methodDecl.Modifiers.HasStatic();
        }
    }
}