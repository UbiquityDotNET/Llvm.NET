using System.IO;
using System.Xml.Linq;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope
{
    internal class Visualizer
        : IVisualizer
    {
        public Visualizer(VisualizationKind kind, string outputPath = "")
        {
            VisualizationKind = kind;
            OutputPath = outputPath;
        }

        public string OutputPath { get; init; } = string.Empty;

        public VisualizationKind VisualizationKind { get; init; }

        public void VisualizeAstDgml( XDocument astDgml )
        {
            astDgml.Save(Path.Combine(OutputPath, "ast.dgml"));
        }

        public void VisualizeBlockDiag( string blockDiag )
        {
            File.WriteAllText(Path.Combine(OutputPath, "ast.blockdiag"), blockDiag);
        }

        public void VisualizeParseTree( XDocument parseTreeXml )
        {
            parseTreeXml.Save(Path.Combine(OutputPath, "ParseTree.xml"));
        }
    }
}
