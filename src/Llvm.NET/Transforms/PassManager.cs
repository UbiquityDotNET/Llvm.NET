// -----------------------------------------------------------------------
// <copyright file="PassManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Llvm.NET.Interop;

/* TODO: implement PassManagerBuilder support... */

namespace Llvm.NET.Transforms
{
    /// <summary>Common base class for pass managers</summary>
    public class PassManager
    {
        internal PassManager( LLVMPassManagerRef handle )
        {
            Handle = handle;
        }

        internal LLVMPassManagerRef Handle { get; }
    }
}
