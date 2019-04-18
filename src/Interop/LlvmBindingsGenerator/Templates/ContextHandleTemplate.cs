// -----------------------------------------------------------------------
// <copyright file="ContextHandleTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
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

        public string FileExtension => "cs";

        public string Generate( ) => TransformText( );
    }
}
