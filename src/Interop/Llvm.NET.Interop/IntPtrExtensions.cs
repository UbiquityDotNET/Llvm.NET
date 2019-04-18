// <copyright file="IntPtrExtensions.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;

namespace Llvm.NET.Interop
{
    internal static class IntPtrExtensions
    {
        public static bool IsNull( this IntPtr self ) => self == IntPtr.Zero;

        public static bool IsNull( this UIntPtr self ) => self == UIntPtr.Zero;
    }
}
