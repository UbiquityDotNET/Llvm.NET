// <copyright file="Store.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Instructions
{
    public class Store
        : Instruction
    {
        public bool IsVolatile
        {
            get { return LLVMGetVolatile( ValueHandle ); }
            set { LLVMSetVolatile( ValueHandle, value ); }
        }

        internal Store( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
