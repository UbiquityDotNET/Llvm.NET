// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Interface to provide support for <see cref="WrappedHandleMarshaller{T}"/></summary>
    /// <typeparam name="T">Type of the handle reference</typeparam>
    /// <remarks>
    /// <para>LLVM "C" API makes heavy use of Opaque pointers and typedefs to them to hide implementation
    /// details. This is used to build the managed wrapper (essentially a typdef but stronger) this
    /// allows managed callers to build APIs that use or return a specific type of handle ONLY
    /// and they are NOT interchangeable.</para>
    /// <para>This interface is used to require types to implement the members. However, it is only used
    /// for marshalling (via <see cref="WrappedHandleMarshaller{T}"/> and the concrete type <typeparamref name="T"/>
    /// is known at such a call site. Thus, this "marshalling" can occur without boxing to this interface.</para>
    /// </remarks>
    public interface IWrappedHandle<T>
        where T : struct, IWrappedHandle<T>/*, allows ref struct*/
    {
        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <returns>The handle as an <see cref="nint"/></returns>
        nint DangerousGetHandle( );

        /// <summary>Creates a type specific handle to hold the native opaque pointer</summary>
        /// <param name="abiValue">ABI value of the handle (xxxRef in LLVM terminology)</param>
        /// <returns>Type specific handle for managed code use</returns>
        /// <remarks>
        /// This is a static abstract "factory" method to allow <see cref="WrappedHandleMarshaller{T}"/>
        /// to construct instances with non-default constructor.
        /// </remarks>
        static abstract T FromABI( nint abiValue );
    }
}
