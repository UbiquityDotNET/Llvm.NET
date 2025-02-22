// -----------------------------------------------------------------------
// <copyright file="IGeneratorCodeTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using CppSharp.Generators;

using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    internal interface ICodeGenerator
    {
        bool IsValid { get; }

        string FileNameWithoutExtension { get; }

        string FileRelativeDirectory { get; }

        ICodeGenTemplate Template { get; }
    }

    internal interface ICodeGeneratorTemplateFactory
    {
        IEnumerable<ICodeGenerator> CreateTemplates( BindingContext bindingContext );

        void SetupPasses( BindingContext bindingContext );
    }
}
