// -----------------------------------------------------------------------
// <copyright file="ByteSpanHelpers.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Interop
{
    public static class ByteSpanHelpers
    {
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
