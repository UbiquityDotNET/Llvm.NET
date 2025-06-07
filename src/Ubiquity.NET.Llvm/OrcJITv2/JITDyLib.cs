// -----------------------------------------------------------------------
// <copyright file="JITDyLib.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>struct for an LLVM ORC JIT v2 Dynamic Library</summary>
    public readonly ref struct JITDyLib
    {
        /// <summary>Add a <see cref="DefinitionGenerator"/> to this instance</summary>
        /// <param name="generator">The generator to add</param>
        /// <remarks>
        /// This method has "MOVE" semantics and will invalidate <paramref name="generator"/>
        /// on successful completion. That is <see cref="DefinitionGenerator.Dispose"/> becomes
        /// a NOP. Thus callers need not care about whether it is transferred or not and just
        /// dispose of it the same either way.
        /// </remarks>
        public void Add( DefinitionGenerator generator )
        {
            ArgumentNullException.ThrowIfNull( generator );
            Handle.ThrowIfInvalid();

            LLVMOrcJITDylibAddGenerator( Handle, generator.Handle );

            // ownership transfer complete, mark it as such so Dispose becomes a NOP.
            generator.Handle.SetHandleAsInvalid();
        }

        /// <summary>Defines (Adds) the materialization unit to this library</summary>
        /// <param name="materializationUnit">Unit to add</param>
        /// <remarks>
        /// On success, the <paramref name="materializationUnit"/> is no longer valid.
        /// Ownership is transferred to the JIT. On failure, an exception is thrown
        /// but ownership remains with the caller. The invalidation leaves the <see cref="IDisposable.Dispose"/>
        /// call as a NOP and any operations on the unit throw an <see cref="ObjectDisposedException"/>.
        /// </remarks>
        public void Define( MaterializationUnit materializationUnit )
        {
            ArgumentNullException.ThrowIfNull( materializationUnit );
            Handle.ThrowIfInvalid();

            using LLVMErrorRef errorRef = LLVMOrcJITDylibDefine(Handle, materializationUnit.Handle);
            errorRef.ThrowIfFailed();

            // successfully transferred ownership to native code, mark it as such
            materializationUnit.Handle.SetHandleAsInvalid();
        }

        /// <summary>Creates a <see cref="ResourceTracker"/> associated with this library</summary>
        /// <returns>New resource tracker</returns>
        [MustUseReturnValue]
        public ResourceTracker CreateResourceTracker( )
        {
            Handle.ThrowIfInvalid();

            return new( LLVMOrcJITDylibCreateResourceTracker( Handle ) );
        }

        /// <summary>Gets the default tracker for this instance</summary>
        /// <returns>Default tracker for this instance</returns>
        [MustUseReturnValue]
        public ResourceTracker GetDefaultTracker( )
        {
            Handle.ThrowIfInvalid();

            return new( LLVMOrcJITDylibGetDefaultResourceTracker( Handle ) );
        }

        /// <summary>Equivalent to calling <see cref="ResourceTracker.RemoveAll"/> on all trackers from this instance</summary>
        public void ClearAllTrackers( )
        {
            Handle.ThrowIfInvalid();

            using var errorRef = LLVMOrcJITDylibClear(Handle);
            errorRef.ThrowIfFailed();
        }

        internal JITDyLib( LLVMOrcJITDylibRef h )
        {
            Handle = h;
        }

        internal JITDyLib( nint abiHandle )
        {
            Handle = LLVMOrcJITDylibRef.FromABI( abiHandle );
        }

        internal LLVMOrcJITDylibRef Handle { get; init; }
    }
}
