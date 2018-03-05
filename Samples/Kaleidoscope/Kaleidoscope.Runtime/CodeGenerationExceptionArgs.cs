// <copyright file="ReplLoop.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Kaleidoscope.Runtime
{
    public class CodeGenerationExceptionArgs
        : EventArgs
    {
        public CodeGenerationExceptionArgs( CodeGeneratorException exception )
        {
            Exception = exception;
        }

        public CodeGeneratorException Exception { get; }
    }
}
