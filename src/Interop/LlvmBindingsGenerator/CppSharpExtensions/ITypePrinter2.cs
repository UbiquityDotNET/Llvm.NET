// -----------------------------------------------------------------------
// <copyright file="ITypePrinter2.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

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
