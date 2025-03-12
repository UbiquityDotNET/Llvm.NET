// -----------------------------------------------------------------------
// <copyright file="AbsoluteMaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#if FUTURE_DEVELOPMENT_AREA
namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    internal class AbsoluteMaterializationUnit
        : MaterializationUnit
    {
        public AbsoluteMaterializationUnit(IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol>> absoluteSymbols)
            : base(MakeHandle(absoluteSymbols))
        {
        }

        private static LLVMOrcMaterializationUnitRef MakeHandle(IReadOnlyList<KeyValuePair<SymbolStringPoolEntry, EvaluatedSymbol>> absoluteSymbols)
        {
            // make a native useable version of the array
            using IMemoryOwner<LLVMOrcCSymbolMapPair> nativeArrayOwner = InitializeNativeCopy( absoluteSymbols );

            // pin the memory and call the native API
            using var nativeMemHandle = nativeArrayOwner.Memory.Pin();
            unsafe
            {
                return LLVMOrcAbsoluteSymbols( (LLVMOrcCSymbolMapPair*)nativeMemHandle.Pointer, absoluteSymbols.Count );
            }
        }
    }
}
#endif
