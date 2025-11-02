// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Implementation of <see cref="IParseErrorListener"/> to collect any errors that occur during a parse</summary>
    public class ParseErrorCollector
        : IParseErrorListener
    {
        /// <inheritdoc/>
        /// <remarks>
        /// This will collect every <see cref="SyntaxError"/> reported during a parse
        /// </remarks>
        public void SyntaxError( SyntaxError syntaxError )
        {
            ArgumentNullException.ThrowIfNull( syntaxError );
            ErrorNodes = ErrorNodes.Add( new ErrorNode( syntaxError.Location, syntaxError.Id, syntaxError.ToString() ) );
        }

        /// <summary>Gets the error nodes found by this listener</summary>
        public ImmutableArray<ErrorNode> ErrorNodes { get; private set; } = [];
    }
}
