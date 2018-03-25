// <copyright file="BlockDiagGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

#if NET47
using System.CodeDom.Compiler;
using System.IO;

namespace Kaleidoscope.Grammar
{
    /// <summary>Extension class to generate a blockdiag file from a DGML graph</summary>
    /// <remarks>
    /// <para>This isn't a generator in the same sense that the <see cref="DgmlGenerator"/> and
    /// <see cref="XDocumentListener"/> are. Rather it is an extension class that allows
    /// generating a blockdiag diagram, from the directed graph created by the DgmlGenerator.
    /// The resulting ".diag" file is convertible to SVG form for documentation.</para>
    /// <para>
    /// The generated diagrams include a numbered element for binary operator expressions
    /// to indicate the precedence value that is dynamically evaluated for the expression.
    /// This is particularly useful for debugging custom operator precedence problems.
    /// </para>
    /// </remarks>
    /// <seealso href="http://blockdiag.com"/>
    public static class BlockDiagGenerator
    {
        public static void WriteBlockDiag( this DgmlGenerator generator, string file )
        {
            using( var strmWriter = new StreamWriter( File.Open( file, FileMode.Create, FileAccess.ReadWrite, FileShare.None ) ) )
            using( var writer = new IndentedTextWriter( strmWriter, "    " ) )
            {
                writer.WriteLine( "blockdiag" );
                writer.WriteLine( '{' );
                ++writer.Indent;
                writer.WriteLine( "default_shape = roundedbox" );
                writer.WriteLine( "orientation = portrait" );

                writer.WriteLineNoTabs( string.Empty );
                writer.WriteLine( "// Nodes" );
                foreach( var node in generator.Graph.Nodes )
                {
                    writer.Write( "N{0} [label= \"{1}\"", node.Id, node.Label );
                    if( node.Properties.TryGetValue("Precedence", out object precedence))
                    {
                        writer.Write( ", numbered = {0}", precedence );
                    }

                    if( node.Category == "Terminal")
                    {
                        writer.Write( ", shape = circle" );
                    }

                    writer.WriteLine("];");
                }

                writer.WriteLineNoTabs( string.Empty );
                writer.WriteLine( "// Edges" );
                foreach( var link in generator.Graph.Links )
                {
                    writer.WriteLine( "N{0} -> N{1}", link.Source, link.Target );
                }

                --writer.Indent;
                writer.WriteLine( '}' );
            }
        }
    }
}
#endif
