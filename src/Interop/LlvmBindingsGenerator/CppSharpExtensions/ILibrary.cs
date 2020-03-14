// -----------------------------------------------------------------------
// <copyright file="ILibrary.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using CppSharp.AST;

namespace LlvmBindingsGenerator
{
    internal interface ILibrary
    {
        void Preprocess( ASTContext ctx );

        void Postprocess( ASTContext ctx );

        void Setup( IDriver driver );

        void SetupPasses( );

        IEnumerable<ICodeGenerator> CreateGenerators( );
    }
}
