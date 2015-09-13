using System.IO;
using Llvm.NET.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    [DeploymentItem("LibLLVM.dll")]
    public class ModuleTests
    {
        [TestMethod]
        public void DisposeTest( )
        {
            using( var module = new Module( "test" ) )
            {
            }
        }

        [TestMethod]
        public void LinkTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void VerifyValidModuleTest( )
        {
            using( var module = new Module( "test" ) )
            {
                Function testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );
                // verify basics
                Assert.IsNotNull( testFunc );
                string msg;
                var isValid = module.Verify( out msg );
                Assert.IsTrue( isValid );
                Assert.IsNull( msg );
            }
        }

        [TestMethod]
        public void VerifyInvalidModuleTest( )
        {
            using( var module = new Module( "test" ) )
            {
                Function testFunc = CreateInvalidFunction( module, "badfunc" );
                // verify basics
                Assert.IsNotNull( testFunc );
                string msg;
                var isValid = module.Verify( out msg );
                Assert.IsFalse( isValid );
                Assert.IsNotNull( msg );
                // while verifying the contents of the message might be a normal test
                // it comes from the underlying wrapped LLVM and is subject to change
                // by the LLVM team and is therefore outside the control of LLVM.NET
            }
        }

        [TestMethod]
        public void AddFunctionGetFunctionTest( )
        {
            using( var module = new Module( "test" ) )
            {
                Function testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );
                // verify basics
                Assert.IsNotNull( testFunc );
                Assert.AreSame( module, testFunc.ParentModule );
                Assert.AreEqual( "foo", testFunc.Name );

                // Verify the function is in the module
                var funcFromModule = module.GetFunction( "foo" );
                Assert.AreSame( testFunc, funcFromModule );
            }
        }

        [TestMethod]
        public void WriteToFileTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        [DeploymentItem("TestModuleAsString.ll")]
        public void AsStringTest( )
        {
            using( var module = new Module( "test" ) )
            {
                Function testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );
                // verify basics
                Assert.IsNotNull( testFunc );
                var txt = module.AsString( );
                Assert.IsFalse( string.IsNullOrWhiteSpace( txt ) );
                var expectedText = File.ReadAllText( "TestModuleAsString.ll" );
                Assert.AreEqual( expectedText, txt );
            }
        }

        [TestMethod]
        public void AddAliasGetAliasTest( )
        {
            using( var module = new Module( "test" ) )
            {
                Function testFunc = CreateSimpleVoidNopTestFunction( module, "_test" );

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

        private static Function CreateSimpleVoidNopTestFunction( Module module, string name )
        {
            var ctx = module.Context;
            Assert.IsNotNull( ctx );
            var testFunc = module.AddFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            testFunc.AppendBasicBlock( "entry" );
            var irBuilder = new InstructionBuilder( testFunc.EntryBlock );
            irBuilder.Return( );
            return testFunc;
        }

        private static Function CreateInvalidFunction( Module module, string name )
        {
            var ctx = module.Context;

            var testFunc = module.AddFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            testFunc.AppendBasicBlock( "entry" );
            // UNTERMINATED BLOCK INTENTIONAL
            return testFunc;
        }
    }
}