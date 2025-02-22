// -----------------------------------------------------------------------
// <copyright file="LLVMOrcObjectLayerRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop.Handles
{
    /// <summary>Global LLVM object handle</summary>
    public class LLVMOrcObjectLayerRef
        : GlobalHandleBase
    {
        /// <summary>Initializes an instance of <see cref="LLVMOrcObjectLayerRef"/> with default values</summary>
        public LLVMOrcObjectLayerRef()
            : base( ownsHandle: true )
        {
        }

        /// <summary>Initializes an instance of <see cref="LLVMOrcObjectLayerRef"/></summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        /// <param name="owner">Value to indicate whether the handle is owned or not</param>
        public LLVMOrcObjectLayerRef(nint handle, bool owner)
            : base( owner )
        {
            SetHandle( handle );
        }

        /// <summary>Gets a Zero (<see langword="null"/>) value handle</summary>
        public static LLVMOrcObjectLayerRef Zero { get; } = new LLVMOrcObjectLayerRef( nint.Zero, false );

        /// <inheritdoc/>
        protected override bool ReleaseHandle()
        {
            // critical safety check, base should never call ReleaseHandle on an invalid handle
            // but ABI usually can't handle that and would just crash the app, so make it
            // a NOP just in case.
            if(handle != nint.Zero)
            {
                LLVMOrcDisposeObjectLayer( handle );
            }

            return true;

            [DllImport( Constants.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMOrcDisposeObjectLayer(nint p);
        }
    }
}
