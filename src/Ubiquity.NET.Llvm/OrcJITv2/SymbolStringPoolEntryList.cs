// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>List of symbol string pool entries</summary>
    /// <remarks>
    /// The entries in this list are NOT owned by this implementation. The <see cref="Dispose"/>
    /// method will dispose this list but NOT the individual strings themselves.
    /// </remarks>
    public ref struct SymbolStringPoolEntryList
    {
        /// <summary>Gets a single entry from the list</summary>
        /// <param name="index">Index of the entry to retrieve</param>
        /// <returns>Entry for this list</returns>
        /// <remarks>
        /// The returned <see cref="SymbolStringPoolEntry"/> is an 'alias' to the actual Entry
        /// there is NO ownership implied. Therefore the instance MUST not be stored anywhere.
        /// If an instance that outlives the underlying native owner is desired then you can
        /// use the <see cref="SymbolStringPoolEntry.AddRef"/> method to gain ownership of
        /// the instance.
        /// </remarks>
        public readonly SymbolStringPoolEntry this[ UInt64 index ] => GetItemAt( index );

        /// <summary>Gets the number of elements in this list</summary>
        public readonly UInt64 Count { get; }

        /// <summary>Disposes of this array of entries</summary>
        /// <remarks>
        /// This does not alter the items in the list as this instance does
        /// NOT own the entries.
        /// </remarks>
        public void Dispose( )
        {
            unsafe
            {
                if(NativeArrayPtr is not null)
                {
                    LLVMOrcDisposeSymbols( NativeArrayPtr );
                    NativeArrayPtr = null;
                }
            }
        }

        internal unsafe SymbolStringPoolEntryList( nint* pHandles, UInt64 len )
        {
            NativeArrayPtr = pHandles;
            Count = len;
        }

        // CONSIDER: This should return a dedicated ref struct type so that the rules of ownership are enforced by compiler instead of documentation...
        private readonly SymbolStringPoolEntry GetItemAt( UInt64 index )
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan( index, Count );

            unsafe
            {
                nint abiHandle = *(NativeArrayPtr + index);
                return new( abiHandle, alias: true );
            }
        }

        private unsafe nint* NativeArrayPtr;
    }
}
