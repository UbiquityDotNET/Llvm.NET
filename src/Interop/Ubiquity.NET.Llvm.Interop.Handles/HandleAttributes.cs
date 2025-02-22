using System;

namespace Ubiquity.NET.Llvm.Interop
{
    [AttributeUsage( AttributeTargets.Struct )]
    public sealed class ContextHandleAttribute
        : Attribute
    {
    }

    [AttributeUsage( AttributeTargets.Class )]
    public sealed class GlobalHandleAttribute
        : Attribute
    {
        public bool IncludeAlias { get; init;} = false;
    }
}
