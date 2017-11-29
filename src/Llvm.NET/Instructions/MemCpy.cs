// <copyright file="MemCpy.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Diagnostics;
using Llvm.NET.Native;
using Llvm.NET.Types;

namespace Llvm.NET.Instructions
{
    /// <summary>Instruction for the LLVM intrinsic llvm.memcpy instruction</summary>
    public class MemCpy
        : MemIntrinsic
    {
        internal MemCpy( LLVMValueRef handle )
            : base( handle )
        {
        }

        internal static string GetIntrinsicNameForArgs( IPointerType dst, IPointerType src, ITypeRef len )
        {
            Debug.Assert( dst != null && dst.ElementType.IsInteger && dst.ElementType.IntegerBitWidth > 0, "Invalid dst" );
            Debug.Assert( src != null && src.ElementType.IsInteger && src.ElementType.IntegerBitWidth > 0, "Invalid src" );
            Debug.Assert( len.IsInteger && len.IntegerBitWidth > 0, "Invalid len" );
            return $"llvm.memcpy.p{dst.AddressSpace}i{dst.ElementType.IntegerBitWidth}.p{src.AddressSpace}i{src.ElementType.IntegerBitWidth}.i{len.IntegerBitWidth}";
        }
    }
}
