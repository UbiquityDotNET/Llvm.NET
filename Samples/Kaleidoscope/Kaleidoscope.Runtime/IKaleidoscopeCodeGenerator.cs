// <copyright file="IKaleidoscopeCodeGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Kaleidoscope.Runtime
{
    /// <summary>Enumeration to define the kinds of diagnostic intermediate data to generate for each function definition</summary>
    [Flags]
    public enum DiagnosticRepresentations
    {
        /// <summary>No diagnostics</summary>
        None,

        /// <summary>Generate an XML representation of the parse tree</summary>
        Xml,

        /// <summary>Generate a DGML representation of the parse tree</summary>
        Dgml,

        /// <summary>Generate a textual representation of the Lllvm IR</summary>
        LlvmIR,

        /// <summary>Emits debug tracing during the parse to an attached debugger</summary>
        DebugTraceParser,
    }

    /// <summary>Interface for a Kaleidoscope code generator</summary>
    /// <typeparam name="TResult">Result type of the generation</typeparam>
    /// <remarks>
    /// <typeparamref name="TResult"/> is normally <see cref="Llvm.NET.Values.Value"/> when
    /// doing actual code generation. For simple diagnostics it is usually
    /// just int and always returns as 0. Though any other type is viable.
    /// </remarks>
    public interface IKaleidoscopeCodeGenerator<TResult>
    {
        /// <summary>Generates output from the tree</summary>
        /// <param name="parser">Parser that generated the tree</param>
        /// <param name="tree">Tree to generate</param>
        /// <param name="additionalDiagnostics">Additional diagnostics to generate</param>
        /// <returns>Generated result</returns>
        /// <remarks>
        /// The behavior of this method depends on the implementation. The common case is to
        /// actally generate an LLVM module for the JIT engine. Normally, any anonymous expressions
        /// (<see cref="Kaleidoscope.Grammar.KaleidoscopeParser.TopLevelExpressionContext"/>) are
        /// JIT compiled and executed. The result of executing the expression is returned.
        /// For Function definitions or declarations, the <see cref="Llvm.NET.Values.Function"/> is returned.
        /// However, that's not required. In a simple syntax analyzer, the generate may do nothing
        /// more than generate diagrams or other diagnostics from the input tree. At a bare minimum
        /// the implementation must track any user defined operators so that subsequent parsing
        /// will evaluate the precedence correctly.
        /// </remarks>
        TResult Generate( Parser parser, IParseTree tree, DiagnosticRepresentations additionalDiagnostics );
    }
}
