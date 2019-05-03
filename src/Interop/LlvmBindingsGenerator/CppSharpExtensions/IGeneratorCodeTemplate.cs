// -----------------------------------------------------------------------
// <copyright file="IGeneratorCodeTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CppSharp.Generators;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    internal interface IGeneratorCodeTemplate
    {
        bool IsValid { get; }

        string FileNameWithoutExtension { get; }

        string FileRelativeDirectory { get; }

        IEnumerable<ICodeGenTemplate> Templates { get; }
    }

    internal interface IGeneratorCodeTemplateFactory
    {
        IEnumerable<IGeneratorCodeTemplate> CreateTemplates( BindingContext bindingContext );

        void SetupPasses( BindingContext bindingContext );
    }
}
