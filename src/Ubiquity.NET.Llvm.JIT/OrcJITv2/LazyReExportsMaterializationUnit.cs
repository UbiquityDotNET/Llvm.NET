// -----------------------------------------------------------------------
// <copyright file="LazyReExportsMaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Lazy Re-exports Materialization unit</summary>
    public class LazyReExportsMaterializationUnit
        : MaterializationUnit
    {
        /// <summary>Initializes a new instance of the <see cref="LazyReExportsMaterializationUnit"/> class.</summary>
        /// <param name="callThruMgr">Call through manager for this unit</param>
        /// <param name="stubsMgr">Stub manager for this unit</param>
        /// <param name="srcLib">Src library for this unit</param>
        /// <param name="symbols">symbols for this unit</param>
        public LazyReExportsMaterializationUnit(
            LazyCallThroughManager callThruMgr,
            LocalIndirectStubsManager stubsMgr,
            JITDyLib srcLib,
            IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, SymbolAliasMapEntry>> symbols)
            : base(MakeHandle(callThruMgr, stubsMgr, srcLib, symbols))
        {
        }

        private static LLVMOrcMaterializationUnitRef MakeHandle(
            LazyCallThroughManager callThruMgr,
            LocalIndirectStubsManager stubsMgr,
            JITDyLib srcLib,
            IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, SymbolAliasMapEntry>> symbols)
        {
            ArgumentNullException.ThrowIfNull(callThruMgr);
            ArgumentNullException.ThrowIfNull(stubsMgr);

            // make a native useable version of the input list, pin it and call the native API
            using var nativeArrayOwner = InitializeNativeCopy( symbols );
            using var nativeMemHandle = nativeArrayOwner.Memory.Pin();
            unsafe
            {
                return LLVMOrcLazyReexports(
                    callThruMgr.Handle,
                    stubsMgr.Handle,
                    srcLib.Handle,
                    (LLVMOrcCSymbolAliasMapPair*)nativeMemHandle.Pointer,
                    symbols.Count
                );
            }
        }
    }
}
