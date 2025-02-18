using System;

using Ubuquity.NET.Llvm.Interop;

namespace ConsoleApp1
{
    [LlvmContextHandle]
    partial readonly record struct LLVMHandleFoo
    {
    }

    partial class Program
    {
        static void Main(/*string[] args*/)
        {
            Console.WriteLine("Hello World");
        }
    }
}
