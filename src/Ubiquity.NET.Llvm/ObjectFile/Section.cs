// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ObjectFileBindings;

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Object;

namespace Ubiquity.NET.Llvm.ObjectFile
{
    // TODO: FIX/Rework this. BAD LLVM API

    /// <summary>Section in an <see cref="TargetBinary"/></summary>
    [DebuggerDisplay( "{Name,nq}" )]
    public readonly record struct Section
    {
        /// <summary>Gets the name of the section</summary>
        public string Name => LLVMGetSectionName( IteratorRef ) ?? string.Empty;

        /// <summary>Gets the <see cref="TargetBinary"/> this section belongs to</summary>
        public TargetBinary ContainingBinary { get; }

        /// <summary>Gets the Relocations in this <see cref="TargetBinary"/></summary>
        public IEnumerable<Relocation> Relocations
        {
            get
            {
                using LLVMRelocationIteratorRef iterator = LLVMGetRelocations( IteratorRef );
                while(!LLVMIsRelocationIteratorAtEnd( IteratorRef, iterator ))
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
                    return new ReadOnlySpan<byte>( LLVMGetSectionContents( IteratorRef ).ToPointer(), sectionSize );
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

        internal Section( TargetBinary objFile, LLVMSectionIteratorRef iterator )
            : this( objFile, iterator, true )
        {
        }

        internal Section( TargetBinary objFile, LLVMSectionIteratorRef iterator, bool clone )
        {
            ContainingBinary = objFile;
            IteratorRef = clone ? LibLLVMSectionIteratorClone( iterator ) : iterator.Move();
        }

#pragma warning disable IDISP006 // Implement IDisposable

        // Can't dispose the iterator, this is just a reference to one element
        // the enumerator that produces these owns the native iterator.
        internal LLVMSectionIteratorRef IteratorRef { get; }
#pragma warning restore IDISP006 // Implement IDisposable
    }
}
