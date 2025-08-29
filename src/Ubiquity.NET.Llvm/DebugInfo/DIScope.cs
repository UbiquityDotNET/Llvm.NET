// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>Base class for all Debug information scopes</summary>
    public class DIScope
        : DINode
    {
        /// <summary>Gets the <see cref="DIFile"/> describing the file this scope belongs to</summary>
        /// <remarks>If this scope is a <see cref="DIFile"/> then this returns <see langword="this"/></remarks>
        [SuppressMessage( "Style", "IDE0046:Convert to conditional expression", Justification = "Multi leveled conditional operators are NOT simpler!" )]
        public DIFile? File
        {
            get
            {
                if(Handle == default)
                {
                    return null;
                }

                return this is DIFile file
                     ? file
                     : (DIFile?)LLVMDIScopeGetFile( Handle ).CreateMetadata();
            }
        }

        /// <summary>Gets the parent scope for this scope or <see langword="null"/> if no parent scope exists</summary>
        public virtual DIScope? Scope => null;

        /// <summary>Gets the name of the scope or an empty string if the scope doesn't have a name</summary>
        public virtual LazyEncodedString Name { get; } = LazyEncodedString.Empty;

        private protected DIScope( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
