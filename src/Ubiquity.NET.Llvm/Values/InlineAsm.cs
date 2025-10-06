// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>Assembly language output dialect</summary>
    public enum AsmDialect
    {
        /// <summary>AT&amp;T Style</summary>
        ATT,

        /// <summary>Intel Style</summary>
        Intel
    }

    /// <summary>Inline Assembly for the target</summary>
    public class InlineAsm : Value
    {
        /// <summary>Initializes a new instance of the <see cref="InlineAsm"/> class from an <see cref="LLVMValueRef"/></summary>
        /// <param name="valueRef">low level LLVM reference</param>
        internal InlineAsm( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }

        // bool HasSideEffects => NativeMethods.LLVMHasSideEffects( Handle );
        // bool IsAlignStack => LLVMNative.IsAlignStack( Handle );
        // AsmDialect Dialect => LLVMNative.GetAsmDialect( Handle );
    }
}
