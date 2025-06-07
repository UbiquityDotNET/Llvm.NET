// -----------------------------------------------------------------------
// <copyright file="GlobalIFunc.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

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
            get => FromHandle<Function>( LLVMGetGlobalIFuncResolver( Handle ).ThrowIfInvalid() )!;
            set => LLVMSetGlobalIFuncResolver( Handle, value.ThrowIfNull().Handle );
        }

        /// <summary>Removes this instance from the parent module without destroying it</summary>
        public void RemoveFromParent( ) => LLVMRemoveGlobalIFunc( Handle );

        internal GlobalIFunc( LLVMValueRef handle )
            : base( handle )
        {
        }
    }
}
