// <copyright file="ModulePassManager.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Llvm.NET.Transforms
{
    /// <summary>Pass manager for running passes against an entire module</summary>
    public sealed class ModulePassManager
        : PassManager
    {
        /// <summary>Initializes a new instance of the <see cref="ModulePassManager"/> class.</summary>
        public ModulePassManager( )
            : base( NativeMethods.LLVMCreatePassManager( ) )
        {
        }

        /// <summary>Runs the passes added to this manager for the target module</summary>
        /// <param name="target">Module to run the passes on</param>
        /// <returns><see langword="true"/> if one of the passes modified the module</returns>
        public bool Run( BitcodeModule target )
        {
            return NativeMethods.LLVMRunPassManager( Handle, target.ModuleHandle );
        }
    }
}
