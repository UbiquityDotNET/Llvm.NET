// -----------------------------------------------------------------------
// <copyright file="FunctionPassManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Llvm.NET.Interop.NativeMethods;

using IrFunction = Llvm.NET.Values.IrFunction;

namespace Llvm.NET.Transforms
{
    /// <summary>LLVM pass manager for functions</summary>
    public sealed class FunctionPassManager
        : PassManager
    {
        /// <summary>Initializes a new instance of the <see cref="FunctionPassManager"/> class.</summary>
        /// <param name="module">Module that owns the functions this manager works on</param>
        public FunctionPassManager( BitcodeModule module )
            : base( LLVMCreateFunctionPassManagerForModule( module.ModuleHandle ))
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
        public bool Run( IrFunction target )
        {
            return LLVMRunFunctionPassManager( Handle, target.ValueHandle );
        }

        /// <summary>Finalizes all of the function passes scheduled in the function pass manager.</summary>
        /// <returns><see langword="true"/>if any of the passes modified the module</returns>
        public bool Finish( )
        {
            return LLVMFinalizeFunctionPassManager( Handle );
        }
    }
}
