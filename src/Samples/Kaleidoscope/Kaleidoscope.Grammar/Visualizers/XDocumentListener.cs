// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Xml.Linq;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using Kaleidoscope.Grammar.ANTLR;

using Ubiquity.NET.ANTLR.Utils;

namespace Kaleidoscope.Grammar.Visualizers
{
    /// <summary>Parse listener that, when used with <see cref="ParseTreeWalker"/> generates an XML representation of the parse tree</summary>
    internal class XDocumentListener
        : KaleidoscopeBaseListener
    {
        public XDocumentListener( IRecognizer recognizer )
        {
            Document = new XDocument { Declaration = new XDeclaration( "1.0", "utf-8", "yes" ) };
            Push( new XElement( "Kaleidoscope" ) );
            Recognizer = recognizer;
        }

        public XDocument Document { get; }

        public override void EnterUnaryOpExpression( KaleidoscopeParser.UnaryOpExpressionContext context )
        {
            ArgumentNullException.ThrowIfNull( context );
            if(ActiveNode is null)
            {
                throw new InvalidOperationException( "ActiveNode is null!" );
            }

            ActiveNode.Add( new XAttribute( "Op", context.Op ) );
        }

        public override void EnterExpression( KaleidoscopeParser.ExpressionContext context )
        {
            ArgumentNullException.ThrowIfNull( context );
            base.EnterExpression( context );
        }

        public override void VisitTerminal( ITerminalNode node )
        {
            ArgumentNullException.ThrowIfNull( node );
            if(ActiveNode is null)
            {
                throw new InvalidOperationException( "ActiveNode is null!" );
            }

            ActiveNode.Add( new XElement( "Terminal", new XAttribute( "Value", node.GetText() ) ) );
        }

        public override void EnterEveryRule( ParserRuleContext context )
        {
            ArgumentNullException.ThrowIfNull( context );
            string typeName = context.GetType( ).Name;
            Push( new XElement( typeName[ ..^ContextTypeNameSuffix.Length ] ) );
        }

        public override void ExitEveryRule( ParserRuleContext context )
        {
            ArgumentNullException.ThrowIfNull( context );
            base.ExitEveryRule( context );
            if(ActiveNode is null)
            {
                throw new InvalidOperationException( "ActiveNode is null!" );
            }

            ActiveNode.Add( new XAttribute( "Text", context.GetSourceText( Recognizer ) ) );
            ActiveNode.Add( new XAttribute( "RuleIndex", context.RuleIndex ) );
            ActiveNode.Add( new XAttribute( "SourceInterval", context.SourceInterval.ToString() ) );
            if(context.exception != null)
            {
                ActiveNode.Add( new XAttribute( "Exception", context.exception ) );
            }

            Pop();
        }

        private void Pop( )
        {
            ActiveNode = ActiveNode?.Parent;
        }

        private void Push( XElement element )
        {
            if(ActiveNode == null)
            {
                Document.Add( element );
            }
            else
            {
                ActiveNode.Add( element );
            }

            ActiveNode = element;
        }

        private const string ContextTypeNameSuffix = "Context";

        private XElement? ActiveNode;
        private readonly IRecognizer Recognizer;
    }
}
