// -----------------------------------------------------------------------
// <copyright file="IParseErrorListener.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar
{
    public interface IParseErrorListener
    {
        void SyntaxError( SyntaxError syntaxError );
    }
}
