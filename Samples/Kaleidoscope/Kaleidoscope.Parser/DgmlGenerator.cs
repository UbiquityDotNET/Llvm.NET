// <copyright file="DgmlGeneratingListener.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
#if NET47
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using OpenSoftware.DgmlTools.Model;

using static Kaleidoscope.Grammar.KaleidoscopeParser;

namespace Kaleidoscope.Grammar
{
    /// <summary>Parse tree listener to generate a DGML Graph of the parsed syntax</summary>
    /// <remarks>
    /// This is similar to the <see cref="XDocumentListener"/> but allows writing to a
    /// DGML file for visualizing in VisualStudio or any available DGML viewer.
    /// </remarks>
    public class DgmlGenerator
        : KaleidoscopeBaseListener
    {
        public DgmlGenerator( KaleidoscopeParser recognizer )
        {
            Recognizer = recognizer;
            Graph.Categories.Add( new Category( ) { Id = "TreeNode", Background = "White" } );
            Graph.Categories.Add( new Category( ) { Id = "HasException", Background = "Red" } );
            Graph.Categories.Add( new Category( ) { Id = "Terminal", Background = "LightSteelBlue" } );
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

        public override void EnterUnaryOpExpression( [NotNull] UnaryOpExpressionContext context )
        {
            ActiveNode.Properties.Add( "Op", context.Op );
        }

        public override void EnterExpression( [NotNull] ExpressionContext context )
        {
            ActiveNode.Properties.Add( "Precedence", context._p );
        }

        public override void VisitTerminal( [NotNull] ITerminalNode node )
        {
            string nodeName = KaleidoscopeLexer.DefaultVocabulary.GetDisplayName( node.Symbol.Type );
            Graph.Nodes.Add( new Node( )
            {
                Id = node.GetUniqueNodeId( ),
                Label = node.GetText( ),
                Category = "Terminal"
            } );

            if( node.Parent != null )
            {
                Graph.Links.Add( new Link( )
                {
                    Source = node.Parent.GetUniqueNodeId( ),
                    Target = node.GetUniqueNodeId( )
                } );
            }
        }

        public override void EnterEveryRule( [NotNull] ParserRuleContext context )
        {
            string typeName = context.GetType( ).Name;
            Push( new Node( )
            {
                Id = context.GetUniqueNodeId( ),
                Label = typeName.Substring( 0, typeName.Length - ContextTypeNameSuffix.Length ),
                Category = "TreeNode"
            } );
        }

        public override void ExitEveryRule( [NotNull] ParserRuleContext context )
        {
            base.ExitEveryRule( context );

            ActiveNode.Properties.Add( "Text", context.GetSourceText( Recognizer ) );
            ActiveNode.Properties.Add( "RuleIndex", context.RuleIndex );
            ActiveNode.Properties.Add( "SourceInterval", context.SourceInterval.ToString( ) );
            if( context.exception != null )
            {
                ActiveNode.Category = "HasException";
                ActiveNode.Properties.Add( "Exception", context.exception );
            }

            if( context.Parent != null )
            {
                Graph.Links.Add( new Link( )
                {
                    Source = context.Parent.GetUniqueNodeId( ),
                    Target = context.GetUniqueNodeId( )
                } );
            }

            Pop( );
        }

        public void WriteDgmlGraph( string path )
        {
            Graph.WriteToFile( path );
        }

        internal DirectedGraph Graph { get; } = new DirectedGraph( );

        private Node ActiveNode => NodeStack.Peek();

        private Node Pop( )
        {
            return NodeStack.Pop( );
        }

        private void Push( Node element )
        {
            Graph.Nodes.Add( element );
            NodeStack.Push( element );
        }

        private const string ContextTypeNameSuffix = "Context";

        private readonly KaleidoscopeParser Recognizer;
        private Stack<Node> NodeStack = new Stack<Node>();
    }
}
#endif
