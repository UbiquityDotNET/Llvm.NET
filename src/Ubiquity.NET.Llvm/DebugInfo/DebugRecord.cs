// -----------------------------------------------------------------------
// <copyright file="DebugInfoRecord.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Managed wrapper for the new LLVM Debug Record format</summary>
    /// <seealso href="https://llvm.org/docs/RemoveDIsDebugInfo.html#c-api-changes"/>
    public readonly record struct DebugRecord
    {
        // TODO: Add common operations on a Debug Record...

        /// <summary>Gets a value indicating whether this represents a NULL handle</summary>
        public bool IsNull => Handle.IsNull;

        /// <inheritdoc/>
        public override string? ToString( )
        {
            return IsNull ? null : LLVMPrintDbgRecordToString( Handle );
        }

        /// <summary>Gets the next record in a sequence or a default constructed instance (<see cref="DebugRecord.IsNull"/> is <see langword="true"/>)</summary>
        public DebugRecord NextRecord => IsNull ? default : new( LLVMGetNextDbgRecord( Handle ) );

        /// <summary>Gets the previous record in a sequence or a default constructed instance (<see cref="DebugRecord.IsNull"/> is <see langword="true"/>)</summary>
        public DebugRecord PreviousRecord => IsNull ? default : new( LLVMGetPreviousDbgRecord( Handle ) );

        // TODO account for sub types - at least DbgLabelRecord and DbgVariableRecord are known...
        internal DebugRecord( LLVMDbgRecordRef handle )
        {
            Handle = handle;
        }

        private readonly LLVMDbgRecordRef Handle;
    }
}
