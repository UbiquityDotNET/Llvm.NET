// -----------------------------------------------------------------------
// <copyright file="ContextHandleTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LlvmBindingsGenerator.Templates
{
    internal partial class ContextHandleTemplate
        : IHandleCodeTemplate
    {
        public ContextHandleTemplate( string name )
        {
            HandleName = name;
        }

        public string ToolVersion => GetType( ).Assembly.GetAssemblyInformationalVersion();

        public string HandleName { get; }

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public string Generate( ) => TransformText( );
    }
}
