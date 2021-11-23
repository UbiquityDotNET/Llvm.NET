// -----------------------------------------------------------------------
// <copyright file="NullNode.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Null Object pattern implementation for AST Nodes</summary>
    public class NullNode
        : IAstNode
    {
        public SourceSpan Location { get; } = default;

        public IEnumerable<IAstNode> Children { get; } = Enumerable.Empty<IAstNode>( );

        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
            where TResult : class
        {
            return default;
        }

        /// <summary>Gets a singleton null node instance</summary>
        public static NullNode Instance => LazyInstance.Value;

        private static readonly Lazy<NullNode> LazyInstance = new(LazyThreadSafetyMode.PublicationOnly);
    }
}
