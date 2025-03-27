// -----------------------------------------------------------------------
// <copyright file="IParseErrorListener.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Runtime.Utils
{
    public interface IParseErrorListener
    {
        void SyntaxError( SyntaxError syntaxError );
    }
}
