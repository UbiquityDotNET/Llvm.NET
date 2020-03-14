// -----------------------------------------------------------------------
// <copyright file="Driver.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Passes;
using CppSharp.Types;
using CppSharp.Utils;

using LlvmBindingsGenerator.Templates;

#pragma warning disable SA1600

namespace LlvmBindingsGenerator
{
    /// <summary>Provides a more flexible implementation of the general "driver" concept in CppSharp</summary>
    /// <remarks>
    /// Notable differences:
    /// 1. The driver does not setup ANY passes, it is entirely up to the library to do that
    /// 2. There is no intermediate "Generator" needed
    /// 3. Deals in interface rather than concretes
    /// 4. The code generation types are interfaces with names that more accurately reflect what they do
    /// </remarks>
    internal sealed class Driver
        : IDriver
        , IDisposable
    {
        public Driver( )
            : this( new DriverOptions( ) )
        {
        }

        public Driver( DriverOptions options )
        {
            Options = options;
            ParserOptions = new CppSharp.Parser.ParserOptions( );
        }

        public DriverOptions Options { get; }

        public CppSharp.Parser.ParserOptions ParserOptions { get; }

        public BindingContext Context { get; private set; }

        public ITypePrinter TypePrinter
        {
            get => InternalTypePrinter;
            set
            {
                InternalTypePrinter = value;
                CppSharp.AST.Type.TypePrinterDelegate = InternalTypePrinter.ToString;
            }
        }

        public void SetupTypeMaps( ) =>
            Context.TypeMaps = new TypeMapDatabase( Context );

        public void Setup( )
        {
            ValidateOptions( );
            ParserOptions.Setup( );
            Context = new BindingContext( Options, ParserOptions );
        }

        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "context ownership is transfered to wrapper ASTContext" )]
        public bool ParseCode( )
        {
            var astContext = new CppSharp.Parser.AST.ASTContext( );
            var parser = new ClangParser( astContext );
            parser.SourcesParsed += OnSourceFileParsed;

            var sourceFiles = Options.Modules.SelectMany( m => m.Headers );

            if( ParserOptions.UnityBuild )
            {
                using( var parserOptions = ParserOptions.BuildForSourceFile( Options.Modules ) )
                {
                    using( var result = parser.ParseSourceFiles( sourceFiles, parserOptions ) )
                    {
                        Context.TargetInfo = result.TargetInfo;
                    }

                    if( string.IsNullOrEmpty( ParserOptions.TargetTriple ) )
                    {
                        ParserOptions.TargetTriple = parserOptions.TargetTriple;
                    }
                }
            }
            else
            {
                foreach( string sourceFile in sourceFiles )
                {
                    using( var parserOptions = ParserOptions.BuildForSourceFile( Options.Modules, sourceFile ) )
                    using( CppSharp.Parser.ParserResult result = parser.ParseSourceFile( sourceFile, parserOptions ) )
                    {
                        if( Context.TargetInfo == null )
                        {
                            Context.TargetInfo = result.TargetInfo;
                        }
                        else if( result.TargetInfo != null )
                        {
                            result.TargetInfo.Dispose( );
                        }

                        if( string.IsNullOrEmpty( ParserOptions.TargetTriple ) )
                        {
                            ParserOptions.TargetTriple = parserOptions.TargetTriple;
                        }
                    }
                }
            }

            Context.ASTContext = ClangParser.ConvertASTContext( astContext );

            return !hasParsingErrors;
        }

        public void SortModulesByDependencies( )
        {
            var sortedModules = Options.Modules.TopologicalSort( GetAndAddDependencies );
            Options.Modules.Clear( );
            Options.Modules.AddRange( sortedModules );
        }

        public bool ParseLibraries( )
        {
            foreach( var module in Options.Modules )
            {
                foreach( string libraryDir in module.LibraryDirs )
                {
                    ParserOptions.AddLibraryDirs( libraryDir );
                }

                foreach( string library in module.Libraries )
                {
                    if( Context.Symbols.Libraries.Any( l => l.FileName == library ) )
                    {
                        continue;
                    }

                    var parser = new ClangParser( );
                    parser.LibraryParsed += OnFileParsed;

                    using( var res = parser.ParseLibrary( library, ParserOptions ) )
                    {
                        if( res.Kind != CppSharp.Parser.ParserResultKind.Success )
                        {
                            continue;
                        }

                        Context.Symbols.Libraries.Add( ClangParser.ConvertLibrary( res.Library ) );
                    }
                }
            }

            Context.Symbols.IndexSymbols( );
            SortModulesByDependencies( );

            return true;
        }

        public void ProcessCode( )
        {
            Context.RunPasses( );
        }

        public void GenerateCode( IEnumerable<ICodeGenerator> generators )
        {
            string outputPath = Path.GetFullPath( Options.OutputDir );

            if( !Directory.Exists( outputPath ) )
            {
                Directory.CreateDirectory( outputPath );
            }

            foreach( ICodeGenerator generator in generators.Where( o => o.IsValid ) )
            {
                string generatorOutputPath = outputPath;

                if( Options.UseHeaderDirectories )
                {
                    generatorOutputPath = Path.Combine( generatorOutputPath, generator.FileRelativeDirectory );
                    Directory.CreateDirectory( generatorOutputPath );
                }

                foreach( ICodeGenTemplate template in generator.Templates )
                {
                    try
                    {
                        string templateOutputPath = generatorOutputPath;
                        if( !string.IsNullOrWhiteSpace( template.SubFolder ) )
                        {
                            templateOutputPath = Path.Combine( templateOutputPath, template.SubFolder );
                            Directory.CreateDirectory( templateOutputPath );
                        }

                        string fileName = $"{generator.FileNameWithoutExtension}.{template.FileExtension}";

                        string fullFilePathfile = Path.Combine( templateOutputPath, fileName );
                        File.WriteAllText( fullFilePathfile, template.Generate( ) );

                        Diagnostics.Debug( "Generated '{0}'", fileName );
                    }
                    catch( System.IO.IOException ex )
                    {
                        Diagnostics.Error( ex.Message );
                    }
                }
            }
        }

        public void Dispose( )
        {
            Context.TargetInfo.Dispose( );
            ParserOptions.Dispose( );
            CppSharp.AST.Type.TypePrinterDelegate = null;
        }

        public static void Run( ILibrary library )
        {
            using( var driver = new Driver( ) )
            {
                var options = driver.Options;
                library.Setup( driver );

                driver.Setup( );

                if( options.Verbose )
                {
                    Diagnostics.Level = DiagnosticKind.Debug;
                }

                Diagnostics.Message( "Parsing libraries..." );
                if( !driver.ParseLibraries( ) )
                {
                    return;
                }

                Diagnostics.Message( "Parsing code..." );
                if( !driver.ParseCode( ) )
                {
                    Diagnostics.Error( "CppSharp has encountered an error while parsing code." );
                    return;
                }

                new CleanUnitPass { Context = driver.Context }.VisitASTContext( driver.Context.ASTContext );
                options.Modules.RemoveAll( m => m != options.SystemModule && !m.Units.GetGenerated( ).Any( ) );

                Diagnostics.Message( "Processing code..." );

                library.SetupPasses( );
                driver.SetupTypeMaps( );

                library.Preprocess( driver.Context.ASTContext );

                driver.ProcessCode( );
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

        private void ValidateOptions( )
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

                if( module.OutputNamespace == null )
                {
                    module.OutputNamespace = module.LibraryName;
                }
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
            OnFileParsed( new[ ] { file }, result );
        }

        private void OnFileParsed( IEnumerable<string> files, CppSharp.Parser.ParserResult result )
        {
            switch( result.Kind )
            {
            case CppSharp.Parser.ParserResultKind.Success:
                if( !Options.Quiet )
                {
                    Diagnostics.Message( "Parsed '{0}'", string.Join( ", ", files ) );
                }

                break;

            case CppSharp.Parser.ParserResultKind.Error:
                Diagnostics.Error( "Error parsing '{0}'", string.Join( ", ", files ) );
                hasParsingErrors = true;
                break;

            case CppSharp.Parser.ParserResultKind.FileNotFound:
                Diagnostics.Error( "File{0} not found: '{1}'"
                                 , ( files.Count( ) > 1 ) ? "s" : string.Empty
                                 , string.Join( ",", files )
                                 );
                hasParsingErrors = true;
                break;
            }

            for( uint i = 0; i < result.DiagnosticsCount; ++i )
            {
                var diag = result.GetDiagnostics( i );

                if( diag.Level == CppSharp.Parser.ParserDiagnosticLevel.Warning && !Options.Verbose )
                {
                    continue;
                }

                if( diag.Level == CppSharp.Parser.ParserDiagnosticLevel.Note )
                {
                    continue;
                }

                Diagnostics.Message( "{0}({1},{2}): {3}: {4}"
                                   , diag.FileName
                                   , diag.LineNumber
                                   , diag.ColumnNumber
                                   , diag.Level.ToString( ).ToUpperInvariant( )
                                   , diag.Message
                                   );
            }
        }

        private ITypePrinter InternalTypePrinter;
        private bool hasParsingErrors;
    }
}
