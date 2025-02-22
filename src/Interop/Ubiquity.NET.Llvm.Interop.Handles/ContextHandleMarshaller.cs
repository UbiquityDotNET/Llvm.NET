// -----------------------------------------------------------------------
// <copyright file="ContextHandleMarshaller.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    [CustomMarshaller( typeof( CustomMarshallerAttribute.GenericPlaceholder ), MarshalMode.Default, typeof( ContextHandleMarshaller<> ) )]
    public static class ContextHandleMarshaller<T>
        where T : struct, IContextHandle<T>
    {
        /// <summary>Converts an LLVM string to a managed code string</summary>
        /// <param name="abiValue">native handle value</param>
        /// <returns>Managed code string representation of the native string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Design", "CA1000:Do not declare static members on generic types", Justification = "Needed for marshalling" )]
        public static T ConvertToManaged(nint abiValue)
        {
            return T.FromABI( abiValue );
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Design", "CA1000:Do not declare static members on generic types", Justification = "Needed for marshalling" )]
        public static nint ConvertToUnmanaged(T managed)
        {
            return managed.DangerousGetHandle();
        }
    }
}
