// -----------------------------------------------------------------------
// <copyright file="Blake3.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Security.Cryptography;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Blake3;

namespace Ubiquity.NET.Llvm
{
    /// <summary>.NET Wrapper around the LLVM implementation of the Blake3 hash algorithm</summary>
    /// <seealso href="https://en.wikipedia.org/wiki/BLAKE_(hash_function)"/>
    /// <seealso href="https://llvm.org/doxygen/md_lib_Support_BLAKE3_README.html"/>
    /// <seealso href="https://llvm.org/doxygen/classllvm_1_1BLAKE3.html"/>
    public class Blake3Hash
        : HashAlgorithm
    {
        /// <inheritdoc/>
        public override void Initialize()
        {
            llvm_blake3_hasher_init(ref NativeHasher);
        }

        /// <inheritdoc/>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            ArgumentNullException.ThrowIfNull(array);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(array.Length, ibStart + cbSize);

            unsafe
            {
                fixed(byte* p = &MemoryMarshal.GetArrayDataReference(array))
                {
                    llvm_blake3_hasher_update(ref NativeHasher, p + ibStart, cbSize);
                }
            }
        }

        /// <inheritdoc/>
        protected override byte[] HashFinal()
        {
            llvm_blake3_hasher_finalize(ref NativeHasher, out byte[] retVal, out nint _);
            return retVal;
        }

        private llvm_blake3_hasher NativeHasher;
    }
}
