// -----------------------------------------------------------------------
// <copyright file="ContextHandleExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Ubiquity.NET.InteropHelpers;

namespace Ubiquity.NET.Llvm.Interop
{
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
            where THandle : struct, IContextHandle<THandle>
        {
            return handle.DangerousGetHandle() == nint.Zero
                ? throw new UnexpectedNullHandleException( $"[{memberName}] - {sourceFilePath}@{sourceLineNumber} {message} " )
                : handle;
        }
    }
}
