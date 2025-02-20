// -----------------------------------------------------------------------
// <copyright file="FieldExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CppSharp.AST;

namespace LlvmBindingsGenerator
{
    internal static class FieldExtensions
    {
        public static bool IsInlinedArray(this Field f)
        {
            return f.Type is ArrayType at && at.SizeType == ArrayType.ArraySize.Constant;
        }
    }
}
