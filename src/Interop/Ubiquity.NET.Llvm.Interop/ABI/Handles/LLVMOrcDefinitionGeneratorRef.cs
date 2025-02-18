// -----------------------------------------------------------------------
// <copyright file="LLVMOrcDefinitionGeneratorRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Global LLVM object handle</summary>
    [SuppressMessage( "Design", "CA1060:Move pinvokes to native methods class", Justification = "Called ONLY by this class" )]
    public partial class LLVMOrcDefinitionGeneratorRef
        : LlvmObjectRef
    {
        /// <summary>Initializes a new instance of the <see cref="LLVMOrcDefinitionGeneratorRef"/> with default values</summary>
        public LLVMOrcDefinitionGeneratorRef()
            : base(ownsHandle: true)
        {
        }

        /// <summary>Initializes an instance of <see cref="LLVMOrcDefinitionGeneratorRef"/></summary>
        /// <param name="handle">Raw native pointer for the handle</param>
        /// <param name="owner">Value to indicate whether the handle is owned or not</param>
        public LLVMOrcDefinitionGeneratorRef( nint handle, bool owner )
            : base( owner )
        {
            SetHandle( handle );
        }

        /// <summary>Gets a Zero (<see langword="null"/>) value handle</summary>
        public static LLVMOrcDefinitionGeneratorRef Zero { get; } = new LLVMOrcDefinitionGeneratorRef(nint.Zero, false);

        /// <inheritdoc/>
        protected override bool ReleaseHandle( )
        {
            // critical safety check, base should never call ReleaseHandle on an invalid handle
            // but ABI usually can't handle that and would just crash the app, so make it
            // a NOP just in case.
            if( handle != nint.Zero )
            {
                LLVMOrcDisposeDefinitionGenerator( handle );
            }

            return true;
        }

        [LibraryImport( NativeMethods.LibraryPath )]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        private static unsafe partial void LLVMOrcDisposeDefinitionGenerator( nint p );
    }
}
