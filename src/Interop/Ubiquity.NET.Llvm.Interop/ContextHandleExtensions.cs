// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

    /// <summary>Utility class to host extensions for an <see cref="IWrappedHandle{THandle}"/></summary>
    public static class ContextHandleExtensions
    {
        /// <summary>Fluent null handle validation</summary>
        /// <typeparam name="THandle">Type of the handle to test</typeparam>
        /// <param name="handle">Handle to test</param>
        /// <param name="message">Message to use for an exception if thrown</param>
        /// <param name="memberName">Name if the member calling this function (usually provided by compiler via <see cref="CallerMemberNameAttribute"/></param>
        /// <param name="sourceFilePath">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerFilePathAttribute"/></param>
        /// <param name="sourceLineNumber">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerLineNumberAttribute"/></param>
        /// <returns>This object for fluent style use</returns>
        public static THandle ThrowIfInvalid<THandle>(
            this THandle handle,
            string message = "",
            [CallerMemberNameAttribute] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
        )
            where THandle : struct, IWrappedHandle<THandle>
        {
            return handle.DangerousGetHandle() == nint.Zero
                ? throw new UnexpectedNullHandleException( $"[{memberName}] - {sourceFilePath}@{sourceLineNumber} {message}" )
                : handle;
        }

        /// <summary>Fluent null handle validation specialized for <see cref="LLVMErrorRef"/></summary>
        /// <param name="handle">Handle to test</param>
        /// <param name="message">Message to use for an exception if thrown</param>
        /// <param name="memberName">Name if the member calling this function (usually provided by compiler via <see cref="CallerMemberNameAttribute"/></param>
        /// <param name="sourceFilePath">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerFilePathAttribute"/></param>
        /// <param name="sourceLineNumber">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerLineNumberAttribute"/></param>
        /// <returns>This object for fluent style use</returns>
        public static LLVMErrorRef ThrowIfInvalid(
            this LLVMErrorRef? handle,
            string message = "",
            [CallerMemberNameAttribute] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
        )
        {
            return handle is null || handle.IsNull
                ? throw new UnexpectedNullHandleException( $"[{memberName}] - {sourceFilePath}@{sourceLineNumber} {message}" )
                : handle;
        }
    }
}
