// -----------------------------------------------------------------------
// <copyright file="LibLlvmTemplateFactory.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CppSharp;
using CppSharp.Generators;
using CppSharp.Passes;
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
        : IGeneratorCodeTemplateFactory
    {
        public LibLlvmTemplateFactory( HandleTemplateMap map )
        {
            HandleToTemplateMap = map;
        }

        public void SetupPasses( BindingContext bindingContext )
        {
            bindingContext.TranslationUnitPasses.AddPass( new CheckAbiParameters( ) );
        }

        public IEnumerable<IGeneratorCodeTemplate> CreateTemplates( BindingContext bindingContext )
        {
            return CreateHandleTypeTemplates( bindingContext )
                    .Concat( CreatePerHeaderInterop( bindingContext ) )
                    .Concat( CreateStringMarhsallingTemplates( ) )
                    .Concat( CreateMiscTemplates( bindingContext ) );
        }

        private IEnumerable<IGeneratorCodeTemplate> CreateMiscTemplates( BindingContext bindingContext )
        {
            yield return new GeneratorCodeTemplate( true, "EXPORTS", @"..\LibLLVM", new[ ] { new ExportsTemplate( bindingContext.ASTContext ) } );
        }

        private IEnumerable<IGeneratorCodeTemplate> CreateStringMarhsallingTemplates( )
        {
            foreach( StringDisposal kind in GetEnumValues<StringDisposal>())
            {
                var (name, nativeDisposer) = StringDisposalMarshalerMap.LookupMarshaler( kind );
                var t4Template = new StringMarshalerTemplate( name, nativeDisposer );
                yield return new GeneratorCodeTemplate( true, name, "StringMarshaling", new[ ] { t4Template } );
            }
        }

        private IEnumerable<IGeneratorCodeTemplate> CreatePerHeaderInterop( BindingContext ctx )
        {
            foreach( var tu in ctx.ASTContext.GeneratedUnits( ) )
            {
                var t4Template = new PerHeaderInteropTemplate( tu );
                yield return new GeneratorCodeTemplate( tu, new[ ] { t4Template } );
            }
        }

        private IEnumerable<IGeneratorCodeTemplate> CreateHandleTypeTemplates( BindingContext ctx )
        {
            var handles = ctx.ASTContext.GetHandleTypeDefs( );
            foreach( var handle in handles )
            {
                if( HandleToTemplateMap.TryGetValue( handle.Name, out IHandleCodeTemplate template ) )
                {
                    yield return new GeneratorCodeTemplate( true, handle.Name, "Handles", new[ ] { template } );
                }
                else
                {
                    Diagnostics.Error( "No Mapping for handle type {0}", handle.Name );
                }
            }
        }

        private readonly HandleTemplateMap HandleToTemplateMap;
    }
}
