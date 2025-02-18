using System.Collections.Immutable;
using System.IO;
using System.Text;

using Basic.Reference.Assemblies;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SourceGenerator.Test.Utils;

using LlvmHandleGenerator;

namespace LLvmHandleGenerator.UT
{
    [TestClass]
    public class TestGenerators
    {
        [TestMethod]
        public void TestContextHandleSourceGenerator()
        {
            var sourceGenerator = new LlvmHandleSourceGenerator().AsSourceGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(
                generators: [sourceGenerator],
                driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true)
                );

            CSharpCompilation compilation = CreateCompilation("TestProgram.cs");

            var results = driver.RunGeneratorAndAssertResults(compilation, TrackingNames);
            Assert.AreEqual( 0, results.Diagnostics.Length, "Should not have ANY diagnostics reported during generation" );

            // validate the generated trees have the correct count and names
            Assert.AreEqual( 1, results.GeneratedTrees.Length, "Should create 1 'file' during generation" );
            SyntaxTree tree = results.GeneratedTrees[0];

            Assert.AreEqual( "ContextHandles.g.cs", tree.FilePath, "Generated file should use correct name" );
            Assert.AreEqual( Encoding.UTF8, tree.Encoding, $"Generated files should use UTF8." );
        }

        // simple helper for these tests to create a C# Compilation
        internal static CSharpCompilation CreateCompilation(
            string path,
            CSharpParseOptions? options = default,
            ReferenceAssemblyKind refAssemblyKind = ReferenceAssemblyKind.Net80
            )
        {
            using var srcFile = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var st = SourceText.From(srcFile, throwIfBinaryDetected: true);

            return CSharpCompilation.Create( "TestAssembly",
                                            [ CSharpSyntaxTree.ParseText( st, options, Path.GetFullPath( path ) ) ],
                                            [ MetadataReference.CreateFromFile( InteropRefPath ) ],
                                            options: new CSharpCompilationOptions( OutputKind.ConsoleApplication )
                                            )
                                            .WithReferenceAssemblies( refAssemblyKind );

        }

        private readonly ImmutableArray<string> TrackingNames = ["GenerateLLVMHandles"];
        private const string InteropRefPath = "????";
    }
}
