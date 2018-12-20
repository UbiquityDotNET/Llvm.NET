﻿// <copyright file="XDocumentListener.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Xml.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Kaleidoscope.Grammar
{
    /// <summary>Parse listener that, when used with <see cref="ParseTreeWalker"/> generates an XML representation of the parse tree</summary>
    public class XDocumentListener
        : KaleidoscopeBaseListener
    {
        public XDocumentListener( IRecognizer recognizer )
        {
            Document = new XDocument { Declaration = new XDeclaration( "1.0", "utf-8", "yes" ) };
            Push( new XElement( "Kaleidoscope" ) );
            Recognizer = recognizer;
        }

        public XDocument Document { get; }

        public override void EnterUnaryOpExpression( [NotNull] KaleidoscopeParser.UnaryOpExpressionContext context )
        {
            ActiveNode.Add( new XAttribute( "Op", context.Op ) );
        }

        public override void EnterExpression( [NotNull] KaleidoscopeParser.ExpressionContext context )
        {
            base.EnterExpression( context );
        }

        public override void VisitTerminal( [NotNull] ITerminalNode node )
        {
            ActiveNode.Add( new XElement( "Terminal", new XAttribute( "Value", node.GetText( ) ) ) );
        }

        public override void EnterEveryRule( [NotNull] ParserRuleContext context )
        {
            string typeName = context.GetType( ).Name;
            Push( new XElement( typeName.Substring( 0, typeName.Length - ContextTypeNameSuffix.Length ) ) );
        }

        public override void ExitEveryRule( [NotNull] ParserRuleContext context )
        {
            base.ExitEveryRule( context );
            ActiveNode.Add( new XAttribute( "Text", context.GetSourceText( Recognizer ) ) );
            ActiveNode.Add( new XAttribute( "RuleIndex", context.RuleIndex ) );
            ActiveNode.Add( new XAttribute( "SourceInterval", context.SourceInterval.ToString( ) ) );
            if( context.exception != null )
            {
                ActiveNode.Add( new XAttribute( "Exception", context.exception ) );
            }

            Pop( );
        }

        private void Pop( )
        {
            ActiveNode = ActiveNode?.Parent;
        }

        private void Push( XElement element )
        {
            if( ActiveNode == null )
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

        private XElement ActiveNode;
        private readonly IRecognizer Recognizer;
    }
}
