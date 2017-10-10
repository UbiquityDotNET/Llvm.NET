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
        /// <summary>Visibility of this global value</summary>
        public Visibility Visibility
        {
            get => ( Visibility )NativeMethods.GetVisibility( ValueHandle );
            set => NativeMethods.SetVisibility( ValueHandle, ( LLVMVisibility )value );
        }

        /// <summary>Linkage specification for this symbol</summary>
        public Linkage Linkage
        {
            get => ( Linkage )NativeMethods.GetLinkage( ValueHandle );
            set => NativeMethods.SetLinkage( ValueHandle, ( LLVMLinkage )value );
        }

        /// <summary>Flag to indicate if this is an Unnamed address</summary>
        public bool UnnamedAddress
        {
            get => NativeMethods.HasUnnamedAddr( ValueHandle );
            set => NativeMethods.SetUnnamedAddr( ValueHandle, value );
        }

        /// <summary>Flag to indicate if this is a declaration</summary>
        public bool IsDeclaration => NativeMethods.IsDeclaration( ValueHandle );

        /// <summary>Module containing this global value</summary>
        public NativeModule ParentModule => NativeType.Context.GetModuleFor( NativeMethods.GetGlobalParent( ValueHandle ) );

        internal GlobalValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
