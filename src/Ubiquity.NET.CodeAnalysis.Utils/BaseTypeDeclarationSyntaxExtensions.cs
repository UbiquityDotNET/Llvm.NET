// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Utility type to provide extensions for <see cref="BaseTypeDeclarationSyntax"/></summary>
    public static class BaseTypeDeclarationSyntaxExtensions
    {
        // The following 2 extension methods are based on:
        // https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/

        /// <summary>Gets the declared namespace for a <see cref="BaseTypeDeclarationSyntax"/></summary>
        /// <param name="syntax">Syntax to get the namespace for</param>
        /// <returns>Namespace of <paramref name="syntax"/></returns>
        public static string GetDeclaredNamespace(this BaseTypeDeclarationSyntax syntax)
        {
            // If we don't have a namespace at all we'll return an empty string
            // This accounts for the "default namespace" case
            string nameSpace = string.Empty;

            // Get the containing syntax node for the type declaration
            // (could be a nested type, for example)
            SyntaxNode? potentialNamespaceParent = syntax.Parent;

            // Keep moving "out" of nested classes etc until we get to a namespace
            // or until we run out of parents
            while (potentialNamespaceParent != null
                    && potentialNamespaceParent is not NamespaceDeclarationSyntax
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

        /// <summary>Gets the nested class name for a <see cref="BaseTypeDeclarationSyntax"/></summary>
        /// <param name="syntax">Syntax to get the name for</param>
        /// <param name="includeSelf">Flag to indicate if the type itself is included in the name [Default: <see langword="false"/></param>
        /// <returns><see cref="NestedClassName"/> of the syntax or <see langword="null"/></returns>
        public static NestedClassName? GetNestedClassName( this BaseTypeDeclarationSyntax syntax, bool includeSelf = false)
        {
            // Try and get the parent syntax. If it isn't a type like class/struct, this will be null
            TypeDeclarationSyntax? parentSyntax = includeSelf ? syntax as TypeDeclarationSyntax : syntax.Parent as TypeDeclarationSyntax;
            NestedClassName? parentClassInfo = null;

            // We can only be nested in class/struct/record

            // Keep looping while we're in a supported nested type
            while (parentSyntax is not null)
            {
                // NOTE: due to bug https://github.com/dotnet/roslyn/issues/78042 this
                // is not using a local static function to evaluate this in the condition
                // of the while loop [Workaround: go back to "old" extension syntax...]
                var rawKind = parentSyntax.Kind();
                bool isAllowedKind
                    = rawKind == SyntaxKind.ClassDeclaration
                    || rawKind == SyntaxKind.StructDeclaration
                    || rawKind == SyntaxKind.RecordDeclaration;

                if (!isAllowedKind)
                {
                    break;
                }

                // Record the parent type keyword (class/struct etc), name, and constraints
                parentClassInfo = new NestedClassName(
                    keyword: parentSyntax.Keyword.ValueText,
                    name: parentSyntax.Identifier.ToString() + parentSyntax.TypeParameterList,
                    constraints: parentSyntax.ConstraintClauses.ToString(),
                    children: parentClassInfo is null ? [] : [parentClassInfo]); // set the child link (null initially)

                // Move to the next outer type
                parentSyntax = parentSyntax.Parent as TypeDeclarationSyntax;
            }

            // return a link to the outermost parent type
            return parentClassInfo;
        }
    }
}
