// -----------------------------------------------------------------------
// <copyright file="GeneratorCodeTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal class TemplateCodeGenerator
        : ICodeGenerator
    {
        public TemplateCodeGenerator( TranslationUnit unit, IEnumerable<ICodeGenTemplate> templates )
            : this( unit.IsValid,
                    unit.FileNameWithoutExtension,
                    unit.IncludePath == null ? string.Empty : unit.FileRelativeDirectory,
                    templates )
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
