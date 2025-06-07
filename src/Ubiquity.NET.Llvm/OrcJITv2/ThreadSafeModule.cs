// -----------------------------------------------------------------------
// <copyright file="ThreadSafeModule.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Thread Safe Module</summary>
    public sealed class ThreadSafeModule
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose( ) => Handle.Dispose();

        /// <summary>Gets a value indicating whether this instance is disposed</summary>
        public bool IsDisposed => Handle is null || Handle.IsInvalid || Handle.IsClosed;

        /// <summary>Throws an <see cref="ObjectDisposedException"/> if <see cref="IsDisposed"/> is <see langword="true"/></summary>
        public void ThrowIfIDisposed( ) => ObjectDisposedException.ThrowIf( IsDisposed, this );

        /// <summary>Delegate declaration for a generic operation on a module</summary>
        /// <param name="module">module to operate on</param>
        /// <returns>Error info from the operation</returns>
        /// <remarks>
        /// Implementations of this delegate should not throw exceptions. Any exceptions
        /// are caught and translated to an LLVMErrorRef for the native code so they do NOT
        /// pass up into the native layer as that knows nothing about managed code exceptions.
        /// </remarks>
        public delegate ErrorInfo GenericModuleOperation( IModule module );

        /// <summary>Performs an operation <paramref name="op"/> with a per thread module owned by this <see cref="ThreadSafeModule"/></summary>
        /// <param name="op">Operation to perform with the module</param>
        public void WithPerThreadModule( GenericModuleOperation op )
        {
            ThrowIfIDisposed();

            var opHandle = GCHandle.Alloc(op);
            try
            {
                unsafe
                {
                    using LLVMErrorRef errRef = LLVMOrcThreadSafeModuleWithModuleDo(
                        Handle,
                        &NativePerThreadModuleCallback,         // Use static method for native callback
                        GCHandle.ToIntPtr(opHandle).ToPointer() // provide allocated handle as the context for the callback
                        );
                    errRef.ThrowIfFailed();
                }
            }
            finally
            {
                opHandle.Free();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ThreadSafeModule"/> class.</summary>
        /// <param name="context">Thread safe ContextAlias to place the module into</param>
        /// <param name="module">module to make thread safe</param>
        /// <remarks>
        /// <note type="important">
        /// This is a "move" constructor and takes ownership of the module. The module provided is NOT
        /// usable after this except to dispose it (which is a NOP).
        /// </note>
        /// </remarks>
        public ThreadSafeModule( ThreadSafeContext context, Module module )
            : this( MakeHandle( context, module ) )
        {
            module.InvalidateFromMove();
        }

        internal ThreadSafeModule( nint h, bool alias = false )
        {
            Handle = new( h, !alias );
        }

        internal ThreadSafeModule( LLVMOrcThreadSafeModuleRef h )
        {
            Handle = h.Move();
        }

        internal LLVMOrcThreadSafeModuleRef Handle { get; init; }

        private static LLVMOrcThreadSafeModuleRef MakeHandle( ThreadSafeContext context, Module module )
        {
            ArgumentNullException.ThrowIfNull( context );
            ArgumentNullException.ThrowIfNull( module );

            LLVMOrcThreadSafeModuleRef retVal = LLVMOrcCreateNewThreadSafeModule(module.GetOwnedHandle(), context.Handle);
            module.InvalidateFromMove();
            return retVal;
        }

        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "All instances are created as an alias or 'moved' to native Dispose() not needed" )]
        private static unsafe /*LLVMErrorRef*/ nint NativePerThreadModuleCallback( void* context, LLVMModuleRefAlias moduleHandle )
        {
            try
            {
                if(!MarshalGCHandle.TryGet<GenericModuleOperation>( context, out GenericModuleOperation? self ))
                {
                    return LLVMErrorRef.CreateForNativeOut( "Internal Error: Invalid context provided for native callback"u8 );
                }

                IModule mod = new ModuleAlias(moduleHandle);
                return mod is not null
                    ? self( mod ).MoveToNative()
                    : LLVMErrorRef.CreateForNativeOut( "Internal Error: Could not create wrapped module for native method"u8 );
            }
            catch(Exception ex)
            {
                return LLVMErrorRef.CreateForNativeOut( ex.Message );
            }
        }
    }
}
