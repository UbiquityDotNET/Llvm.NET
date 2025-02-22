using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Utils
{
    public static class BaseTypeDeclarationSyntaxExtensions
    {
        // The following 2 methods are based on: https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
        public static string GetNamespace(this BaseTypeDeclarationSyntax syntax)
        {
            // If we don't have a namespace at all we'll return an empty string
            // This accounts for the "default namespace" case
            string nameSpace = string.Empty;

            // Get the containing syntax node for the type declaration
            // (could be a nested type, for example)
            SyntaxNode? potentialNamespaceParent = syntax.Parent;

            // Keep moving "out" of nested classes etc until we get to a namespace
            // or until we run out of parents
            while (potentialNamespaceParent != null &&
                    potentialNamespaceParent is not NamespaceDeclarationSyntax
                    && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
            {
                potentialNamespaceParent = potentialNamespaceParent.Parent;
            }

            // Build up the final namespace by looping until we no longer have a namespace declaration
            if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
            {
                // We have a namespace. Use that as the type
                nameSpace = namespaceParent.Name.ToString();

                // Keep moving "out" of the namespace declarations until there
                // are no more nested namespace declarations.
                while (true)
                {
                    if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                    {
                        break;
                    }

                    // Add the outer namespace as a prefix to the final namespace
                    nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                    namespaceParent = parent;
                }
            }

            // return the final namespace
            return nameSpace;
        }

        public static NestedClassName? GetNestedClassName(this BaseTypeDeclarationSyntax typeSyntax, bool includeSelf = false)
        {
            // Try and get the parent syntax. If it isn't a type like class/struct, this will be null
            TypeDeclarationSyntax? parentSyntax = includeSelf ? typeSyntax as TypeDeclarationSyntax : typeSyntax.Parent as TypeDeclarationSyntax;
            NestedClassName? parentClassInfo = null;

            // Keep looping while we're in a supported nested type
            while (parentSyntax != null && IsAllowedKind(parentSyntax.Kind()))
            {
                // Record the parent type keyword (class/struct etc), name, and constraints
                parentClassInfo = new NestedClassName(
                    keyword: parentSyntax.Keyword.ValueText,
                    name: parentSyntax.Identifier.ToString() + parentSyntax.TypeParameterList,
                    constraints: parentSyntax.ConstraintClauses.ToString(),
                    child: parentClassInfo); // set the child link (null initially)

                // Move to the next outer type
                parentSyntax = parentSyntax.Parent as TypeDeclarationSyntax;
            }

            // return a link to the outermost parent type
            return parentClassInfo;

            // We can only be nested in class/struct/record
            static bool IsAllowedKind(SyntaxKind kind) =>
                kind == SyntaxKind.ClassDeclaration ||
                kind == SyntaxKind.StructDeclaration ||
                kind == SyntaxKind.RecordDeclaration;
        }
    }
}