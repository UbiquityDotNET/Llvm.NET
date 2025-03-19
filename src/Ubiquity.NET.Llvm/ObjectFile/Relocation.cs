// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ObjectFileBindings;

namespace Ubiquity.NET.Llvm.ObjectFile
{
    // TODO FIX/Rework this. BAD LLVM API
    // LLVMRelocationIteratorRef is literally an iterator, however the elements it iterates
    // are not referenceable, so ALL "properties" of the element are instead exposed
    // as LLVM-C API calls If this is disposed, then the entire iteration is Disposed
    // not just a reference to the element.
    // CONSIDER: perhaps figure out how to make this a ref struct like Comdat...

    /// <summary>Relocation entry in an <see cref="ObjectFile"/></summary>
    [DebuggerDisplay( "{Symbol.Name,nq}({Description,nq})[{Offset}]:{Value}" )]
    public readonly record struct Relocation
    {
        /// <summary>Gets the offset for this relocation</summary>
        public ulong Offset => LLVMGetRelocationOffset( IteratorRef );

        /// <summary>Gets the symbol associated with this relocation</summary>
        public Symbol Symbol => new( Section.ContainingBinary, LLVMGetRelocationSymbol( IteratorRef ) );

#pragma warning disable IDISP012 // Property should not return created disposable

        // TODO: Remove DisposeMessageString as a return type. Pretty much all are just converting to a string
        //       and the IDisposable makes that a real PITA, let the marshalling do it.
        // CONSIDER: Remove DisposeMessageString as out param as well...

        /// <summary>Gets the kind of relocation as a string for display purposes</summary>
        public DisposeMessageString Description => LLVMGetRelocationTypeName( IteratorRef );

        /// <summary>Gets the relocation value as a string</summary>
        public DisposeMessageString Value => LLVMGetRelocationValueString( IteratorRef );
#pragma warning restore IDISP012 // Property should not return created disposable

        /// <summary>Gets the relocation type for this relocation</summary>
        /// <remarks>The meaning of the values are target obj file format specific, there is no standard</remarks>
        public ulong Kind => LLVMGetRelocationType( IteratorRef );

        /// <summary>Gets the <see cref="Ubiquity.NET.Llvm.ObjectFile.Section"/> this relocation belongs to</summary>
        public Section Section { get; }

        internal Relocation( Section owningSection, LLVMRelocationIteratorRef iterator )
            : this( owningSection, iterator, true )
        {
        }

        internal Relocation( Section owningSection, LLVMRelocationIteratorRef iterator, bool clone )
        {
            Section = owningSection;
            IteratorRef = clone ? LibLLVMRelocationIteratorClone( iterator ) : iterator.Move();
        }

#pragma warning disable IDISP006 // Implement IDisposable

        // Can't dispose the iterator, this is just a reference to one element
        // the enumerator that produces these owns the native iterator.
        internal LLVMRelocationIteratorRef IteratorRef { get; }
#pragma warning restore IDISP006 // Implement IDisposable
    }
}
