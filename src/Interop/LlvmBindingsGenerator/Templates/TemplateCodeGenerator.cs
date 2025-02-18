// -----------------------------------------------------------------------
// <copyright file="GeneratorCodeTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace LlvmBindingsGenerator.Templates
{
    internal class TemplateCodeGenerator
        : ICodeGenerator
    {
        public TemplateCodeGenerator(
            string fileNameWithoutExtension,
            string fileRelativeDirectory,
            IEnumerable<ICodeGenTemplate> templates
            )
            : this( true, fileNameWithoutExtension, fileRelativeDirectory, templates)
        {
        }

        public TemplateCodeGenerator(
            bool isValid,
            string fileNameWithoutExtension,
            string fileRelativeDirectory,
            IEnumerable<ICodeGenTemplate> templates
            )
        {
            IsValid = isValid;
            FileNameWithoutExtension = fileNameWithoutExtension;
            FileRelativeDirectory = fileRelativeDirectory;
            Templates = templates;
        }

        public bool IsValid { get; }

        public string FileNameWithoutExtension { get; }

        public string FileRelativeDirectory { get; }

        public IEnumerable<ICodeGenTemplate> Templates { get; }
    }
}
