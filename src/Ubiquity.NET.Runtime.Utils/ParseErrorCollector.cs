// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Implementation of <see cref="IParseErrorListener{T}"/> to collect any errors that occur during a parse</summary>
    /// <typeparam name="T">Type of the enum for diagnostic IDs</typeparam>
    public class ParseErrorCollector<T>
        : IParseErrorListener<T>
        where T : struct, Enum
    {
        /// <inheritdoc/>
        /// <remarks>
        /// This will collect every <see cref="SyntaxError"/> reported during a parse
        /// </remarks>
        public void SyntaxError( SyntaxError<T> syntaxError )
        {
            ArgumentNullException.ThrowIfNull( syntaxError );
            ErrorNodes = ErrorNodes.Add( new ErrorNode<T>( syntaxError.Location, syntaxError.Id, syntaxError.ToString() ) );
        }

        /// <summary>Gets the error nodes found by this listener</summary>
        public ImmutableArray<ErrorNode<T>> ErrorNodes { get; private set; } = [];
    }
}
