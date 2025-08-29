// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Transforms.Legacy
{
    /// <summary>Pass manager for running passes against an entire module</summary>
    [Obsolete( "Legacy pass manager support is considered obsolete - use one of the TryRunPasses overloads on Module instead" )]
    public sealed class ModulePassManager
        : PassManager
    {
        /// <summary>Initializes a new instance of the <see cref="ModulePassManager"/> class.</summary>
        public ModulePassManager( )
            : base( LLVMCreatePassManager() )
        {
        }

        /// <summary>Runs the passes added to this manager for the target module</summary>
        /// <param name="target">Module to run the passes on</param>
        /// <returns><see langword="true"/> if one of the passes modified the module</returns>
        public bool Run( Module target )
        {
            ArgumentNullException.ThrowIfNull( target );
            return LLVMRunPassManager( Handle, target.GetUnownedHandle() );
        }
    }
}
