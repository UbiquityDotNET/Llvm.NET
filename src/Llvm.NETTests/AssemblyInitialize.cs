// <copyright file="AssemblyInitialize.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Llvm.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NETTests
{
    /// <summary>Provides common location for one time initialization for all tests in this assembly</summary>
    [TestClass]
    public static class AssemblyInitialize
    {
        /// <summary>Initializes Llvm.NET state for use with all available targets</summary>
        /// <param name="ctx">Context for the test run</param>
        [AssemblyInitialize]
        [SuppressMessage( "Redundancies in Symbol Declarations", "RECS0154:Parameter is never used", Justification = "Not needed and signature is defined by test framework" )]
        public static void InitializeAssembly(TestContext ctx)
        {
            LlvmInit = StaticState.InitializeLLVM( );
            StaticState.RegisterAll( );
        }

        [AssemblyCleanup]
        public static void UninitializeAssembly()
        {
            LlvmInit.Dispose( );
        }

        private static IDisposable LlvmInit;
    }
}
