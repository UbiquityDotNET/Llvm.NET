// <copyright file="IKaleidoscopeCodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using JetBrains.Annotations;
using Kaleidoscope.Grammar.AST;

namespace Kaleidoscope.Runtime
{
    /// <summary>Interface for a Kaleidoscope code generator</summary>
    /// <typeparam name="TResult">Result type of the generation</typeparam>
    /// <remarks>
    /// For eager JIT and AOT compilation <typeparamref name="TResult"/> is normally
    /// <see cref="Llvm.NET.Values.Value"/>. Though any type is viable.
    /// </remarks>
    public interface IKaleidoscopeCodeGenerator<TResult>
    {
        /// <summary>Generates output from the tree</summary>
        /// <param name="ast">Tree to generate</param>
        /// <param name="codeGenerationErroHandler">Error handling action for code generator errors</param>
        /// <returns>Generated result</returns>
        /// <remarks>
        /// <para>The behavior of this method depends on the implementation. The common case is to
        /// actually generate an LLVM module for the JIT engine. Normally, any anonymous expressions
        /// (<see cref="Kaleidoscope.Grammar.KaleidoscopeParser.TopLevelExpressionContext"/>) are
        /// JIT compiled and executed. The result of executing the expression is returned.
        /// For Function definitions or declarations, the <see cref="Llvm.NET.Values.Function"/> is returned.
        /// However, that's not required. In a simple syntax analyzer, the generate may do nothing
        /// more than generate diagrams or other diagnostics from the input tree.</para>
        /// <para>For a lazy compilation JIT the generator will defer the actual generation of code and instead
        /// will create stubs for each function definition. When those functions are called, the stubs trigger a
        /// callback to the application that will then generate the code for the function "on the fly". In this case,
        /// only a top level expression is immediately generated/executed.</para>
        /// <para>For <typeparamref name="TResult"/> that is a value type the default value for the type is special
        /// and indicates no result. This is used when a <see cref="CodeGeneratorException"/> is swallowed, and
        /// reported to the caller via the provided <paramref name="codeGenerationErroHandler"/> for reference types
        /// null is returned. Thus the default value indicates an unusable result</para>
        /// </remarks>
        TResult Generate( IAstNode ast, [CanBeNull] Action<CodeGeneratorException> codeGenerationErroHandler );
    }
}
