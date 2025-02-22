using SourceGenerator.Utils;

namespace LlvmHandleGenerator
{
    // Since this assembly generates code based on these attributes into a project, they don't exist within
    // this assembly itself. So, the 'nameof(x)' expression won't work. Additionally, that doesn't
    // provide Fully Qualified (FQN) names. Doing so at compile time is actually a fairly
    // major PITA:
    // https://stackoverflow.com/questions/40855818/how-to-use-nameof-to-get-the-fully-qualified-name-of-a-property-in-a-class-in-c
    // https://github.com/dotnet/csharplang/discussions/5483
    // https://github.com/dotnet/csharplang/discussions/701
    // https://github.com/dotnet/csharplang/issues/8505 [Rejected!]
    // This this handles it in a fashion that simplifies things as much as possible in a consistent fashion for
    // source generators.
    internal static class Names
    {
        internal static readonly AttributeNames ContextHandleAttribute = new("Ubiquity.NET.Llvm.Interop", "ContextHandleAttribute");
        internal static readonly AttributeNames GlobalHandleAttribute = new("Ubiquity.NET.Llvm.Interop", "GlobalHandleAttribute");
    }
}
