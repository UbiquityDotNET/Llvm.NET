// <copyright file="cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

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
