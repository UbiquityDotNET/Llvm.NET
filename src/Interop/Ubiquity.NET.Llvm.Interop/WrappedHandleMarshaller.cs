// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Template to provide <see cref="LibraryImportAttribute"/> compatible marshalling for an LLVM context handle</summary>
    /// <typeparam name="T">LLVM Context handle type to provide marshalling for</typeparam>
    [CustomMarshaller( typeof( CustomMarshallerAttribute.GenericPlaceholder ), MarshalMode.Default, typeof( WrappedHandleMarshaller<> ) )]
    public static class WrappedHandleMarshaller<T>
        where T : struct, IWrappedHandle<T>/*, allows ref struct*/
    {
        /// <summary>Converts an LLVM native handle to a type specific managed code wrapper</summary>
        /// <param name="abiValue">native handle value</param>
        /// <returns>Managed code representation of the native opaque handle</returns>
        [SuppressMessage( "Design", "CA1000:Do not declare static members on generic types", Justification = "Needed for marshalling" )]
        public static T ConvertToManaged( nint abiValue )
        {
            // NOTE: The AOT compiler will know what type T is here AND it can validate it implements
            //       the static method of the interface, therefore NO BOXING occurs and this is all
            //       a likely candidate for inlining. (Generated IL uses a constrained prefix instruction)
            return T.FromABI( abiValue );
        }

        [SuppressMessage( "Design", "CA1000:Do not declare static members on generic types", Justification = "Needed for marshalling" )]
        public static nint ConvertToUnmanaged( T managed )
        {
            return managed.DangerousGetHandle();
        }
    }
}
