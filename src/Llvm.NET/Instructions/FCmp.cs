// <copyright file="FCmp.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class FCmp
        : Cmp
    {
        internal FCmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
