// -----------------------------------------------------------------------
// <copyright file="CodeGenerationExceptionEventArgs.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Kaleidoscope.Runtime
{
    /// <summary>Event arguments to report <see cref="CodeGeneratorException"/> while processing the REPL input</summary>
    public class CodeGenerationExceptionEventArgs
        : EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="CodeGenerationExceptionEventArgs"/> class.</summary>
        /// <param name="exception">Exception that occurred during code generation</param>
        public CodeGenerationExceptionEventArgs( CodeGeneratorException exception )
        {
            Exception = exception;
        }

        /// <summary>Gets the Exception that occurred during code generation</summary>
        public CodeGeneratorException Exception { get; }
    }
}
