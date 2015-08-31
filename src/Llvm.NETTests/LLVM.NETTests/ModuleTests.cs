using Microsoft.VisualStudio.TestTools.UnitTesting;
using Llvm.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llvm.NET.Tests
{
    [TestClass]
    public class ModuleTests
    {
        private Context Ctx;

        [TestInitialize]
        public void TestMethodInit()
        {
            Ctx = Context.CreateThreadContext( );
        }

        [TestCleanup]
        public void TestMethodCleanup()
        {
            Ctx.Dispose( );
            Ctx = null;
        }

        [TestMethod]
        public void DisposeTest( )
        {
            var module = Ctx.CreateModule( "test " );
            module.Dispose( );
        }

        [TestMethod]
        public void LinkTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void VerifyTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void GetFunctionTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddFunctionTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void WriteToFileTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AsStringTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddAliasTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddAliasTest1( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddGlobalTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddGlobalTest1( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddGlobalTest2( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void GetTypeByNameTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void GetNamedGlobalTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddModuleFlagTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddNamedMetadataOperandTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void AddVersionIdentMetadataTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void LoadFromTest( )
        {
            Assert.Inconclusive( );
        }
    }
}