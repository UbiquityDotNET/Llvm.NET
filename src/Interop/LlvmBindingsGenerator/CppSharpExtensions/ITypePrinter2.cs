// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using CppSharp.AST;

namespace LlvmBindingsGenerator
{
    internal enum TypeNameKind
    {
        Native,
        Managed,
    }

    internal interface ITypePrinter2
        : ITypePrinter
    {
        string GetName( CppSharp.AST.Type t, TypeNameKind kind = TypeNameKind.Native );
    }
}
