// -----------------------------------------------------------------------
// <copyright file="LibLlvmGeneratorLibrary.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using CppSharp.AST;

using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.Passes;
using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    /// <summary>ILibrary implementation for the Ubiquity.NET.Llvm Interop</summary>
    /// <remarks>
    /// This class provides the library specific bridging from the generalized
    /// CppSharp infrastructure for the specific needs of the Ubiquity.NET.Llvm.Interop library
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
        public LibLlvmGeneratorLibrary( IGeneratorConfig configuration, string llvmRoot, string extensionsRoot, string outputPath )
        {
            Configuration = configuration;
            CommonInclude = Path.Combine( llvmRoot, "include" );
            ArchInclude = Path.Combine( llvmRoot, "x64-Release", "include" );
            ExtensionsInclude = Path.Combine( extensionsRoot, "include" );
            OutputPath = Path.GetFullPath( outputPath );
            InternalTypePrinter = new LibLLVMTypePrinter();

            // Hook in the custom type printer via static delegate
            Type.TypePrinterDelegate = t => InternalTypePrinter.GetName( t, TypeNameKind.Native );
        }

        public void Setup( IDriver driver )
        {
            Driver = driver;
            driver.Options.UseHeaderDirectories = true;
            driver.Options.GenerationOutputMode = CppSharp.GenerationOutputMode.FilePerUnit;

            driver.Options.OutputDir = OutputPath;

            driver.ParserOptions.SetupMSVC();
            driver.ParserOptions.AddIncludeDirs( CommonInclude );
            driver.ParserOptions.AddIncludeDirs( ArchInclude );
            driver.ParserOptions.AddIncludeDirs( ExtensionsInclude );

            var coreHeaders = Directory.EnumerateFiles( Path.Combine( CommonInclude, "llvm-c" ), "*.h", SearchOption.AllDirectories );
            var extHeaders = Directory.EnumerateFiles( Path.Combine( ExtensionsInclude, "libllvm-c" ), "*.h", SearchOption.AllDirectories );
            var module = driver.Options.AddModule( "Ubiquity.NET.Llvm.Interop" );
            module.Headers.AddRange( coreHeaders );
            module.Headers.AddRange( extHeaders );
        }

        public void SetupPasses( )
        {
            // Analysis passes that markup, but don't otherwise modify the AST run first
            // always start the passes with the IgnoreSystemHeaders pass to ensure that
            // transformation only occurs for the desired headers. Other passes depend on
            // TranslationUnit.IsGenerated to ignore headers.
            Driver!.AddTranslationUnitPass( new IgnoreSystemHeadersPass( Configuration.IgnoredHeaders ) );
            Driver!.AddTranslationUnitPass( new IgnoreDuplicateNamesPass( ) );

            // modifying pass(es)
            Driver!.AddTranslationUnitPass( new MarkFunctionsInternalPass( ) );

            // Verification/sanity checking passes
            Driver!.AddTranslationUnitPass( new ValidateExtensionNamingPass( ) );
        }

        public void Preprocess( ASTContext ctx )
        {
            // purge all the CppSharp type mapping to prevent any conversions/mapping
            // Only the raw source should be in the AST until later stages adjust it.
            Driver!.Context.TypeMaps.TypeMaps.Clear();
            InternalTypePrinter.Context = Driver.Context;
        }

        public void Postprocess( ASTContext ctx )
        {
        }

        public IEnumerable<ICodeGenerator> CreateGenerators( )
        {
            var templateFactory = new LibLlvmTemplateFactory( Configuration );
            return templateFactory.CreateTemplates( Driver!.Context );
        }

        private IDriver? Driver;
        private readonly LibLLVMTypePrinter InternalTypePrinter;
        private readonly IGeneratorConfig Configuration;
        private readonly string CommonInclude;
        private readonly string ArchInclude;
        private readonly string ExtensionsInclude;
        private readonly string OutputPath;
    }
}
