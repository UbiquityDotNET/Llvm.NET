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
        [TestMethod]
        public void DisposeTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            using( var module = ctx.CreateModule( "test" ) )
            {
            }
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
            using( var ctx = Context.CreateThreadContext( ) )
            using( var module = ctx.CreateModule( "test" ) )
            {
                // create a fully defined function to make an alias to 
                var testFunc = module.AddFunction( "_test", ctx.GetFunctionType( ctx.VoidType ) );
                testFunc.AppendBasicBlock( "entry" );
                var irBuilder = new InstructionBuilder( testFunc.EntryBlock );
                irBuilder.Return( );

                var alias = module.AddAlias( testFunc, "test" );
                Assert.AreSame( alias, module.GetAlias( "test" ) );
                Assert.AreSame( module, alias.ParentModule );
                Assert.AreSame( testFunc, alias.Aliasee );
                Assert.AreEqual( "test", alias.Name );
                Assert.AreEqual( Linkage.External, alias.Linkage );
                Assert.AreSame( testFunc.Type, alias.Type );
                
                // alias.Operands[ 0 ] is just another way to get alias.Aliasee
                Assert.AreEqual( 1, alias.Operands.Count );
                Assert.AreSame( testFunc, alias.Operands[ 0 ] );

                Assert.IsFalse( alias.IsNull );
                Assert.IsFalse( alias.IsUndefined );
                Assert.IsFalse( alias.IsZeroValue );
            }
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