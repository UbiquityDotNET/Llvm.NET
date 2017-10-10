// <copyright file="WrappedNativeCallback.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Native
{
    // This generates a holder for a delegate that allows a native
    // function pointer for the delegate to remain valid until the
    // instance of this wrapper is disposed.
    // NOTE:
    // This doesn't actually pin the delegate, but it does add
    // an additional reference
    // see: https://msdn.microsoft.com/en-us/library/367eeye0.aspx
    internal class WrappedNativeCallback
    {
        public WrappedNativeCallback( System.Delegate d )
        {
            UnpinnedDelegate = d;
            Handle = System.Runtime.InteropServices.GCHandle.Alloc( UnpinnedDelegate );
        }

        public void Dispose( )
        {
            Handle.Free( );
            UnpinnedDelegate = null;
        }

        public IntPtr GetFuncPointer( )
        {
            return System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate( UnpinnedDelegate );
        }

        private System.Delegate UnpinnedDelegate;
        private System.Runtime.InteropServices.GCHandle Handle;
    }
}
