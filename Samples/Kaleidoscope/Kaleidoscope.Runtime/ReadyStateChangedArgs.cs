// <copyright file="ReplLoop.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Kaleidoscope.Runtime
{
    /// <summary>Event arguments for the ready stat changed event</summary>
    public class ReadyStateChangedArgs
        : EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="ReadyStateChangedArgs"/> class.</summary>
        /// <param name="partialParse">Indicates if the event is for a partial parse</param>
        public ReadyStateChangedArgs( bool partialParse )
        {
            PartialParse = partialParse;
        }

        /// <summary>Gets a value indicating whether the parse resulted in a complete value</summary>
        public bool PartialParse { get; }

        /// <summary>Gets a value for a partial parse event</summary>
        /// <remarks>Using this static value that doesn't change reduces the number of allocated instances</remarks>
        public static ReadyStateChangedArgs PartialParseArgs { get; } = new ReadyStateChangedArgs( true );

        /// <summary>Gets a value for a complete parse event</summary>
        /// <remarks>Using this static value that doesn't change reduces the number of allocated instances</remarks>
        public static ReadyStateChangedArgs CompleteParseArgs { get; } = new ReadyStateChangedArgs( false );
    }
}
