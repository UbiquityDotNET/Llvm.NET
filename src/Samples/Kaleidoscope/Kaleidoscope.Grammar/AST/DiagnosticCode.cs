// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Kaleidoscope.Grammar.AST
{
    public enum DiagnosticCode
        : int
    {
        None = 0,

        // Codes with integer values < 1000 are reserved for grammar states

        // AST Builder
        SyntaxError = 1000,
        UnknownVariable,
        InvokeUnknownFunction,
        InvalidUnaryOp,
        InvalidUnaryOpRef,
        InvalidBinaryOp,
        UnaryOpNotFound,
        IncompatibleRedclaration,

        // Post parse
        ParseCanceled = 2000,
    }
}
