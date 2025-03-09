// -----------------------------------------------------------------------
// <copyright file="Blake3.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Security.Cryptography;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>LLVM implementation of the Blake3 hash algorithm</summary>
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
