// -----------------------------------------------------------------------
// <copyright file="LLVMOrcJITDylibRef.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Simple type safe handle to wrap an opaque pointer for interop with "C" API exported from LibLLVM</summary>
    /// <remarks>
    ///    This handle is owned by it's container and therefore isn't disposed by the
    ///    calling App.
    /// <note type="important">
    ///     Since the object this handle refers to is not owned by the App, the object is
    ///     destroyed whenever it's container is destroyed, which will invalidate this handle.
    ///     Use of this handle after the container is destroyed will produce undefined
    ///     behavior, including, and most likely, memory access violations.
    /// </note>
    /// </remarks>
    [NativeMarshalling(typeof(ContextHandleMarshaller<LLVMOrcJITDylibRef>))]
    public readonly record struct LLVMOrcJITDylibRef
        : IContextHandle<LLVMOrcJITDylibRef>
    {
        /// <summary>Fluent null handle validation</summary>
        /// <param name="message">Message to use for an exception if thrown</param>
        /// <param name="memberName">Name if the member calling this function (usually provided by compiler via <see cref="CallerMemberNameAttribute"/></param>
        /// <param name="sourceFilePath">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerFilePathAttribute"/></param>
        /// <param name="sourceLineNumber">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerLineNumberAttribute"/></param>
        /// <returns>This object for fluent style use</returns>
        public LLVMOrcJITDylibRef ThrowIfInvalid(
            string message = "",
            [CallerMemberNameAttribute] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0 )
        {
            return DangerousGetHandle() == nint.Zero
                ? throw new UnexpectedNullHandleException( $"[{memberName}] - {sourceFilePath}@{sourceLineNumber} {message} " )
                : this;
        }

        /// <summary>Gets a value indicating whether this handle is a <see langword="null"/> value</summary>
        public bool IsNull => Handle == nint.Zero;

        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <returns>The handle as an <see cref="nint"/></returns>
        public nint DangerousGetHandle() => Handle;

        /// <summary>Interface defined factory for an instance of <see cref="LLVMOrcJITDylibRef"/></summary>
        /// <param name="abiValue">Native ABI value of the handle</param>
        /// <returns>Type specific wrapper around the native ABI handle</returns>
        public static LLVMOrcJITDylibRef FromABI(nint abiValue) => new(abiValue);

        /// <summary>Gets a zero (<see langword="null"/>) value handle</summary>
        public static LLVMOrcJITDylibRef Zero => FromABI(nint.Zero);

        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <param name="value">Handle to convert</param>
        /// <returns>The handle as an <see cref="nint"/></returns>
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "It has one called DangerousGetHandle()" )]
        public static implicit operator nint(LLVMOrcJITDylibRef value) => value.DangerousGetHandle();

        private LLVMOrcJITDylibRef( nint p )
        {
            Handle = p;
        }

        private readonly nint Handle;
    }
}
