// <copyright file="cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

/* TODO: implement PassManagerBuilder support... */

namespace Llvm.NET.Transforms
{
    /// <summary>Common base class for pass managers</summary>
    public partial class PassManager
    {
        internal PassManager( LLVMPassManagerRef handle )
        {
            Handle = handle;
        }

        internal LLVMPassManagerRef Handle { get; }
    }
}
