// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using Kaleidoscope.Grammar.ANTLR;

using OpenSoftware.DgmlTools.Model;

using Ubiquity.NET.ANTLR.Utils;

using static Kaleidoscope.Grammar.ANTLR.KaleidoscopeParser;

namespace Kaleidoscope.Grammar.Visualizers
{
    /// <summary>ANTLR4 Parse tree listener to generate a DGML Graph of the parsed syntax</summary>
    /// <remarks>
    /// This is similar to the <see cref="XDocumentListener"/> but allows writing to a
    /// DGML file for visualizing in VisualStudio or any available DGML viewer.
    /// </remarks>
    internal class DgmlGenerator
        : KaleidoscopeBaseListener
    {
        /// <summary>Initializes a new instance of the <see cref="DgmlGenerator"/> class.</summary>
        /// <param name="recognizer">Parser to listen to for the generation</param>
        public DgmlGenerator( KaleidoscopeParser recognizer )
        {
            Recognizer = recognizer;
            Graph.Categories.Add( new Category() { Id = "TreeNode", Background = "White" } );
            Graph.Categories.Add( new Category() { Id = "HasException", Background = "Red" } );
            Graph.Categories.Add( new Category() { Id = "Terminal", Background = "LightSteelBlue" } );
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

        /// <inheritdoc/>
        public override void EnterUnaryOpExpression( UnaryOpExpressionContext context )
        {
            ActiveNode.Properties.Add( "Op", context.Op );
        }

        /// <inheritdoc/>
        public override void EnterExpression( ExpressionContext context )
        {
            ActiveNode.Properties.Add( "Precedence", context._p );
        }

        /// <inheritdoc/>
        public override void VisitTerminal( ITerminalNode node )
        {
            string nodeName = KaleidoscopeLexer.DefaultVocabulary.GetDisplayName( node.Symbol.Type );
            Graph.Nodes.Add( new Node()
            {
                Id = node.GetUniqueNodeId(),
                Label = nodeName,
                Category = "Terminal"
            } );

            if(node.Parent != null)
            {
                Graph.Links.Add( new Link()
                {
                    Source = node.Parent.GetUniqueNodeId(),
                    Target = node.GetUniqueNodeId()
                } );
            }
        }

        /// <inheritdoc/>
        public override void EnterEveryRule( ParserRuleContext context )
        {
            string typeName = context.GetType( ).Name;
            Push( new Node()
            {
                Id = context.GetUniqueNodeId(),
                Label = typeName[ ..^ContextTypeNameSuffix.Length ],
                Category = "TreeNode"
            } );
        }

        /// <inheritdoc/>
        public override void ExitEveryRule( ParserRuleContext context )
        {
            base.ExitEveryRule( context );

            ActiveNode.Properties.Add( "Text", context.GetSourceText( Recognizer ) );
            ActiveNode.Properties.Add( "RuleIndex", context.RuleIndex );
            ActiveNode.Properties.Add( "SourceInterval", context.SourceInterval.ToString() );
            if(context.exception != null)
            {
                ActiveNode.Category = "HasException";
                ActiveNode.Properties.Add( "Exception", context.exception );
            }

            if(context.Parent != null)
            {
                Graph.Links.Add( new Link()
                {
                    Source = context.Parent.GetUniqueNodeId(),
                    Target = context.GetUniqueNodeId()
                } );
            }

            Pop();
        }

        internal DirectedGraph Graph { get; } = new DirectedGraph();

        private Node ActiveNode => NodeStack.Peek();

        private Node Pop( )
        {
            return NodeStack.Pop();
        }

        private void Push( Node element )
        {
            Graph.Nodes.Add( element );
            NodeStack.Push( element );
        }

        private const string ContextTypeNameSuffix = "Context";

        private readonly KaleidoscopeParser Recognizer;
        private readonly Stack<Node> NodeStack = new();
    }
}
