// <copyright file="Cast.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Interop;
using Llvm.NET.Types;
using Llvm.NET.Values;

namespace Llvm.NET.Instructions
{
    /// <summary>Base class for cast instructions</summary>
    public class Cast
        : UnaryInstruction
    {
        /// <summary>Gets the source type of the cast</summary>
        public ITypeRef FromType => GetOperand<Value>(0).NativeType;

        /// <summary>Gets the destination type of the cast</summary>
        public ITypeRef ToType => NativeType;

        internal Cast( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
