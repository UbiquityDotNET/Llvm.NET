// -----------------------------------------------------------------------
// <copyright file="GeneratorCodeTemplate.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal class GeneratorCodeTemplate
        : IGeneratorCodeTemplate
    {
        public GeneratorCodeTemplate( TranslationUnit unit, IEnumerable<ICodeGenTemplate> templates )
            : this( unit.IsValid,
                    unit.FileNameWithoutExtension,
                    unit.IncludePath == null ? string.Empty : unit.FileRelativeDirectory,
                    templates )
        {
        }

        public GeneratorCodeTemplate(
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
