// <copyright file="Generated.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Maps to LLVMBool in LLVM-C for methods that return 0 on success.</summary>
    /// <remarks>
    /// This was hand added to help clarify use when a return value is not really
    /// a bool but a status where (0==SUCCESS). Sadly, the LLVM API makes a lot of use
    /// of this anti-pattern and it requires reading the docs/code to know which is which.
    /// </remarks>
    public readonly record struct LLVMStatus
    {
        /// <summary>Gets a value indicating whether the operation was successful</summary>
        public bool Succeeded => ErrorCode == 0;

        public int ErrorCode { get; init; }

        /// <summary>Gets a value indicating whether the operation Failed</summary>
        public bool Failed => !Succeeded;

        internal LLVMStatus(int value)
        {
            ErrorCode = value;
        }
    }
}
