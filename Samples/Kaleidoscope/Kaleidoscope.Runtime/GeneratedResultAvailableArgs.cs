// <copyright file="ReplLoop.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Kaleidoscope.Runtime
{
    public class GeneratedResultAvailableArgs<TResult>
        : EventArgs
    {
        public GeneratedResultAvailableArgs( TResult result, Parser recognizer, IParseTree parseTree )
        {
            Result = result;
            ParseTree = parseTree;
            Recognizer = recognizer;
        }

        public TResult Result { get; }

        public IParseTree ParseTree { get; }

        public Parser Recognizer { get; }
    }
}
