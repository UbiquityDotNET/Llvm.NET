// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

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
