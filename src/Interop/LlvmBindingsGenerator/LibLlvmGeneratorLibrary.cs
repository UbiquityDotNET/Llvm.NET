// -----------------------------------------------------------------------
// <copyright file="LibLlvmGeneratorLibrary.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;
using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.Passes;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    /// <summary>ILibrary implementation for the Lllvm.NET Interop</summary>
    /// <remarks>
    /// This class provides the library specific bridging from the general
    /// CppSharp infrastructure for the specific needs of the Llvm.NET.Interop library
    /// </remarks>
    internal class LibLlvmGeneratorLibrary
        : ILibrary
    {
        /// <summary>Initializes a new instance of the <see cref="LibLlvmGeneratorLibrary"/> class.</summary>
        /// <param name="llvmRoot">Root path to standard LLVM source</param>
        /// <param name="outputPath">Output path of the generated code files</param>
        /// <remarks>
        /// The <paramref name="llvmRoot"/> only needs to have the files required to parse the LLVM-C API
        /// </remarks>
        public LibLlvmGeneratorLibrary( GeneratorConfig configuration, string llvmRoot, string extensionsRoot, string outputPath )
        {
            Configuration = configuration;
            CommonInclude = Path.Combine( llvmRoot, "include" );
            ArchInclude = Path.Combine( llvmRoot, "x64-Release", "include" );
            ExtensionsInclude = Path.Combine( extensionsRoot, "include" );
            OutputPath = Path.GetFullPath( outputPath );
        }

        public void Setup( Driver driver )
        {
            // ALWAYS DryRun to disable CppSharp standard code generation as it isn't flexible enough to do what is needed for this lib
            // Instead, the PostProcess function will do the generation based on the ASTContext and T4 templates
            driver.Options.DryRun = true;
            driver.Options.Quiet = true;
            driver.Options.UseHeaderDirectories = true;
            driver.Options.GenerateSingleCSharpFile = false;

            driver.Options.OutputDir = OutputPath;
            driver.ParserOptions.AddIncludeDirs( CommonInclude );
            driver.ParserOptions.AddIncludeDirs( ArchInclude );
            driver.ParserOptions.AddIncludeDirs( ExtensionsInclude );

            var coreHeaders = Directory.EnumerateFiles( Path.Combine( CommonInclude, "llvm-c" ), "*.h", SearchOption.AllDirectories );
            var extHeaders = Directory.EnumerateFiles( Path.Combine( ExtensionsInclude, "libllvm-c"), "*.h", SearchOption.AllDirectories );
            var module = driver.Options.AddModule( "Llvm.NET.Interop" );
            module.Headers.AddRange( coreHeaders );
            module.Headers.AddRange( extHeaders );
        }

        public void SetupPasses( Driver driver )
        {
            // Don't setup pases here, need to wait until driver has added all standard passes to remove them to completely control the passes
        }

        public void Preprocess( Driver driver, ASTContext ctx )
        {
            // remove all the default passes so we can control the exact set of passes used
            driver.Context.TranslationUnitPasses.Passes.Clear( );

            // always start the passes with the IgnoreSystemHeaders pass to ensure that generation
            // only occurs for the desired headers.
            driver.AddTranslationUnitPass( new IgnoreSystemHeaders( ) );
            driver.AddTranslationUnitPass( new CheckFlagEnumsPass( ) );
            driver.AddTranslationUnitPass( new DeAnonymizeEnumsPass( Configuration.AnonymousEnumNames ) );
            driver.AddTranslationUnitPass( new MapHandleTypeDefsPass( ) );
            driver.AddTranslationUnitPass( new PODToValueTypePass( ) );
            driver.AddTranslationUnitPass( new RemoveDuplicateNamesPass( ) );
            driver.AddTranslationUnitPass( new AddMissingParameterNamesPass( ) );
            driver.AddTranslationUnitPass( new MarkFunctionsIgnoredPass( Configuration.HandleToTemplateMap.DisposeFunctionNames.Concat( Configuration.IgnoredFunctions ) ) );
            driver.AddTranslationUnitPass( new MarkDeprecatedFunctionsAsObsoletePass( Configuration.DeprecatedFunctionToMessageMap, true ) );
            driver.AddTranslationUnitPass( new AddMarshalingAttributesPass( Configuration.MarshalingInfo ) );
            driver.AddTranslationUnitPass( new ConvertLLVMBoolPass( Configuration.StatusReturningFunctions ) );
        }

        public void Postprocess( Driver driver, ASTContext ctx )
        {
            // Don't rely on CppSHarp built-in code generation as it doesn't handle the needs of this library well.
            GenerateCode( driver );
        }

        private void GenerateCode( Driver driver )
        {
            string outputPath = Path.GetFullPath( driver.Options.OutputDir );

            if( !Directory.Exists( outputPath ) )
            {
                Directory.CreateDirectory( outputPath );
            }

            var templateFactory = new LibLlvmTemplateFactory( Configuration.HandleToTemplateMap );
            var templates = templateFactory.CreateTemplates( driver.Context );

            foreach( IGeneratorCodeTemplate output in templates.Where( o => o.IsValid ) )
            {
                string fileBase = output.FileNameWithoutExtension;

                if( driver.Options.UseHeaderDirectories )
                {
                    string dir = Path.Combine( outputPath, output.FileRelativeDirectory );
                    Directory.CreateDirectory( dir );
                    fileBase = Path.Combine( output.FileRelativeDirectory, fileBase );
                }

                foreach( ICodeGenTemplate template in output.Templates )
                {
                    string fileRelativePath = $"{fileBase}.{template.FileExtension}";

                    string file = Path.Combine( outputPath, fileRelativePath );
                    File.WriteAllText( file, template.Generate( ) );

                    Diagnostics.Message( "Generated '{0}'", fileRelativePath );
                }
            }
        }

        private readonly GeneratorConfig Configuration;
        private readonly string CommonInclude;
        private readonly string ArchInclude;
        private readonly string ExtensionsInclude;
        private readonly string OutputPath;
    }
}
