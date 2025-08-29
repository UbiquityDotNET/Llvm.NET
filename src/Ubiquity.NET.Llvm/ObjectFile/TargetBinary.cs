// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Object;

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
    public sealed class TargetBinary
        : IDisposable
    {
        /*
        TODO: (Needs extension C APIs)
            string FileFormatName { get; }
            TripleArchType ArchKind { get; }
            SubTargetFeatures Features { get; }
            UInt64 StartAddress { get; }
            Triple Triple { get; }
            bool IsRelocatable { get; }
            void SetArmSubArch( Triple triple); // should validate triple is an arm arch and has a subArch to set...
        */

        /// <summary>Gets the kind of binary this instance contains</summary>
        public BinaryKind Kind => (BinaryKind)LLVMBinaryGetType( Handle );

        /// <summary>Gets the symbols in this <see cref="TargetBinary"/></summary>
        public IEnumerable<Symbol> Symbols
        {
            get
            {
                using LLVMSymbolIteratorRef iterator = LLVMObjectFileCopySymbolIterator(Handle);
                while(!LLVMObjectFileIsSymbolIteratorAtEnd( Handle, iterator ))
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
                using LLVMSectionIteratorRef iterator = LLVMObjectFileCopySectionIterator( Handle );
                while(!LLVMObjectFileIsSectionIteratorAtEnd( Handle, iterator ))
                {
                    yield return new Section( this, iterator );
                    LLVMMoveToNextSection( iterator );
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            Handle.Dispose();
            BackingBuffer.Dispose();
        }

        /// <summary>Initializes a new instance of the <see cref="TargetBinary"/> class.</summary>
        /// <param name="buffer">Memory buffer containing the raw binary data of the object file</param>
        /// <param name="context">ContextAlias for the object file</param>
        /// <remarks>The <paramref name="buffer"/> is 'moved" into this instance. That is buffer is the
        /// <see cref="MemoryBuffer.IsDisposed"/> property is <see langword="true"/> after this call completes
        /// without any exceptions. If there is an exception, transfer is not complete and ownership remains
        /// with the caller.
        /// </remarks>
        internal TargetBinary( MemoryBuffer buffer, IContext context )
        {
            ArgumentNullException.ThrowIfNull( buffer );
            ArgumentNullException.ThrowIfNull( context );

            BackingBuffer = new( buffer.Handle.Move() );
            Handle = LLVMCreateBinary( BackingBuffer.Handle, context.GetUnownedHandle(), out string? errMsg );
            if(Handle.IsInvalid)
            {
                throw new InternalCodeGeneratorException( errMsg?.ToString() ?? string.Empty );
            }
        }

        internal LLVMBinaryRef Handle { get; }

        private readonly MemoryBuffer BackingBuffer;
    }
}
