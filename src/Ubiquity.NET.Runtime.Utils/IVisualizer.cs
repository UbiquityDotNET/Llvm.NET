// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
        VisualizationKind VisualizationKind { get; }

        /// <summary>Processes the raw parse tree as XML if <see cref="VisualizationKind"/> has the <see cref="VisualizationKind.Xml"/> flag</summary>
        /// <param name="parseTreeXml">XML representation of the full parse tree</param>
        /// <remarks>
        /// This is an XML form of the raw parse tree (Not reduced to the AST)
        /// </remarks>
        public void VisualizeParseTree( XDocument parseTreeXml );

        /// <summary>
        /// processes the DGML representation of the Kaleidoscope AST if <see cref="VisualizationKind"/> has the <see cref="VisualizationKind.Dgml"/> flag
        /// </summary>
        /// <param name="astDgml">DGML representation of the Kaleidoscope AST</param>
        public void VisualizeAstDgml( XDocument astDgml );

        /// <summary>
        /// Called to process the BlockDiag representation of the Kaleidoscope AST if <see cref="VisualizationKind"/> has the <see cref="VisualizationKind.BlockDiag"/> flag
        /// </summary>
        /// <param name="blockDiag">BlockDiag representation of the parse</param>
        void VisualizeBlockDiag( string blockDiag );
    }
}
