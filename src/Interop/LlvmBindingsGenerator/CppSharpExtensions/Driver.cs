// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Parser;
using CppSharp.Passes;
using CppSharp.Types;
using CppSharp.Utils;

using ClangParser = CppSharp.ClangParser;

namespace LlvmBindingsGenerator
{
    /// <summary>Provides a more flexible implementation of the general "driver" concept in CppSharp</summary>
    /// <remarks>
    /// Notable differences:
    /// 1. The driver does not setup ANY passes, it is entirely up to the library to do that
    /// 2. There is no intermediate "Generator" needed
    /// 3. Deals in interfaces rather than concrete types so it is more extensible
    /// 4. The code generation types are interfaces with names that more accurately reflect what they do
    /// </remarks>
    internal sealed class Driver
        : IDriver
        , IDisposable
    {
        public Driver()
            : this( new DriverOptions() )
        {
        }

        public Driver( DriverOptions options )
        {
            Options = options;
            ParserOptions = new CppSharp.Parser.ParserOptions();
            Context = new BindingContext( Options, ParserOptions );
        }

        public DriverOptions Options { get; }

        public CppSharp.Parser.ParserOptions ParserOptions { get; }

        public BindingContext Context { get; private set; }

        public void SetupTypeMaps() =>
            Context.TypeMaps = new TypeMapDatabase( Context );

        public void Setup()
        {
            ValidateOptions();
            ParserOptions.Setup(TargetPlatform.Windows);
        }

        public bool ParseCode()
        {
            ClangParser.SourcesParsed += OnSourceFileParsed;

            var files = Options.Modules.SelectMany( m => m.Headers );
            ParserOptions.BuildForSourceFile( Options.Modules );

            using( CppSharp.Parser.ParserResult result = ClangParser.ParseSourceFiles( files, ParserOptions ) )
            {
                Context.TargetInfo = result.TargetInfo;
            }

            Context.ASTContext = ClangParser.ConvertASTContext( ParserOptions.ASTContext );
            ClangParser.SourcesParsed -= OnSourceFileParsed;
            return !HasParsingErrors;
        }

        public void SortModulesByDependencies()
        {
            var sortedModules = Options.Modules.TopologicalSort( GetAndAddDependencies );
            Options.Modules.Clear();
            Options.Modules.AddRange( sortedModules );
        }

        public bool ParseLibraries()
        {
            ClangParser.LibraryParsed += OnFileParsed;
            foreach( Module module in Options.Modules )
            {
                using var linkerOptions = new LinkerOptions(Context.LinkerOptions);
                foreach( string libraryDir in module.LibraryDirs )
                {
                    linkerOptions.AddLibraryDirs( libraryDir );
                }

                foreach( string library in module.Libraries )
                {
                    if( !Context.Symbols.Libraries.Any( l => l.FileName == library ) )
                    {
                        linkerOptions.AddLibraries( library );
                    }
                }

                using ParserResult parserResult = ClangParser.ParseLibrary(linkerOptions);
                if( parserResult.Kind == ParserResultKind.Success )
                {
                    for( uint num = 0u; num < parserResult.LibrariesCount; num++ )
                    {
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                        // This is "cloned" from CppSharp code and it is assumed to transfer ownership
                        // documentation is sadly lacking on most of this library
                        Context.Symbols.Libraries.Add( ClangParser.ConvertLibrary( parserResult.GetLibraries( num ) ) );
#pragma warning restore IDISP004 // Don't ignore created IDisposable
                    }
                }
            }

            ClangParser.LibraryParsed -= OnFileParsed;
            Context.Symbols.IndexSymbols();
            SortModulesByDependencies();
            return true;
        }

        public void ProcessCode()
        {
            Context.RunPasses();
        }

        public void GenerateCode( IEnumerable<ICodeGenerator> generators )
        {
            string defaultOutputPath = Path.GetFullPath( Options.OutputDir );

            if( !Directory.Exists( defaultOutputPath ) )
            {
                Directory.CreateDirectory( defaultOutputPath );
            }

            foreach( ICodeGenerator generator in generators.Where( o => o.IsValid ) )
            {
                string generatorOutputPath = generator.FileAbsolutePath ?? defaultOutputPath;

                if( Options.UseHeaderDirectories && generator.FileRelativeDirectory is not null)
                {
                    generatorOutputPath = Path.Combine( generatorOutputPath, generator.FileRelativeDirectory );
                    Directory.CreateDirectory( generatorOutputPath );
                }

                try
                {
                    string templateOutputPath = generatorOutputPath;
                    if( !string.IsNullOrWhiteSpace( generator.Template.SubFolder ) )
                    {
                        templateOutputPath = Path.Combine( templateOutputPath, generator.Template.SubFolder );
                        Directory.CreateDirectory( templateOutputPath );
                    }

                    if( generator.FileAbsolutePath is null)
                    {
                        string fileName = $"{generator.FileNameWithoutExtension}.{generator.Template.FileExtension}";
                        string fullFilePath = Path.Combine( templateOutputPath, fileName );
                        File.WriteAllText( fullFilePath, generator.Template.Generate() );
                        Diagnostics.Message( "Generated '{0}'", fullFilePath );
                    }
                    else
                    {
                        File.WriteAllText( generator.FileAbsolutePath, generator.Template.Generate() );
                        Diagnostics.Message( "Generated '{0}'", generator.FileAbsolutePath );
                    }
                }
                catch( System.IO.IOException ex )
                {
                    Diagnostics.Error( ex.Message );
                }
            }
        }

        public void Dispose()
        {
            Context?.TargetInfo?.Dispose();

            ParserOptions.Dispose();
            CppSharp.AST.Type.TypePrinterDelegate = null;
        }

        public static void Run( ILibrary library )
        {
            using var driver = new Driver();
            var options = driver.Options;
            library.Setup( driver );
            driver.Setup();

            Diagnostics.Message( "Parsing libraries..." );
            if( !driver.ParseLibraries() )
            {
                return;
            }

            Diagnostics.Message( "Parsing code..." );
            if( !driver.ParseCode() )
            {
                Diagnostics.Error( "Encountered one or more errors while parsing source code - no code generation performed." );
                return;
            }

            new CleanUnitPass { Context = driver.Context }.VisitASTContext( driver.Context.ASTContext );
            options.Modules.RemoveAll( m => m != options.SystemModule && !m.Units.GetGenerated().Any() );

            Diagnostics.Message( "Processing code..." );

            library.SetupPasses();
            driver.SetupTypeMaps();

            library.Preprocess( driver.Context.ASTContext );

            if( CppSharp.AST.Type.TypePrinterDelegate is null)
            {
                throw new InvalidOperationException("Type.TypePrinterDelegate is null; Library must configure it to a non-null value to prevent null reference crashes");
            }

            driver.ProcessCode();
            library.Postprocess( driver.Context.ASTContext );

            if( !options.DryRun )
            {
                bool hasErrors = Diagnostics.Implementation is ErrorTrackingDiagnostics etd && etd.ErrorCount > 0;

                if( hasErrors )
                {
                    Diagnostics.Error( "Errors in previous stages, skipping code generation" );
                }
                else
                {
                    Diagnostics.Message( "Generating code..." );

                    var generators = library.CreateGenerators( );
                    driver.GenerateCode( generators );
                }
            }
        }

        private IEnumerable<Module> GetAndAddDependencies( Module m )
        {
            var dependencies = ( from library in Context.Symbols.Libraries
                                 where m.Libraries.Contains( library.FileName )
                                 from module in Options.Modules
                                 where library.Dependencies.Intersect( module.Libraries ).Any( )
                                 select module ).ToList( );
            if( m != Options.SystemModule )
            {
                m.Dependencies.Add( Options.SystemModule );
            }

            m.Dependencies.AddRange( dependencies );
            return m.Dependencies;
        }

        private void ValidateOptions()
        {
            if( !Options.Compilation.Platform.HasValue )
            {
                Options.Compilation.Platform = Platform.Host;
            }

            foreach( var module in Options.Modules )
            {
                if( string.IsNullOrWhiteSpace( module.LibraryName ) )
                {
                    throw new InvalidOptionException( "One of your modules has no library name." );
                }

                module.OutputNamespace ??= module.LibraryName;
            }

            if( Options.NoGenIncludeDirs != null )
            {
                foreach( string incDir in Options.NoGenIncludeDirs )
                {
                    ParserOptions.AddIncludeDirs( incDir );
                }
            }
        }

        private void OnSourceFileParsed( IEnumerable<string> files, CppSharp.Parser.ParserResult result )
        {
            OnFileParsed( files, result );
        }

        private void OnFileParsed( string file, CppSharp.Parser.ParserResult result )
        {
            OnFileParsed( [file], result );
        }

        private void OnFileParsed( IEnumerable<string> files, CppSharp.Parser.ParserResult result )
        {
            switch( result.Kind )
            {
            case ParserResultKind.Success:
                if( !Options.Quiet )
                {
                    Diagnostics.Debug( "Parsed '{0}'", string.Join( ", ", files ) );
                }

                break;

            case ParserResultKind.Error:
                Diagnostics.Error( "Error parsing '{0}'", string.Join( ", ", files ) );
                HasParsingErrors = true;
                break;

            case ParserResultKind.FileNotFound:
                Diagnostics.Error( "File{0} not found: '{1}'"
                                 , ( files.Count() > 1 ) ? "s" : string.Empty
                                 , string.Join( ",", files )
                                 );
                HasParsingErrors = true;
                break;
            }

            for( uint i = 0; i < result.DiagnosticsCount; ++i )
            {
                using ParserDiagnostic diag = result.GetDiagnostics( i );

                if( diag.Level == ParserDiagnosticLevel.Warning && !Options.Verbose )
                {
                    continue;
                }

                if( diag.Level == ParserDiagnosticLevel.Note )
                {
                    continue;
                }

                Diagnostics.Message( "{0}({1},{2}): {3}: {4}"
                                   , diag.FileName
                                   , diag.LineNumber
                                   , diag.ColumnNumber
                                   , diag.Level.ToString().ToUpperInvariant()
                                   , diag.Message
                                   );
            }
        }

        private bool HasParsingErrors;
    }
}
