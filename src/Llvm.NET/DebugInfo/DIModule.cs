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
        public override string Name => GetOperandString( 1 );

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
