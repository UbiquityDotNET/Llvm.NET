using System;

using Kaleidoscope.Grammar.AST;

using OpenSoftware.DgmlTools.Model;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.Visualizers
{
    public static class AstNodeExtensions
    {
        public static DirectedGraph CreateGraph( this IAstNode node )
        {
            ArgumentNullException.ThrowIfNull(node);

            var generator = new AstGraphGenerator();
            node.Accept( generator );
            return generator.Graph;
        }
    }
}
