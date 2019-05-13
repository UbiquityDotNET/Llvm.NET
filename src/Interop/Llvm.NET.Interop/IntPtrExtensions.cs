// <copyright file="IntPtrExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Interop
{
    /// <summary>Extension methods for <see cref="IntPtr"/> and <see cref="UIntPtr"/></summary>
    public static class IntPtrExtensions
    {
        /// <summary>Tests an IntPtr for a zero value</summary>
        /// <param name="self">IntPtr to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsNull( this IntPtr self ) => self == IntPtr.Zero;

        /// <summary>Tests a UIntPtr for a zero value</summary>
        /// <param name="self">UIntPtr to test</param>
        /// <returns>Result of the test</returns>
        public static bool IsNull( this UIntPtr self ) => self == UIntPtr.Zero;
    }
}
