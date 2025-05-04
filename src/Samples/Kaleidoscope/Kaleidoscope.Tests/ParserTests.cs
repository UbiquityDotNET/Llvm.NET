using System.IO;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.Tests
{
    file class TestVisualizer
        : IVisualizer
    {
        public TestVisualizer(VisualizationKind kind = VisualizationKind.All)
        {
            VisualizationKind = kind;
        }

        public VisualizationKind VisualizationKind {get; init;}

        public void VisualizeAstDgml( XDocument astDgml )
        {
        }

        public void VisualizeBlockDiag( string blockDiag )
        {
        }

        public void VisualizeParseTree( XDocument parseTreeXml )
        {
        }
    }

    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void ParserTest( )
        {
            var parser = new Parser(LanguageLevel.SimpleExpressions, new TestVisualizer());
            using var input = File.OpenText( "simpleExpressions.kls" );
            var resultNode = parser.Parse(input);
        }
    }
}