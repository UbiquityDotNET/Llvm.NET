// -----------------------------------------------------------------------
// <copyright file="FunctionPassManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.Transforms.Legacy
{
    /// <summary>LLVM pass manager for functions</summary>
    [Obsolete("Legacy pass manager support is considered obsolete - use one of the TryRunPasses overloads on Function instead")]
    public sealed class FunctionPassManager
        : PassManager
    {
        /// <summary>Initializes a new instance of the <see cref="FunctionPassManager"/> class.</summary>
        /// <param name="module">Module that owns the functions this manager works on</param>
        public FunctionPassManager( Module module )
            : base( LLVMCreateFunctionPassManagerForModule( module.ThrowIfNull().GetUnownedHandle() ) )
        {
        }

        /// <summary>Initializes the passes registered in the pass manager</summary>
        /// <returns><see langword="true"/>if any of the passes modified the module</returns>
        public bool Initialize( )
        {
            return LLVMInitializeFunctionPassManager( Handle );
        }

        /// <summary>Runs the passes registered in the pass manager</summary>
        /// <param name="target">Function to run the passes on</param>
        /// <returns><see langword="true"/>if any of the passes modified the module</returns>
        public bool Run( Function target )
        {
            return LLVMRunFunctionPassManager( Handle, target.ThrowIfNull().Handle );
        }

        /// <summary>Finalizes all of the function passes scheduled in the function pass manager.</summary>
        /// <returns><see langword="true"/>if any of the passes modified the module</returns>
        public bool Finish( )
        {
            return LLVMFinalizeFunctionPassManager( Handle );
        }
    }
}
