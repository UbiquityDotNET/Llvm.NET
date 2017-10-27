// <copyright file="FunctionPassManager.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Values;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET.Transforms
{
    public sealed class FunctionPassManager
        : PassManager
    {
        public FunctionPassManager( BitcodeModule module )
            : base( LLVMCreateFunctionPassManagerForModule( module.ModuleHandle ))
        {
        }

        public bool Initialize( )
        {
            return LLVMInitializeFunctionPassManager( Handle );
        }

        public bool Run( Function target )
        {
            return LLVMRunFunctionPassManager( Handle, target.ValueHandle );
        }

        public bool Finish( )
        {
            return LLVMFinalizeFunctionPassManager( Handle );
        }
    }
}
