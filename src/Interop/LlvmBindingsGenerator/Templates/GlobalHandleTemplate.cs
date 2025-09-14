// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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

        public string ToolVersion => GetType( ).Assembly.GetAssemblyInformationalVersion( );

        public string HandleName { get; }

        public string HandleDisposeFunction { get; }

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public bool NeedsAlias { get; }

        public string Generate( ) => TransformText( );
    }
}
