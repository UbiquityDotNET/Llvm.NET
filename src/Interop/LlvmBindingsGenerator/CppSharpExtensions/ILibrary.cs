// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
