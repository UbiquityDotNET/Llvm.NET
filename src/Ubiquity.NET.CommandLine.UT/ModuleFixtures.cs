// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

#if USE_MODULE_FIXTURES
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.NET.CommandLine.UT
{
    // Provides common location for one time initialization for all tests in this assembly
    [TestClass]
    public static class ModuleFixtures
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize( TestContext ctx )
        {
            ArgumentNullException.ThrowIfNull( ctx );
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup( )
        {
        }
    }
}
#endif
