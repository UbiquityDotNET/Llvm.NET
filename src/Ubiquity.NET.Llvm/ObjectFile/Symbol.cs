// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ObjectFileBindings;
using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.ObjectFile
{
    /// <summary>Symbol in an <see cref="TargetBinary"/></summary>
    [DebuggerDisplay( "{Name,nq}@{Address}[{Size}]" )]
    public readonly struct Symbol
        : IEquatable<Symbol>
    {
        /// <summary>Gets the <see cref="Ubiquity.NET.Llvm.ObjectFile.TargetBinary"/> this symbol belongs to</summary>
        public TargetBinary ContainingBinary { get; }

        /// <summary>Gets the section this symbol belongs to</summary>
        public Section Section
        {
            get
            {
                LLVMSectionIteratorRef iterator = LLVMObjectFileCopySectionIterator( ContainingBinary.BinaryRef );
                LLVMMoveToContainingSection( iterator, IteratorRef );
                return new Section( ContainingBinary, iterator, false );
            }
        }

        /// <summary>Gets the name of the symbol</summary>
        public string Name => LLVMGetSymbolName( IteratorRef ) ?? string.Empty;

        /// <summary>Gets the address of the symbol</summary>
        public ulong Address => LLVMGetSymbolAddress( IteratorRef );

        /// <summary>Gets the size of the symbol</summary>
        public ulong Size => LLVMGetSymbolSize( IteratorRef );

        /// <summary>Tests an <see cref="object"/> for equality</summary>
        /// <param name="obj"><see cref="object"/> to compare with this instance</param>
        /// <returns><see langword="true"/> if the </returns>
        public override bool Equals( object? obj ) => ( obj is Symbol other ) && Equals( other );

        /// <summary>Gets a hash code for this <see cref="Section"/></summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode( ) => IteratorRef.GetHashCode( );

        /// <summary>Equality comparison</summary>
        /// <param name="left">left side of comparison</param>
        /// <param name="right">right side of comparison</param>
        /// <returns>Result of equality test</returns>
        public static bool operator ==( Symbol left, Symbol right ) => left.IteratorRef.Equals( right );

        /// <summary>Inequality comparison</summary>
        /// <param name="left">left side of comparison</param>
        /// <param name="right">right side of comparison</param>
        /// <returns>Result of inequality test</returns>
        public static bool operator !=( Symbol left, Symbol right ) => !( left.IteratorRef.Equals( right ) );

        /// <summary>Tests an <see cref="Symbol"/> for equality</summary>
        /// <param name="other"><see cref="Symbol"/> to compare with this instance</param>
        /// <returns><see langword="true"/> if the </returns>
        public bool Equals( Symbol other ) => IteratorRef.Equals( other.IteratorRef );

        internal Symbol( TargetBinary objFile, LLVMSymbolIteratorRef iterator )
            : this( objFile, iterator, true )
        {
        }

        internal Symbol( TargetBinary binary, LLVMSymbolIteratorRef iterator, bool clone )
        {
            IteratorRef = clone ? LibLLVMSymbolIteratorClone( iterator ) : iterator;
            ContainingBinary = binary;
        }

        internal LLVMSymbolIteratorRef IteratorRef { get; }
    }
}
