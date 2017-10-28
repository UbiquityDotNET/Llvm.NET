// <copyright file="GlobalAlias.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET.Values
{
    /// <summary>LLVM Global Alias for a function or global value</summary>
    public class GlobalAlias
        : GlobalValue
    {
        public Constant Aliasee
        {
            get
            {
                if( ValueHandle == default )
                {
                    return null;
                }

                var handle = NativeMethods.LLVMGetAliasee( ValueHandle );
                if( handle == default )
                {
                    return null;
                }

                return FromHandle<Constant>( handle );
            }
        }

        internal GlobalAlias( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
