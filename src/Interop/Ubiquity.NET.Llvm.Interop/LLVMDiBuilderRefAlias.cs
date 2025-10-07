// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Interop
{
    /// <summary>Simple type safe handle to wrap an opaque pointer</summary>
    /// <remarks>
    /// This is a specialized form of an alias, it is NOT inherent in the LLVM APIs
    /// as no LLVM objects own a DIBuilder, but many consumers will. This allows for
    /// consistency in the ownership consideration of a given DIBuilder instance.
    /// </remarks>
    [NativeMarshalling(typeof(WrappedHandleMarshaller<LLVMDIBuilderRefAlias>))]
    public readonly record struct LLVMDIBuilderRefAlias
        : IWrappedHandle<LLVMDIBuilderRefAlias>
    {
        /// <summary>Gets a value indicating whether this handle is a <see langword="null"/> value</summary>
        public bool IsNull => Handle == nint.Zero;

        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <returns>The handle as an <see cref="nint"/></returns>
        public nint DangerousGetHandle() => Handle;

        public IntPtr ToIntPtr() => Handle;

        /// <summary>Interface defined factory for an instance of <see cref="LLVMDIBuilderRefAlias"/></summary>
        /// <param name="abiValue">Native ABI value of the handle</param>
        /// <returns>Type specific wrapper around the native ABI handle</returns>
        public static LLVMDIBuilderRefAlias FromABI(nint abiValue) => new(abiValue);

        /// <summary>Gets a zero (<see langword="null"/>) value handle</summary>
        public static LLVMDIBuilderRefAlias Zero => FromABI(nint.Zero);

        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <param name="value">Handle to convert</param>
        /// <returns>The handle as an <see cref="nint"/></returns>
        public static implicit operator nint(LLVMDIBuilderRefAlias value) => value.Handle;

        public static LLVMDIBuilderRefAlias From( LLVMDIBuilderRefAlias self )
        {
            self.ThrowIfInvalid();
            return FromABI(self.DangerousGetHandle());
        }

        // define implicit conversion from an OWNED value
        [SuppressMessage( "Usage", "CA2225:Operator overloads have named alternates", Justification = "Exists, tooling is too stupid to see it" )]
        public static implicit operator LLVMDIBuilderRefAlias( LLVMDIBuilderRef other ) => FromABI( other.DangerousGetHandle() );

        private LLVMDIBuilderRefAlias( nint p )
        {
            Handle = p;
        }

        private readonly nint Handle;
    }
}
