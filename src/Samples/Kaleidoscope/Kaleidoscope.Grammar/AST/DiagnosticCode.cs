// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Kaleidoscope.Grammar.AST
{
    /// <summary>Symbolic names for code values reported in diagnostic messages</summary>
    public enum DiagnosticCode
        : int
    {
        /// <summary>No code value</summary>
        None = 0,

        // Codes with integer values < 1000 are reserved for grammar states

        // AST Builder

        /// <summary>Syntax error, input is not parsable</summary>
        SyntaxError = 1000,

        /// <summary>Syntax indicates a variable but that symbol's name is unknown</summary>
        UnknownVariable,

        /// <summary>Syntax indicates an invocation of a function but that function's name is unknown</summary>
        InvokeUnknownFunction,

        /// <summary>Unary op expression is invalid</summary>
        InvalidUnaryOp,

        /// <summary>Invalid reference to an operator</summary>
        InvalidUnaryOpRef,

        /// <summary>Binary op expression is invalid</summary>
        InvalidBinaryOp,

        /// <summary>Specified unary operator was not found</summary>
        UnaryOpNotFound,

        /// <summary>Re-declaration is incompatible with the original</summary>
        IncompatibleRedclaration,

        // Post parse

        /// <summary>Parse was cancelled</summary>
        ParseCanceled = 2000,
    }
}
