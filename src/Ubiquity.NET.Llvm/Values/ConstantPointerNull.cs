// -----------------------------------------------------------------------
// <copyright file="ConstantPointerNull.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Represents a constant Null pointer</summary>
    public sealed class ConstantPointerNull
        : ConstantData
    {
        /// <summary>Creates a constant null pointer to a given type</summary>
        /// <param name="type">Type of the pointer</param>
        /// <returns>Constant null value of the specified type</returns>
        public static ConstantPointerNull From( ITypeRef type )
        {
            return FromHandle<ConstantPointerNull>( LLVMConstPointerNull( type.GetTypeRef() ).ThrowIfInvalid() )!;
        }

        internal ConstantPointerNull( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
