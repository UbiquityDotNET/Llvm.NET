// -----------------------------------------------------------------------
// <copyright file="LLVMMetadataRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom.Compiler;
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
    [GeneratedCode( "LlvmBindingsGenerator", "20.1.0-alpha.0.0.ci-ZZZ.601755488+2c442300e0dbcc1976dfb1243d8f4824d380c8d2" )]
    [NativeMarshalling( typeof( ContextHandleMarshaller<LLVMMetadataRef> ) )]
    public readonly record struct LLVMMetadataRef
        : IContextHandle<LLVMMetadataRef>
    {
        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <returns>The handle as an <see cref="nint"/></returns>
        public nint DangerousGetHandle() => Handle;

        /// <summary>Interface defined factory for an instance of <see cref="LLVMMetadataRef"/></summary>
        /// <param name="abiValue">Native ABI value of the handle</param>
        /// <returns>Type specific wrapper around the native ABI handle</returns>
        public static LLVMMetadataRef FromABI(nint abiValue) => new( abiValue );

        private LLVMMetadataRef(nint p)
        {
            Handle = p;
        }

        private readonly nint Handle;
    }
}
