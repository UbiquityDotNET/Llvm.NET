// <copyright file="IKaleidoscopeParser.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Kaleidoscope.Grammar;

namespace Kaleidoscope.Runtime
{
    /// <summary>Interface for a Kaleidoscope parser</summary>
    public interface IKaleidoscopeParser
    {
        /// <summary>Gets or sets the language level for parsing</summary>
        LanguageLevel LanguageLevel { get; set; }

        /// <summary>Gets the global state for this parser</summary>
        /// <remarks>
        /// The global state retains information across parses of potentially partial
        /// input text. In particular for Kaleidoscope, this stores the user defined
        /// operator information to allow the parse to adapt based on the defined
        /// operators.
        /// </remarks>
        DynamicRuntimeState GlobalState { get; }

        /// <summary>Try parsing the given input text</summary>
        /// <param name="txt">Text to parse</param>
        /// <param name="additionalDiagnostics">Additional diagnostics to generate</param>
        /// <returns>Parse tree and the parser that was used to generate it as a <see cref="System.ValueTuple"/></returns>
        /// <remarks>
        /// If the parse fails then the resulting tuple is the default value ( in this case, both items <see langword="null"/>
        /// Errors from the parse are reported through error listeners provided
        /// to the parser. Normally this is done via the contructor of a type
        /// implementing this interface.
        /// </remarks>
        (IParseTree parseTree, Parser recognizer) Parse( string txt, DiagnosticRepresentations additionalDiagnostics );
    }
}
