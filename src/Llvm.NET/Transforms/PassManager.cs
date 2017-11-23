// <copyright file="cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET.Transforms
{
    /// <summary>Common base class for pass managers</summary>
    public class PassManager
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose( )
        {
            Dispose( true );
        }

        internal PassManager( LLVMPassManagerRef handle )
        {
            Handle = handle;
        }

        internal LLVMPassManagerRef Handle { get; }

        protected virtual void Dispose( bool disposing )
        {
            if( disposing )
            {
                if( !Handle.IsClosed )
                {
                    Handle.Close( );
                }
            }
        }
    }
}
