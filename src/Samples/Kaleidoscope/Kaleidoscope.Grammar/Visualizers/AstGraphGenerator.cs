// -----------------------------------------------------------------------
// <copyright file="AstGraphGenerator.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using OpenSoftware.DgmlTools.Model;

using Ubiquity.NET.Runtime.Utils;

namespace Kaleidoscope.Grammar.AST
{
    public class AstGraphGenerator
        : KaleidoscopeAstVisitorBase<object>
    {
        public AstGraphGenerator( )
            : base( null )
        {
            Graph.Categories.Add( new Category( ) { Id = "TreeNode", Background = "White" } );
            var style = new Style
            {
                TargetType = "Node",
                Setter =
                [
                    new Setter( )
                    {
                        Property = "Style",
                        Value = "glass"
                    }

                ]
            };

            Graph.Styles.Add( style );
        }

        public override object? Visit( RootNode root )
        {
            StartGraphNode( root );
            VisitChildren( root );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( Prototype prototype )
        {
            StartGraphNode( prototype );
            VisitChildren( prototype );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( FunctionDefinition definition )
        {
            StartGraphNode( definition );
            ActiveNode.Label = $"{ActiveNode.Label}: {definition.Name}";
            VisitChildren( definition );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( ConstantExpression constant )
        {
            StartGraphNode( constant );
            ActiveNode.Label = $"{ActiveNode.Label}: {constant.Value}";

            VisitChildren( constant );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( VariableReferenceExpression reference )
        {
            StartGraphNode( reference );
            ActiveNode.Label = $"{ActiveNode.Label}: {reference.Name}";

            VisitChildren( reference );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( FunctionCallExpression functionCall )
        {
            StartGraphNode( functionCall );
            ActiveNode.Label = $"{ActiveNode.Label}: {functionCall.FunctionPrototype.Name}";

            VisitChildren( functionCall );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( BinaryOperatorExpression binaryOperator )
        {
            StartGraphNode( binaryOperator );
            ActiveNode.Label = $"{ActiveNode.Label}: {binaryOperator.Name}";

            VisitChildren( binaryOperator );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( VarInExpression varInExpression )
        {
            StartGraphNode( varInExpression );
            VisitChildren( varInExpression );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( ParameterDeclaration parameterDeclaration )
        {
            StartGraphNode( parameterDeclaration );
            ActiveNode.Label = $"{ActiveNode.Label}: {parameterDeclaration.Name}";

            VisitChildren( parameterDeclaration );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( ConditionalExpression conditionalExpression )
        {
            StartGraphNode( conditionalExpression );
            VisitChildren( conditionalExpression );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( ForInExpression forInExpression )
        {
            StartGraphNode( forInExpression );
            VisitChildren( forInExpression );
            NodeStack.Pop( );
            return null;
        }

        public override object? Visit( LocalVariableDeclaration localVariableDeclaration )
        {
            StartGraphNode( localVariableDeclaration );
            ActiveNode.Label = $"{ActiveNode.Label}: {localVariableDeclaration.Name}";

            VisitChildren( localVariableDeclaration );
            NodeStack.Pop( );
            return null;
        }

        public DirectedGraph Graph { get; } = new DirectedGraph( );

        private void StartGraphNode( IAstNode node )
        {
            var graphNode = new Node( )
            {
                Id = CreateNodeId( node ),
                Label = node.GetType( ).Name,
                Category = "TreeNode"
            };
            graphNode.Properties.Add( "SourceInterval", node.Location.ToString( ) );

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

        private readonly Stack<Node> NodeStack = new();
    }
}
