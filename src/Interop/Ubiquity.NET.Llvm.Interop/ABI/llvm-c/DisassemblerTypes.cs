// -----------------------------------------------------------------------
// <copyright file="DisassemblerTypes.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    [StructLayout( LayoutKind.Sequential )]
    [SuppressMessage( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "ABI interop struct, equality not relevant" )]
    public unsafe struct LLVMOpInfoSymbol1
    {
        public UInt64 Present;
        public byte* Name;
        public UInt64 Value;
    }

    [StructLayout( LayoutKind.Sequential )]
    public record struct LLVMOpInfo1
    {
        public LLVMOpInfoSymbol1 AddSymbol;
        public LLVMOpInfoSymbol1 SubtractSymbol;
        public UInt64 Value;
        public UInt64 VariantKind;
    }
}
