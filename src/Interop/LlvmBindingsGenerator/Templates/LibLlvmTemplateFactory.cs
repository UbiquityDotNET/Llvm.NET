// -----------------------------------------------------------------------
// <copyright file="LibLlvmTemplateFactory.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using CppSharp;
using CppSharp.Generators;
using CppSharp.Passes;

using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.Templates;

using static LlvmBindingsGenerator.EnumExtensions;

namespace LlvmBindingsGenerator
{
    // Factory class for the templates needed in code generation
    // This is essentially a more flexible form of the CppSharp.CodeGenerator,
    // which, despite the name, doesn't generate code, but instead creates
    // templates in the form of GeneratorOutput. Therefore, IGeneratorCodeTemplate
    // here is the functional equivalent to CppSharp's GeneratorOutput.
    internal class LibLlvmTemplateFactory
        : ICodeGeneratorTemplateFactory
    {
        public LibLlvmTemplateFactory( IGeneratorConfig config )
        {
            HandleToTemplateMap = config.BuildTemplateMap( );
        }

        public void SetupPasses( BindingContext bindingContext )
        {
            bindingContext.TranslationUnitPasses.AddPass( new CheckAbiParameters( ) );
        }

        public IEnumerable<ICodeGenerator> CreateTemplates( BindingContext bindingContext )
        {
            return CreateHandleTypeTemplates( bindingContext )
                    .Concat( CreatePerHeaderInterop( bindingContext ) )
                    .Concat( CreateStringMarhsallingTemplates( ) )
                    .Concat( CreateMiscTemplates( bindingContext ) );
        }

        private static IEnumerable<ICodeGenerator> CreateMiscTemplates( BindingContext bindingContext )
        {
            yield return new TemplateCodeGenerator( true, "EXPORTS", Path.Combine( "..", "LibLLVM" ), new[ ] { new ExportsTemplate( bindingContext.ASTContext ) } );
        }

        private static IEnumerable<ICodeGenerator> CreateStringMarhsallingTemplates( )
        {
            foreach( StringDisposal kind in GetEnumValues<StringDisposal>( ) )
            {
                var (name, nativeDisposer) = StringDisposalMarshalerMap.LookupMarshaler( kind );
                var t4Template = new StringMarshalerTemplate( name, nativeDisposer );
                yield return new TemplateCodeGenerator( true, name, Path.Combine( GeneratedCodePath, "StringMarshaling" ), new[ ] { t4Template } );
            }
        }

        private static IEnumerable<ICodeGenerator> CreatePerHeaderInterop( BindingContext ctx )
        {
            foreach( var tu in ctx.ASTContext.GeneratedUnits( ) )
            {
                var t4Template = new PerHeaderInteropTemplate( tu );
                var t4XmlDocsTemplate = new ExternalDocXmlTemplate( tu );
                yield return new TemplateCodeGenerator( tu.IsValid
                                                      , tu.FileNameWithoutExtension
                                                      , tu.IncludePath == null ? GeneratedCodePath : Path.Combine( GeneratedCodePath, tu.FileRelativeDirectory )
                                                      , new ICodeGenTemplate[ ] { t4Template, t4XmlDocsTemplate }
                                                      );
            }
        }

        private IEnumerable<ICodeGenerator> CreateHandleTypeTemplates( BindingContext ctx )
        {
            // filter out known handle types with non-templated implementations
            var handles = from handle in ctx.ASTContext.GetHandleTypeDefs( )
                          where handle.Name != "LLVMErrorRef"
                          select handle;

            foreach( var handle in handles )
            {
                if( HandleToTemplateMap.TryGetValue( handle.Name, out IHandleCodeTemplate template ) )
                {
                    yield return new TemplateCodeGenerator( true, handle.Name, Path.Combine( GeneratedCodePath, "Handles" ), new[ ] { template } );
                }
                else
                {
                    Diagnostics.Error( "No Mapping for handle type {0} - {1}@{2}", handle.Name, handle.TranslationUnit.FileRelativePath, handle.LineNumberStart );
                }
            }
        }

        private readonly HandleTemplateMap HandleToTemplateMap;

        private const string GeneratedCodePath = "GeneratedCode";
    }
}
