using System.CodeDom.Compiler;
using System.IO;
using System.Text;

using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator.Utils
{
    /// <summary>A <see cref="SourceText"/> implementation over a <see cref="StringBuilder"/></summary>
    /// <remarks>
    /// Not only does this provide a <see cref="SourceText"/> implementation over a <see cref="StringBuilder"/>
    /// it provides access to the underlying builder AND construction of <see cref="IndentedTextWriter"/> to
    /// manage indentation in the output. Manual generation of the output with indentation tracking is the
    /// recommended approach to generating source output.
    /// </remarks>
    /// <seealso href="https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md#use-an-indented-text-writer-not-syntaxnodes-for-generation"/>
    public class StringBuilderText
        : SourceText
    {
        /// <summary>Initializes an instance of a <see cref="StringBuilderText"/></summary>
        /// <param name="builder">Builder to use for building strings</param>
        /// <param name="encoding">Encoding to use for the strings</param>
        /// <param name="algorithm">Hash algorithm to use for debug symbols in the source</param>
        public StringBuilderText(StringBuilder builder, Encoding encoding, SourceHashAlgorithm algorithm = SourceHashAlgorithm.Sha1)
            : base(checksumAlgorithm: algorithm)
        {
            Builder = builder;
            InternalEncoding = encoding;
        }

        /// <summary>Initializes an instance of a <see cref="StringBuilderText"/></summary>
        /// <param name="encoding">Encoding to use for the strings</param>
        /// <param name="algorithm">Hash algorithm to use for debug symbols in the source</param>
        /// <remarks>
        /// This constructor overload will create a new <see cref="StringBuilder"/> as the underlying
        /// store for the strings. This is likely the most common case.
        /// </remarks>
        public StringBuilderText(Encoding encoding, SourceHashAlgorithm algorithm = SourceHashAlgorithm.Sha1)
            : this(new StringBuilder(), encoding, algorithm)
        {
        }

        /// <summary>Creates a new <see cref="IndentedTextWriter"/> over the internal <see cref="Builder"/></summary>
        /// <param name="tabs">String to use for indentation (default is 4 spaces)</param>
        /// <returns>The newly created <see cref="IndentedTextWriter"/></returns>
        /// <seealso href="https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md#use-an-indented-text-writer-not-syntaxnodes-for-generation"/>
        public IndentedTextWriter CreateIndentedWriter(string tabs = "    ")
        {
            return new IndentedTextWriter(new StringWriter(Builder), tabs);
        }

        /// <summary>Gets the the internal builder</summary>
        public StringBuilder Builder { get; init; }

        /// <inheritdoc/>
        public override char this[int position] => Builder[position];

        /// <inheritdoc/>
        public override Encoding Encoding => InternalEncoding;

        /// <inheritdoc/>
        public override int Length => Builder.Length;

        /// <inheritdoc/>
        public override void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            Builder.CopyTo(sourceIndex, destination, destinationIndex, count);
        }

        /// <summary>Converts the specified span in the underlying builder into a string</summary>
        /// <param name="span">Span to convert</param>
        /// <returns>Text from the builder as a string</returns>
        public override string ToString(TextSpan span)
        {
            return Builder.ToString(span.Start, span.Length);
        }

        private readonly Encoding InternalEncoding;
    }
}
