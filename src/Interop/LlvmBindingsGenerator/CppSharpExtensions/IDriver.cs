// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using CppSharp;
using CppSharp.Generators;
using CppSharp.Parser;
using CppSharp.Passes;

namespace LlvmBindingsGenerator
{
    internal interface IDriver
        : IDisposable
    {
        DriverOptions Options { get; }

        ParserOptions ParserOptions { get; }

        BindingContext Context { get; }
    }

    internal static class DriverExtensions
    {
        public static void AddTranslationUnitPass( this IDriver driver, TranslationUnitPass pass )
        {
            driver.Context.TranslationUnitPasses.AddPass( pass );
        }
    }
}
