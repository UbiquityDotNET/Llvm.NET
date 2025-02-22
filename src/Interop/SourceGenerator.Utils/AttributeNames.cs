using System.Text;

namespace SourceGenerator.Utils
{
    /// <summary>Provides names for an attribute that is not directly referenced by a generator so `nameof` doesn't work</summary>
    /// <param name="NamespaceName">Full namespace name of the namespace for the attribute</param>
    /// <param name="AttributeTypeName">Name of the attribute</param>
    /// <param name="ResourceNamesapceName">Resource namespace for the resource file containing the definition of this attribute</param>
    /// <remarks>
    /// <para>In addition to the potential lack of access to a `nameof` expression there is the problem
    /// of getting a fully qualified namespace name (including any nested types...) for the attribute.
    /// Doing so at compile time  in any sort of consistent/generalized form is actually a fairly major PITA.</para>
    /// <para>This struct handles these subtleties in as simple as possible, but consistent, manner for all source
    /// generators.</para>
    /// </remarks>
    /// <seealso href="https://stackoverflow.com/questions/40855818/how-to-use-nameof-to-get-the-fully-qualified-name-of-a-property-in-a-class-in-c"/>
    /// <seealso href="https://github.com/dotnet/csharplang/discussions/5483"/>
    /// <seealso href="https://github.com/dotnet/csharplang/discussions/701"/>
    /// <seealso href="https://github.com/dotnet/csharplang/issues/8505"/>
    public readonly struct AttributeNames(string NamespaceName, string AttributeTypeName)
    {
        /// <summary>Gets the FullyQualified name (without the 'global::' prefix) of the attribute</summary>
        /// <remarks>This is a derived value and, therefore, does not participate in equality</remarks>
        public string FQN => new StringBuilder(AttributeTypeName.Length + 1 + NamespaceName.Length)
                           .Append(NamespaceName)
                           .Append('.')
                           .Append(AttributeTypeName)
                           .ToString();

        /// <summary>Gets the name of the generated attribute file.</summary>
        /// <remarks>This is a derived value and, therefore, does not participate in equality</remarks>
        public string GeneratedAttributeFileName => new StringBuilder(AttributeTypeName.Length + GeneratedExtension.Length)
                                        .Append(AttributeTypeName)
                                        .Append(GeneratedExtension)
                                        .ToString();

        /// <summary>Gets the resource file name of the attribute's definition</summary>
        /// <remarks>This is a derived value and, therefore, does not participate in equality</remarks>
        public string ResourceFileName => new StringBuilder(AttributeTypeName.Length + ResourceFileExtension.Length)
                                        .Append(AttributeTypeName)
                                        .Append(ResourceFileExtension)
                                        .ToString();

        public const string GeneratedExtension = ".g.cs";
        public const string ResourceFileExtension = ".cs";
    }
}
