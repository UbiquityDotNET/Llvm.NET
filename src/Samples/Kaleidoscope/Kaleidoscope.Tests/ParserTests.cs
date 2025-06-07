// -----------------------------------------------------------------------
// <copyright file="ParserTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParserTest( )
        {
            var parser = new Parser(LanguageLevel.SimpleExpressions, new TestVisualizer());
            using var input = File.OpenText( "simpleExpressions.kls" );
            var resultNode = parser.Parse(input);
        }
    }

    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "DUH! File scoped" )]
    file class TestVisualizer
        : IVisualizer
    {
        public TestVisualizer( VisualizationKind kind = VisualizationKind.All )
        {
            VisualizationKind = kind;
        }

        public VisualizationKind VisualizationKind { get; init; }

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
}
