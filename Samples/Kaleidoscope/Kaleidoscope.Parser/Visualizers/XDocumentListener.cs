// -----------------------------------------------------------------------
// <copyright file="XDocumentListener.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Xml.Linq;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using Kaleidoscope.Grammar.ANTLR;

using Ubiquity.NET.ArgValidators;

namespace Kaleidoscope.Grammar
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

        public override void EnterUnaryOpExpression( [ValidatedNotNull] KaleidoscopeParser.UnaryOpExpressionContext context )
        {
            context.ValidateNotNull( nameof( context ) );
            if( ActiveNode is null )
            {
                throw new InvalidOperationException( "ActiveNode is null!" );
            }

            ActiveNode.Add( new XAttribute( "Op", context.Op ) );
        }

        public override void EnterExpression( [ValidatedNotNull] KaleidoscopeParser.ExpressionContext context )
        {
            context.ValidateNotNull( nameof( context ) );
            base.EnterExpression( context );
        }

        public override void VisitTerminal( [ValidatedNotNull] ITerminalNode node )
        {
            node.ValidateNotNull( nameof( node ) );
            if( ActiveNode is null )
            {
                throw new InvalidOperationException( "ActiveNode is null!" );
            }

            ActiveNode.Add( new XElement( "Terminal", new XAttribute( "Value", node.GetText( ) ) ) );
        }

        public override void EnterEveryRule( [ValidatedNotNull] ParserRuleContext context )
        {
            context.ValidateNotNull( nameof( context ) );
            string typeName = context.GetType( ).Name;
            Push( new XElement( typeName.Substring( 0, typeName.Length - ContextTypeNameSuffix.Length ) ) );
        }

        public override void ExitEveryRule( [ValidatedNotNull] ParserRuleContext context )
        {
            context.ValidateNotNull( nameof( context ) );
            base.ExitEveryRule( context );
            if( ActiveNode is null )
            {
                throw new InvalidOperationException( "ActiveNode is null!" );
            }

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

        private XElement? ActiveNode;
        private readonly IRecognizer Recognizer;
    }
}
