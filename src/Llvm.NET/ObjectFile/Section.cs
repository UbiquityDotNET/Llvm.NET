// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Llvm.NET.Interop;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.ObjectFile
{
    /// <summary>Section in an <see cref="ObjectFile"/></summary>
    [DebuggerDisplay( "{Name,nq}" )]
    public struct Section
        : IEquatable<Section>
    {
        /// <summary>Gets the name of the section</summary>
        public string Name => LLVMGetSectionName( IteratorRef );

        /// <summary>Gets the object file this section belongs to</summary>
        public TargetObjectFile ObjectFile { get; }

        /// <summary>Gets the Relocations in this <see cref="TargetObjectFile"/></summary>
        public IEnumerable<Relocation> Relocations
        {
            get
            {
                using( LLVMRelocationIteratorRef iterator = LLVMGetRelocations( IteratorRef ) )
                {
                    while( !LLVMIsRelocationIteratorAtEnd( IteratorRef, iterator ) )
                    {
                        yield return new Relocation( this, iterator );
                        LLVMMoveToNextRelocation( iterator );
                    }
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

        /// <inheritdoc/>
        public override bool Equals( object obj ) => ( obj is Section other ) && Equals( other );

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool Equals( Section other ) => IteratorRef.Equals( other.IteratorRef );

        internal Section( TargetObjectFile objFile, LLVMSectionIteratorRef iterator )
            : this( objFile, iterator, true )
        {
        }

        internal Section( TargetObjectFile objFile, LLVMSectionIteratorRef iterator, bool clone )
        {
            ObjectFile = objFile;
            IteratorRef = clone ? LibLLVMSectionIteratorClone( iterator ) : iterator;
        }

        internal LLVMSectionIteratorRef IteratorRef { get; }
    }
}
