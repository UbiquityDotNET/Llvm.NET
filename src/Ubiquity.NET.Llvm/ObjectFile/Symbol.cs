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
    /// <summary>Symbol in an <see cref="TargetObjectFile"/></summary>
    [DebuggerDisplay( "{Name,nq}@{Address}[{Size}]" )]
    public struct Symbol
        : IEquatable<Symbol>
    {
        /// <summary>Gets the <see cref="Ubiquity.NET.Llvm.ObjectFile"/> this symbol belongs to</summary>
        public TargetObjectFile ObjectFile { get; }

        /// <summary>Gets the section this symbol belongs to</summary>
        public Section Section
        {
            get
            {
                LLVMSectionIteratorRef iterator = LLVMGetSections( ObjectFile.ObjFileRef );
                LLVMMoveToContainingSection( iterator, IteratorRef );
                return new Section( ObjectFile, iterator, false );
            }
        }

        /// <summary>Gets the name of the symbol</summary>
        public string Name => LLVMGetSymbolName( IteratorRef );

        /// <summary>Gets the address of the symbol</summary>
        public ulong Address => LLVMGetSymbolAddress( IteratorRef );

        /// <summary>Gets the size of the symbol</summary>
        public ulong Size => LLVMGetSymbolSize( IteratorRef );

        /// <inheritdoc/>
        public override bool Equals( object obj ) => ( obj is Symbol other ) && Equals( other );

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool Equals( Symbol other ) => IteratorRef.Equals( other.IteratorRef );

        internal Symbol( TargetObjectFile objFile, LLVMSymbolIteratorRef iterator )
            : this( objFile, iterator, true )
        {
        }

        internal Symbol( TargetObjectFile objFile, LLVMSymbolIteratorRef iterator, bool clone )
        {
            IteratorRef = clone ? LibLLVMSymbolIteratorClone( iterator ) : iterator;
            ObjectFile = objFile;
        }

        internal LLVMSymbolIteratorRef IteratorRef { get; }
    }
}
