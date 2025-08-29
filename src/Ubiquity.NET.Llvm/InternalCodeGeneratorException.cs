// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm
{
    /// <summary>Exception generated when the internal state of the code generation cannot proceed due to an internal error</summary>
    [Serializable]
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
