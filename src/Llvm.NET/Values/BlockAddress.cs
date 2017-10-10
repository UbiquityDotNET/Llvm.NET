// <copyright file="BlockAddress.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    public class BlockAddress : Constant
    {
        internal BlockAddress( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
