using Llvm.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NETTests
{
    [TestClass]
    public static class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext ctx)
        {
            StaticState.RegisterAll( );
        }
    }
}
