// -----------------------------------------------------------------------
// <copyright file="StringMarshalerTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LlvmBindingsGenerator.Templates
{
    internal partial class StringMarshalerTemplate
        : ICodeGenTemplate
    {
        public StringMarshalerTemplate( string name, string disposeFunctionName)
        {
            Name = name;
            NativeDisposer = disposeFunctionName;
        }

        public bool HasNativeDisposer => !string.IsNullOrEmpty( NativeDisposer );

        public string Name { get; }

        public string NativeDisposer { get; }

        public string ToolVersion => GetType( ).Assembly.GetAssemblyInformationalVersion( );

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public string Generate( )
        {
            return TransformText( );
        }
    }
}
