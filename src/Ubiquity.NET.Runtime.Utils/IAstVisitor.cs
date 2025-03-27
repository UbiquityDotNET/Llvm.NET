// -----------------------------------------------------------------------
// <copyright file="IAstVisitor.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Runtime.Utils
{
    public interface IAstVisitor<out TResult>
    {
        TResult? Visit(IAstNode node);
    }

    public interface IAstVisitor<out TResult, TArg>
        where TArg : struct, allows ref struct
    {
        TResult? Visit(IAstNode node, ref readonly TArg arg);
    }
}
