// -----------------------------------------------------------------------
// <copyright file="LLVMErrorRefTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
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

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public string Generate( ) => TransformText( );
    }
}
