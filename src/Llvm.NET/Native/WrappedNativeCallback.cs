// <copyright file="WrappedNativeCallback.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

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
        public WrappedNativeCallback( Delegate d )
        {
            UnpinnedDelegate = d;
            Handle = GCHandle.Alloc( UnpinnedDelegate );
        }

        public void Dispose( )
        {
            Handle.Free( );
            UnpinnedDelegate = null;
        }

        public IntPtr GetFuncPointer( )
        {
            return Marshal.GetFunctionPointerForDelegate( UnpinnedDelegate );
        }

        private Delegate UnpinnedDelegate;
        private GCHandle Handle;
    }
}
