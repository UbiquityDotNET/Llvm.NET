// -----------------------------------------------------------------------
// <copyright file="IKaleidoscopeCodeGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Xml.Linq;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Interface to process the results of a parse for "visualization"</summary>
    /// <parameter>
    /// Generally this is used before or in place of generating any actual code to aid
    /// with diagnosing issues with the parsing itself.
    /// </parameter>
    public interface IVisualizer
    {
        /// <summary>Gets the kind of visualizations supported (if any)</summary>
        VisualizationKind VisualizationKind {get;}

        /// <summary>Processes the raw parse tree as XML if <see cref="VisualizationKind"/> has the <see cref="VisualizationKind.Xml"/> flag</summary>
        /// <param name="parseTreeXml">XML representation of the full parse tree</param>
        /// <remarks>
        /// This is an XML form of the raw parse tree (Not reduced to the AST)
        /// </remarks>
        public void VisualizeParseTree(XDocument parseTreeXml);

        /// <summary>
        /// processes the DGML representation of the Kaleidoscope AST if <see cref="VisualizationKind"/> has the <see cref="VisualizationKind.Dgml"/> flag
        /// </summary>
        /// <param name="astDgml">DGML representation of the Kaleidoscope AST</param>
        public void VisualizeAstDgml(XDocument astDgml);

        /// <summary>
        /// Called to process the BlockDiag representation of the Kaleidoscope AST if <see cref="VisualizationKind"/> has the <see cref="VisualizationKind.BlockDiag"/> flag
        /// </summary>
        /// <param name="blockDiag">BlockDiag representation of the parse</param>
        void VisualizeBlockDiag(string blockDiag);
    }

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
