// <copyright file="AssemblyInitialize.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET;
using Llvm.NET.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Unit Tests" )]

namespace Llvm.NETTests
{
    // Provides common location for one time initialization for all tests in this assembly
    [TestClass]
    public static class ModuleFixtures
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize( TestContext ctx )
        {
            if( ctx == null )
            {
                throw new ArgumentNullException( nameof( ctx ) );
            }

            LlvmInit = Library.InitializeLLVM( );
            Library.RegisterAll( );
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup( ) => LlvmInit.Dispose( );

        private static IDisposable LlvmInit;
    }
}
