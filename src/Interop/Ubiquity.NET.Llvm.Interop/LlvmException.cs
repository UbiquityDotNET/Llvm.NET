// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Exception thrown from LLVM Error Messages</summary>
    [Serializable]
    public class LlvmException
        : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="LlvmException"/> class.</summary>
        public LlvmException( )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LlvmException"/> class.</summary>
        /// <param name="message">Exception Message</param>
        public LlvmException( string message )
            : base( message )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LlvmException"/> class.</summary>
        /// <param name="message">Exception Message</param>
        /// <param name="inner">Inner Exception</param>
        public LlvmException( string message, Exception inner )
            : base( message, inner )
        {
        }
    }
}
