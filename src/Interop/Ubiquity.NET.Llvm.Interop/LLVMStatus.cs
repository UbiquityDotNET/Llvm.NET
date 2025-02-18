// <copyright file="Generated.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>maps to LLVMBool in LLVM-C for methods that return 0 on success.</summary>
    /// <remarks>
    /// This was hand added to help clarify use when a return value is not really
    /// a bool but a status where (0==SUCCESS)
    /// </remarks>
    public readonly record struct LLVMStatus
    {
        /// <summary>Gets a value indicating whether the operation was successful</summary>
        public bool Succeeded => ErrorCode == 0;

        /// <summary>Gets a value indicating whether the operation Failed</summary>
        public bool Failed => !Succeeded;

        internal LLVMStatus(int value)
        {
            ErrorCode = value;
        }

        private readonly int ErrorCode;
    }
}
