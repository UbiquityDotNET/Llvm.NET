// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Represents an IAstNode where an error occurred in the parse</summary>
    /// <remarks>
    /// The error may represent a syntax or semantic error but is used to mark the node
    /// where the error occurred. This allows for AST generation and consumers to "recover"
    /// from the error, but still report it as well as report multiple errors that might
    /// occur.
    /// </remarks>
    public class ErrorNode
        : IAstNode
    {
        /// <summary>Initializes a new instance of the <see cref="ErrorNode"/> class</summary>
        /// <param name="location">Original location of the error in source</param>
        /// <param name="err">Error message for the error</param>
        public ErrorNode( SourceLocation location, string err )
        {
            ArgumentNullException.ThrowIfNull( err );

            Location = location;
            Error = err;
        }

        /// <summary>Gets the source location for this error</summary>
        public SourceLocation Location { get; }

        /// <summary>Gets the string message for this error</summary>
        public string Error { get; }

        /// <inheritdoc/>
        public IEnumerable<IAstNode> Children { get; } = [];

        /// <inheritdoc/>
        public TResult? Accept<TResult>( IAstVisitor<TResult> visitor )
        {
            ArgumentNullException.ThrowIfNull( visitor );

            return visitor.Visit( this );
        }

        /// <inheritdoc/>
        public virtual TResult? Accept<TResult, TArg>( IAstVisitor<TResult, TArg> visitor, ref readonly TArg arg )
            where TArg : struct, allows ref struct
        {
            ArgumentNullException.ThrowIfNull( visitor );

            return visitor.Visit( this, in arg );
        }

        /// <inheritdoc/>
        public override string ToString( ) => $"{Location}:{Error}";
    }
}
