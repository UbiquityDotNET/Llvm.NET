// -----------------------------------------------------------------------
// <copyright file="LLVMModuleRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Global LLVM object handle</summary>
    public class LLVMModuleRef
        : LlvmObjectRef
    {
        /// <summary>Initializes an instance of <see cref="LLVMModuleRef"/> with default values</summary>
        public LLVMModuleRef()
            : base( ownsHandle: true )
        {
        }

        /// <summary>Initializes an instance of <see cref="LLVMModuleRef"/></summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        /// <param name="owner">Value to indicate whether the handle is owned or not</param>
        public LLVMModuleRef(nint handle, bool owner)
            : base( owner )
        {
            SetHandle( handle );
        }

        /// <summary>Gets a Zero (<see langword="null"/>) value handle</summary>
        public static LLVMModuleRef Zero { get; } = new LLVMModuleRef( nint.Zero, false );

        /// <inheritdoc/>
        protected override bool ReleaseHandle()
        {
            // critical safety check, base should never call ReleaseHandle on an invalid handle
            // but ABI usually can't handle that and would just crash the app, so make it
            // a NOP just in case.
            if(handle != nint.Zero)
            {
                LLVMDisposeModule( handle );
            }

            return true;

            [DllImport( NativeMethods.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeModule(nint p);
        }
    }

    // CONSIDER: Make the alias a `ref struct` to prevent boxing or heap allocation.
    //           This would model the intended semantics in code and enforce the
    //           lifetime rules better. (Not perfectly as a ref struct could still
    //           escape the lifetime of the owner, but compile time catching most
    //           cases is still WAY better than random runtime bugs.

    /// <summary>Alias (non-owning) handle for a <see cref="LLVMModuleRef"/></summary>
    /// <remarks>
    /// Sometimes a global object is exposed via a child that maintains a reference to the parent.
    /// In such cases, the handle isn't owned by the App (it's an alias) and therefore should not be
    /// disposed or destroyed. This handle type takes care of that in a type safe manner and does not
    /// perform any automatic cleanup. [That is, this is a PURE reference to an object]
    /// </remarks>
    public class LLVMModuleRefAlias
        : LLVMModuleRef
    {
        public LLVMModuleRefAlias()
            : base( nint.Zero, false )
        {
        }
    }
}
