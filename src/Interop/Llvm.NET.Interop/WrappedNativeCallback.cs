// <copyright file="WrappedNativeCallback.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET.Interop.Properties;
using Ubiquity.ArgValidators;

namespace Llvm.NET.Interop
{
    /// <summary>Keep alive holder to ensure native call back delegates are not destroyed while registered with native code</summary>
    /// <remarks>
    /// This generates a holder for a delegate that allows a native function pointer for the delegate to remain valid until the
    /// instance of this wrapper is disposed.
    ///
    /// <note type="note">
    /// This doesn't actually pin the delegate, but it does add
    /// an additional reference
    /// see: https://msdn.microsoft.com/en-us/library/367eeye0.aspx for more info.
    /// </note>
    /// </remarks>
    public sealed class WrappedNativeCallback
        : IDisposable
    {
        /// <summary>Initializes a new instance of the <see cref="WrappedNativeCallback"/> class.</summary>
        /// <param name="d">Delegate to keep alive until this instance is disposed</param>
        public WrappedNativeCallback( Delegate d )
        {
            d.ValidateNotNull( nameof( d ) );
            if( d.GetType().IsGenericType )
            {
                // Marshal.GetFunctionPointerForDelegate will create an exception for this but the
                // error message is, pardon the pun, a bit too generic. Hopefully, this makes it a
                // bit more clear.
                throw new ArgumentException( Resources.Marshaling_of_Generic_delegate_types_to_a_native_callback_is_not_supported );
            }

            if( d.GetType( ).GetCustomAttributes( typeof( UnmanagedFunctionPointerAttribute ), true ).Length == 0 )
            {
                throw new ArgumentException( Resources.Marshalling_a_delegate_to_a_native_callback_requires_an_UnmanagedFunctionPointerAttribute_for_the_delegate_type );
            }

            UnpinnedDelegate = d;
            Handle = GCHandle.Alloc( UnpinnedDelegate );
            NativeFuncPtr = Marshal.GetFunctionPointerForDelegate( UnpinnedDelegate );
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            Handle.Free( );
            UnpinnedDelegate = null;
        }

        /// <summary>Gets the raw native function pointer for the pinned delegate</summary>
        public IntPtr NativeFuncPtr { get; }

        // keeps a live ref for the delegate around so GC won't clean it up
        private Delegate UnpinnedDelegate;

        private readonly GCHandle Handle;
    }
}
