// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.OrcJITv2Bindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Delegate for an error reporter callback</summary>
    /// <param name="info">Information about the error</param>
    public delegate void ErrorReporter( ref readonly ErrorInfo info );

    /// <summary>ORC JIT v2 Execution Session</summary>
    /// <remarks>
    /// This is a `ref struct` as the JIT is ultimately the owner
    /// of the underlying session. Thus, this instance does not
    /// retain any state not part of the underlying native support.
    /// </remarks>
    public readonly ref struct ExecutionSession
    {
        /// <summary>Gets a reference to the symbol string pool for this session</summary>
        public SymbolStringPool SymbolStringPool => new( LLVMOrcExecutionSessionGetSymbolStringPool( Handle ) );

        /// <summary>Creates a Lazy Call Through manager for this session</summary>
        /// <param name="triple">Triple to use for this factory</param>
        /// <param name="errorHandlerAddress">Native JIT address of an error handler</param>
        /// <returns>New call through manager</returns>
        public LazyCallThroughManager CreateLazyCallThroughManager( LazyEncodedString triple, UInt64 errorHandlerAddress = 0 )
        {
            ArgumentNullException.ThrowIfNull( triple );

            using LLVMErrorRef errRef = LLVMOrcCreateLocalLazyCallThroughManager(
                                            triple,
                                            Handle,
                                            errorHandlerAddress,
                                            out LLVMOrcLazyCallThroughManagerRef resultHandle
                                            );

            // ownership is transferred to return, this using scope handles release on exception
            using(resultHandle)
            {
                errRef.ThrowIfFailed();
                return new( resultHandle );
            }
        }

        /*
                [Experimental]
                public void Lookup(LookupKind kind, scoped ReadOnlySpan<SearchOrder> order, scoped ReadOnlySpan<LookupSet> symbols, LookupResultHandler handler)
                {
                    // validate args...
                    var ctx = GCHandle.Alloc(handler).ToPointer;
                    var abiOrder[] = ????
                    var abiSymbols = ????

                    LLVMOrcExecutionSessionLookup(Handle, (LLVMOrcLookupKind)kind, abiOrder, abiOrder.Length, abiSymbols, abiSymbols.Length, &NativeLookupHandleResult, ctx);
                }

                // Caller [LLVM internals] OWNS and retains ownership of the array `In` semantics, and takes ownership of ALL entries
                // Implementations must take action to retain any strings it provides if they have meaning beyond this call (Normally through
                // some form of `AddRefHandle`)
                [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
                [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
                private static void NativeLookupHandleResult([LLVMErrorRef] nint err, LLVMOrcCSymbolMapPair* result, nint numPairs, void* ctx)
                {
                    try
                    {
                        var errInfo = new ErrorInfo(err);
                        if(errInfo.Failed)
                        {
                            string msg = errInfo.ToString();
                            // What to do with the string?
                            // What logging mechanism?
                            // Can't use Ctx as it is officially undefined if err is a failure...
                            return;
                        }

                        if(MarshalGCHandle.TryGet<LookupResultHandler>(context, out LookupResultHandler? self))
                        {
                            // This would bleed the interop type into the caller, but is the most efficient.
                            self(new Span<LLVMOrcCSymbolMapPair>(result, numPairs));
                        }

                        GCHandle.Free(ctx); // release the handle [One-time call]
                    }
                }
        */

#if FUTURE_DEVELOPMENT_AREA
        // CONSIDER: It might be better if this was a method on the JIT (and internal on this type)
        // so that the JIT instance could hold on to the lifetime to keep callers from needing to
        // deal with lifetime management. Lifetime management and native callbacks is is a difficult
        // issue, made more complex by AOT support. Marshal.GetFunctionPointerForDelegate MUST
        // dynamically build a native callable thunk, that performs all marshalling. But that is
        // not an option for AOT scenarios. [Sadly that API doesn't appear to be flagged as off
        // limits for an AOT compatible app/library...]
        // TODO: Follow pattern in other places of this library and define a custom holder.

        /// <summary>Set the error reporter for the session</summary>
        /// <param name="errorReporter">Error reporter to set</param>
        /// <remarks>
        /// The type of <paramref name="errorReporter"/> is <see cref="DelegateHolder{T}"/>
        /// to enable setting of the callback without the need to retain this particular
        /// instance. This <see cref="ExecutionSession"/> instance is only a reference
        /// to the internally owned session and therefore the instance the delegate refers
        /// to must ALSO remain valid as long as it is set. It is the caller's responsibility
        /// to ensure that the value provided to <paramref name="errorReporter"/> remains
        /// alive while set. This instance of <see cref="ExecutionSession"/> usually does
        /// not out live the required lifetime of the error reporter. The session this refers
        /// to is OWNED by the JIT and thus the lifetime of the reporter must outlast that
        /// of the JIT itself.
        /// </remarks>
        public void SetErrorReporter(DelegateHolder<ErrorReporter> errorReporter)
        {
            ArgumentNullException.ThrowIfNull(errorReporter);

            unsafe
            {
                LLVMOrcExecutionSessionSetErrorReporter(Handle, &NativeErrorReporterCallback, (void*)errorReporter.Context);
            }
        }
#endif

        /// <summary>Gets an existing <see cref="JITDyLib"/> or creates a new one</summary>
        /// <param name="name">name of the library</param>
        /// <returns>The dynamic lib for the name</returns>
        /// <remarks>
        /// This does not populate any symbols for the library when creating a new one.
        /// All configuration is the responsibility of the caller.
        /// </remarks>
        public JITDyLib GetOrCreateBareDyLib( LazyEncodedString name )
        {
            return TryGetDyLib( name, out JITDyLib foundLib )
                   ? foundLib
                   : new( LLVMOrcExecutionSessionCreateBareJITDylib( Handle, name ) );
        }

        /// <summary>Gets or creates a <see cref="JITDyLib"/> in this session by name</summary>
        /// <param name="name">name of the library</param>
        /// <returns>The library from this session</returns>
        /// <remarks>
        /// <para>If the library is created, this will add symbols for any attached platforms.
        /// If there are no attached platforms then this is the same as calling <see cref="GetOrCreateBareDyLib"/>.</para>
        /// <para><paramref name="name"/> is a LazyEncodedString to allow for the possibility of retrieval of the name from
        /// native code and then providing it back again without going through any sort of marshal/unmarshal sequence. This
        /// allows for the most efficient use of data that is likely to come from the underlying native code.
        /// </para>
        /// </remarks>
        public JITDyLib GetOrCreateDyLib( LazyEncodedString name )
        {
            ArgumentNullException.ThrowIfNull( name );

            // check if already present and use that as the out value...
            if(TryGetDyLib( name, out JITDyLib lib ))
            {
                return lib;
            }

            using LLVMErrorRef err = LLVMOrcExecutionSessionCreateJITDylib(Handle, out LLVMOrcJITDylibRef libHandle, name);
            err.ThrowIfFailed();
            return new( libHandle );
        }

        /// <summary>Tries to get or create a <see cref="JITDyLib"/> in this session by name</summary>
        /// <param name="name">name of the library</param>
        /// <param name="lib">Library or <see langword="null"/> if not found</param>
        /// <param name="errInfo"><see cref="ErrorInfo"/> for any errors in creating the library if it didn't exist</param>
        /// <returns><see langword="true"/> if successful and <see langword="false"/> if not</returns>
        /// <remarks>
        /// <para>This will add symbols for any attached platforms. If there are no attached platforms then this
        /// is the same as calling <see cref="GetOrCreateBareDyLib"/>.</para>
        /// <para><paramref name="name"/> is a LazyEncodedString to allow for the possibility of retrieval of the name from
        /// native code and then providing it back again without going through any sort of marshal/unmarshal sequence. This
        /// allows for the most efficient use of data that is likely to come from the underlying native code.
        /// </para>
        /// </remarks>
        public bool TryGetOrCreateDyLib( LazyEncodedString name, out JITDyLib lib, out ErrorInfo errInfo )
        {
            ArgumentNullException.ThrowIfNull( name );

            errInfo = default; // defaults to success!

            // check if already present and use that as the out value...
            if(TryGetDyLib( name, out lib ))
            {
                return true;
            }

#pragma warning disable IDISP004 // Don't ignore created IDisposable

            // ownership of return LLVMErrorRef is transferred (MOVE Semantics) to errInfo 'out' param
            errInfo = new( LLVMOrcExecutionSessionCreateJITDylib( Handle, out LLVMOrcJITDylibRef libHandle, name ).ThrowIfInvalid() );
#pragma warning restore IDISP004 // Don't ignore created IDisposable

            if(errInfo.Success)
            {
                lib = new( libHandle );
            }

            return errInfo.Success;
        }

        /// <summary>Removes a <see cref="JITDyLib"/> from this session by name</summary>
        /// <param name="name">Name of the lib</param>
        /// <returns><see langword="true"/> if the library is found; <see langword="false"/> if not</returns>
        /// <exception cref="LlvmException">Error from the native interop LLVM API</exception>
        public bool RemoveDyLib( LazyEncodedString name )
        {
            if(TryGetDyLib( name, out JITDyLib lib ))
            {
                using LLVMErrorRef err = LibLLVMExecutionSessionRemoveDyLib(Handle, lib.Handle);
                err.ThrowIfFailed();
                return true;
            }

            return false;
        }

        /// <summary>Tries to get a named library from this session</summary>
        /// <param name="name">name of the library</param>
        /// <param name="lib">[out] library if found or <see langword="null"/></param>
        /// <returns><see langword="true"/> if found or <see langword="false"/> if not</returns>
        public bool TryGetDyLib( LazyEncodedString name, out JITDyLib lib )
        {
            ArgumentNullException.ThrowIfNull( name );

            lib = default;
            LLVMOrcJITDylibRef nativeLib = LLVMOrcExecutionSessionGetJITDylibByName(Handle, name);
            if(nativeLib.IsNull)
            {
                return false;
            }

            lib = new( nativeLib );
            return true;
        }

        /// <summary>Interns a string in the pool</summary>
        /// <param name="name">NameField of the symbol to intern in the pool for this session</param>
        /// <returns>Entry to the string in the pool</returns>
        public SymbolStringPoolEntry Intern( string name )
        {
            return new( LLVMOrcExecutionSessionIntern( Handle, name ) );
        }

        internal ExecutionSession( LLVMOrcExecutionSessionRef h )
        {
            Handle = h;
        }

        internal ExecutionSession( nint h )
            : this( LLVMOrcExecutionSessionRef.FromABI( h ) )
        {
        }

        internal LLVMOrcExecutionSessionRef Handle { get; init; }

        /// <summary>Native code callback for error reporting</summary>
        /// <param name="context">The context for the callback is a <see cref="GCHandle"/> with a target of <see cref="ErrorReporter"/></param>
        /// <param name="abiErrorRef">ABI handle for an error ref</param>
        [UnmanagedCallersOnly( CallConvs = [ typeof( CallConvCdecl ) ] )]
        [SuppressMessage( "Design", "CA1031:Do not catch general exception types", Justification = "REQUIRED for unmanaged callback - Managed exceptions must never cross the boundary to native code" )]
        private static unsafe void NativeErrorReporterCallback( void* context, /*LLVMErrorRef*/ nint abiErrorRef )
        {
            try
            {
                if(MarshalGCHandle.TryGet<ErrorReporter>( context, out ErrorReporter? self ))
                {
                    // It is Unclear, if this native handler is supposed to Dispose the provided error ref
                    // or not (it makes sense for it to do so). This will do it in one place so that if it
                    // is found otherwise, then it is corrected once for all. [Thus the `ref readonly`
                    // declaration of the parameter in the delegate]
                    // Ownership is "moved" to the ErrorInfo instance created
                    using var errInfo = new ErrorInfo(abiErrorRef);
                    self( in errInfo );
                }
            }
            catch
            {
                // The most likely reason for hitting this assert is that the GC has collected the memory for
                // the context. This indicates that the caller of SetErrorReporter is NOT keeping the instance
                // alive as long as it is still needed!
                Debug.Assert( false, "Exception in native callback!" );
            }
        }
    }
}
