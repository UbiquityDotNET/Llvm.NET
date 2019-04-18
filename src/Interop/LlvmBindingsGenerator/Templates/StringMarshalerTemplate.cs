// -----------------------------------------------------------------------
// <copyright file="StringMarshalerTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class StringMarshalerTemplate
        : ICodeGenTemplate
    {
        public StringMarshalerTemplate( string name )
            : this( name, string.Empty)
        {
        }

        public StringMarshalerTemplate( string name, string disposeFunctionName)
        {
            Name = name;
            NativeDisposer = disposeFunctionName;
        }

        public bool HasNativeDisposer => !string.IsNullOrEmpty( NativeDisposer );

        public string Name { get; }

        public string NativeDisposer { get; }

        public Version ToolVersion => GetType( ).Assembly.GetName( ).Version;

        public string FileExtension => "cs";

        public string Generate( )
        {
            return TransformText( );
        }
    }
}
