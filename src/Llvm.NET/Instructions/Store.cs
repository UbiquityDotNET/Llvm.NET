// <copyright file="Store.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class Store
        : Instruction
    {
        public bool IsVolatile
        {
            get { return NativeMethods.GetVolatile( ValueHandle ); }
            set { NativeMethods.SetVolatile( ValueHandle, value ); }
        }

        internal Store( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
