// <copyright file="InternalCodeGeneratorException.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET
{
    /// <summary>Exception generated when the internal state of the code generation cannot proceed due to an internal error</summary>
    public class InternalCodeGeneratorException
        : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="InternalCodeGeneratorException"/> class.</summary>
        public InternalCodeGeneratorException( )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InternalCodeGeneratorException"/> class.</summary>
        /// <param name="message">Message for the exception</param>
        public InternalCodeGeneratorException( string message )
            : base( message )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="InternalCodeGeneratorException"/> class.</summary>
        /// <param name="message">Message for the exception</param>
        /// <param name="inner">Inner exception</param>
        public InternalCodeGeneratorException( string message, Exception inner )
            : base( message, inner )
        {
        }
    }
}
