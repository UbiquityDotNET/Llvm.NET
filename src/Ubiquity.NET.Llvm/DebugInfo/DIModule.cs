// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.Llvm.DebugInfo
{
    /// <summary>A source module that is imported by a compile unit</summary>
    public class DIModule
        : DIScope
    {
        /// <inheritdoc/>
        public override DIScope? Scope => GetOperand<DIScope>( 0 );

        /// <inheritdoc/>
        public override LazyEncodedString Name => GetOperandString( 1 );

        /// <summary>Gets the configuration macros for the module</summary>
        public string ConfigurationMacros => GetOperandString( 2 );

        /// <summary>Gets the include path for the module</summary>
        public string IncludePath => GetOperandString( 3 );

        /// <summary>Gets the ISysRoot for the module</summary>
        public string SysRoot => GetOperandString( 4 );

        internal DIModule( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
