// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Null Object pattern implementation for AST Nodes</summary>
    public class NullNode
        : IAstNode
    {
        /// <inheritdoc/>
        public SourceLocation Location { get; } = default;

        /// <inheritdoc/>
        public IEnumerable<IAstNode> Children { get; } = [];

        /// <inheritdoc/>
        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
        {
            return default;
        }

        /// <inheritdoc/>
        public virtual TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TArg : struct, allows ref struct
        {
            ArgumentNullException.ThrowIfNull( visitor );
            return default;
        }

        /// <summary>Gets a singleton null node instance</summary>
        public static NullNode Instance => LazyInstance.Value;

        private static readonly Lazy<NullNode> LazyInstance = new(LazyThreadSafetyMode.PublicationOnly);
    }
}
