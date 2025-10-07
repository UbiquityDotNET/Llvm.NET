// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;

namespace Ubiquity.NET.Llvm.Interop
{
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "BS, extension" )]
    [SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "BS, Extension" )]
    public static class LLVMValueRefExtensions
    {
        extension( LLVMValueRef self )
        {
            public LibLLVMValueKind ValueKind => LibLLVMGetValueKind(self);
        }
    }
}
