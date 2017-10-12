﻿// <copyright file="DIEnumerator.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>Debug Information for an enumerated type</summary>
    /// <seealso href="xref:llvm_langref#dienumerator">LLVM DIEnumerator</seealso>
    public class DIEnumerator
        : DINode
    {
        /// <summary>Creates a new <see cref="DIEnumerator"/> from an LLVM handle</summary>
        /// <param name="handle">Native LLVM reference for an enumerator</param>
        internal DIEnumerator( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}