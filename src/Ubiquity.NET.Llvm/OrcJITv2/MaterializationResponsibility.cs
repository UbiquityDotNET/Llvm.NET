// -----------------------------------------------------------------------
// <copyright file="MaterializationUnit.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    // NOTE: This seems "ill named" at best but this is the naming LLVM is using...

    /// <summary>LLVM ORC JIT v2 MaterializationResponsibility</summary>
    public readonly ref struct MaterializationResponsibility
    {
        /// <summary>Gets the library associated with thee current materialization</summary>
        public readonly JITDyLib TargetDyLib => new( LLVMOrcMaterializationResponsibilityGetTargetDylib( Handle ) );

        /// <summary>Gets the session associated with thee current materialization</summary>
        public readonly ExecutionSession Session => new( LLVMOrcMaterializationResponsibilityGetExecutionSession( Handle ) );

        /// <summary>Gets a value indicating whether this instance is disposed</summary>
        public readonly bool IsDisposed => Handle is null || Handle.IsInvalid || Handle.IsClosed;

        /// <summary>Throws an <see cref="ObjectDisposedException"/> if <see cref="IsDisposed"/> is <see langword="true"/></summary>
        public readonly void ThrowIfIDisposed( ) => ObjectDisposedException.ThrowIf( IsDisposed, typeof( MaterializationResponsibility ) );

        /// <summary>Transfers responsibility of all symbols defined by <paramref name="unit"/> to it</summary>
        /// <param name="unit">Unit to transfer responsibility for</param>
        /// <returns>Error information about the attempt to perform the replace</returns>
        public ErrorInfo Replace( MaterializationUnit unit )
        {
            return new( LLVMOrcMaterializationResponsibilityReplace( Handle, unit.Handle ) );
        }

        /// <summary>Gets the symbols associated with this request</summary>
        /// <returns>Array of symbols</returns>
        public readonly SymbolStringPoolEntryList GetRequestedSymbols( )
        {
            ThrowIfIDisposed();

            unsafe
            {
                return new SymbolStringPoolEntryList(
                    LLVMOrcMaterializationResponsibilityGetRequestedSymbols( Handle, out nuint numSymbols ),
                    numSymbols.ToUInt64()
                );
            }
        }

        /// <summary>Indicates to the JIT that this materialization unit has failed</summary>
        public readonly void Fail( )
        {
            ThrowIfIDisposed();

            LLVMOrcMaterializationResponsibilityFailMaterialization( Handle );
        }

        /// <summary>Disposes of this instance</summary>
        public void Dispose( )
        {
            Handle.Dispose();
        }

        /// <summary>Gets the pseudo-symbol name of the initializer for this responsibility, if any</summary>
        /// <returns>Static Initializer or <see langword="null"/> if none exists</returns>
        public SymbolStringPoolEntry? GetInitializerSymbol( )
        {
            var h = LLVMOrcMaterializationResponsibilityGetInitializerSymbol(Handle);
            return h.IsNull ? null : new( h );
        }

        /// <summary>Attempt to Claim responsibility for new symbols</summary>
        /// <param name="symbols">Symbols to claim responsibility for</param>
        /// <returns>Error information for the attempt</returns>
        /// <remarks>
        /// <para>This method can be used to claim responsibility for symbols that are added to a
        /// materialization unit during the compilation process (e.g. literal pool symbols). Symbol
        /// linkage rules are the same as for symbols that are defined up front: duplicate strong
        /// definitions will result in errors. Duplicate weak definitions will be discarded (in which
        /// case they will not be added to this responsibility instance).</para>
        /// <para>This method can be used by materialization units that want to add additional symbols
        /// at materialization time (e.g. stubs, compile callbacks, metadata).</para>
        /// </remarks>
        public ErrorInfo TryClaim( IReadOnlyCollection<KeyValuePair<SymbolStringPoolEntry, SymbolFlags>> symbols )
        {
            // get a native form of the input array, pin it, and then call the LLVM-C API.
            // TODO: Optimize this so a copy/marshal/conversion is NOT needed and the input is pinnable
            // To do that, the projected structures MUST be layout compatible with the native forms
            // so a simple pin/cast is all that is needed.
            using IMemoryOwner<LLVMOrcCSymbolFlagsMapPair> nativeOwner = symbols.InitializeNativeCopy();
            using MemoryHandle nativeHandle = nativeOwner.Memory.Pin();
            unsafe
            {
                return new( LLVMOrcMaterializationResponsibilityDefineMaterializing(
                            Handle,
                            (LLVMOrcCSymbolFlagsMapPair*)nativeHandle.Pointer,
                            checked((nuint)symbols.Count)
                            )
                          );
            }
        }

        internal MaterializationResponsibility( LLVMOrcMaterializationResponsibilityRef h )
        {
            Handle = h.Move();
        }

        internal MaterializationResponsibility( nint h, bool alias = false )
        {
            Handle = new( h, !alias );
        }

        internal LLVMOrcMaterializationResponsibilityRef Handle { get; }
    }
}
