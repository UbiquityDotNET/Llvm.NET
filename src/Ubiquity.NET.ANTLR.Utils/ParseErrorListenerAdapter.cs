// -----------------------------------------------------------------------
// <copyright file="ParseErrorListenerAdapter.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

using Ubiquity.NET.Runtime.Utils;

namespace Ubiquity.NET.ANTLR.Utils
{
    public class ParseErrorListenerAdapter
        : IAntlrErrorListener<int>
        , IAntlrErrorListener<IToken>
    {
        public ParseErrorListenerAdapter( IParseErrorListener innerListener )
        {
            InnerListener = innerListener;
        }

        public void SyntaxError( [NotNull] IRecognizer recognizer
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
                                                      , new SourceSpan( line, charPositionInLine, line, charPositionInLine )
                                                      , msg
                                                      , e
                                                      )
                                     );
        }

        public void SyntaxError( [NotNull] IRecognizer recognizer
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
                                                      , new SourceSpan( line, charPositionInLine, line, charPositionInLine )
                                                      , msg
                                                      , e
                                                      )
                                     );
        }

        private readonly IParseErrorListener InnerListener;
    }
}
