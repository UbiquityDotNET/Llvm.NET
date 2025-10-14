// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.CppSharpExtensions;

namespace LlvmBindingsGenerator.Templates
{
    // Manually maintained partial of the T4 Template generated type
    // This provides the support needed by the "code" in the template.
    // General idea is to minimize the "logic" in the T4 template to simple conditional
    // expressions and property accessors. This keeps the template as simple as possible
    // (they can get unreadable/unmaintainable really fast).
    internal partial class WrappedHandleTemplate
        : IHandleCodeTemplate
    {
        public WrappedHandleTemplate( HandleDetails details, bool isAlias = false )
        {
            if(isAlias && !details.Alias)
            {
                throw new ArgumentException("Only a handle that supports an alias can be generated as an alias type", nameof(isAlias));
            }

            Details = details;
            IsAlias = isAlias;
        }

        public string ToolVersion => GetType( ).Assembly.GetAssemblyInformationalVersion( );

        public string HandleName => IsAlias ? $"{Details.Name}Alias" : Details.Name;

        public bool IsDisposable => Details.Disposer is not null && !IsAlias;

        public string? HandleDisposeFunction => Details.Disposer;

        /// <summary>Gets a value indicating whether this handle <em><b>has</b></em> an alias form</summary>
        public bool HasAlias => Details.Alias && !IsAlias;

        /// <summary>Gets a value indicating whether this handle <em><b>is</b></em> an alias form</summary>
        public bool IsAlias { get; }

        public string FileExtension => "g.cs";

        public string SubFolder => string.Empty;

        public string Generate( ) => TransformText( );

        private readonly HandleDetails Details;
    }
}
