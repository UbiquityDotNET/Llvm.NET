// -----------------------------------------------------------------------
// <copyright file="LLVMTargetDataRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Global LLVM object handle</summary>
    public class LLVMTargetDataRef
        : LlvmObjectRef
    {
        /// <summary>Initializes an instance of <see cref="LLVMTargetDataRef"/> with default values</summary>
        public LLVMTargetDataRef()
            : base( ownsHandle: true )
        {
        }

        /// <summary>Initializes an instance of <see cref="LLVMTargetDataRef"/></summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        /// <param name="owner">Value to indicate whether the handle is owned or not</param>
        public LLVMTargetDataRef(nint handle, bool owner)
            : base( owner )
        {
            SetHandle( handle );
        }

        /// <summary>Gets a Zero (<see langword="null"/>) value handle</summary>
        public static LLVMTargetDataRef Zero { get; } = new LLVMTargetDataRef( nint.Zero, false );

        /// <inheritdoc/>
        protected override bool ReleaseHandle()
        {
            // critical safety check, base should never call ReleaseHandle on an invalid handle
            // but ABI usually can't handle that and would just crash the app, so make it
            // a NOP just in case.
            if(handle != nint.Zero)
            {
                LLVMDisposeTargetData( handle );
            }

            return true;

            [DllImport( NativeMethods.LibraryPath )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeTargetData(nint p);
        }
    }

    // CONSIDER: Make the alias a `ref struct` to prevent boxing or heap allocation.
    //           This would model the intended semantics in code and enforce the
    //           lifetime rules better. (Not perfectly as a ref struct could still
    //           escape the lifetime of the owner, but compile time catching most
    //           cases is still WAY better than random runtime bugs.

    /// <summary>Alias (non-owning) handle for a <see cref="LLVMTargetDataRef"/></summary>
    ///<remarks>
    /// Sometimes a global object is exposed via a child that maintains a reference to the parent.
    /// In such cases, the handle isn't owned by the App (it's an alias) and therefore should not be
    /// disposed or destroyed. This handle type takes care of that in a type safe manner and does not
    /// perform any automatic cleanup. [That is, this is a PURE reference to an object]
    ///</remarks>
    [GeneratedCode( "LlvmBindingsGenerator", "20.1.0-alpha.0.0.ci-ZZZ.601755488+2c442300e0dbcc1976dfb1243d8f4824d380c8d2" )]
    public class LLVMTargetDataRefAlias
        : LLVMTargetDataRef
    {
        public LLVMTargetDataRefAlias()
            : base( nint.Zero, false )
        {
        }
    }
}
