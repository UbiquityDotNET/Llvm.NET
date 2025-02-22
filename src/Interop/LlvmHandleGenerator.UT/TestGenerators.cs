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
            Assert.AreEqual( 2, results.GeneratedTrees.Length, "Should create 2 'files' during generation" );

            // the partial implementation(s) of the Context handle should be the first one...
            string contextHandleTxt = TestTree(results.GeneratedTrees[0],  @"LlvmHandleGenerator\LlvmHandleGenerator.LlvmHandleSourceGenerator\ContextHandles.g.cs");
            string globalHandleTxt = TestTree(results.GeneratedTrees[1], @"LlvmHandleGenerator\LlvmHandleGenerator.LlvmHandleSourceGenerator\GlobalHandles.g.cs");
        }

        internal static string TestTree(SyntaxTree tree, string expectedPath)
        {
            Assert.AreEqual( expectedPath, tree.FilePath, "Generated file should use correct name");
            Assert.AreEqual( Encoding.UTF8, tree.Encoding, "Generated files should use UTF8." );
            Assert.IsTrue(tree.TryGetText(out SourceText? contextHandleSrcTxt));
            return contextHandleSrcTxt.ToString();
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
                                            [ ], // reference assemblies come from `WithReferenceAssemblies` extension
                                            options: new CSharpCompilationOptions( OutputKind.ConsoleApplication )
                                            )
                                            .WithReferenceAssemblies( refAssemblyKind );

        }

        private readonly ImmutableArray<string> TrackingNames = ["GenerateContextHandles"];
    }
}
