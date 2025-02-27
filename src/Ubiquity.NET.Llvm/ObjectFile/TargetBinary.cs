// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Ubiquity.NET.ArgValidators;
using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

namespace Ubiquity.NET.Llvm.ObjectFile
{
    /// <summary>Enumeration for kinds of binary/object files</summary>
    public enum BinaryKind
    {
        /// <summary>Archive File</summary>
        Archive = LLVMBinaryType.LLVMBinaryTypeArchive,

        /// <summary>Mach-O Universal Binary file</summary>
        MachOUniversalBinary = LLVMBinaryType.LLVMBinaryTypeMachOUniversalBinary,

        /// <summary>COFF Import File</summary>
        CoffImportFile = LLVMBinaryType.LLVMBinaryTypeCOFFImportFile,

        /// <summary>LLVM IR</summary>
        LlvmIR = LLVMBinaryType.LLVMBinaryTypeIR,

        /// <summary>Windows Resource (.res) file</summary>
        WinRes = LLVMBinaryType.LLVMBinaryTypeWinRes,

        /// <summary>COFF Object file</summary>
        Coff = LLVMBinaryType.LLVMBinaryTypeCOFF,

        /// <summary>ELF 32-bit, little endian</summary>
        Elf32LittleEndian = LLVMBinaryType.LLVMBinaryTypeELF32L,

        /// <summary>ELF 32-bit, big endian</summary>
        Elf32BigEndian = LLVMBinaryType.LLVMBinaryTypeELF32B,

        /// <summary>ELF 64-bit, little endian</summary>
        Elf64LittleEndian = LLVMBinaryType.LLVMBinaryTypeELF64L,

        /// <summary>ELF 64-bit, big endian</summary>
        Elf64BigEndian = LLVMBinaryType.LLVMBinaryTypeELF64B,

        /// <summary>Mach-O 32-bit, little endian</summary>
        MachO32LittleEndian = LLVMBinaryType.LLVMBinaryTypeMachO32L,

        /// <summary>Mach-O 32-bit, big endian</summary>
        MachO32BigEndian = LLVMBinaryType.LLVMBinaryTypeMachO32B,

        /// <summary>Mach-O 64-bit, little endian</summary>
        MachO64LittleEndian = LLVMBinaryType.LLVMBinaryTypeMachO64L,

        /// <summary>Mach-O 64-bit, big endian</summary>
        MachO64BigEndian = LLVMBinaryType.LLVMBinaryTypeMachO64B,

        /// <summary>Web Assembly</summary>
        Wasm = LLVMBinaryType.LLVMBinaryTypeWasm,
    }

    /// <summary>Object file information</summary>
    public class TargetBinary
    {
        /*
        TODO: (Needs extension C APIs)
            string FileFormatName { get; }
            TripleArchType ArchType { get; }
            SubTargetFeatures Features { get; }
            UInt64 StartAddress { get; }
            Triple Triple { get; }
            bool IsRelocatable { get; }
            void SetArmSubArch( Triple triple); // should validate triple is an arm arch and has a subArch to set...
        */

        /// <summary>Gets the kind of binary this instance contains</summary>
        public BinaryKind Kind => ( BinaryKind )LLVMBinaryGetType( BinaryRef );

        /// <summary>Gets the symbols in this <see cref="TargetBinary"/></summary>
        public IEnumerable<Symbol> Symbols
        {
            get
            {
                using LLVMSymbolIteratorRef iterator = LLVMObjectFileCopySymbolIterator(BinaryRef);
                while( !LLVMObjectFileIsSymbolIteratorAtEnd( BinaryRef, iterator ) )
                {
                    yield return new Symbol( this, iterator );
                    LLVMMoveToNextSymbol( iterator );
                }
            }
        }

        /// <summary>Gets the sections in this <see cref="TargetBinary"/></summary>
        public IEnumerable<Section> Sections
        {
            get
            {
                using LLVMSectionIteratorRef iterator = LLVMObjectFileCopySectionIterator( BinaryRef );
                while( !LLVMObjectFileIsSectionIteratorAtEnd( BinaryRef, iterator ) )
                {
                    yield return new Section( this, iterator );
                    LLVMMoveToNextSection( iterator );
                }
            }
        }

        /// <summary>Initializes a new instance of the <see cref="TargetBinary"/> class.</summary>
        /// <param name="buffer">Memory buffer containing the raw binary data of the object file</param>
        /// <param name="context">Context for the object file</param>
        internal TargetBinary( MemoryBuffer buffer, Context context )
        {
            buffer.ValidateNotNull( nameof( buffer ) );
            context.ValidateNotNull( nameof( context ) );

            DisposeMessageString? errMsg = null;
            try
            {
                BinaryRef = LLVMCreateBinary( buffer.BufferHandle, context.ContextHandle, out errMsg );
                if( BinaryRef.IsInvalid )
                {
                    throw new InternalCodeGeneratorException( errMsg.ToString() );
                }
            }
            finally
            {
                errMsg?.Dispose();
            }
        }

        internal LLVMBinaryRef BinaryRef { get; }
    }
}
