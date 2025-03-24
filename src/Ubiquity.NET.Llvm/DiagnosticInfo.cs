// -----------------------------------------------------------------------
// <copyright file="ContextAlias.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
        public DiagnosticSeverity Severity => (DiagnosticSeverity)LLVMGetDiagInfoSeverity(Handle);

        /// <summary>Gets the description for this diagnostic</summary>
        public string Description => LLVMGetDiagInfoDescription(Handle);

        internal DiagnosticInfo(nint abi)
        {
            Handle = LLVMDiagnosticInfoRef.FromABI(abi);
        }

        internal LLVMDiagnosticInfoRef Handle { get; }
    }
}
