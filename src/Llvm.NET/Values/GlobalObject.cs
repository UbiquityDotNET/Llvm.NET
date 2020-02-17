// -----------------------------------------------------------------------
// <copyright file="GlobalObject.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Llvm.NET.Interop;
using Llvm.NET.Properties;
using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.Values
{
    /// <summary>Base class for Global objects in an LLVM Module</summary>
    public class GlobalObject
        : GlobalValue
    {
        /// <summary>Gets or sets the alignment requirements for this object</summary>
        public uint Alignment
        {
            get => LLVMGetAlignment( ValueHandle );
            set => LLVMSetAlignment( ValueHandle, value );
        }

        /// <summary>Gets or sets the linker section this object belongs to</summary>
        public string Section
        {
            get => LLVMGetSection( ValueHandle );
            set => LLVMSetSection( ValueHandle, value );
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
                LLVMComdatRef comdatRef = LLVMGetComdat( ValueHandle );
                return comdatRef == default ? null : new Comdat( ParentModule, comdatRef );
            }

            set
            {
                if( value != null && value.Module != ParentModule )
                {
                    throw new ArgumentException( Resources.Mismatched_modules_for_Comdat, nameof( value ) );
                }

                LLVMSetComdat( ValueHandle, value?.ComdatHandle ?? LLVMComdatRef.Zero );
            }
        }

        /// <summary>Sets metadata for this value</summary>
        /// <param name="kindID">Kind id for the metadata</param>
        /// <param name="node">Metadata wrapped as a value</param>
        public void SetMetadata( uint kindID, LlvmMetadata node )
        {
            node.ValidateNotNull( nameof( node ) );
            LLVMGlobalSetMetadata( ValueHandle, kindID, node.MetadataHandle );
        }

        /// <summary>Gets a snap-shot collection of the metadata for this global</summary>
        /// <returns>Enumerable of the metadata nodes for the global</returns>
        public IEnumerable<MDNode> Metadata
        {
            get
            {
                using( var entries = LLVMGlobalCopyAllMetadata( ValueHandle, out size_t numEntries ) )
                {
                    for( long i = 0; i < numEntries.ToInt32( ); ++i )
                    {
                        LLVMMetadataRef handle = LLVMValueMetadataEntriesGetMetadata( entries, ( uint )i );
                        yield return MDNode.FromHandle<MDNode>( handle );
                    }
                }
            }
        }

        internal GlobalObject( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
