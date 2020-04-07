// -----------------------------------------------------------------------
// <copyright file="IKaleidoscopeParser.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;

using Kaleidoscope.Grammar.AST;

namespace Kaleidoscope.Grammar
{
    /// <summary>Language level supported</summary>
    public enum LanguageLevel
    {
        /// <summary>Chapters 2-4 Simple Expressions</summary>
        SimpleExpressions,

        /// <summary>Chapter 5 - Control Flow</summary>
        ControlFlow,

        /// <summary>Chapter 6 - User defined operators </summary>
        UserDefinedOperators,

        /// <summary>Chapter 7 - Mutable Variables </summary>
        MutableVariables,
    }

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

        /// <summary>Gets the additional diagnostics for this parser stack</summary>
        DiagnosticRepresentations Diagnostics { get; }

        /// <summary>Try parsing the given input text</summary>
        /// <param name="txt">Text to parse</param>
        /// <returns>Parse results as an <see cref="IAstNode"/></returns>
        /// <remarks>
        /// If the parse fails then the result is <see langword="false"/>.
        /// Errors from the parse are reported through error listeners provided
        /// to the parser. Normally this is done via the constructor of a type
        /// implementing this interface.
        /// </remarks>
        IAstNode Parse( string txt );

        /// <summary>Try parsing the given input text as full source, potentially containing multiple definitions</summary>
        /// <param name="reader">TextReader to parse</param>
        /// <returns>Parse results as an <see cref="IAstNode"/></returns>
        /// <remarks>
        /// If the parse fails then the result is <see langword="null"/>.
        /// Errors from the parse are reported through error listeners provided
        /// to the parser. Normally this is done via the constructor of a type
        /// implementing this interface.
        /// </remarks>
        IAstNode Parse( TextReader reader );
    }
}
