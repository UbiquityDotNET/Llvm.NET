using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using SourceGenerator.Utils;

namespace LlvmHandleGenerator
{
    /// <summary>Roslyn source generator for types marked with `ContextHandleAttribute`</summary>
    [Generator(LanguageNames.CSharp)]
    public class LlvmHandleSourceGenerator
        : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // build "pipeline" for context handles
            var contextHandleAttributeProvider
                = context.SyntaxProvider
                         .ForAttributeWithMetadataName( Names.ContextHandleAttribute.FQN,
                                                        ContextHandleAttributePredicate,
                                                        CollectDataForGenerator
                                                      )
                                                      .Collect() // collect all of the attributes in one go to allow generating all at once
                                                      .WithTrackingName("GenerateContextHandles");

            context.RegisterSourceOutput(contextHandleAttributeProvider, GenerateContextHandlesSource);

            // build "pipeline" for Global handles
            var globalHandleAttributeProvider
                = context.SyntaxProvider
                         .ForAttributeWithMetadataName( Names.GlobalHandleAttribute.FQN,
                                                        GlobalHandleAttributePredicate,
                                                        CollectDataForGenerator
                                                      )
                                                      .Collect() // collect all of the attributes in one go to allow generating all at once
                                                      .WithTrackingName("GenerateGlobalHandles");

            context.RegisterSourceOutput(globalHandleAttributeProvider, GenerateGlobalHandlesSource);

        }

        private static bool ContextHandleAttributePredicate(SyntaxNode sn, CancellationToken t)
        {
            return sn is RecordDeclarationSyntax rds
                && rds.ClassOrStructKeyword.ValueText == "struct"
                && rds.Modifiers.Any(SyntaxKind.ReadOnlyKeyword)
                && rds.Modifiers.Any(SyntaxKind.PartialKeyword)
                && !rds.Modifiers.Any(SyntaxKind.FileKeyword); // can't "extend" a file scoped type in another file...
        }

        private static bool GlobalHandleAttributePredicate(SyntaxNode sn, CancellationToken t)
        {
            return sn is ClassDeclarationSyntax cds
                && cds.Modifiers.Any(SyntaxKind.PartialKeyword)
                && !cds.Modifiers.Any(SyntaxKind.FileKeyword); // can't "extend" a file scoped type in another file...
        }

        private static LlvmHandleInfo CollectDataForGenerator(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            // Gather all the information needed to generate the source for a single attribute instance here.
            // The return type type MUST implement IEquatable<T> so that it is cacheable.
            // (Which is the whole point of the "pipelines" of Incremental generators.)
            //
            var targetHandle = (TypeDeclarationSyntax)context.TargetNode;

            // Try to get the "IncludeAlias" property from the attribute if there is one. [Context handle attribute will never have it]
            KeyValuePair<string, TypedConstant> includeAliasParam = context.Attributes[0].NamedArguments.FirstOrDefault(x=>x.Key == "IncludeAlias");
            bool includeAlias = includeAliasParam.Key is not null
                              && ((includeAliasParam.Value.Value as bool?) ?? false);

            return new LlvmHandleInfo(
                                targetHandle.GetNamespace(),
                                targetHandle.GetNestedClassName(includeSelf: true),
                                context.TargetSymbol.Name,
                                includeAlias
                                );
        }

        private static void GenerateContextHandlesSource(SourceProductionContext ctx, ImmutableArray<LlvmHandleInfo> results)
        {
            // might not be any - so bail early
            if (results.Length == 0)
            {
                return;
            }

            string templateText = GetManifestResourceText("ContextHandle.template");

            var srcTxt = new StringBuilderText(Encoding.UTF8);
            using var writer = srcTxt.CreateIndentedWriter();
            writer.WriteLine("using System.Runtime.InteropServices.Marshalling;");
            writer.WriteLine();

            foreach(var result in results)
            {
                writer.WriteLine("namespace {0}", result.NamespaceName);
                writer.WriteLine('{');
                using(writer.PushIndent())
                {
                    writer.WriteLines(templateText, "%%HANDLENAME%%", result.HandleName);
                }

                writer.WriteLine('}');
            }

            ctx.AddSource("ContextHandles.g.cs", srcTxt);
        }

        private static void GenerateGlobalHandlesSource(SourceProductionContext ctx, ImmutableArray<LlvmHandleInfo> results)
        {
            // might not be any - so bail early
            if (results.Length == 0)
            {
                return;
            }

            string templateText = GetManifestResourceText("GlobalHandle.template");
            string aliasTemplateText = GetManifestResourceText("GlobalAlias.template");

            var srcTxt = new StringBuilderText(Encoding.UTF8);
            using var writer = srcTxt.CreateIndentedWriter();
            writer.WriteLine("using System.Runtime.CompilerServices;");
            writer.WriteLine("using System.Runtime.InteropServices;");
            writer.WriteLine();

            foreach(var result in results)
            {
                writer.WriteLine("namespace {0}", result.NamespaceName);
                writer.WriteLine('{');
                using(writer.PushIndent())
                {
                    writer.WriteLines(templateText, "%%HANDLENAME%%", result.HandleName);

                    if (result.IncludeAlias)
                    {
                        writer.WriteBlankLine();
                        writer.WriteLines(aliasTemplateText, "%%HANDLENAME%%", result.HandleName);
                    }
                }

                writer.WriteLine('}');
            }

            ctx.AddSource("GlobalHandles.g.cs", srcTxt);
        }

        private static Stream? GetManifestResourceStream(string resourceName)
        {
            string fullName = $"{nameof(LlvmHandleGenerator)}.{resourceName}";
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName);
        }

        private static string GetManifestResourceText(string resourceName)
        {
            using var rdr = new StreamReader(GetManifestResourceStream(resourceName));
            return rdr.ReadToEnd();
        }

        //private static void AddAttributeSources(IncrementalGeneratorPostInitializationContext context)
        //{
        //    // make sure stream is closed as AddStaticSourceFile won't do that!
        //    using var strm = GetManifestResourceStream(Names.ContextHandleAttribute.ResourceFileName);
        //    if (strm is null)
        //    {
        //        Debug.Assert(false); // if this hits, then the name is incorrect, or the resource file is missing
        //        throw new System.IO.FileNotFoundException("Resource file not found", "Names.ContextHandleAttribute.ResourceFileName");
        //    }

        //    context.AddStaticSourceFile(strm, Names.ContextHandleAttribute.GeneratedAttributeFileName);
        //    // TODO: add other attribute or "static" files here

        //    // NOTE: adding static files like this has some issues. [Probably NOT the best idea...]
        //    //       It is usually better to add a reference to an attribute implementation via a NuGet Package.
        //    //       see: https://andrewlock.net/creating-a-source-generator-part-7-solving-the-source-generator-marker-attribute-problem-part1/
        //}
    }
}
