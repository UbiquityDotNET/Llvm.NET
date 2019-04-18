// -----------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;

[assembly: CLSCompliant(false)]

namespace Llvm.NET.Interop
{
    /// <summary>Interop methods for the Ubiquity.NET LibLLVM library</summary>
    public static partial class NativeMethods
    {
        internal const string LibraryPath = "LibLLVM";
    }
}
