// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Resources;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#pragma warning disable SA1316 // Tuple element names should use correct casing

namespace Ubiquity.NET.SourceGenerator.Test.Utils
{
    /// <summary>Dictionary to map a resource name to information about expected generated files</summary>
    /// <typeparam name="TGenerator">Type of the generator</typeparam>
    public class ResourceSourceTextDictionary<TGenerator>
        : IReadOnlyDictionary<string, (string filename, SourceText content)>
        where TGenerator : IIncrementalGenerator
    {
        /// <summary>Initializes a new instance of the <see cref="ResourceSourceTextDictionary{TGenerator}"/> class.</summary>
        /// <param name="containingAssembly">Assembly that contains the resources</param>
        /// <param name="nameTransform">Transform function to convert the resource name into a generated file name</param>
        /// <remarks>
        /// If <paramref name="nameTransform"/> is <see langword="null"/> then the default is to use a C# transformation
        /// that assumes the input resource name is a file name and extracts the file name (without extension) and adds
        /// a '.g.cs' to the end. This is the most common, if a test requires something different then it can provide
        /// a transformation function for <paramref name="nameTransform"/>
        /// </remarks>
        public ResourceSourceTextDictionary( Func<string, string>? nameTransform = null)
        {
            GeneratorAssembly = typeof(TGenerator).Assembly;
            NameTransform = nameTransform ?? CSharpNameTransform;
        }

        /// <summary>Gets the containing assembly for the resources</summary>
        public Assembly GeneratorAssembly { get; }

        /// <summary>Adds a named resource to the map</summary>
        /// <param name="resourceName">name of the resource in the assembly</param>
        /// <returns>This instance for fluent use</returns>
        /// <exception cref="MissingManifestResourceException">resource is missing in the resources (programming error)</exception>
        public ResourceSourceTextDictionary<TGenerator> Add(string resourceName)
        {
            // {GeneratorAssemblyName}\{Generator FQN}\{GeneratorNamespace}.{HintName}
            // Is this the assembly name or the namespace name? (In first use case they are the same)
            // It makes sense that Roslyn generators use the assembly name.
            string generatorAssemblyName = GeneratorAssembly.GetName().Name ?? throw new InvalidOperationException("Internal Error: Generator assembly has no name!");
            string generatorFQN = typeof(TGenerator).FullName ?? throw new InvalidOperationException("Internal Error: Generator type has no FQN!");
            string generatorNamespace = typeof(TGenerator).Namespace ?? throw new InvalidOperationException("Internal Error: Generator type has no namespace!");
            string fullResourceName = string.Join('.', generatorNamespace, resourceName);
            string fullGeneratedSource = Path.Combine(generatorAssemblyName, generatorFQN,generatorNamespace, resourceName);
            using Stream stream = GeneratorAssembly.GetManifestResourceStream(fullResourceName)
                               ?? throw new MissingManifestResourceException($"GENERATOR BUG: Missing resource '{fullResourceName}'");

            string expectedGeneratedName = NameTransform(fullGeneratedSource);
            var srcTxt = SourceText.From(stream, throwIfBinaryDetected: true, canBeEmbedded: true);
            InnerDictionary.Add(resourceName, (expectedGeneratedName, srcTxt));
            return this;
        }

        /// <summary>Adds a resource file name and <see cref="GeneratedFile"/> information</summary>
        /// <param name="resourceName">Name of the manifest resource</param>
        /// <param name="generatedFile">Generated file information for the resource</param>
        public void Add(string resourceName, (string filename, SourceText content) generatedFile)
        {
            InnerDictionary.Add(resourceName, generatedFile);
        }

        /// <inheritdoc/>
        public (string filename, SourceText content) this[ string key ] => InnerDictionary[ key ];

        /// <inheritdoc/>
        public IEnumerable<string> Keys => InnerDictionary.Keys;

        /// <inheritdoc/>
        public IEnumerable<(string filename, SourceText content)> Values => InnerDictionary.Values;

        /// <inheritdoc/>
        public int Count => InnerDictionary.Count;

        /// <inheritdoc/>
        public bool ContainsKey( string key )
        {
            return InnerDictionary.ContainsKey( key );
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, (string filename, SourceText content)>> GetEnumerator( )
        {
            return InnerDictionary.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool TryGetValue( string key, [MaybeNullWhen( false )] out (string filename, SourceText content) value )
        {
            return InnerDictionary.TryGetValue( key, out value );
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator( )
        {
            return InnerDictionary.GetEnumerator();
        }

        private readonly Func<string, string> NameTransform;

        private readonly Dictionary<string, (string filename, SourceText content)> InnerDictionary = [];

        private string CSharpNameTransform(string resourceName)
        {
            return $"{Path.GetDirectoryName(resourceName)}.{Path.GetFileNameWithoutExtension(resourceName)}.g.cs";
        }
    }
}
