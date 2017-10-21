// <copyright file="GlobalValue.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Global value </summary>
    public class GlobalValue
        : Constant
    {
        /// <summary>Gets or sets the visibility of this global value</summary>
        public Visibility Visibility
        {
            get => ( Visibility )NativeMethods.LLVMGetVisibility( ValueHandle );
            set => NativeMethods.LLVMSetVisibility( ValueHandle, ( LLVMVisibility )value );
        }

        /// <summary>Gets or sets the linkage specification for this symbol</summary>
        public Linkage Linkage
        {
            get => ( Linkage )NativeMethods.LLVMGetLinkage( ValueHandle );
            set => NativeMethods.LLVMSetLinkage( ValueHandle, ( LLVMLinkage )value );
        }

        /// <summary>Gets or sets a value indicating whether this is an Unnamed address</summary>
        public bool UnnamedAddress
        {
            get => NativeMethods.LLVMHasUnnamedAddr( ValueHandle );
            set => NativeMethods.LLVMSetUnnamedAddr( ValueHandle, value );
        }

        /// <summary>Gets a value indicating whether this is a declaration</summary>
        public bool IsDeclaration => NativeMethods.LLVMIsDeclaration( ValueHandle );

        /// <summary>Gets the Module containing this global value</summary>
        public BitcodeModule ParentModule => NativeType.Context.GetModuleFor( NativeMethods.LLVMGetGlobalParent( ValueHandle ) );

        internal GlobalValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
