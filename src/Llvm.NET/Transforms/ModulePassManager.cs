// <copyright file="ModulePassManager.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    internal class ModulePassManager
        : PassManager
    {
        public ModulePassManager( )
            : base( LLVMCreatePassManager( ) )
        {
        }

        public bool Run( BitcodeModule target )
        {
            return LLVMRunPassManager( Handle, target.ModuleHandle );
        }
    }
}
