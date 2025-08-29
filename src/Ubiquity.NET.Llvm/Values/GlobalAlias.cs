// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.Values
{
    /// <summary>LLVM Global Alias for a function or global value</summary>
    public class GlobalAlias
        : GlobalIndirectSymbol
    {
        /// <summary>Gets or sets the aliasee that this Alias refers to</summary>
        [DisallowNull]
        public Constant Aliasee
        {
            get => IndirectSymbol!;
            set => IndirectSymbol = value.ThrowIfNull();
        }

        internal GlobalAlias( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
