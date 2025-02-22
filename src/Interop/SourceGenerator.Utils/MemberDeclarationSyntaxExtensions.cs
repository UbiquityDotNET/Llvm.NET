using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Utils
{
    public static class MemberDeclarationSyntaxExtensions
    {
        public static bool IsExtern(this MemberDeclarationSyntax? memberDecl)
        {
            return memberDecl is not null && memberDecl.Modifiers.HasExtern();
        }

        public static bool IsPartial(this MemberDeclarationSyntax? memberDecl)
        {
            return memberDecl is not null && memberDecl.Modifiers.HasPartialKeyword();
        }

        public static bool IsReadOnlyRecordStruct(this MemberDeclarationSyntax? memberDecl)
        {
            return memberDecl is not null
                && memberDecl.Modifiers.Any(SyntaxKind.ReadOnlyKeyword)
                && memberDecl.Modifiers.Any(SyntaxKind.RecordStructDeclaration);
        }

        public static bool IsVisibility(this SyntaxToken t)
        {
            return t.Kind() switch
            {
                SyntaxKind.PublicKeyword or
                SyntaxKind.ProtectedKeyword or
                SyntaxKind.InternalKeyword or
                SyntaxKind.PrivateKeyword or
                SyntaxKind.FileKeyword => true,
                _ => false
            };
        }

        public static IEnumerable<string> GetVisibility(this MemberDeclarationSyntax? memberDecl)
        {
            if (memberDecl is null)
            {
                return [];
            }

            return from token in memberDecl.Modifiers
                   where token.IsVisibility()
                   select token.ToString();
        }

        public static bool HasAttribute(this MemberDeclarationSyntax? memberDecl, string attributeName)
        {
            return memberDecl is not null && memberDecl.TryGetAttribute(attributeName, out _);
        }

        public static bool TryGetAttribute(this MemberDeclarationSyntax? memberDecl, string typeName, /*[NotNullWhen(true)]*/ out AttributeSyntax? value)
        {
            value = null;
            if (memberDecl is null)
            {
                return false;
            }

            string shortName = typeName;
            if (typeName.EndsWith("Attribute"))
            {
                shortName = shortName.Substring(0, shortName.Length - 9);
            }
            else
            {
                typeName += "Attribute";
            }

            var q = from attributeList in memberDecl.AttributeLists
                    from attribute in attributeList.Attributes
                    let name = attribute.GetName()
                    where name == typeName || name == shortName
                    select attribute;
            value = q.FirstOrDefault();
            return value != null;
        }
    }
}