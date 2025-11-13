// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.CodeAnalysis.Utils
{
    /// <summary>A <see cref="SourceText"/> implementation over a <see cref="StringBuilder"/></summary>
    /// <remarks>
    /// This provides a <see cref="SourceText"/> implementation over a <see cref="StringBuilder"/>
    /// it provides access to the underlying builder to allow construction of an <see cref="IndentedTextWriter"/>
    /// to manage indentation in the output. Manual generation of the output with indentation tracking is the
    /// recommended approach to generating source output.
    /// </remarks>
    /// <seealso href="https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md#use-an-indented-text-writer-not-syntaxnodes-for-generation"/>
    public class StringBuilderText
        : SourceText
    {
        /// <summary>Initializes a new instance of the <see cref="StringBuilderText"/> class</summary>
        /// <param name="builder">Builder to use for building strings</param>
        /// <param name="encoding">Encoding to use for the strings</param>
        /// <param name="algorithm">Hash algorithm to use for debug symbols in the source</param>
        public StringBuilderText(StringBuilder builder, Encoding encoding, SourceHashAlgorithm algorithm = SourceHashAlgorithm.Sha1)
            : base(checksumAlgorithm: algorithm)
        {
            Builder = builder;
            InternalEncoding = encoding;
        }

        /// <summary>Initializes a new instance of the <see cref="StringBuilderText"/> class</summary>
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

        /// <summary>Creates a new <see cref="StringWriter"/> over the internal <see cref="Builder"/></summary>
        /// <returns>The newly created <see cref="StringWriter"/></returns>
        /// <remarks>
        /// <para>The <see cref="StringWriter"/> created, does NOT dispose of or invalidate the underlying
        /// <see cref="StringBuilder"/>. This allows things like <see cref="StringWriter.GetStringBuilder"/>
        /// to work even after <see cref="TextWriter.Dispose()"/> is called.</para>
        /// <para>The created writer is commonly wrapped in an instance of <see cref="IndentedTextWriter"/>
        /// for generating source output in a source generator.</para>
        /// </remarks>
        public StringWriter CreateWriter()
        {
            return new StringWriter(Builder);
        }

        /// <summary>Gets the internal builder</summary>
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
