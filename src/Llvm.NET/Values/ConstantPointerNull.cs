// <copyright file="ConstPointerNull.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;
using Llvm.NET.Types;

namespace Llvm.NET.Values
{
    public class ConstantPointerNull
        : Constant
    {
        /// <summary>Creates a constant null pointer to a given type</summary>
        /// <param name="type">Type of the pointer</param>
        /// <returns>Constant null value of the specified type</returns>
        public static ConstantPointerNull From( ITypeRef type )
        {
            return FromHandle<ConstantPointerNull>( NativeMethods.LLVMConstPointerNull( type.GetTypeRef() ) );
        }

        internal ConstantPointerNull( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
