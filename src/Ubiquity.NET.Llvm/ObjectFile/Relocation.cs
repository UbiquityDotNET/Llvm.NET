// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.ObjectFile
{
    /// <summary>Relocation entry in an <see cref="ObjectFile"/></summary>
    [DebuggerDisplay( "{Symbol.Name,nq}({Description,nq})[{Offset}]:{Value}" )]
    public struct Relocation
        : IEquatable<Relocation>
    {
        /// <summary>Gets the offset for this relocation</summary>
        public ulong Offset => LLVMGetRelocationOffset( IteratorRef );

        /// <summary>Gets the symbol associated with this relocation</summary>
        public Symbol Symbol => new Symbol( Section.ObjectFile, LLVMGetRelocationSymbol( IteratorRef ) );

        /// <summary>Gets the kind of relocation as a string for display purposes</summary>
        public string Description => LLVMGetRelocationTypeName( IteratorRef );

        /// <summary>Gets the relocation value as a string</summary>
        public string Value => LLVMGetRelocationValueString( IteratorRef );

        /// <summary>Gets the relocation type for this relocation</summary>
        /// <remarks>The meaning of the values are target obj file format specific, there is no standard</remarks>
        public ulong Kind => LLVMGetRelocationType( IteratorRef );

        /// <summary>Gets the <see cref="Ubiquity.NET.Llvm.ObjectFile.Section"/> this relocation belongs to</summary>
        public Section Section { get; }

        /// <inheritdoc/>
        public override bool Equals( object obj ) => ( obj is Relocation other ) && Equals( other );

        /// <inheritdoc/>
        public override int GetHashCode( ) => IteratorRef.GetHashCode( );

        /// <summary>Equality comparison</summary>
        /// <param name="left">left side of comparison</param>
        /// <param name="right">right side of comparison</param>
        /// <returns>Result of equality test</returns>
        public static bool operator ==( Relocation left, Relocation right ) => left.Equals( right );

        /// <summary>Inequality comparison</summary>
        /// <param name="left">left side of comparison</param>
        /// <param name="right">right side of comparison</param>
        /// <returns>Result of inequality test</returns>
        public static bool operator !=( Relocation left, Relocation right ) => !( left == right );

        /// <inheritdoc/>
        public bool Equals( Relocation other ) => IteratorRef.Equals( other.IteratorRef );

        internal Relocation( Section owningSection, LLVMRelocationIteratorRef iterator )
            : this( owningSection, iterator, true )
        {
        }

        internal Relocation( Section owningSection, LLVMRelocationIteratorRef iterator, bool clone )
        {
            Section = owningSection;
            IteratorRef = clone ? LibLLVMRelocationIteratorClone( iterator ) : iterator;
        }

        internal LLVMRelocationIteratorRef IteratorRef { get; }
    }
}
