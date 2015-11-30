using Llvm.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NETTests
{
    /// <summary>Provides common location for one time initalization for all tests in this assembly</summary>
    [TestClass]
    public static class AssemblyInitialize
    {
        /// <summary>Initializes Llvm.NET state for use with all available targets</summary>
        /// <param name="ctx">Context for the test run</param>
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext ctx)
        {
            StaticState.RegisterAll( );
        }
    }
}
