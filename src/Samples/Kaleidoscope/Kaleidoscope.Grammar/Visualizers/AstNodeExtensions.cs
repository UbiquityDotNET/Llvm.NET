// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using OpenSoftware.DgmlTools.Model;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.Visualizers
{
    /// <summary>Utility extension class for <see cref="IAstNode"/></summary>
    public static class AstNodeExtensions
    {
        /// <summary>Create a <see cref="DirectedGraph"/> from a node</summary>
        /// <param name="node">Node to generate the graph for</param>
        /// <returns><see cref="DirectedGraph"/> for the node</returns>
        /// <remarks>
        /// This is used to aid in documentation for an AST. Usually, <paramref name="node"/>
        /// is the root node for a parse to show all details but any valid node will do.
        /// </remarks>
        public static DirectedGraph CreateGraph( this IAstNode node )
        {
            ArgumentNullException.ThrowIfNull( node );

            var generator = new AstGraphGenerator();
            node.Accept( generator );
            return generator.Graph;
        }
    }
}
