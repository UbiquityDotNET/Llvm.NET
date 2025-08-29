// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.IO.Hashing;

namespace Ubiquity.NET.Extensions
{
    /// <summary>Utility class to provide extension methods for a span of bytes</summary>
    public static class ByteSpanHelpers
    {
        /// <summary>Computes the hash code of the contents for a span of bytes</summary>
        /// <param name="span">Span of bytes to compute a hash code for</param>
        /// <returns>Hash code value for the span</returns>
        public static int ComputeHashCode( this ReadOnlySpan<byte> span )
        {
            return unchecked((int)XxHash3.HashToUInt64( span ));
        }
    }
}
