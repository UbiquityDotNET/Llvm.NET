// -----------------------------------------------------------------------
// <copyright file="DisposeMessageMarshaller.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Represents a marshaller for LLVM strings that require a call to LLVMDisposeMessage to release the native representation.</summary>
    [CLSCompliant(false)]
    [CustomMarshaller(typeof(string), MarshalMode.Default, typeof(DisposeMessageMarshaller))]
    public static unsafe class DisposeMessageMarshaller
    {
        /// <summary>Converts a string to an unmanaged version.</summary>
        /// <param name="managed">A managed string to convert.</param>
        /// <returns>The converted unmanaged string.</returns>
        /// <exception cref="NotSupportedException">Always - conversion from managed into native is not supported</exception>
        public static byte* ConvertToUnmanaged(string? managed)
        {
            // technically this could be supported by use of a LazyEncodedString and then calling
            // a [Lib]LLVMxxx API to allocate the disposable message... But that's a LOT of
            // extra cruft for something not currently needed. Only need to go from native to managed
            throw new NotSupportedException("Marshalling from managed to a string requiring LLVMDisposeMessage is not supported");
        }

        /// <summary>Converts an unmanaged string to a managed version.</summary>
        /// <param name="unmanaged">An unmanaged string to convert.</param>
        /// <returns>The converted managed string.</returns>
        public static string? ConvertToManaged(byte* unmanaged)
        {
            return ExecutionEncodingStringMarshaller.ConvertToManaged(unmanaged);
        }

        /// <summary>
        /// Frees the memory for the unmanaged string.
        /// </summary>
        /// <param name="unmanaged">The memory allocated for the unmanaged string.</param>
        public static void Free(byte* unmanaged)
        {
            LLVMDisposeMessage(unmanaged);

            [DllImport( LibraryName )]
            [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
            static extern void LLVMDisposeMessage(byte* p);
        }
    }
}
