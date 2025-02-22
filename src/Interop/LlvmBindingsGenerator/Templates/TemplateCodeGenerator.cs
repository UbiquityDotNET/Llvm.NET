// -----------------------------------------------------------------------
// <copyright file="GeneratorCodeTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LlvmBindingsGenerator.Templates
{
    internal class TemplateCodeGenerator
        : ICodeGenerator
    {
        public TemplateCodeGenerator(
            string fileNameWithoutExtension,
            string fileRelativeDirectory,
            ICodeGenTemplate template
            )
            : this( isValid: true, fileNameWithoutExtension, fileRelativeDirectory, template)
        {
        }

        public TemplateCodeGenerator(
            bool isValid,
            string fileNameWithoutExtension,
            string fileRelativeDirectory,
            ICodeGenTemplate template
            )
        {
            IsValid = isValid;
            FileNameWithoutExtension = fileNameWithoutExtension;
            FileRelativeDirectory = fileRelativeDirectory;
            Template = template;
        }

        public bool IsValid { get; }

        public string FileNameWithoutExtension { get; }

        public string FileRelativeDirectory { get; }

        public ICodeGenTemplate Template { get; }
    }
}
