// <copyright file="AddressSpaceCast.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;

namespace Llvm.NET.Instructions
{
    /// <summary>Address space cast instruction</summary>
    /// <seealso href="xref:llvm_langref#addrspaceast-to-instruction">LLVM addrspacecast .. to</seealso>
    public class AddressSpaceCast : Cast
    {
        internal AddressSpaceCast( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
