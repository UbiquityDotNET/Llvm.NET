// -----------------------------------------------------------------------
// <copyright file="LlvmException.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Llvm.NET.Interop
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

        /// <summary>Initializes a new instance of the <see cref="LlvmException"/> class.</summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected LlvmException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
        }
    }
}
