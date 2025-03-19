// -----------------------------------------------------------------------
// <copyright file="ByteSpanHelpers.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Utility class to provide extension methods for a span of bytes</summary>
    public static class ByteSpanHelpers
    {
        /// <summary>Computes the hash code of the contents for a span of bytes</summary>
        /// <param name="span">Span of bytes to compute a hash code for</param>
        /// <returns>Hash code</returns>
        public static int ComputeHashCode(ReadOnlySpan<byte> span)
        {
            // if there's no data, then the hash is 0
            if (span.Length == 0)
            {
                return 0;
            }

            // use BLAKE3 to compute a hash code of the span data, and then hash the resulting array.
            llvm_blake3_hasher hasher = default;
            llvm_blake3_hasher_init(ref hasher);
            unsafe
            {
                fixed(byte* p = &MemoryMarshal.GetReference(span))
                {
                    llvm_blake3_hasher_update(ref hasher, p, span.Length);
                }

                llvm_blake3_hasher_finalize(ref hasher, out byte[] hashCode, out nint _);
                return hashCode.GetHashCode();
            }
        }
    }
}
