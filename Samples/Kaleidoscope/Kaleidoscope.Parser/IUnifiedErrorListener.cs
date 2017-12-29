// <copyright file="IUnifiedErrorListener.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Antlr4.Runtime;

namespace Kaleidoscope.Grammar
{
    /// <summary>Interface for a combined Lexer and parser error listener</summary>
    public interface IUnifiedErrorListener
        : IAntlrErrorListener<int>
        , IAntlrErrorListener<IToken>
    {
    }
}
