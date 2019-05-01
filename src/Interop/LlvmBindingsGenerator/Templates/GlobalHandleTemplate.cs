// -----------------------------------------------------------------------
// <copyright file="GlobalHandleTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class GlobalHandleTemplate
        : IHandleCodeTemplate
    {
        public GlobalHandleTemplate( string name, string disposerFunctionName, bool needsAlias = false )
        {
            HandleName = name;
            HandleDisposeFunction = disposerFunctionName;
            NeedsAlias = needsAlias;
        }

        public Version ToolVersion => GetType( ).Assembly.GetName( ).Version;

        public string HandleName { get; }

        public string HandleDisposeFunction { get; }

        public string FileExtension => "cs";

        public bool NeedsAlias { get; }

        public string Generate( ) => TransformText( );
    }
}
