// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.OrcJITv2Bindings;
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

        /// <summary>Gets a value indicating whether this pool is empty</summary>
        public bool IsEmpty => LibLLVMOrcSymbolStringPoolIsEmpty(Handle);

        /// <inheritdoc/>
        public override string ToString( )
        {
            return LibLLVMOrcSymbolStringPoolGetDiagnosticRepresentation(Handle);
        }

        #if DEBUG
        /// <summary>Gets the symbols in this pool</summary>
        /// <returns>Array of symbol information in this pool</returns>
        /// <remarks>
        /// <note type="important">
        /// This is intended as a diagnostic utility and is only available in a debug build. It is
        /// NOT in any way considered performant or even stable. This implementation depends on an
        /// officially undocumented string formatting of a pool and parsing that. Direct access to
        /// the reference count of symbols is NOT available via any API. The count is completely
        /// buried in private implementation details. This is an unfortunate state of affairs as
        /// knowing the count is an important diagnostic for detection of when the count is off
        /// (extra decrement or dangling references)</note>
        /// </remarks>
        public ImmutableArray<SymbolEntryInfo> GetSymbolsInPool()
        {
            return Handle.GetSymbolsInPool();
        }
        #endif

        internal SymbolStringPool( LLVMOrcSymbolStringPoolRef h )
        {
            Handle = h;
        }

        internal LLVMOrcSymbolStringPoolRef Handle { get; init; }
    }
}
