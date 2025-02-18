using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
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
            context.RegisterPostInitializationOutput(AddAttributeSources);

            // build "pipelines" used during compilation
            var myTestAttributeProvider
                = context.SyntaxProvider
                         .ForAttributeWithMetadataName( Names.ContextHandleAttribute.FQN,
                                                        IsAttributeApplicable, // Normal usage constraints for an attribute already accomplished by compiler
                                                        CollectDataForGenerator
                                                      )
                                                      .Where(x => x != null)
                                                      .Collect() // collect all of the attributes in one go to allow generating all at once
                                                      .WithTrackingName("GenerateLLVMHandles");

            // connect condition pipeline to final source generator.
            context.RegisterSourceOutput(myTestAttributeProvider, GenerateSourceFile);
        }

        private static bool IsAttributeApplicable(SyntaxNode sn, CancellationToken token)
        {
            return sn is StructDeclarationSyntax targetStruct // Must be a struct declaration (attribute usage marks this so this is a safety check)
                && targetStruct.IsPartial() // must be partial struct to generate the partial backend
                && targetStruct.IsReadOnlyRecordStruct(); // Must be readonly record struct.
        }

        private static LlvmHandleInfo CollectDataForGenerator(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            // Gather all the information needed to generate the source for a single attribute instance here.
            // The return type type MUST implement IEquatable<T> so that it is cacheable.
            // (Which is the whole point of the "pipelines" of Incremental generators.)
            //
            var targetHandle = (StructDeclarationSyntax)context.TargetNode;
            return new LlvmHandleInfo(
                                targetHandle.GetNamespace(),
                                targetHandle.GetNestedClassName(includeSelf: true),
                                context.TargetSymbol.Name
                                );
        }

        private static void GenerateSourceFile(SourceProductionContext ctx, ImmutableArray<LlvmHandleInfo> results)
        {
            var srcTxt = new StringBuilderText(Encoding.UTF8);
            using var writer = srcTxt.CreateIndentedWriter();
            writer.WriteLine("using System.CodeDom.Compiler;");
            writer.WriteLine("System.Runtime.CompilerServices;");
            writer.WriteLine("using System.Runtime.InteropServices.Marshalling;");
            writer.WriteLine();

            // Note: While file scoped namespaces are allowed in some versions of C#, this
            // generator is not limited to only those versions. So, this does not generate
            // code that way. (There may be some form of "option" to enable that if it is
            // supported by the target, but that's not implemented here.)
            writer.WriteLine($"namespace {results[0].NamespaceName}");
            writer.WriteLine("{");
            using (writer.PushIndent())
            {
                foreach( var handleInfo in results)
                {
                    // not using push/IDisposable for inner layers to reduce overhead and
                    // simplify the code generation. Writer tracks indentation level already
                    // so just use that to reduce to expected starting level.

                    foreach (var classInfo in handleInfo.ContainingTypes)
                    {
                        // Bump indentation level for this layer
                        ++writer.Indent;
                        writer.WriteLine($"partial {classInfo.Keyword} {classInfo.Name}");
                        if (classInfo.HasConstraints)
                        {
                            using (writer.PushIndent())
                            {
                                writer.WriteLine($": {classInfo.Constraints}");
                            }
                        }

                        writer.WriteLine("{");
                    }

                    GenerateHandleType(writer, handleInfo);

                    // close off each indented level for nested type name.
                    // This should restore indentation to where things started
                    while(writer.Indent > 1)
                    {
                        --writer.Indent;
                        writer.WriteLine("}");
                    }
                }
            }

            // close the namespace (indentation handled by push/IDisposable already)
            Debug.Assert(writer.Indent == 0);
            writer.WriteLine("}");

            ctx.AddSource("ContextHandles.g.cs", srcTxt);
        }

        private static void GenerateHandleType(IndentedTextWriter writer, LlvmHandleInfo descriptor)
        {
            using(writer.PushIndent())
            {
                writer.WriteLine(ContextHandleDocComments);
                writer.WriteLine($"[NativeMarshalling(typeof(ContextHandleMarshaller<{descriptor.HandleName}>))]");
                writer.WriteLine($"public readonly record partial struct {descriptor.HandleName}");
                using (writer.PushIndent())
                {
                    writer.WriteLine($": IContextHandle<{descriptor.HandleName}>");
                }

                writer.WriteLine("{");
                using (writer.PushIndent())
                {
                    writer.WriteLine(ContextHandleBody, descriptor.HandleName);
                }

                writer.WriteLine("}");
            }
        }

        private static void AddAttributeSources(IncrementalGeneratorPostInitializationContext context)
        {
            // make sure stream is closed as AddStaticSourceFile won't do that!
            using var strm = Assembly.GetExecutingAssembly().GetManifestResourceStream(Names.ContextHandleAttribute.ResourceFileName);
            context.AddStaticSourceFile(strm, Names.ContextHandleAttribute.GeneratedAttributeFileName);
            // TODO: add other attribute or "static" files here

            // NOTE: adding static files like this has some issues. [Probably NOT the best idea...]
            //       It is usually better to add a reference to an attribute implementation via a NuGet Package.
            //       see: https://andrewlock.net/creating-a-source-generator-part-7-solving-the-source-generator-marker-attribute-problem-part1/
        }

        private const string ContextHandleDocComments ="""
        /// <summary>Simple type safe handle to wrap an opaque pointer for interop with "C" API exported from LibLLVM</summary>
        /// <remarks>
        ///    This handle is owned by it's container and therefore isn't disposed by the
        ///    calling App.
        /// <note type="important">
        ///     Since the object this handle refers to is not owned by the App, the object is
        ///     destroyed whenever it's container is destroyed, which will invalidate this handle.
        ///     Use of this handle after the container is destroyed will produce undefined
        ///     behavior, including, and most likely, memory access violations.
        /// </note>
        /// </remarks>
        """;

        private const string ContextHandleBody ="""
        /// <summary>Fluent null handle validation</summary>
        /// <param name="message">Message to use for an exception if thrown</param>
        /// <param name="memberName">Name if the member calling this function (usually provided by compiler via <see cref="CallerMemberNameAttribute"/></param>
        /// <param name="sourceFilePath">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerFilePathAttribute"/></param>
        /// <param name="sourceLineNumber">Source file path of the member calling this function (usually provided by compiler via <see cref="CallerLineNumberAttribute"/></param>
        /// <returns>This object for fluent style use</returns>
        public {0} ThrowIfInvalid(
            string message = "",
            [CallerMemberNameAttribute] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0 )
        {
            return DangerousGetHandle() == nint.Zero
                ? throw new UnexpectedNullHandleException( $"[{memberName}] - {sourceFilePath}@{sourceLineNumber} {message} " )
                : this;
        }

        /// <summary>Gets a value indicating if this handle is a <see langword="null"/> value</summary>
        public bool IsNull => Handle == nint.Zero;

        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <returns>The handle as an <see cref="nint"/></returns>
        public nint DangerousGetHandle() => Handle;

        /// <summary>Interface defined factory for an instance of <see cref="{0}"/></summary>
        /// <param name="abiValue">Native ABI value of the handle</param>
        /// <returns>Type specific wrapper around the native ABI handle</returns>
        public static {0} FromABI(nint abiValue) => new(abiValue);

        /// <summary>Gets a zero (<see langword="null"/>) value handle</summary>
        public static {0} Zero => FromABI(nint.Zero);

        /// <summary>Gets the handle as an <see cref="nint"/> suitable for passing to native code</summary>
        /// <param name="value">Handle to convert</param>
        /// <returns>The handle as an <see cref="nint"/></returns>
        public static implicit operator nint({0} value) => value.Handle;

        private {0}( nint p )
        {
            Handle = p;
        }

        private readonly nint Handle;
        """;
    }
}
