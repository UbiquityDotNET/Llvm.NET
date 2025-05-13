// -----------------------------------------------------------------------
// <copyright file="MDString.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.MetadataBindings;

namespace Ubiquity.NET.Llvm.Metadata
{
    /// <summary>Stores a string in IrMetadata</summary>
    public class MDString
        : IrMetadata
    {
        /// <summary>Gets the string from the metadata node without the overhead of any encoding</summary>
        /// <returns>String from the metadata</returns>
        /// <remarks>
        /// This captures/copies the raw bytes of the native string but does NOT perform any additional
        /// transformation/encoding to a managed string. This allows more efficient use of the string with
        /// other native APIs. If the managed form of the string is needed it is obtainable via the <see cref="LazyEncodedString.ToString()"/>
        /// method on the returned value or by calling <see cref="ToString"/> instead.
        /// </remarks>
        public LazyEncodedString ToLazyEncodedString()
        {
            return LibLLVMGetMDStringText( Handle ) ?? LazyEncodedString.Empty;
        }

        /// <summary>Gets the string from the metadata node</summary>
        /// <returns>String this node wraps</returns>
        public override string ToString( )
        {
            return ToLazyEncodedString().ToString();
        }

        internal MDString( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
