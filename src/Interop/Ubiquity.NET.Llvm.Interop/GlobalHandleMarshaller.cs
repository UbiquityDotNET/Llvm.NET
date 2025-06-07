// -----------------------------------------------------------------------
// <copyright file="GlobalHandleMarshaller.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop
{
    [CustomMarshaller( typeof( CustomMarshallerAttribute.GenericPlaceholder ), MarshalMode.ElementIn, typeof( GlobalHandleMarshaller<> ) )]
    [CustomMarshaller( typeof( CustomMarshallerAttribute.GenericPlaceholder ), MarshalMode.ElementOut, typeof( GlobalHandleMarshaller<> ) )]
    [SuppressMessage( "Design", "CA1000:Do not declare static members on generic types", Justification = "Matches required pattern" )]
    public static class GlobalHandleMarshaller<T>
        where T : GlobalHandleBase, new()
    {
        public static nint ConvertToUnmanaged( T managed )
        {
            return managed.DangerousGetHandle();
        }

        public static T ConvertToManaged( nint unmanaged )
        {
            var retVal = new T();
            Marshal.InitHandle( retVal, unmanaged );
            return retVal;
        }
    }
}
