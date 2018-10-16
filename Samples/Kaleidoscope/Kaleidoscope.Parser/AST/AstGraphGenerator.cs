// <copyright file="AstDgmlGenerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

#if NET47

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using OpenSoftware.DgmlTools.Model;

namespace Kaleidoscope.Grammar.AST
{
    public class AstGraphGenerator
        : AstVisitorBase<int>
    {
        public AstGraphGenerator( )
            : base( 0 )
        {
            Graph.Categories.Add( new Category( ) { Id = "TreeNode", Background = "White" } );
            var style = new Style
            {
                TargetType = "Node",
                Setter = new List<Setter>
                {
                    new Setter( )
                    {
                        Property = "Style",
                        Value = "glass"
                    }
                }
            };

            Graph.Styles.Add( style );
        }

        public override int Visit( RootNode root )
        {
            StartGraphNode( root );
            VisitChildren( root );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( Prototype prototype )
        {
            StartGraphNode( prototype );
            VisitChildren( prototype );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( FunctionDefinition definition )
        {
            StartGraphNode( definition );
            ActiveNode.Label = $"{ActiveNode.Label}: {definition.Name}";
            VisitChildren( definition );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( ConstantExpression constant )
        {
            StartGraphNode( constant );
            ActiveNode.Label = $"{ActiveNode.Label}: {constant.Value}";

            VisitChildren( constant );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( VariableReferenceExpression reference )
        {
            StartGraphNode( reference );
            ActiveNode.Label = $"{ActiveNode.Label}: {reference.Name}";

            VisitChildren( reference );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( FunctionCallExpression functionCall )
        {
            StartGraphNode( functionCall );
            ActiveNode.Label = $"{ActiveNode.Label}: {functionCall.FunctionPrototype.Name}";

            VisitChildren( functionCall );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( BinaryOperatorExpression binaryOperator )
        {
            StartGraphNode( binaryOperator );
            ActiveNode.Label = $"{ActiveNode.Label}: {binaryOperator.Name}";

            VisitChildren( binaryOperator );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( VarInExpression varInExpression )
        {
            StartGraphNode( varInExpression );
            VisitChildren( varInExpression );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( ParameterDeclaration parameterDeclaration )
        {
            StartGraphNode( parameterDeclaration );
            ActiveNode.Label = $"{ActiveNode.Label}: {parameterDeclaration.Name}";

            VisitChildren( parameterDeclaration );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( ConditionalExpression conditionalExpression )
        {
            StartGraphNode( conditionalExpression );
            VisitChildren( conditionalExpression );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( ForInExpression forInExpression )
        {
            StartGraphNode( forInExpression );
            VisitChildren( forInExpression );
            NodeStack.Pop( );
            return 0;
        }

        public override int Visit( LocalVariableDeclaration localVariableDeclaration )
        {
            StartGraphNode( localVariableDeclaration );
            ActiveNode.Label = $"{ActiveNode.Label}: {localVariableDeclaration.Name}";

            VisitChildren( localVariableDeclaration );
            NodeStack.Pop( );
            return 0;
        }

        public void WriteDgmlGraph( string path )
        {
            Graph.WriteToFile( path );
        }

        public void WriteBlockDiag( string file )
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
                foreach( var node in Graph.Nodes )
                {
                    writer.Write( "N{0} [label= \"{1}\"", node.Id, node.Label );
                    writer.WriteLine( "];" );
                }

                writer.WriteLineNoTabs( string.Empty );
                writer.WriteLine( "// Edges" );
                foreach( var link in Graph.Links )
                {
                    writer.WriteLine( "N{0} -> N{1}", link.Source, link.Target );
                }

                --writer.Indent;
                writer.WriteLine( '}' );
            }
        }

        internal DirectedGraph Graph { get; } = new DirectedGraph( );

        private void StartGraphNode( IAstNode node )
        {
            var graphNode = new Node( )
            {
                Id = CreateNodeId( node ),
                Label = node.GetType( ).Name,
                Category = "TreeNode"
            };
            graphNode.Properties.Add( "SourceInteval", node.Location.ToString( ) );

            if( NodeStack.Count > 0 )
            {
                var link = new Link( )
                {
                    Source = NodeStack.Peek( ).Id,
                    Target = graphNode.Id
                };

                Graph.Links.Add( link );
            }

            Graph.Nodes.Add( graphNode );
            NodeStack.Push( graphNode );
        }

        private Node ActiveNode => NodeStack.Peek( );

        private static string CreateNodeId( IAstNode node )
        {
            return $"{node.GetType( ).GUID}-{node.GetHashCode( )}";
        }

        private Stack<Node> NodeStack = new Stack<Node>();
    }
}
#endif
