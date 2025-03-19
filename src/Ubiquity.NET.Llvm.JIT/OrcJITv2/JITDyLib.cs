// -----------------------------------------------------------------------
// <copyright file="JITDyLib.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>struct for an LLVM ORC JIT v2 Dynamic Library</summary>
    public readonly ref struct JITDyLib
    {
        /// <summary>Defines (Adds) the materialization unit to this library</summary>
        /// <param name="materializationUnit">Unit to add</param>
        /// <remarks>
        /// On success, the <paramref name="materializationUnit"/> is no longer valid.
        /// Ownership is transferred to the JIT. On failure, an exception is thrown
        /// but ownership remains with the caller. The invalidation leaves the <see cref="IDisposable.Dispose"/>
        /// call as a NOP and any operations on the unit throw an <see cref="ObjectDisposedException"/>.
        /// </remarks>
        public void Define(MaterializationUnit materializationUnit)
        {
            ArgumentNullException.ThrowIfNull(materializationUnit);
            using LLVMErrorRef errorRef = LLVMOrcJITDylibDefine(Handle, materializationUnit.Handle);
            errorRef.ThrowIfFailed();

            // successfully transferred ownership to native code, mark it as such
            materializationUnit.Handle.SetHandleAsInvalid();
        }

        /// <summary>Creates a <see cref="ResourceTracker"/> associated with this library</summary>
        /// <returns>New resource tracker</returns>
        public ResourceTracker CreateResourceTracker()
        {
            return new(LLVMOrcJITDylibCreateResourceTracker(Handle));
        }

        internal JITDyLib(LLVMOrcJITDylibRef h)
        {
            Handle = h;
        }

        internal JITDyLib(nint abiHandle)
        {
            Handle = LLVMOrcJITDylibRef.FromABI(abiHandle);
        }

        internal LLVMOrcJITDylibRef Handle { get; init; }
    }
}
