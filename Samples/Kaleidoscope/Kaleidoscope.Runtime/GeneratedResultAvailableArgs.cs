// <copyright file="ReplLoop.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Kaleidoscope.Runtime
{
    /// <summary>Contains the results of code generation</summary>
    /// <typeparam name="TResult">Result type for code generation</typeparam>
    public class GeneratedResultAvailableArgs<TResult>
        : EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="GeneratedResultAvailableArgs{TResult}"/> class.</summary>
        /// <param name="result">Result of the code generation</param>
        public GeneratedResultAvailableArgs( TResult result )
        {
            Result = result;
        }

        /// <summary>Gets the result of the code generation</summary>
        public TResult Result { get; }
    }
}
