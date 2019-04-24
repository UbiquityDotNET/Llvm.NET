// -----------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Llvm.NET.Interop
{
    /// <summary>Interop methods for the Ubiquity.NET LibLLVM library</summary>
    public static partial class NativeMethods
    {
        internal const string LibraryPath = "LibLLVM";

        /// <summary>retrieves the raw underlying native C++ ValueKind enumeration for a value</summary>
        /// <remarks>
        /// This is generally only used in the mapping of an LLVMValueRef to the Llvm.NET
        /// instance wrapping it. Since the Stable C API uses a distinct enum for the
        /// instruction codes, they don't actually match the underlying C++ kind and
        /// actually overlap it in incompatible ways. So, this uses the underlying enum
        /// to build up the correct .NET types for a given LLVMValueRef.
        /// </remarks>
        /// <param name="valueRef">Value to get the true <see cref="ValueKind"/> for</param>
        /// <returns>
        /// <see cref="ValueKind"/> for <paramref name="valueRef"/>
        /// </returns>
        public static ValueKind LLVMGetValueIdAsKind( LLVMValueRef valueRef ) => ( ValueKind )LLVMGetValueID( valueRef );
    }
}
