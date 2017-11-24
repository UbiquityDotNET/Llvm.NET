// <copyright file="DIModule.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Native;

namespace Llvm.NET.DebugInfo
{
    /// <summary>A (Clang) module that is imported by a compile unit</summary>
    public class DIModule
        : DIScope
    {
        /// <inheritdoc/>
        public override DIScope Scope => GetOperand<DIScope>( 0 );

        /// <inheritdoc/>
        public override string Name => GetOperand<MDString>( 1 ).ToString( );

        /// <summary>Gets the configuration macros for the module</summary>
        public string ConfigurationMacros => GetOperand<MDString>( 2 ).ToString( );

        /// <summary>Gets the include path for the module</summary>
        public string IncludePath => GetOperand<MDString>( 3 ).ToString( );

        /// <summary>Gets the ISysRoot for the module</summary>
        public string SysRoot => GetOperand<MDString>( 4 ).ToString( );

        internal DIModule( LLVMMetadataRef handle )
            : base( handle )
        {
        }
    }
}
