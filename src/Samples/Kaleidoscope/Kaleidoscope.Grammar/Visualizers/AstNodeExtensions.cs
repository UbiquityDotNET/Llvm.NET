// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
            ArgumentNullException.ThrowIfNull( node );

            var generator = new AstGraphGenerator();
            node.Accept( generator );
            return generator.Graph;
        }
    }
}
