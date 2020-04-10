// -----------------------------------------------------------------------
// <copyright file="IAstNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using OpenSoftware.DgmlTools.Model;

using Ubiquity.ArgValidators;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Root interface for nodes in the Abstract Syntax Tree</summary>
    public interface IAstNode
    {
        /// <summary>Gets the source location covering the original source for the node</summary>
        SourceSpan Location { get; }

        /// <summary>Gets a collection of children for the node</summary>
        IEnumerable<IAstNode> Children { get; }

        /// <summary>Visitor pattern support for implementations to dispatch the concrete node type to a visitor</summary>
        /// <typeparam name="TResult">Result type for the visitor</typeparam>
        /// <param name="visitor">Visitor to dispatch the concrete type to</param>
        /// <returns>Result of visiting this node</returns>
        TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class;
    }

    /// <summary>Extensions for IAstNode</summary>
    /// <remarks>
    /// While default interface methods seems like a great idea, it's not yet complete enough to be useful.
    /// In particular there's pretty much no debugger support for evaluating such things, leaving you
    /// with no way to see what they produce when used as a property. Hopefully, that will be resolved in
    /// the future - but for now it is more a hindrance than it is a help.
    /// </remarks>
    public static class AstNodeExtensions
    {
        /// <summary>Gets the complete collection of errors for this node and children</summary>
        /// <param name="node">Node to traverse for errors</param>
        /// <remarks>Traverses the node hierarchy to find all error node at any depth</remarks>
        /// <returns>Collection of errors found</returns>
        public static IReadOnlyCollection<ErrorNode> CollectErrors( [ValidatedNotNull] this IAstNode node )
        {
            node.ValidateNotNull( nameof( node ) );
            var collector = new ErrorNodeCollector();
            node.Accept<string>( collector );
            return collector.Errors;
        }

        public static DirectedGraph CreateGraph( [ValidatedNotNull] this IAstNode node )
        {
            node.ValidateNotNull( nameof( node ) );

            var generator = new AstGraphGenerator();
            node.Accept( generator );
            return generator.Graph;
        }
    }
}
