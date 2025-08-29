// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.IO;
using System.Xml.Linq;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope
{
    internal class Visualizer
        : IVisualizer
    {
        public Visualizer( VisualizationKind kind, string outputPath = "" )
        {
            VisualizationKind = kind;
            OutputPath = outputPath;
        }

        public string OutputPath { get; init; } = string.Empty;

        public VisualizationKind VisualizationKind { get; init; }

        public void VisualizeAstDgml( XDocument astDgml )
        {
            astDgml.Save( Path.Combine( OutputPath, "ast.dgml" ) );
        }

        public void VisualizeBlockDiag( string blockDiag )
        {
            File.WriteAllText( Path.Combine( OutputPath, "ast.blockdiag" ), blockDiag );
        }

        public void VisualizeParseTree( XDocument parseTreeXml )
        {
            parseTreeXml.Save( Path.Combine( OutputPath, "ParseTree.xml" ) );
        }
    }
}
