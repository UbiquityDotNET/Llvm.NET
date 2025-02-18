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
    // templates in the form of GeneratorOutput. Therefore, ICodeGenerator
    // here is the functional equivalent to CppSharp's GeneratorOutput.
    internal class LibLlvmTemplateFactory
        : ICodeGeneratorTemplateFactory
    {
        public LibLlvmTemplateFactory( IGeneratorConfig config, ITypePrinter2 typePrinter )
        {
            HandleToTemplateMap = config.BuildTemplateMap( );
            TypePrinter = typePrinter;
        }

        public void SetupPasses( BindingContext bindingContext )
        {
            bindingContext.TranslationUnitPasses.AddPass( new CheckAbiParameters( ) );
        }

        public IEnumerable<ICodeGenerator> CreateTemplates( BindingContext bindingContext )
        {
            return CreateHandleTypeTemplates( bindingContext )
                    .Concat( CreatePerHeaderInterop( bindingContext ) )
                    .Concat( CreateStringMarshallingTemplates( ) )
                    .Concat( CreateMiscTemplates( bindingContext ) );
        }

        private static IEnumerable<ICodeGenerator> CreateMiscTemplates( BindingContext bindingContext )
        {
            yield return new TemplateCodeGenerator( "EXPORTS", Path.Combine( "..", "LibLLVM" ), [new ExportsTemplate( bindingContext.ASTContext )] );
        }

        private static IEnumerable<ICodeGenerator> CreateStringMarshallingTemplates( )
        {
            return from kind in GetEnumValues<StringDisposal>( )
                   where kind != StringDisposal.None // standard AnsiStringMarshaller is used; no custom marshaller is needed
                   let marshalInfo = StringDisposalMarshalerMap.LookupMarshaller( kind )
                   let template = new StringMarshallerTemplate( marshalInfo.Name, marshalInfo.NativeDisposer )
                   select new TemplateCodeGenerator( marshalInfo.Name, Path.Combine( GeneratedCodePath, "StringMarshaling" ), [template] );
        }

        private IEnumerable<ICodeGenerator> CreatePerHeaderInterop( BindingContext ctx )
        {
            return from tu in ctx.ASTContext.GeneratedUnits( )
                   let t4 = new PerHeaderInteropTemplate( tu, TypePrinter )
                   select new TemplateCodeGenerator( tu.IsValid
                                                   , tu.FileNameWithoutExtension
                                                   , tu.IncludePath == null ? GeneratedCodePath : Path.Combine( GeneratedCodePath, tu.FileRelativeDirectory )
                                                   , [t4]
                                                   );
        }

        private IEnumerable<ICodeGenerator> CreateHandleTypeTemplates( BindingContext ctx )
        {
            // filter out known handle types with non-templated implementations
            // LLVMErrorRef is rather unique with disposal and requires a distinct
            // implementation. (If the message is retrieved, the handle is destroyed,
            // and it is destroyed if "consumed" without getting the message.)
            var handles = from handle in ctx.ASTContext.GetHandleTypeDefs( )
                          where handle.Name != "LLVMErrorRef"
                          select handle;

            foreach( var handle in handles )
            {
                if( HandleToTemplateMap.TryGetValue( handle.Name, out IHandleCodeTemplate template ) )
                {
                    yield return new TemplateCodeGenerator( handle.Name, Path.Combine( GeneratedCodePath, "Handles" ), [template] );
                }
                else
                {
                    Diagnostics.Error( "No Mapping for handle type {0} - {1}@{2}", handle.Name, handle.TranslationUnit.FileRelativePath, handle.LineNumberStart );
                }
            }
        }

        private readonly HandleTemplateMap HandleToTemplateMap;
        private readonly ITypePrinter2 TypePrinter;

        private const string GeneratedCodePath = "GeneratedCode";
    }
}
