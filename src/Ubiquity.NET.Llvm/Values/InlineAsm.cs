// -----------------------------------------------------------------------
// <copyright file="InlineAsm.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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

        // bool HasSideEffects => NativeMethods.LLVMHasSideEffects( NativeHandle );
        // bool IsAlignStack => LLVMNative.IsAlignStack( NativeHandle );
        // AsmDialect Dialect => LLVMNative.GetAsmDialect( NativeHandle );
    }
}
