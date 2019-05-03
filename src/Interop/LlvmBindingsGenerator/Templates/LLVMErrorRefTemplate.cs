// -----------------------------------------------------------------------
// <copyright file="LLVMErrorRefTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class LLVMErrorRefTemplate
        : IHandleCodeTemplate
    {
        public Version ToolVersion => GetType( ).Assembly.GetName( ).Version;

        public string HandleName => "LLVMErrorRef";

        public string FileExtension => "cs";

        public string Generate( ) => TransformText( );
    }
}
