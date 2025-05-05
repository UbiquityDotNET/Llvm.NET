// -----------------------------------------------------------------------
// <copyright file="PassManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Transforms.Legacy
{
    /// <summary>Common base class for pass managers</summary>
    [Obsolete("Legacy pass manager support is considered obsolete - use one of the TryRunPasses overloads on Module or Function instead")]
    public class PassManager
    {
        /// <summary>Add target specific analysis passes to this manager</summary>
        /// <param name="targetMachine">Target machine to add the passes for</param>
        public void AddAnalysisPasses( TargetMachine targetMachine)
        {
            ArgumentNullException.ThrowIfNull(targetMachine);

            LLVMAddAnalysisPasses(targetMachine.Handle, Handle);
        }

        internal PassManager( LLVMPassManagerRef handle )
        {
            Handle = handle;
        }

        internal LLVMPassManagerRef Handle { get; }
    }
}
