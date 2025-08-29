// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Severity level for diagnostics</summary>
    public enum DiagnosticSeverity
    {
        /// <summary>Errors reported from native code</summary>
        Error = LLVMDiagnosticSeverity.LLVMDSError,

        /// <summary>Warnings reported from native code</summary>
        Warning = LLVMDiagnosticSeverity.LLVMDSWarning,

        /// <summary>Remarks reported from native code</summary>
        Remark = LLVMDiagnosticSeverity.LLVMDSRemark,

        /// <summary>Note level diagnostics reported from native code</summary>
        Note = LLVMDiagnosticSeverity.LLVMDSNote,
    }

    /// <summary>Structure to represent a Diagnostic information reported from a context diagnostic handler</summary>
    public readonly ref struct DiagnosticInfo
    {
        /// <summary>Gets the severity for this diagnostic</summary>
        public DiagnosticSeverity Severity => (DiagnosticSeverity)LLVMGetDiagInfoSeverity( Handle );

        /// <summary>Gets the description for this diagnostic</summary>
        public string Description => LLVMGetDiagInfoDescription( Handle );

        internal DiagnosticInfo( nint abi )
        {
            Handle = LLVMDiagnosticInfoRef.FromABI( abi );
        }

        internal LLVMDiagnosticInfoRef Handle { get; }
    }
}
