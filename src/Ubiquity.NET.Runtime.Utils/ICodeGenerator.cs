// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Interface for a Kaleidoscope code generator</summary>
    /// <typeparam name="TResult">Result type of the generation</typeparam>
    /// <remarks>
    /// For eager JIT and AOT compilation <typeparamref name="TResult"/> is normally
    /// a "value" for the JIT or IR. Though any type is viable.
    /// </remarks>
    public interface ICodeGenerator<TResult>
        : IDisposable
    {
        /// <summary>Generates output from the tree</summary>
        /// <param name="ast">Tree to generate</param>
        /// <returns>Generated result</returns>
        /// <remarks>
        /// <para>The behavior of this method depends on the implementation. The common case is to
        /// actually generate a module for the JIT engine. Normally, any anonymous expressions
        /// are JIT compiled and executed. The result of executing the expression is returned.
        /// For Function definitions or declarations, the function is returned.
        /// However, that's not required. In a simple syntax analyzer, the generate may do nothing
        /// more than generate diagrams or other diagnostics from the input tree.</para>
        /// <para>For a lazy compilation JIT the generator will defer the actual generation of code and instead
        /// will create stubs for each function definition. When those functions are called, the stubs trigger a
        /// callback to the application that will then generate the code for the function "on the fly". In this case,
        /// only a top level expression is immediately generated/executed to produce a value.</para>
        /// </remarks>
        TResult? Generate( IAstNode ast );
    }
}
