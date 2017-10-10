// <copyright file="IntCmp.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class IntCmp
        : Cmp
    {
        internal IntCmp( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
