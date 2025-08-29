// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>ORC JIT v2 symbol string pool</summary>
    public readonly ref struct SymbolStringPool
    {
        /// <summary>Clears all unreferenced strings in the symbol pool</summary>
        /// <remarks>
        /// This clears all the unused entries in an <see cref="SymbolStringPool"/>.
        /// Since this must lock the pool for the duration, which prevents interning
        /// new strings, it is recommended that this is called infrequently. Ideally
        /// this is only called when the caller has reason to know that some entries
        /// will no longer have references such as after removing a module or closing
        /// a <see cref="JITDyLib"/>.
        /// </remarks>
        public void ClearDeadEntries( )
        {
            LLVMOrcSymbolStringPoolClearDeadEntries( Handle );
        }

        internal SymbolStringPool( LLVMOrcSymbolStringPoolRef h )
        {
            Handle = h;
        }

        internal LLVMOrcSymbolStringPoolRef Handle { get; init; }
    }
}
