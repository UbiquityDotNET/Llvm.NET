// -----------------------------------------------------------------------
// <copyright file="KaleidoscopeParser.ExternalDeclarationContext.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Kaleidoscope.Grammar.ANTLR
{
    public partial class KaleidoscopeParser
    {
        public partial class ExternalDeclarationContext
        {
            public PrototypeContext Signature => prototype( );
        }
    }
}
