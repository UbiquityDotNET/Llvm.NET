using System;

using Ubiquity.NET.Llvm.Interop.Handles;

namespace Ubiquity.NET.Llvm.Interop.Handles
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

namespace ConsoleApp1
{
    [ContextHandle]
    readonly partial record struct LLVMHandleFoo;

    [GlobalHandle(IncludeAlias=true)]
    partial class LLVMGlobalHandleFoo;

    partial class Program
    {
        static void Main(/*string[] args*/)
        {
            Console.WriteLine("Hello World");
        }
    }
}
