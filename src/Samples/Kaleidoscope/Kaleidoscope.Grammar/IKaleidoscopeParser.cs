// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Ubiquity.NET.Runtime.Utils;

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
        : IParser
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
    }
}
