// <copyright file="cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET.Transforms
{
    public class PassManager
        : IDisposable
    {
        ~PassManager( )
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose( false );
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        internal PassManager( LLVMPassManagerRef handle )
        {
            Handle = handle;
        }

        internal LLVMPassManagerRef Handle { get; }

        protected virtual void Dispose( bool disposing )
        {
            if( !Handle.IsClosed )
            {
                if( disposing )
                {
                    // TODO: dispose managed state (managed objects).
                }

                Handle.Dispose( );
            }
        }
    }
}
