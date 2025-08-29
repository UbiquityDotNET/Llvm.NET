// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Generic and target specific flags for a symbol</summary>
    /// <param name="Generic">Generic options for this symbol</param>
    /// <param name="Target">Target specific flags for this symbol</param>
    public readonly record struct SymbolFlags( SymbolGenericOption Generic, byte Target )
    {
        /// <summary>Initializes a new instance of the <see cref="SymbolFlags"/> struct.</summary>
        /// <param name="genericOptions">Generic options for this symbol</param>
        public SymbolFlags( SymbolGenericOption genericOptions )
            : this( genericOptions, 0 )
        {
        }

        internal LLVMJITSymbolFlags ToABI( ) => new( (LLVMJITSymbolGenericFlags)Generic, Target );
    }
}
