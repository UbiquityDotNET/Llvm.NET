// -----------------------------------------------------------------------
// <copyright file="CustomStringMarshalerBase.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Llvm.NET.Interop
{
    /// <summary>Base class for custom string marshaling</summary>
    public class CustomStringMarshalerBase
        : ICustomMarshaler
    {
        /// <summary>Performs necessary cleanup of the managed data when it is no longer needed.</summary>
        /// <param name="ManagedObj">The managed object to be destroyed.</param>
        public void CleanUpManagedData( object ManagedObj )
        {
        }

        /// <summary>Performs necessary cleanup of the unmanaged data when it is no longer needed.</summary>
        /// <param name="pNativeData">A pointer to the unmanaged data to be destroyed.</param>
        public void CleanUpNativeData( IntPtr pNativeData )
            => NativeDisposer?.Invoke( pNativeData );

        /// <summary>A pointer to the unmanaged data to be destroyed.</summary>
        /// <returns>The size, in bytes, of the native data.</returns>
        [SuppressMessage( "Design", "CA1024:Use properties where appropriate.", Justification = "Name and signature defined by interface" )]
        public int GetNativeDataSize( ) => -1;

        /// <summary>Converts the managed data to unmanaged data.</summary>
        /// <param name="ManagedObj">The managed object to be converted.</param>
        /// <returns>A pointer to the native view of the managed object.</returns>
        public IntPtr MarshalManagedToNative( object ManagedObj )
            => throw new NotImplementedException( );

        /// <summary>Converts the unmanaged data to managed data.</summary>
        /// <param name="pNativeData">A pointer to the unmanaged data to be wrapped.</param>
        /// <returns>An object that represents the managed view of the COM data.</returns>
        public object MarshalNativeToManaged( IntPtr pNativeData )
            => StringNormalizer.NormalizeLineEndings( pNativeData );

        internal CustomStringMarshalerBase( Action<IntPtr>? disposer = null )
        {
            NativeDisposer = disposer;
        }

        private readonly Action<IntPtr>? NativeDisposer;
    }
}
