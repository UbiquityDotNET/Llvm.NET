// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
