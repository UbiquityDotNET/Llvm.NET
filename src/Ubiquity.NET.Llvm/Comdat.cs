// -----------------------------------------------------------------------
// <copyright file="Comdat.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Ubiquity.NET.ArgValidators;
using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Comdat kind/behavior</summary>
    public enum ComdatKind
    {
        /// <summary>Linker may choose any COMDAT key, the choice is arbitrary</summary>
        Any = LLVMComdatSelectionKind.LLVMAnyComdatSelectionKind,

        /// <summary>Linker may choose any COMDAT key but sections must contain the same data</summary>
        ExactMatch = LLVMComdatSelectionKind.LLVMExactMatchComdatSelectionKind,

        /// <summary>The linker will choose the section containing the targets COMDAT key</summary>
        Largest = LLVMComdatSelectionKind.LLVMLargestComdatSelectionKind,

        /// <summary>The linker requires that only one section with this COMDAT key exists</summary>
        NoDuplicates = LLVMComdatSelectionKind.LLVMNoDeduplicateComdatSelectionKind,

        /// <summary>Linker may choose any COMDAT key but sections must contain the same amount of data</summary>
        SameSize = LLVMComdatSelectionKind.LLVMSameSizeComdatSelectionKind
    }

    /// <summary>Comdat entry for a module</summary>
    /// <remarks>
    /// A COMDAT is a named kind pair to ensure that, within a given module there are no two named COMDATs
    /// with different kinds. Ultimately, Comdat is 'owned' by the module, if the module is disposed the
    /// Comdats it owns are invalidated. Using a Comdat instance after the module is disposed results in
    /// an effective NOP.
    /// </remarks>
    public class Comdat
    {
        /// <summary>Gets the name of the <see cref="Comdat"/></summary>
        public string Name => Module.IsDisposed ? string.Empty : LibLLVMComdatGetName( ComdatHandle ).ToString();

        /// <summary>Gets or sets the <see cref="ComdatKind"/> for this <see cref="Comdat"/></summary>
        public ComdatKind Kind
        {
            get => Module.IsDisposed ? default : (ComdatKind)LLVMGetComdatSelectionKind( ComdatHandle );

            set
            {
                if(Module.IsDisposed)
                {
                    return;
                }

                LLVMSetComdatSelectionKind( ComdatHandle, (LLVMComdatSelectionKind)value );
            }
        }

        /// <summary>Gets the module the <see cref="Comdat"/> belongs to</summary>
        public BitcodeModule Module { get; }

        /// <summary>Initializes a new instance of the <see cref="Comdat"/> class from an LLVM module and reference</summary>
        /// <param name="module">Owning module for the comdat</param>
        /// <param name="comdatRef">LLVM-C API handle for the comdat</param>
        internal Comdat( BitcodeModule module, LLVMComdatRef comdatRef )
        {
            module.ValidateNotNull( nameof( module ) );
            comdatRef.ValidateNotDefault( nameof( comdatRef ) );

            Module = module;
            ComdatHandle = comdatRef;
        }

        /// <summary>Gets the wrapped <see cref="LLVMComdatRef"/></summary>
        internal LLVMComdatRef ComdatHandle { get; }
    }
}
