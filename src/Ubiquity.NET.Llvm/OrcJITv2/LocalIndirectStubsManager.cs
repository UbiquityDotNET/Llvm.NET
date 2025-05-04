// -----------------------------------------------------------------------
// <copyright file="LocalIndirectStubsManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Local Indirect Stubs Manager</summary>
    public sealed class LocalIndirectStubsManager
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="LocalIndirectStubsManager"/> class.</summary>
        /// <param name="triple">Triple string for the manager</param>
        public LocalIndirectStubsManager(LazyEncodedString triple)
            : this(MakeHandle(triple))
        {
        }

        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        internal LocalIndirectStubsManager(LLVMOrcIndirectStubsManagerRef h)
        {
            Handle = h.Move();
        }

        internal LLVMOrcIndirectStubsManagerRef Handle { get; init; }

        private static LLVMOrcIndirectStubsManagerRef MakeHandle(LazyEncodedString triple)
        {
            ArgumentNullException.ThrowIfNull(triple);

            unsafe
            {
                using MemoryHandle nativeMem = triple.Pin();
                return LLVMOrcCreateLocalIndirectStubsManager((byte*)nativeMem.Pointer);
            }
        }
    }
}
