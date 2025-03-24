// -----------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Interop methods for the Ubiquity.NET LibLLVM library</summary>
    public static partial class NativeMethods
    {
        /// <summary>Name of the native interop library</summary>
        /// <remarks>
        /// This name is referenced by the code generated for the handles via `LlvmBindingsGenerator` AND
        /// ALL of the <see cref="LibraryImportAttribute"/> marked P/Invoke signatures. Any change in the
        /// FULLY Qualified name of this symbol will necessitate a change in those as well. Normal refactoring
        /// would cover much, but not all, of such cases as there is an additional code generation tool involved.
        /// </remarks>
        public const string LibraryName = "Ubiquity.NET.LibLLVM";
    }
}
