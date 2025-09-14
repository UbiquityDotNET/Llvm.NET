// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Reflection;

namespace LlvmBindingsGenerator.Templates
{
    internal static class AssemblyExtensions
    {
        public static string GetAssemblyInformationalVersion( this Assembly asm )
        {
            var attr = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return attr?.InformationalVersion ?? throw new InvalidOperationException("Assembly does not have in information version");
        }
    }
}
