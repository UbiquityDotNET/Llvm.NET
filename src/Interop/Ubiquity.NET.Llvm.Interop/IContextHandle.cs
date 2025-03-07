// -----------------------------------------------------------------------
// <copyright file="IContextHandle.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Interface to provide support for <see cref="ContextHandleMarshaller{T}"/></summary>
    /// <typeparam name="THandle">Type of the handle reference</typeparam>
    /// <remarks>
    /// <para>LLVM "C" API makes heavy use of Opaque pointers and typedefs to them to hide implementation
    /// details. This is used to build the managed wrapper (essentially a typdef but stronger) this
    /// allows managed callers to build APIs that use or return a specific type of handle ONLY
    /// and they are NOT interchangeable.</para>
    /// <para>This interface is used to require types to implement the members. However, it is only used
    /// for marshalling (via <see cref="ContextHandleMarshaller{T}"/> and the concrete type <typeparamref name="THandle"/>
    /// is known at such a call site. Thus, this "marshalling" can occur without boxing to this interface.</para>
    /// </remarks>
    public interface IContextHandle<THandle>
        where THandle : struct, IContextHandle<THandle>
    {
        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <returns>The handle as an <see cref="nint"/></returns>
        nint DangerousGetHandle();

        /// <summary>Creates a type specific handle to hold the native opaque pointer</summary>
        /// <param name="abiValue">ABI value of the handle (xxxRef in LLVM terminology)</param>
        /// <returns>Type specific handle for managed code use</returns>
        /// <remarks>
        /// This is a static abstract "factory" method to allow <see cref="ContextHandleMarshaller{T}"/>
        /// to construct instances with non-default constructor.
        /// </remarks>
        static abstract THandle FromABI(nint abiValue);
    }
}
