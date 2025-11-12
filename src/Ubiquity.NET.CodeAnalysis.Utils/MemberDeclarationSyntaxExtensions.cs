// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Utility class to provide extensions for <see cref="MemberDeclarationSyntax"/></summary>
    public static class MemberDeclarationSyntaxExtensions
    {
        /// <summary>Determines if a <see cref="MemberDeclarationSyntax"/> contains a specified attribute or not</summary>
        /// <param name="self"><see cref="MemberDeclarationSyntax"/> to test</param>
        /// <param name="attributeName">name of the attribute</param>
        /// <returns><see langword="true"/> if the attribute is found or <see langword="false"/> if not</returns>
        /// <exception cref="ArgumentNullException"><paramref name="self"/> is <see langword="null"/></exception>
        public static bool HasAttribute(this MemberDeclarationSyntax self, string attributeName)
        {
            return self is not null
                  ? self.TryGetAttribute(attributeName, out _)
                  : throw new ArgumentNullException(nameof(self));
        }

        /// <summary>Tries to get an <see cref="AttributeSyntax"/> from a <see cref="MemberDeclarationSyntax"/></summary>
        /// <param name="self">The <see cref="MemberDeclarationSyntax"/> to get the attribute from</param>
        /// <param name="attributeName">name of the attribute</param>
        /// <param name="value">resulting <see cref="AttributeSyntax"/> or <see langword="null"/> if not found</param>
        /// <returns><see langword="true"/> if the attribute is found or <see langword="false"/> if not</returns>
        public static bool TryGetAttribute(
            this MemberDeclarationSyntax self,
            string attributeName,
            [NotNullWhen(true)] out AttributeSyntax? value
            )
        {
            value = null;
            string shortName = attributeName;
            if (attributeName.EndsWith("Attribute"))
            {
                shortName = shortName[..^9];
            }
            else
            {
                attributeName += "Attribute";
            }

            var q = from attributeList in self.AttributeLists
                    from attribute in attributeList.Attributes
                    let name = attribute.GetIdentifierName()
                    where name == attributeName || name == shortName
                    select attribute;

            value = q.FirstOrDefault();
            return value != null;
        }

        /// <summary>Determines if a declartion has an extern modifier</summary>
        /// <param name="self"><see cref="MemberDeclarationSyntax"/> to test</param>
        /// <returns><see langword="true"/> if <paramref name="self"/> has the modifier or <see langword="false"/> not</returns>
        /// <exception cref="ArgumentNullException"><paramref name="self"/> is null</exception>
        public static bool IsExtern(this MemberDeclarationSyntax self)
        {
            return self is not null
                 ? self.Modifiers.HasExtern()
                 : throw new ArgumentNullException(nameof(self));
        }

        /// <summary>Determines if a declartion has a partial modifier</summary>
        /// <inheritdoc cref="IsExtern(MemberDeclarationSyntax)"/>
        public static bool IsPartial(this MemberDeclarationSyntax self)
        {
            return self is not null
                 ? self.Modifiers.HasPartialKeyword()
                 : throw new ArgumentNullException(nameof(self));
        }

        /// <summary>Determines if a declartion has a static modifier</summary>
        /// <inheritdoc cref="IsExtern(MemberDeclarationSyntax)"/>
        public static bool IsStatic(this MemberDeclarationSyntax self)
        {
            return self is not null
                 ? self.Modifiers.HasStatic()
                 : throw new ArgumentNullException(nameof(self));
        }
    }
}
