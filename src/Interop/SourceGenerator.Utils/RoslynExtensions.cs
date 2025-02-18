using System.IO;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator.Utils
{
    // place-holder for extensions that don't fit anywhere else and don't really warrant their own file.
    public static class RoslynExtensions
    {
        public static string GetName(this AttributeSyntax? attr)
        {
            return attr is null
                ? string.Empty
                : attr.Name is not IdentifierNameSyntax identifier ? string.Empty : identifier.Identifier.ValueText;
        }

        // handles adding a source file from a stream. This also declares it as embeddable so that
        // a checksum is computed. Otherwise when the compiler reaches the point of generating the PDB it
        // can't do so (some really weird encoding reasons...) and crashes the compilation with an exception
        /*
            System.ArgumentException
              HResult=0x80070057
              Message=SourceText cannot be embedded. Provide encoding or canBeEmbedded=true at construction.
            Parameter name: text
              Source=Microsoft.CodeAnalysis
              StackTrace:
               at Microsoft.CodeAnalysis.EmbeddedText.FromSource(String filePath, SourceText text) in Microsoft.CodeAnalysis\EmbeddedText.cs:line 78
               at Microsoft.CodeAnalysis.CommonCompiler.CompileAndEmit(TouchedFileLogger touchedFilesLogger, Compilation& compilation, ImmutableArray`1 analyzers, ImmutableArray`1 generators, ImmutableArray`1 additionalTextFiles, AnalyzerConfigSet analyzerConfigSet, ImmutableArray`1 sourceFileAnalyzerConfigOptions, ImmutableArray`1 embeddedTexts, DiagnosticBag diagnostics, ErrorLogger errorLogger, CancellationToken cancellationToken, CancellationTokenSource& analyzerCts, AnalyzerDriver& analyzerDriver, Nullable`1& generatorTimingInfo) in Microsoft.CodeAnalysis\CommonCompiler.cs:line 1099
               at Microsoft.CodeAnalysis.CommonCompiler.RunCore(TextWriter consoleOutput, ErrorLogger errorLogger, CancellationToken cancellationToken) in Microsoft.CodeAnalysis\CommonCompiler.cs:line 990
               at Microsoft.CodeAnalysis.CommonCompiler.Run(TextWriter consoleOutput, CancellationToken cancellationToken) in Microsoft.CodeAnalysis\CommonCompiler.cs:line 868
               at Microsoft.CodeAnalysis.CSharp.CommandLine.Csc.<>c__DisplayClass1_0.<Run>b__0(TextWriter tw)
               at Microsoft.CodeAnalysis.CommandLine.ConsoleUtil.RunWithUtf8Output[T](Func`2 func)
               at Microsoft.CodeAnalysis.CommandLine.ConsoleUtil.RunWithUtf8Output[T](Boolean utf8Output, TextWriter textWriter, Func`2 func)
               at Microsoft.CodeAnalysis.CSharp.CommandLine.Csc.Run(String[] args, BuildPaths buildPaths, TextWriter textWriter, IAnalyzerAssemblyLoader analyzerLoader)
               at Microsoft.CodeAnalysis.CommandLine.BuildClient.RunLocalCompilation(String[] arguments, BuildPaths buildPaths, TextWriter textWriter)
               at Microsoft.CodeAnalysis.CommandLine.BuildClient.RunCompilation(IEnumerable`1 originalArguments, BuildPaths buildPaths, TextWriter textWriter, String pipeName)
               at Microsoft.CodeAnalysis.CommandLine.BuildClient.Run(IEnumerable`1 arguments, RequestLanguage language, CompileFunc compileFunc, CompileOnServerFunc compileOnServerFunc, ICompilerServerLogger logger)
               at Microsoft.CodeAnalysis.CSharp.CommandLine.Program.MainCore(String[] args)
               at Microsoft.CodeAnalysis.CSharp.CommandLine.Program.Main(String[] args)
        */
        public static void AddStaticSourceFile(this IncrementalGeneratorPostInitializationContext context, Stream strm, string generatedName)
        {
            // must explicitly declare source text as embeddable or compilation of target will fail
            // with an incomprehensible exception stack dump the end user can't resolve...
            //
            // Embedded sources in PDBs require some precomputation that only happens
            // if this flag is set. This is required EVEN if the target project is
            // configured to not emit source in the PDBs [Sigh!].
            context.AddSource(generatedName, SourceText.From(strm, canBeEmbedded: true));
        }
    }
}
