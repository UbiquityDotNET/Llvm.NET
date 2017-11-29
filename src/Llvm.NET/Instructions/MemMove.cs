// <copyright file="MemMove.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System.Diagnostics;
using Llvm.NET.Native;
using Llvm.NET.Types;

namespace Llvm.NET.Instructions
{
    /// <summary>Intrinsic call to target optimized memmove</summary>
    public class MemMove
        : MemIntrinsic
    {
        internal MemMove( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }

        internal static string GetIntrinsicNameForArgs( IPointerType dst, IPointerType src, ITypeRef len )
        {
            Debug.Assert( dst != null && dst.ElementType.IsInteger && dst.ElementType.IntegerBitWidth > 0, "Invalid dst" );
            Debug.Assert( src != null && src.ElementType.IsInteger && src.ElementType.IntegerBitWidth > 0, "Invalid src" );
            Debug.Assert( len.IsInteger && len.IntegerBitWidth > 0, "Invalid len" );
            return $"llvm.memmove.p{dst.AddressSpace}i{dst.ElementType.IntegerBitWidth}.p{src.AddressSpace}i{src.ElementType.IntegerBitWidth}.i{len.IntegerBitWidth}";
        }
    }
}
