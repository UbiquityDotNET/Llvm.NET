// -----------------------------------------------------------------------
// <copyright file="ParseErrorListenerAdapter.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.IO;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

using Ubiquity.NET.Runtime.Utils;

namespace Ubiquity.NET.ANTLR.Utils
{
    /// <summary>Adapter to translate ANTLR error listeners to an <see cref="IParseErrorListener"/></summary>
    public class ParseErrorListenerAdapter
        : IAntlrErrorListener<int>
        , IAntlrErrorListener<IToken>
    {
        /// <summary>Initializes a new instance of the <see cref="ParseErrorListenerAdapter"/> class</summary>
        /// <param name="innerListener">Inner listener to route all notifications to</param>
        public ParseErrorListenerAdapter( IParseErrorListener innerListener )
        {
            InnerListener = innerListener;
        }

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
            InnerListener.SyntaxError( new SyntaxError( ParseErrorSource.Lexer
                                                      , recognizer.InputStream.SourceName
                                                      , recognizer.State
                                                      , string.Empty
                                                      , new SourceLocation( line, charPositionInLine, line, charPositionInLine )
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
            InnerListener.SyntaxError( new SyntaxError( ParseErrorSource.Parser
                                                      , recognizer.InputStream.SourceName
                                                      , recognizer.State
                                                      , offendingSymbol.Text
                                                      , new SourceLocation( line, charPositionInLine, line, charPositionInLine )
                                                      , msg
                                                      , e
                                                      )
                                     );
        }

        private readonly IParseErrorListener InnerListener;
    }
}
