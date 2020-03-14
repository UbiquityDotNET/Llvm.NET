// -----------------------------------------------------------------------
// <copyright file="PassManager.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.Llvm.Interop;

namespace Ubiquity.NET.Llvm.Transforms
{
    /// <summary>Common base class for pass managers</summary>
    public class PassManager
        : DisposableObject
    {
        internal PassManager( LLVMPassManagerRef handle )
        {
            Handle = handle;
        }

        internal LLVMPassManagerRef Handle { get; }

        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            Handle.Dispose( );
        }
    }
}
