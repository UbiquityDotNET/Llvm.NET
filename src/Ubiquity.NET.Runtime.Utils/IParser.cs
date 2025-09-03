// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.IO;

// CONSIDER: Adaptation that accepts IParsesErrorReporter as a param argument to the parse APIs instead of relying on
//           an implementation to "hold" one.

namespace Ubiquity.NET.Runtime.Utils
{
    /// <summary>Core interface for a general parser that parses input text into an AST represented as a root <see cref="IAstNode"/></summary>
    public interface IParser
    {
        /// <summary>Try parsing the given input text</summary>
        /// <param name="txt">Text to parse</param>
        /// <returns>Parse results as an <see cref="IAstNode"/></returns>
        /// <remarks>
        /// If the parse fails then the result is <see langword="null"/>.
        /// Errors from the parse are reported through error listeners provided
        /// to the parser. Normally, this is done via the constructor of a type
        /// implementing this interface.
        /// </remarks>
        IAstNode? Parse( string txt );

        /// <summary>Try parsing the given input text as full source, potentially containing multiple definitions</summary>
        /// <param name="reader">TextReader to parse</param>
        /// <returns>Parse results as an <see cref="IAstNode"/></returns>
        /// <remarks>
        /// If the parse fails then the result is <see langword="null"/>.
        /// Errors from the parse are reported through error listeners provided
        /// to the parser. Normally this is done via the constructor of a type
        /// implementing this interface.
        /// </remarks>
        IAstNode? Parse( TextReader reader );
    }
}
