// <copyright file="ReplLoop.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Kaleidoscope.Runtime
{
    public class GeneratedResultAvailableArgs<TResult>
        : EventArgs
    {
        public GeneratedResultAvailableArgs( TResult result )
        {
            Result = result;
        }

        public TResult Result { get; }
    }
}
