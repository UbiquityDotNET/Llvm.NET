// <copyright file="MemSet.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Diagnostics;
using Llvm.NET.Native;
using Llvm.NET.Types;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction for the LLVM intrinsic memset function</summary>
    public class MemSet
        : MemIntrinsic
    {
        internal MemSet( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }

        internal static string GetIntrinsicNameForArgs( IPointerType dst, ITypeRef src, ITypeRef len )
        {
            Debug.Assert( dst != null && dst.ElementType.IsInteger && dst.ElementType.IntegerBitWidth > 0, "Invalid dst" );
            Debug.Assert( len.IsInteger && len.IntegerBitWidth > 0, "Invalid len" );
            return $"llvm.memset.p{dst.AddressSpace}i{src.IntegerBitWidth}.i{len.IntegerBitWidth}";
        }
    }
}
