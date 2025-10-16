// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

using CppSharp.AST;

using LlvmBindingsGenerator.Configuration;
using LlvmBindingsGenerator.Passes;

namespace LlvmBindingsGenerator
{
    /// <summary>ILibrary implementation for the LlvmBindingsGenerator</summary>
    /// <remarks>
    /// This class provides the library specific bridging from the generalized
    /// CppSharp infrastructure for the specific needs of the LlvmBindingsGenerator.
    /// </remarks>
    internal class LibLlvmGeneratorLibrary
        : ILibrary
    {
        /// <summary>Initializes a new instance of the <see cref="LibLlvmGeneratorLibrary"/> class.</summary>
        /// <param name="options">Command line options to use</param>
        public LibLlvmGeneratorLibrary( CmdLineArgs options )
        {
            CmdLineOptions = options;

            CommonInclude = Path.Combine( options.LlvmRoot.FullName, "include" );

            // NOTE: The target specific LLVMInitializeXXX APIs are included in CMAKE generated headers
            // This is OK as they should NOT appear in the set of APIs exported from the library anyway
            // Instead LibLLVM handles all target registration in an extended abstract API LibLLVMRegisterTarget(...)
            // that handles the behavior for the supported targets of that binary's build.
            ExtensionsInclude = Path.Combine( options.ExtensionsRoot.FullName, "include" );

            InternalTypePrinter = new LibLLVMTypePrinter();

            // Hook in the custom type printer via static delegate
            Type.TypePrinterDelegate = t => InternalTypePrinter.GetName( t, TypeNameKind.Native );
        }

        public void Setup( IDriver driver )
        {
            Driver = driver;
            driver.Options.UseHeaderDirectories = true;
            driver.Options.GenerationOutputMode = CppSharp.GenerationOutputMode.FilePerUnit;

            // For generating the EXPORTS or the more generic handle types force use of MSVC settings
            // For non-Windows platforms it won't matter anyway as the exports.def isn't used and the
            // generated handle code is platform independent.
            driver.ParserOptions.SetupMSVC();

            // Workaround: https://github.com/mono/CppSharp/issues/1940
            // This isn't generating native code so the compiler differences for STL
            // aren't important. This define has no impact on non-MSVC libraries.
            // If such a conflict is ever determined, this is either hopefully fixed
            // in the dependent library OR this code needs to set this conditionally.
            driver.ParserOptions.AddDefines("_ALLOW_COMPILER_AND_STL_VERSION_MISMATCH");

            driver.ParserOptions.AddIncludeDirs( CommonInclude );
            driver.ParserOptions.AddIncludeDirs( ExtensionsInclude );

            var coreHeaders = Directory.EnumerateFiles( Path.Combine( CommonInclude, "llvm-c" ), "*.h", SearchOption.AllDirectories );
            var extHeaders = Directory.EnumerateFiles( Path.Combine( ExtensionsInclude, "libllvm-c" ), "*.h", SearchOption.AllDirectories );

            // Library used needs a module and name, even if it isn't used...
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
            return templateFactory.CreateTemplates( Driver!.Context, CmdLineOptions );
        }

        private IDriver? Driver;
        private readonly LibLLVMTypePrinter InternalTypePrinter;

        // in this version of the generator, the configuration is hard coded and not read from a file
        private readonly GeneratorConfig Configuration = new();

        private readonly string CommonInclude;
        private readonly string ExtensionsInclude;
        private readonly CmdLineArgs CmdLineOptions;
    }
}
