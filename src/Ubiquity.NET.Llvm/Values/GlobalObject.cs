// -----------------------------------------------------------------------
// <copyright file="GlobalObject.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Base class for Global objects in an LLVM ModuleHandle</summary>
    public class GlobalObject
        : GlobalValue
    {
        /// <summary>Gets or sets the alignment requirements for this object</summary>
        public uint Alignment
        {
            get => LLVMGetAlignment( Handle );
            set => LLVMSetAlignment( Handle, value );
        }

        /// <summary>Gets or sets the linker section this object belongs to</summary>
        public string Section
        {
            get => LLVMGetSection( Handle ) ?? string.Empty;
            set => LLVMSetSection( Handle, value );
        }

        /// <summary>Gets or sets the comdat attached to this object, if any</summary>
        /// <remarks>
        /// Setting this property to <see langword="null"/> or an
        /// empty string will remove any comdat setting for the
        /// global object.
        /// </remarks>
        public Comdat Comdat
        {
            get
            {
                LLVMComdatRef comdatRef = LLVMGetComdat( Handle );
                return comdatRef == default ? default : new Comdat( comdatRef );
            }

            set => LLVMSetComdat( Handle, value.Handle );
        }

        /// <summary>Sets metadata for this value</summary>
        /// <param name="kindID">Id id for the metadata</param>
        /// <param name="node">IrMetadata wrapped as a value</param>
        public void SetMetadata( uint kindID, IrMetadata node )
        {
            ArgumentNullException.ThrowIfNull( node );
            LLVMGlobalSetMetadata( Handle, kindID, node.Handle );
        }

        /// <summary>Gets a snap-shot collection of the metadata for this global</summary>
        /// <returns>Enumerable of the metadata nodes for the global</returns>
        public IEnumerable<MDNode> Metadata
        {
            get
            {
                using var entries = LLVMGlobalCopyAllMetadata( Handle, out size_t numEntries );
                for( long i = 0; i < numEntries.ToInt32( ); ++i )
                {
                    LLVMMetadataRef handle = LLVMValueMetadataEntriesGetMetadata( entries, ( uint )i );
                    yield return (MDNode)handle.ThrowIfInvalid( ).CreateMetadata( )!;
                }
            }
        }

        internal GlobalObject( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
