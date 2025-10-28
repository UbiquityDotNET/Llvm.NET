// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;

namespace Ubiquity.NET.Llvm.Interop
{
    // This does NOT use the new C# 14 extension syntax due to several reasons
    // 1) Code lens does not work https://github.com/dotnet/roslyn/issues/79006 [Sadly, marked as "not planned" - e.g., dead-end]
    // 2) MANY analyzers get things wrong and need to be supressed (CA1000, CA1034, and many others [SAxxxx])
    // 3) Many tools (like docfx) don't support the new syntax yet and it isn't clear if they will in the future.
    // 4) No clear support for Caller* attributes ([CallerArgumentExpression(...)]).
    //
    // Bottom line it's a good idea with an incomplete implementation lacking support
    // in the overall ecosystem. Don't use it unless you absolutely have to until all
    // of that is sorted out.

    public static class LLVMValueRefExtensions
    {
        /// <summary>Gets the kind of this value</summary>
        /// <param name="self">Value reference</param>
        /// <returns>The kind of the value</returns>
        /// <remarks>This is useful for debugging to check the kind reported by the native code</remarks>
        public static LibLLVMValueKind GetValueKind(this LLVMValueRef self )
        {
            return LibLLVMGetValueKind( self );
        }
    }
}
