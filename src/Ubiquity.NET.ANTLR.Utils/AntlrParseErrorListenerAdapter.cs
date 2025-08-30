// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

using Ubiquity.NET.Runtime.Utils;
using Ubiquity.NET.TextUX;

namespace Ubiquity.NET.ANTLR.Utils
{
    /// <summary>Adapter to translate ANTLR error listeners to an <see cref="IParseErrorListener{T}"/></summary>
    /// <typeparam name="T">Type of the enum for diagnostic IDs</typeparam>
    /// <remarks>
    /// <para>This intentionally ignores the <see cref="TextWriter"/> provided by ANTLR and uses the <see cref="IParseErrorListener{T}"/>
    /// provided in the constructor. This allows a much greater level of flexibility in reporting of diagnostics from
    /// a parser. Especially in abstracting the underlying parse technology from the diagnostic reporting</para>
    /// <para>
    /// The <see cref="IdentifierMap"/> is used to allow for future adaptation of the parser to map errors from a
    /// recognizer state, which is not stable if the grammar changes. This ensures that the ID values remain unique
    /// even if the underlying grammar changes. THe default is to use a 1:1 mapping where the ID values are used
    /// directly. Any value not in the map is used directly.
    /// </para>
    /// </remarks>
    public class AntlrParseErrorListenerAdapter<T>
        : IAntlrErrorListener<int>
        , IAntlrErrorListener<IToken>
        where T : struct, Enum
    {
        /// <summary>Initializes a new instance of the <see cref="AntlrParseErrorListenerAdapter{T}"/> class</summary>
        /// <param name="innerListener">Inner listener to route all notifications to</param>
        /// <param name="identifierMap">Map of ids to translate <see cref="IRecognizer.State"/> values to an ID</param>
        public AntlrParseErrorListenerAdapter(
            IParseErrorListener<T> innerListener,
            ImmutableDictionary<T, T>? identifierMap = default
            )
        {
            InnerListener = innerListener;
            IdentifierMap = identifierMap;
        }

        /// <summary>Gets the mapping for identifiers. If this is <see langword="null"/> then no mapping is used.</summary>
        public ImmutableDictionary<T, T>? IdentifierMap { get; }

        /// <inheritdoc/>
        public void SyntaxError( TextWriter output // ignored
                               , [NotNull] IRecognizer recognizer
                               , [Nullable] int offendingSymbol
                               , int line
                               , int charPositionInLine
                               , [NotNull] string msg
                               , [Nullable] RecognitionException e
                               )
        {
            T mappedId = GetMappedId(recognizer.State);

            InnerListener.SyntaxError( new SyntaxError<T>( ParseErrorSource.Lexer
                                                         , recognizer.InputStream.SourceName
                                                         , mappedId
                                                         , string.Empty
                                                         , new SourcePosition(line, charPositionInLine, recognizer.InputStream.Index)
                                                         , msg
                                                         , e
                                                         )
                                     );
        }

        /// <inheritdoc/>
        public void SyntaxError( TextWriter output // ignored
                               , [NotNull] IRecognizer recognizer
                               , [Nullable] IToken offendingSymbol
                               , int line
                               , int charPositionInLine
                               , [NotNull] string msg
                               , [Nullable] RecognitionException e
                               )
        {
            T mappedId = GetMappedId(recognizer.State);

            InnerListener.SyntaxError( new SyntaxError<T>( ParseErrorSource.Parser
                                                         , recognizer.InputStream.SourceName
                                                         , mappedId
                                                         , offendingSymbol.Text
                                                         , new SourcePosition(line, charPositionInLine, recognizer.InputStream.Index)
                                                         , msg
                                                         , e
                                                         )
                                     );
        }

        // The IdentifierMap is used to allow for future adaptation of the parser to map errors from a
        // recognizer state, which is not stable if the grammar changes. This ensures that the ID values remain unique
        // even if the underlying grammar changes. The default is to use a 1:1 mapping where the ID values are used
        // directly. Any value not in the map is used directly.
        private T GetMappedId(int state)
        {
            var mappedId = (T)Convert.ChangeType(state, typeof(T), CultureInfo.InvariantCulture);

            if(IdentifierMap is not null && IdentifierMap.IsEmpty)
            {
                if(IdentifierMap.TryGetValue(mappedId, out T mappedValue))
                {
                    mappedId = mappedValue;
                }
            }

            return mappedId;
        }

        private readonly IParseErrorListener<T> InnerListener;
    }
}
