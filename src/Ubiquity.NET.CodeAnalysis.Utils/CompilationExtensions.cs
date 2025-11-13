// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

// Mostly from: https://github.com/Sergio0694/PolySharp/blob/main/src/PolySharp.SourceGenerators/Extensions/CompilationExtensions.cs
// Reformated and adapted to support repo guidelines

using Microsoft.CodeAnalysis.VisualBasic;

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>Structure for a runtime version</summary>
    /// <param name="RuntimeName">Name of the runtime</param>
    /// <param name="Version">Version of the runtime</param>
    [SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Simple record" )]
    public readonly record struct RuntimeVersion( string RuntimeName, Version Version );

    /// <summary>Extension methods for the <see cref="Compilation"/> type.</summary>
    public static class CompilationExtensions
    {
        /// <summary>Checks whether a given compilation (assumed to be for C#) is using at least a given language version.</summary>
        /// <param name="compilation">The <see cref="Compilation"/> to consider for analysis.</param>
        /// <param name="languageVersion">The minimum language version to check.</param>
        /// <returns>Whether <paramref name="compilation"/> is using at least the specified language version.</returns>
        public static bool HasLanguageVersionAtLeastEqualTo( this Compilation compilation, Microsoft.CodeAnalysis.CSharp.LanguageVersion languageVersion )
        {
            return compilation is not CSharpCompilation csharpCompilation
                ? throw new ArgumentNullException( nameof( compilation ) )
                : csharpCompilation.LanguageVersion >= languageVersion;
        }

        /// <summary>Checks whether a given VB compilation is using at least a given language version.</summary>
        /// <param name="compilation">The <see cref="Compilation"/> to consider for analysis.</param>
        /// <param name="languageVersion">The minimum language version to check.</param>
        /// <returns>Whether <paramref name="compilation"/> is using at least the specified language version.</returns>
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "not Hungarian" )]
        public static bool HasLanguageVersionAtLeastEqualTo( this Compilation compilation, Microsoft.CodeAnalysis.VisualBasic.LanguageVersion languageVersion )
        {
            return compilation is not VisualBasicCompilation vbCompilation
                ? throw new ArgumentNullException( nameof( compilation ) )
                : vbCompilation.LanguageVersion >= languageVersion;
        }

        /// <summary>Gets the runtime version by extracting the version from the assembly implementing <see cref="System.Object"/></summary>
        /// <param name="self">Compilation to get the version information from</param>
        /// <returns>Version of the runtime the compilation is targetting</returns>
        public static RuntimeVersion GetRuntimeVersion(this Compilation self)
        {
            var objectType = self.GetSpecialType(SpecialType.System_Object);
            var runtimeAssembly = objectType.ContainingAssembly;
            return new(runtimeAssembly.Identity.Name, runtimeAssembly.Identity.Version);
        }

        /// <summary>Gets a value indicating wheter the compilation has a minium version of the runtime</summary>
        /// <param name="self">Compilation to test</param>
        /// <param name="minVersion">Minimum version accepted</param>
        /// <returns><see langword="true"/> if the runtime version targetted by the compilation is at least <paramref name="minVersion"/>; <see langword="false"/> otherwise</returns>
        public static bool HasRuntimeVersionAtLeast(this Compilation self, RuntimeVersion minVersion)
        {
            var runtimeVersion = GetRuntimeVersion(self);
            return runtimeVersion.RuntimeName == minVersion.RuntimeName && runtimeVersion.Version >= minVersion.Version;
        }

        /// <summary>Checks whether or not a type with a specified metadata name is accessible from a given <see cref="Compilation"/> instance.</summary>
        /// <param name="compilation">The <see cref="Compilation"/> to consider for analysis.</param>
        /// <param name="fullyQualifiedMetadataName">The fully-qualified metadata type name to find.</param>
        /// <returns>Whether a type with the specified metadata name can be accessed from the given compilation.</returns>
        /// <remarks>
        /// This method enumerates candidate type symbols to find a match in the following order:
        /// 1) If only one type with the given name is found within the compilation and its referenced assemblies, check its accessibility.<br/>
        /// 2) If the current <paramref name="compilation"/> defines the symbol, check its accessibility.<br/>
        /// 3) Otherwise, check whether the type exists and is accessible from any of the referenced assemblies.<br/>
        /// </remarks>
        public static bool HasAccessibleTypeWithMetadataName( this Compilation compilation, string fullyQualifiedMetadataName )
        {
            if(compilation is null)
            {
                throw new ArgumentNullException( nameof( compilation ) );
            }

            if(string.IsNullOrWhiteSpace( fullyQualifiedMetadataName ))
            {
                throw new ArgumentException( $"'{nameof( fullyQualifiedMetadataName )}' cannot be null or whitespace.", nameof( fullyQualifiedMetadataName ) );
            }

            // If there is only a single matching symbol, check its accessibility
            if(compilation.GetTypeByMetadataName( fullyQualifiedMetadataName ) is INamedTypeSymbol typeSymbol)
            {
                return compilation.IsSymbolAccessibleWithin( typeSymbol, compilation.Assembly );
            }

            // Otherwise, check all available types
            foreach(INamedTypeSymbol currentTypeSymbol in compilation.GetTypesByMetadataName( fullyQualifiedMetadataName ))
            {
                if(compilation.IsSymbolAccessibleWithin( currentTypeSymbol, compilation.Assembly ))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Checks whether or not a type with a specified metadata name is accessible from a given <see cref="Compilation"/> instance.</summary>
        /// <param name="compilation">The <see cref="Compilation"/> to consider for analysis.</param>
        /// <param name="fullyQualifiedMetadataName">The fully-qualified metadata type name to find.</param>
        /// <param name="memberName">Name of the member</param>
        /// <returns>Whether a type with the specified metadata name can be accessed from the given compilation.</returns>
        /// <remarks>
        /// This method enumerates candidate type symbols to find a match in the following order:
        /// 1) If only one type with the given name is found within the compilation and its referenced assemblies, check its accessibility.<br/>
        /// 2) If the current <paramref name="compilation"/> defines the symbol, check its accessibility.<br/>
        /// 3) Otherwise, check whether the type exists and is accessible from any of the referenced assemblies.<br/>
        /// </remarks>
        public static bool HasAccessibleMember( this Compilation compilation, string fullyQualifiedMetadataName, string memberName )
        {
            // If there is only a single matching symbol, check its accessibility
            if(compilation.GetTypeByMetadataName( fullyQualifiedMetadataName ) is INamedTypeSymbol typeSymbol)
            {
                return compilation.IsSymbolAccessibleWithin( typeSymbol, compilation.Assembly )
                     && compilation.HasAccessibleMemberWithin( typeSymbol, memberName, compilation.Assembly);
            }

            // Otherwise, check all available types
            foreach(INamedTypeSymbol currentTypeSymbol in compilation.GetTypesByMetadataName( fullyQualifiedMetadataName ))
            {
                if(compilation.IsSymbolAccessibleWithin( currentTypeSymbol, compilation.Assembly )
                 && compilation.HasAccessibleMemberWithin( currentTypeSymbol, memberName, compilation.Assembly)
                 )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Tests if a <see cref="Compilation"/> has a type with an accessible member of a given name</summary>
        /// <param name="self"><see cref="Compilation"/> to test</param>
        /// <param name="typeSymbol">Type symbol for the type to test</param>
        /// <param name="memberName">Name of the member to test for</param>
        /// <param name="within">Symbol to test if the member is accessible within</param>
        /// <param name="throughType">Symbol to use for "protected access" [default: null]</param>
        /// <returns><see langword="true"/> if the member is accesible and <see langword="false"/></returns>
        public static bool HasAccessibleMemberWithin(
            this Compilation self,
            ITypeSymbol typeSymbol,
            string memberName,
            ISymbol within,
            ITypeSymbol? throughType = null
            )
        {
            if(self is null)
            {
                throw new ArgumentNullException( nameof( self ) );
            }

            if(typeSymbol is null)
            {
                throw new ArgumentNullException( nameof( typeSymbol ) );
            }

            if(string.IsNullOrEmpty( memberName ))
            {
                throw new ArgumentException( $"'{nameof( memberName )}' cannot be null or empty.", nameof( memberName ) );
            }

            if(within is null)
            {
                throw new ArgumentNullException( nameof( within ) );
            }

            var memberSymbol = typeSymbol.GetMembers().Where(s=>s.Name == memberName).FirstOrDefault();
            return memberSymbol is not null
                && self.IsSymbolAccessibleWithin(memberSymbol, within, throughType);
        }
    }
}
