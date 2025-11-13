// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using global::System;
using global::System.Text;

namespace System.Text
{
    internal static class PolyFillEncodingExtensions
    {
        public static unsafe string GetString(this Encoding self, ReadOnlySpan<byte> bytes)
        {
            fixed (byte* bytesPtr = bytes)
            {
                return self.GetString(bytesPtr, bytes.Length);
            }
        }
    }
}
