// -----------------------------------------------------------------------
// <copyright file="ObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Llvm.NET.Interop;
using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET.ObjectFile
{
    /// <summary>Object file information</summary>
    public class TargetObjectFile
        : DisposableObject
    {
        /// <summary>Initializes a new instance of the <see cref="TargetObjectFile"/> class.</summary>
        /// <param name="buffer">Memory buffer containing the raw binary data of the object file</param>
        public TargetObjectFile( MemoryBuffer buffer )
        {
            buffer.ValidateNotNull( nameof( buffer ) );
            ObjFileRef = LLVMCreateObjectFile( buffer.BufferHandle );

            // ObjFile now internally owns the buffer, so detach to prevent GC from releasing it in
            // non-deterministic finalization.
            buffer.Detach( );
        }

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

        /// <summary>Gets the symbols in this <see cref="TargetObjectFile"/></summary>
        public IEnumerable<Symbol> Symbols
        {
            get
            {
                using( LLVMSymbolIteratorRef iterator = LLVMGetSymbols( ObjFileRef ) )
                {
                    while( !LLVMIsSymbolIteratorAtEnd( ObjFileRef, iterator ) )
                    {
                        yield return new Symbol( this, iterator );
                        LLVMMoveToNextSymbol( iterator );
                    }
                }
            }
        }

        /// <summary>Gets the sections in this <see cref="TargetObjectFile"/></summary>
        public IEnumerable<Section> Sections
        {
            get
            {
                using( LLVMSectionIteratorRef iterator = LLVMGetSections( ObjFileRef ) )
                {
                    while( !LLVMIsSectionIteratorAtEnd( ObjFileRef, iterator ) )
                    {
                        yield return new Section( this, iterator );
                        LLVMMoveToNextSection( iterator );
                    }
                }
            }
        }

        /// <summary>Opens a <see cref="TargetObjectFile"/> from a path</summary>
        /// <param name="path">path to the object file binary</param>
        /// <returns>new object file</returns>
        /// <exception cref="System.IO.IOException">File IO failures</exception>
        public static TargetObjectFile Open( string path )
        {
            return new TargetObjectFile( new MemoryBuffer( path ) );
        }

        internal LLVMObjectFileRef ObjFileRef { get; }

        /// <inheritdoc/>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                ObjFileRef.Dispose( );
            }
        }
    }
}
