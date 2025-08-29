// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Comdat;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

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
        /// <param name="kindID">Id for the metadata</param>
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
                // using keeps the allocated data alive while enumerating
                // once complete the Dispose method is called to release the native
                // array.
                using var entries = LLVMGlobalCopyAllMetadata( Handle, out nuint numEntries );
                uint indexLimit = checked((uint)numEntries);
                for(uint i = 0; i < indexLimit; ++i)
                {
                    LLVMMetadataRef handle = LLVMValueMetadataEntriesGetMetadata( entries, i );
                    yield return (MDNode)handle.ThrowIfInvalid().CreateMetadata()!;
                }
            }
        }

        internal GlobalObject( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
