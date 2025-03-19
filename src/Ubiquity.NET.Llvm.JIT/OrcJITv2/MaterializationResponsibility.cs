// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    // NOTE: This seems "ill named" at best but this is the naming LLVM is using...

    /// <summary>LLVM ORC JIT v2 MaterializationResponsibility</summary>
    public readonly ref struct MaterializationResponsibility
    {
        /// <summary>Gets a value indicating whether this instance is disposed</summary>
        public readonly bool IsDisposed => Handle is null || Handle.IsInvalid || Handle.IsClosed;

        /// <summary>Throws an <see cref="ObjectDisposedException"/> if <see cref="IsDisposed"/> is <see langword="true"/></summary>
        public readonly void ThrowIfIDisposed() => ObjectDisposedException.ThrowIf(IsDisposed, typeof(MaterializationResponsibility));

        /// <summary>Gets the symbols associated with this request</summary>
        /// <returns>Array of symbols</returns>
        public readonly SymbolStringPoolEntryList GetRequestedSymbols()
        {
            ThrowIfIDisposed();

            unsafe
            {
                return new SymbolStringPoolEntryList(
                    LLVMOrcMaterializationResponsibilityGetRequestedSymbols(Handle, out size_t numSymbols),
                    numSymbols.ToUInt64()
                );
            }
        }

        /// <summary>Indicates to the JIT that this materialization unit has failed</summary>
        public readonly void Fail()
        {
            ThrowIfIDisposed();

            LLVMOrcMaterializationResponsibilityFailMaterialization(Handle);
        }

        /// <summary>Disposes of this instance</summary>
        public void Dispose()
        {
            Handle.Dispose();
        }

        /// <summary>Gets the library associated with thee current materialization</summary>
        public readonly JITDyLib TargetDyLib => new(LLVMOrcMaterializationResponsibilityGetTargetDylib(Handle));

        /// <summary>Gets the session associated with thee current materialization</summary>
        public readonly ExecutionSession Session => new(LLVMOrcMaterializationResponsibilityGetExecutionSession(Handle));

        internal MaterializationResponsibility(LLVMOrcMaterializationResponsibilityRef h)
        {
            Handle = h.Move();
        }

        internal MaterializationResponsibility(nint h, bool alias = false)
        {
            Handle = new(h, !alias);
        }

        internal LLVMOrcMaterializationResponsibilityRef Handle { get; }
    }
}
