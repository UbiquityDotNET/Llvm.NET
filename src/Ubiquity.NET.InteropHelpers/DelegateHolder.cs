// <copyright file="WrappedNativeCallback2.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

#if FUTURE_DEVELOPMENT_AREA
// Lifetime management and native callbacks is a difficult issue, made more complex by AOT support.
// Marshal.GetFunctionPointerForDelegate MUST dynamically build a native callable thunk, that performs
// all marshalling. Since it is a runtime function that reflection and code emission is done at runtime,
// but that is not an option for AOT scenarios. [Sadly that API doesn't appear to be flagged as off
// limits for an AOT compatible app/library...]

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.InteropHelpers
{
    /// <summary>Keep alive holder to ensure native call back delegates are not destroyed while registered with native code</summary>
    /// <typeparam name="T">Delegate signature of the native callback</typeparam>
    /// <remarks>
    /// This generates a holder for a delegate that allows a native function pointer for the delegate to remain valid until the
    /// instance of this wrapper is disposed. This is generally only necessary where the native call back must remain valid for
    /// an extended period of time. (e.g. beyond the call that provides the callback)
    ///
    /// <note type="note">
    /// This doesn't actually pin the delegate, but it does add
    /// an additional reference
    /// see: https://docs.microsoft.com/en-us/cpp/dotnet/how-to-marshal-callbacks-and-delegates-by-using-cpp-interop for more info.
    /// </note>
    /// </remarks>
    public sealed class DelegateHolder<T>
        : IDisposable
        where T : Delegate
    {
        /// <summary>Initializes a new instance of the <see cref="DelegateHolder{T}"/> class.</summary>
        /// <param name="d">Delegate</param>
        public DelegateHolder(T d)
        {
            ArgumentNullException.ThrowIfNull(d);

#if DEBUG
            // TODO: Ideally, these should be moved to or replicated in an analyzer for this library.

            // These checks are based on type and impact the runtime for AOT, so are only enabled for a debug build.
            // For a release build they are just perf overhead for well tested consumers.
            if(d.GetType().IsGenericType)
            {
                // Marshal.GetFunctionPointerForDelegate will create an exception for this but the
                // error message is, pardon the pun, a bit too generic. Hopefully, this makes it a
                // bit more clear.
                throw new ArgumentException( "Marshaling of generic delegate types to a native callback is not supported" );
            }

            // Technically speaking this is NOT required for the API calls below to succeed.
            // However, without this attribute the SYSTEM DEFAULT calling convention is used,
            // which may or may not match the intended usage. So enforcing that attribute is
            // usually helpful.
            if(d.GetType().GetCustomAttributes( typeof( UnmanagedFunctionPointerAttribute ), true ).Length == 0)
            {
                throw new ArgumentException( "Marshalling a delegate to a native callback requires an UnmanagedFunctionPointerAttribute for the delegate type" );
            }
#endif
            UnpinnedDelegate = d;
            Handle = GCHandle.Alloc( UnpinnedDelegate );
            NativeFuncPtr = Marshal.GetFunctionPointerForDelegate<T>( UnpinnedDelegate );
        }

        /// <summary>Gets the raw native function pointer for the pinned delegate</summary>
        /// <returns>Native callable function pointer</returns>
        /// <exception cref="ObjectDisposedException">This instance was already disposed</exception>
        public nint ToABI()
        {
            ObjectDisposedException.ThrowIf(UnpinnedDelegate is null, this);
            return NativeFuncPtr;
        }

        /// <summary>Gets the native context for this callback (GCHandle for the delegate as <see cref="nint"/>)</summary>
        public nint Context => Handle.IsAllocated ? GCHandle.ToIntPtr( Handle ) : 0;

        /// <inheritdoc/>
        public void Dispose()
        {
            Handle.Free();
            UnpinnedDelegate = null;
        }

        /// <summary>Converts a callback to an IntPtr suitable for passing to native code</summary>
        /// <param name="cb">Callback to cast to an <see cref="IntPtr"/></param>
        /// <exception cref="ObjectDisposedException">This instance was already disposed</exception>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "It has one called ToABI()" )]
        public static implicit operator nint(DelegateHolder<T> cb) => cb?.ToABI() ?? nint.Zero;

        /// <summary>Gets the delegate this instance wraps</summary>
        /// <param name="cb">Callback to get the delegate for</param>
        /// <returns>The delegate this instance wraps</returns>
        /// <exception cref="ObjectDisposedException">This instance was already disposed</exception>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "ToDelegate serves the purpose without confusion on generic parameter name" )]
        public static implicit operator T?(DelegateHolder<T> cb) => cb?.UnpinnedDelegate ?? null;

        private readonly nint NativeFuncPtr;

        // keeps a live ref for the delegate around so GC won't clean it up
        private T? UnpinnedDelegate;

        private readonly GCHandle Handle;
    }
}
#endif
