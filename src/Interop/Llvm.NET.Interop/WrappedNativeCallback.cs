// <copyright file="WrappedNativeCallback.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Llvm.NET.Interop.Properties;
using Ubiquity.ArgValidators;

namespace Llvm.NET.Interop
{
    /// <summary>Keep alive holder to ensure native call back delegates are not destroyed while registered with native code</summary>
    /// <typeparam name="T">Delegate signature of the native callback</typeparam>
    /// <remarks>
    /// This generates a holder for a delegate that allows a native function pointer for the delegate to remain valid until the
    /// instance of this wrapper is disposed. This is generally only necessary where the native call back must remain valid for
    /// an extended period of time. (e.g. beyond the call that provides the callback)
    ///
    /// <note type="note">
    /// This doesn't actually pin the delegate, but it does add
    /// an additional reference
    /// see: https://msdn.microsoft.com/en-us/library/367eeye0.aspx for more info.
    /// </note>
    /// </remarks>
    public abstract class WrappedNativeCallback
        : DisposableObject
    {
        /// <summary>Gets the raw native function pointer for the pinned delegate</summary>
        /// <returns>Native callable function pointer</returns>
        public IntPtr ToIntPtr( ) => NativeFuncPtr;

        public static implicit operator IntPtr( WrappedNativeCallback cb ) => cb.ToIntPtr( );

        /// <summary>Initializes a new instance of the <see cref="WrappedNativeCallback"/> class.</summary>
        /// <param name="d">Delegate</param>
        protected internal WrappedNativeCallback( Delegate d )
        {
            d.ValidateNotNull( nameof( d ) );
            if( d.GetType( ).IsGenericType )
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
        protected override void Dispose( bool disposing )
        {
            Handle.Free( );
            UnpinnedDelegate = null;
        }

        /// <summary>Gets a delegate from the raw native callback</summary>
        /// <typeparam name="T">Type of delegate to convert to</typeparam>
        /// <returns>Delegate suitable for passing as an "in" parameter to native methods</returns>
        protected T ToDelegate<T>( )
            where T : Delegate
        {
            return (T)Marshal.GetDelegateForFunctionPointer( ToIntPtr( ), typeof( T ) );
        }

        private readonly IntPtr NativeFuncPtr;

        // keeps a live ref for the delegate around so GC won't clean it up
        private Delegate UnpinnedDelegate;

        private readonly GCHandle Handle;
    }

    /// <summary>Keep alive holder to ensure native call back delegates are not destroyed while registered with native code</summary>
    /// <typeparam name="T">Delegate signature of the native callback</typeparam>
    /// <remarks>
    /// This generates a holder for a delegate that allows a native function pointer for the delegate to remain valid until the
    /// instance of this wrapper is disposed. This is generally only necessary where the native call back must remain valid for
    /// an extended period of time. (e.g. beyond the call that provides the callback)
    ///
    /// <note type="note">
    /// This doesn't actually pin the delegate, but it does add
    /// an additional reference
    /// see: https://msdn.microsoft.com/en-us/library/367eeye0.aspx for more info.
    /// </note>
    /// </remarks>
    [SuppressMessage( "StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Simple generic variant" )]
    public sealed class WrappedNativeCallback<T>
        : WrappedNativeCallback
        where T : Delegate
    {
        /// <summary>Initializes a new instance of the <see cref="WrappedNativeCallback{T}"/> class.</summary>
        /// <param name="d">Delegate to keep alive until this instance is disposed</param>
        public WrappedNativeCallback( T d )
            : base(d)
        {
        }

        /// <summary>Gets a delegate from the raw native callback</summary>
        /// <returns>Delegate suitable for passing as an "in" parameter to native methods</returns>
        public T ToDelegate( ) => ToDelegate<T>();

        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "ToDelegate serves the purpose without confusion on generic parameter name" )]
        public static implicit operator T(WrappedNativeCallback<T> cb) => cb.ToDelegate();
    }
}
