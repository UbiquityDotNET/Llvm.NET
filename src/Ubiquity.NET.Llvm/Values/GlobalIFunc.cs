// -----------------------------------------------------------------------
// <copyright file="GlobalIFunc.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Global Indirect Function</summary>
    /// <remarks>
    /// represents a single indirect function in the IR. Indirect function uses
    /// ELF symbol type extension to mark that the address of a declaration should
    /// be resolved at runtime by calling a resolver function.
    /// </remarks>
    public class GlobalIFunc
        : GlobalIndirectSymbol
    {
        /// <summary>Gets or sets the ifunc resolver</summary>
        public Constant Resolver
        {
            get => FromHandle<Function>( LLVMGetGlobalIFuncResolver( ValueHandle ).ThrowIfInvalid( ) )!;
            set => LLVMSetGlobalIFuncResolver( ValueHandle, value.ThrowIfNull().ValueHandle );
        }

        /// <summary>Removes this instance from the parent module without destroying it</summary>
        public void RemoveFromParent( ) => LLVMRemoveGlobalIFunc( ValueHandle );

        internal GlobalIFunc( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
