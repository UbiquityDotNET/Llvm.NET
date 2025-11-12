// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Utility class for general Roslyn extensions</summary>
    /// <remarks>
    /// This is a place-holder for extensions that don't fit anywhere else and don't really warrant their own type/file.
    /// </remarks>
    public static class RoslynExtensions
    {
        /// <summary>Gets an identifier name or (<see cref="string.Empty"/>) if the <see cref="AttributeSyntax"/> is not <see cref="IdentifierNameSyntax"/></summary>
        /// <param name="self"><see cref="AttributeSyntax"/> to get the identifier from</param>
        /// <returns>Identifier name</returns>
        /// <exception cref="ArgumentNullException"><paramref name="self"/> is <see langword="null"/></exception>
        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Nested conditionals are NOT simpler" )]
        public static string GetIdentifierName(this AttributeSyntax self)
        {
            if(self is null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return self.Name is not IdentifierNameSyntax identifier
                    ? string.Empty
                    : identifier.Identifier.ValueText;
        }

        /// <summary>Adds a source file from a manifest resource</summary>
        /// <param name="self">The <see cref="IncrementalGeneratorPostInitializationContext"/> to add the source to</param>
        /// <param name="resourceAssembly">Assembly hosting the resource</param>
        /// <param name="resourceName">Name of the resource</param>
        /// <param name="hintName">Hint name for the generated file</param>
        /// <param name="encoding">Encoding for the source [Default: <see cref="Encoding.UTF8"/>]</param>
        public static void AddSourceFromResource(
            this IncrementalGeneratorPostInitializationContext self,
            Assembly resourceAssembly,
            string resourceName,
            string hintName,
            Encoding? encoding = null
            )
        {
            encoding ??= Encoding.UTF8;
            using var reader = new StreamReader(resourceAssembly.GetManifestResourceStream(resourceName), encoding);
            self.AddSource(hintName, SourceText.From(reader, checked((int)reader.BaseStream.Length), encoding));
        }
    }
}
