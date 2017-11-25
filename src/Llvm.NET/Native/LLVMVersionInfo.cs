// <copyright file="LLVMVersionInfo.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace Llvm.NET.Native
{
    // Version information pulled from LibLLVM.dll
    // to use in detection of mismatched components
    internal struct LLVMVersionInfo
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Patch;

        public override string ToString( )
        {
            return Marshal.PtrToStringAnsi( VersionStringPtr );
        }

        public static implicit operator Version( LLVMVersionInfo versionInfo )
            => new Version( versionInfo.Major, versionInfo.Minor, versionInfo.Patch );

        private readonly IntPtr VersionStringPtr;
    }
}
