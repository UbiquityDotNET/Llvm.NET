// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Ubiquity.NET.TextUX;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Represents an IAstNode where an error occurred in the parse</summary>
    /// <typeparam name="T">Type of the enum for diagnostic IDs</typeparam>
    /// <remarks>
    /// The error may represent a syntax or semantic error but is used to mark the node
    /// where the error occurred. This allows for AST generation and consumers to "recover"
    /// from the error, but still report it as well as report multiple errors that might
    /// occur.
    /// </remarks>
    public class ErrorNode<T>
        : IAstNode
        where T : struct, Enum
    {
        /// <summary>Initializes a new instance of the <see cref="ErrorNode{T}"/> class</summary>
        /// <param name="location">Original location of the error in source</param>
        /// <param name="code">Identifier code for the error</param>
        /// <param name="err">Error message for the error</param>
        /// <param name="level">Message level [default: <see cref="MsgLevel.Error"/>]</param>
        public ErrorNode( SourceRange location, T code, string err, MsgLevel level = MsgLevel.Error )
        {
            ArgumentNullException.ThrowIfNull( err );

            Location = location;
            Code = code;
            Message = err;
            Level = level;
        }

        /// <summary>Gets the source location for this error</summary>
        public SourceRange Location { get; }

        /// <summary>Gets the code for the error</summary>
        public T Code { get; }

        /// <summary>Gets the message level of this node</summary>
        public MsgLevel Level { get; }

        /// <summary>Gets the string message for this error</summary>
        public string Message { get; }

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
#if NET9_0_OR_GREATER
        where TArg : struct, allows ref struct
#else
        where TArg : struct
#endif
        {
            ArgumentNullException.ThrowIfNull( visitor );

            return visitor.Visit( this, in arg );
        }

        /// <inheritdoc/>
        public override string ToString( ) => $"{Location}:{Message}";
    }
}
