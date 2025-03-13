// -----------------------------------------------------------------------
// <copyright file="ThreadSafeModule.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.JIT.OrcJITv2
{
    /// <summary>LLVM ORC JIT v2 Thread Safe Module</summary>
    public sealed class ThreadSafeModule
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose() => Handle.Dispose();

        /// <summary>Gets a value indicating whether this instance is disposed</summary>
        public bool IsDisposed => Handle is null || Handle.IsInvalid || Handle.IsClosed;

        /// <summary>Throws an <see cref="ObjectDisposedException"/> if <see cref="IsDisposed"/> is <see langword="true"/></summary>
        public void ThrowIfIDisposed() => ObjectDisposedException.ThrowIf(IsDisposed, this);

        /// <summary>Delegate declaration for a generic operation on a module</summary>
        /// <param name="module">module to operate on</param>
        /// <returns>Error info from the operation</returns>
        /// <remarks>
        /// Implementations of this delegate should not throw exceptions. Any exceptions
        /// are caught and translated to an LLVMErrorRef for the native code so they do NOT
        /// pass up into the native layer as that knows nothing about managed code exceptions.
        /// </remarks>
        public delegate ErrorInfo GenericModuleOperation(BitcodeModule module);

        /// <summary>Performs an operation <paramref name="op"/> with a per thread module owned by this <see cref="ThreadSafeModule"/></summary>
        /// <param name="op">Operation to perform with the module</param>
        public void WithPerThreadModule(GenericModuleOperation op)
        {
            ThrowIfIDisposed();

            var opHandle = GCHandle.Alloc(op);
            unsafe
            {
                LLVMErrorRef errRef = LLVMOrcThreadSafeModuleWithModuleDo(Handle, &NativePerThreadModuleCallback, GCHandle.ToIntPtr(opHandle).ToPointer());
                errRef.ThrowIfFailed();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ThreadSafeModule"/> class.</summary>
        /// <param name="context">Thread safe Context to place the module into</param>
        /// <param name="module">module to make thread safe</param>
        public ThreadSafeModule(ThreadSafeContext context, BitcodeModule module)
            : this(MakeHandle(context, module))
        {
            module.Detach()
                  .SetHandleAsInvalid();
        }

        internal ThreadSafeModule(nint h, bool alias = false)
        {
            Handle = new(h, !alias);
        }

        internal ThreadSafeModule(LLVMOrcThreadSafeModuleRef h)
        {
            Handle = h;
        }

        internal LLVMOrcThreadSafeModuleRef Handle { get; init; }

        private static LLVMOrcThreadSafeModuleRef MakeHandle(ThreadSafeContext context, BitcodeModule module)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(module);

            return LLVMOrcCreateNewThreadSafeModule(module.ModuleHandle, context.Handle);
        }

        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "All instances are created as an alias or 'moved' to native Dispose() not needed" )]
        private static unsafe /*LLVMErrorRef*/ nint NativePerThreadModuleCallback(void* context, /*LLVMModuleRef*/ nint abiModuleHandle)
        {
            try
            {
                ErrorInfo errInfo = default;
                if(context is not null && GCHandle.FromIntPtr( (nint)context ).Target is GenericModuleOperation self)
                {
                    // module and underlying LLVMModuleRef created here are aliases, no need to dispose them
                    var moduleHandle = new LLVMModuleRefAlias();
                    Marshal.InitHandle(moduleHandle, abiModuleHandle);

                    var mod = BitcodeModule.FromHandle(moduleHandle);
                    errInfo = mod is not null
                        ? self(mod)
                        : ErrorInfo.Create("Internal Error: Could not create wrapped module for native method");
                }
                else
                {
                    errInfo = ErrorInfo.Create("Internal Error: Invalid context provided for native callback");
                }

                // errInfo is "moved" to native return; Dispose is wasted overhead for a NOP
                return errInfo.MoveToNative();
            }
            catch(Exception ex)
            {
                // resulting instance is "moved" to native return; Dispose is wasted overhead for a NOP
                return ErrorInfo.Create(ex)
                                .MoveToNative();
            }
        }
    }
}
