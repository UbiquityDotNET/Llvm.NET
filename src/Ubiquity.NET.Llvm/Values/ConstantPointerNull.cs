// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
