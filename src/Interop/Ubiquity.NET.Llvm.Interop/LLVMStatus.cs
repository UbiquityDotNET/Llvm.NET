// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

        internal LLVMStatus( int value )
        {
            ErrorCode = value;
        }
    }
}
