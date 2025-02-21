// -----------------------------------------------------------------------
// <copyright file="LLVMOrcObjectTransformLayerRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Simple type safe handle to wrap an opaque pointer for interop with "C" API exported from LibLLVM</summary>
    /// <remarks>
    ///    This handle is owned by it's container and therefore isn't disposed by the
    ///    calling App.
    /// <note type="important">
    ///     Since the object this handle refers to is not owned by the App, the object is
    ///     destroyed whenever it's container is destroyed, which will invalidate this handle.
    ///     Use of this handle after the container is destroyed will produce undefined
    ///     behavior, including, and most likely, memory access violations.
    /// </note>
    /// </remarks>
    [NativeMarshalling( typeof( ContextHandleMarshaller<LLVMOrcObjectTransformLayerRef> ) )]
    public readonly record struct LLVMOrcObjectTransformLayerRef
        : IContextHandle<LLVMOrcObjectTransformLayerRef>
    {
        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <returns>The handle as an <see cref="nint"/></returns>
        public nint DangerousGetHandle() => Handle;

        /// <summary>Interface defined factory for an instance of <see cref="LLVMOrcObjectTransformLayerRef"/></summary>
        /// <param name="abiValue">Native ABI value of the handle</param>
        /// <returns>Type specific wrapper around the native ABI handle</returns>
        public static LLVMOrcObjectTransformLayerRef FromABI(nint abiValue) => new( abiValue );

        private LLVMOrcObjectTransformLayerRef(nint p)
        {
            Handle = p;
        }

        private readonly nint Handle;
    }
}
