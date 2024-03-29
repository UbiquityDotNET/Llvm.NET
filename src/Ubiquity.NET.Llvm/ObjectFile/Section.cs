﻿// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.ObjectFile
{
    /// <summary>Section in an <see cref="TargetBinary"/></summary>
    [DebuggerDisplay( "{Name,nq}" )]
    public struct Section
        : IEquatable<Section>
    {
        /// <summary>Gets the name of the section</summary>
        public string Name => LLVMGetSectionName( IteratorRef );

        /// <summary>Gets the <see cref="TargetBinary"/> this section belongs to</summary>
        public TargetBinary ContainingBinary { get; }

        /// <summary>Gets the Relocations in this <see cref="TargetBinary"/></summary>
        public IEnumerable<Relocation> Relocations
        {
            get
            {
                using LLVMRelocationIteratorRef iterator = LLVMGetRelocations( IteratorRef );
                while( !LLVMIsRelocationIteratorAtEnd( IteratorRef, iterator ) )
                {
                    yield return new Relocation( this, iterator );
                    LLVMMoveToNextRelocation( iterator );
                }
            }
        }

        /// <summary>Gets the contents of the section</summary>
        public ReadOnlySpan<byte> Contents
        {
            get
            {
                int sectionSize = checked((int)LLVMGetSectionSize( IteratorRef ));
                unsafe
                {
                    return new ReadOnlySpan<byte>( LLVMGetSectionContents( IteratorRef ).ToPointer( ), sectionSize );
                }
            }
        }

        /// <summary>Tests whether the section contains a given symbol</summary>
        /// <param name="symbol">Symbol to test for</param>
        /// <returns><see langword="true" /> if the section contains <paramref name="symbol"/></returns>
        public bool ContainsSymbol( Symbol symbol ) => LLVMGetSectionContainsSymbol( IteratorRef, symbol.IteratorRef );

        /// <summary>Move the iterator to the section containing the provided symbol</summary>
        /// <param name="symbol">symbol to find the section for</param>
        public void MoveToContainingSection( Symbol symbol )
        {
            LLVMMoveToContainingSection( IteratorRef, symbol.IteratorRef );
        }

        /// <summary>Gets the address of the section</summary>
        public ulong Address => LLVMGetSectionAddress( IteratorRef );

        /// <summary>Tests an <see cref="object"/> for equality</summary>
        /// <param name="obj"><see cref="object"/> to compare with this instance</param>
        /// <returns><see langword="true"/> if the </returns>
        public override bool Equals( object? obj ) => ( obj is Section other ) && Equals( other );

        /// <summary>Gets a hash code for this <see cref="Section"/></summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode( ) => IteratorRef.GetHashCode( );

        /// <summary>Equality comparison</summary>
        /// <param name="left">left side of comparison</param>
        /// <param name="right">right side of comparison</param>
        /// <returns>Result of equality test</returns>
        public static bool operator ==( Section left, Section right ) => left.Equals( right );

        /// <summary>Inequality comparison</summary>
        /// <param name="left">left side of comparison</param>
        /// <param name="right">right side of comparison</param>
        /// <returns>Result of inequality test</returns>
        public static bool operator !=( Section left, Section right ) => !( left == right );

        /// <summary>Tests an <see cref="Section"/> for equality</summary>
        /// <param name="other"><see cref="Section"/> to compare with this instance</param>
        /// <returns><see langword="true"/> if the </returns>
        public bool Equals( Section other ) => IteratorRef.Equals( other.IteratorRef );

        internal Section( TargetBinary objFile, LLVMSectionIteratorRef iterator )
            : this( objFile, iterator, true )
        {
        }

        internal Section( TargetBinary objFile, LLVMSectionIteratorRef iterator, bool clone )
        {
            ContainingBinary = objFile;
            IteratorRef = clone ? LibLLVMSectionIteratorClone( iterator ) : iterator;
        }

        internal LLVMSectionIteratorRef IteratorRef { get; }
    }
}
