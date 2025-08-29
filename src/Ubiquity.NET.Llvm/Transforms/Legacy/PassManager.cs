// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.TargetMachine;

namespace Ubiquity.NET.Llvm.Transforms.Legacy
{
    /// <summary>Common base class for pass managers</summary>
    [Obsolete( "Legacy pass manager support is considered obsolete - use one of the TryRunPasses overloads on Module or Function instead" )]
    public class PassManager
    {
        /// <summary>Add target specific analysis passes to this manager</summary>
        /// <param name="targetMachine">Target machine to add the passes for</param>
        public void AddAnalysisPasses( TargetMachine targetMachine )
        {
            ArgumentNullException.ThrowIfNull( targetMachine );

            LLVMAddAnalysisPasses( targetMachine.Handle, Handle );
        }

        internal PassManager( LLVMPassManagerRef handle )
        {
            Handle = handle;
        }

        internal LLVMPassManagerRef Handle { get; }
    }
}
