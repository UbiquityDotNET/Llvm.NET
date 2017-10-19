﻿// <copyright file="DebugMemberLayout.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

namespace Llvm.NET.DebugInfo
{
    /// <summary>DebugMemberLayout is used to define custom layout information for structure members</summary>
    /// <remarks>
    /// Ordinarily layout information is handle automatically in
    /// <see cref="DebugStructType.SetBody(bool, BitcodeModule, DIScope, DIFile, uint, DebugInfoFlags, System.Collections.Generic.IEnumerable{DebugMemberInfo})"/>
    /// however in cases where explicitly controlled (or "packed") layout is required, instances of DebugMemberLayout are
    /// used to provide the information necessary to generate a proper type and debug information.
    /// </remarks>
    public class DebugMemberLayout
    {
        /// <summary>Initializes a new instance of the <see cref="DebugMemberLayout"/> class.</summary>
        /// <param name="bitSize">Size of the member in bits</param>
        /// <param name="bitAlignment">Alignment of the member in bits</param>
        /// <param name="bitOffset">Offset of the member in bits</param>
        public DebugMemberLayout( ulong bitSize, uint bitAlignment, ulong bitOffset )
        {
            BitSize = bitSize;
            BitAlignment = bitAlignment;
            BitOffset = bitOffset;
        }

        /// <summary>Gets the bit size for the field</summary>
        public ulong BitSize { get; }

        /// <summary>Gets the bit alignment for the field</summary>
        public uint BitAlignment { get; }

        /// <summary>Gets the bit offset for the field in it's containing type</summary>
        public ulong BitOffset { get; }
    }
}
