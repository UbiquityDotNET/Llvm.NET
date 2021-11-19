// -----------------------------------------------------------------------
// <copyright file="ExternalDocXmlTemplate.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using CppSharp.AST;

namespace LlvmBindingsGenerator.Templates
{
    internal partial class ExternalDocXmlTemplate
        : ICodeGenTemplate
    {
        public ExternalDocXmlTemplate( TranslationUnit tu, ITypePrinter2 typePrinter )
        {
            Unit = tu;
            TypePrinter = typePrinter;
        }

        public string ToolVersion => GetType( ).Assembly.GetAssemblyInformationalVersion( );

        public string FileExtension => "xml";

        public string SubFolder => "GeneratedDocs";

        public string Generate( )
        {
            return TransformText( );
        }

        public IEnumerable<ParsedEnum> Enums
            => from e in Unit.Enums
               select new ParsedEnum( e );

        public IEnumerable<ParsedFunctionSignature> Functions
            => from f in Unit.Functions
               where f.IsGenerated
               select new ParsedFunctionSignature( f, TypePrinter );

        public IEnumerable<ParsedFunctionSignature> Delegates
            => from td in Unit.Typedefs
               where td.IsDelegateTypeDef( )
               select new ParsedFunctionSignature( td, TypePrinter );

        public IEnumerable<Class> ValueTypes
            => from cls in Unit.Classes
               where cls.IsValueType
               select cls;

        private readonly TranslationUnit Unit;
        private readonly ITypePrinter2 TypePrinter;
    }
}
