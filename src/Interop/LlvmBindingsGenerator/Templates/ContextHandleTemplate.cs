// -----------------------------------------------------------------------
// <copyright file="ContextHandleTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class ContextHandleTemplate
        : IHandleCodeTemplate
    {
        public ContextHandleTemplate( string name )
        {
            HandleName = name;
        }

        public Version ToolVersion => GetType( ).Assembly.GetName( ).Version;

        public string HandleName { get; }

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public string Generate( ) => TransformText( );
    }
}
