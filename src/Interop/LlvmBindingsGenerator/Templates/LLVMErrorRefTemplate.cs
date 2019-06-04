// -----------------------------------------------------------------------
// <copyright file="LLVMErrorRefTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LlvmBindingsGenerator.Templates
{
    internal partial class LLVMErrorRefTemplate
        : IHandleCodeTemplate
    {
        public string ToolVersion => GetType( ).Assembly.GetAssemblyInformationalVersion( );

        public string HandleName => "LLVMErrorRef";

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public string Generate( ) => TransformText( );
    }
}
