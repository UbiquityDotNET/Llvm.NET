// -----------------------------------------------------------------------
// <copyright file="IDriver.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using CppSharp;
using CppSharp.AST;
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

        ITypePrinter TypePrinter { get; set; }
    }

    internal static class DriverExtensions
    {
        public static void AddTranslationUnitPass( this IDriver driver, TranslationUnitPass pass )
        {
            driver.Context.TranslationUnitPasses.AddPass( pass );
        }
    }
}
