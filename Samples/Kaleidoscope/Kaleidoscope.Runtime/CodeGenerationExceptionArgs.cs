// <copyright file="ReplLoop.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Kaleidoscope.Runtime
{
    /// <summary>Event arguments to report <see cref="CodeGeneratorException"/> while processing the <see cref="ReplLoop{TResult}"/></summary>
    public class CodeGenerationExceptionArgs
        : EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="CodeGenerationExceptionArgs"/> class.</summary>
        /// <param name="exception">Exception that occurred during code generation</param>
        public CodeGenerationExceptionArgs( CodeGeneratorException exception )
        {
            Exception = exception;
        }

        /// <summary>Gets the Exception that occurred during code generation</summary>
        public CodeGeneratorException Exception { get; }
    }
}
