// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.NET.SrcGeneration
{
    /// <summary>Extensions to <see cref="IndentedTextWriter"/> to support generic source generation</summary>
    [SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "extension" )]
    [SuppressMessage( "Design", "CA1034:Nested types should not be visible", Justification = "extension" )]
    public static class IndentedTextWriterExtensions
    {
        /// <summary>Writes an indented block to an <see cref="IndentedTextWriter"/></summary>
        /// <param name="self">Writer to apply extension method to</param>
        /// <param name="open">Opening value of block (on it's own line)</param>
        /// <param name="close">Closing value of block (on it's own line)</param>
        /// <param name="leadingLine">Line of text preceding the block</param>
        /// <param name="indented">Indicates if additional content written is indented or not [Default: <see langword="true"/>]</param>
        /// <returns><see cref="IDisposable"/> that will automatically emit <paramref name="close"/> and out dent the writer.</returns>
        /// <remarks>
        /// <note type="important">
        /// This does NOT end the line. This allows writing a comment or other output after the block is closed on the same line
        /// as the <paramref name="close"/>.
        /// </note>
        /// </remarks>
        public static IDisposable Block(this IndentedTextWriter self, string open, string close, string? leadingLine = null, bool indented = true )
        {
            ArgumentNullException.ThrowIfNull( self );
            ArgumentNullException.ThrowIfNull( open );
            ArgumentNullException.ThrowIfNull( close );

            if(leadingLine is not null)
            {
                self.WriteLine( leadingLine );
            }

            self.WriteLine( open );
            if(indented)
            {
                ++self.Indent;
            }

            // presence of this when using `extension` keyword triggers bugs in C#14 preview 3
            // https://github.com/dotnet/roslyn/issues/78135
            // https://github.com/dotnet/roslyn/issues/78042
            return new Extensions.DisposableAction( ( ) =>
            {
                if(indented)
                {
                    --self.Indent;
                }

                self.Write( close );
            } );
        }

        /// <summary>Pushes the indentation and returns an <see cref="IDisposable"/> that will restore it on Dispose (RAII pattern)</summary>
        /// <param name="self">Writer to apply extension method to</param>
        /// <returns>Disposable that when invoked, will reduce the indentation.</returns>
        public static IDisposable PushIndent( this IndentedTextWriter self )
        {
            ArgumentNullException.ThrowIfNull( self );

            ++self.Indent;

            // presence of lambda when using `extension` keyword triggers bugs in C#14 preview 3
            // https://github.com/dotnet/roslyn/issues/78135
            // https://github.com/dotnet/roslyn/issues/78042
            return new Extensions.DisposableAction( ( ) => --self.Indent );
        }

        /// <summary>Writes a blank line (New line, with no additional characters or whitespace)</summary>
        /// <param name="self">Writer to apply extension method to</param>
        public static void WriteEmptyLine( this IndentedTextWriter self )
        {
            ArgumentNullException.ThrowIfNull( self );

            self.WriteLineNoTabs( string.Empty );
        }
    }
}
