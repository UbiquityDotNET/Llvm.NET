// -----------------------------------------------------------------------
// <copyright file="MDString.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Stores a string in IrMetadata</summary>
    public class MDString
        : IrMetadata
    {
        /// <summary>Gets the string from the metadata node</summary>
        /// <returns>String this node wraps</returns>
        public override string ToString( )
        {
            unsafe
            {
                // Return may not have a terminator so go through LazyEncodedString
                // as a span of bytes.
                byte* pNativeString = LibLLVMGetMDStringText( Handle, out uint len );
                if (len == 0)
                {
                    return string.Empty;
                }

                var s = new LazyEncodedString(new ReadOnlySpan<byte>(pNativeString, checked((int)len)));
                return s.ToString();
            }
        }

        internal MDString( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
