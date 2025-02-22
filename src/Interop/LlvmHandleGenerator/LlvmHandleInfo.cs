using System.Collections.Generic;

using SourceGenerator.Utils;

namespace LlvmHandleGenerator
{
    // Cached data structure for the results of a source generation run.
    internal readonly record struct LlvmHandleInfo(
        string? NamespaceName,
        NestedClassName? ContainingType,
        string HandleName,
        bool IncludeAlias // ignored for context handles
        )
    {
        // Derived property, doesn't participate in equality.
        public string FullyQualifiedName => $"{NamespaceName ?? string.Empty}.{string.Join("+",ContainingType?.Names ?? [])}.{HandleName}";

        // Derived property, doesn't participate in equality.
        public IEnumerable<NestedClassName> ContainingTypes => ContainingType is null ? [] : ContainingType.Hierarchy;
    }
}
