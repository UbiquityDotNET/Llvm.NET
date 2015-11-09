using System.IO;
using System.Linq;
using Llvm.NET.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    [DeploymentItem("LibLLVM.dll")]
    public class ModuleTests
    {

        [TestMethod]
        public void DefaultConstructorTest( )
        {
            using( var module = new NativeModule( ) )
            {
                Assert.AreSame( string.Empty, module.Name );
                Assert.IsNotNull( module );
                Assert.IsNotNull( module.Context );
                Assert.AreSame( string.Empty, module.DataLayoutString );
                Assert.IsNull( module.Layout );
                Assert.AreSame( string.Empty, module.TargetTriple );
                Assert.IsNotNull( module.DIBuilder );

                // until explicitly created DICompileUnit should be null
                Assert.IsNull( module.DICompileUnit );
                
                // Functions collection should be valid but empty
                Assert.IsNotNull( module.Functions );
                Assert.IsFalse( module.Functions.Any( ) );

                // Globals collection should be valid but empty
                Assert.IsNotNull( module.Globals );
                Assert.IsFalse( module.Globals.Any( ) );
            }
        }

        [TestMethod]
        public void ConstructorTestWithName( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                Assert.AreEqual( "test", module.Name );
                Assert.IsNotNull( module );
                Assert.IsNotNull( module.Context );
                Assert.AreSame( string.Empty, module.DataLayoutString );
                Assert.IsNull( module.Layout );
                Assert.AreSame( string.Empty, module.TargetTriple );
                Assert.IsNotNull( module.DIBuilder );

                // until explicitly created DICompileUnit should be null
                Assert.IsNull( module.DICompileUnit );
                
                // Functions collection should be valid but empty
                Assert.IsNotNull( module.Functions );
                Assert.IsFalse( module.Functions.Any( ) );

                // Globals collection should be valid but empty
                Assert.IsNotNull( module.Globals );
                Assert.IsFalse( module.Globals.Any( ) );
            }
        }

        [TestMethod]
        public void ConstructorTestWithNameAndCompileUnit( )
        {
            using( var module = new NativeModule( "test", DebugInfo.SourceLanguage.C99, "test.c", "unitTest", false, string.Empty, 0 ) )
            {
                Assert.AreEqual( "test", module.Name );
                Assert.IsNotNull( module );
                Assert.IsNotNull( module.Context );
                Assert.AreSame( string.Empty, module.DataLayoutString );
                Assert.IsNull( module.Layout );
                Assert.AreSame( string.Empty, module.TargetTriple );
                Assert.IsNotNull( module.DIBuilder );
                Assert.IsNotNull( module.DICompileUnit );
                
                // Functions collection should be valid but empty
                Assert.IsNotNull( module.Functions );
                Assert.IsFalse( module.Functions.Any( ) );

                // Globals collection should be valid but empty
                Assert.IsNotNull( module.Globals );
                Assert.IsFalse( module.Globals.Any( ) );
            }
        }

        [TestMethod]
        public void DisposeTest( )
        {
            using( var module = new NativeModule( "test" ) )
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
            using( var module = new NativeModule( "test" ) )
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
            using( var module = new NativeModule( "test" ) )
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
            using( var module = new NativeModule( "test" ) )
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
            using( var module = new NativeModule( "test" ) )
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
            using( var module = new NativeModule( "test" ) )
            {
                Function testFunc = CreateSimpleVoidNopTestFunction( module, "_test" );

                var alias = module.AddAlias( testFunc, "test" );
                Assert.AreSame( alias, module.GetAlias( "test" ) );
                Assert.AreSame( module, alias.ParentModule );
                Assert.AreSame( testFunc, alias.Aliasee );
                Assert.AreEqual( "test", alias.Name );
                Assert.AreEqual( Linkage.External, alias.Linkage );
                Assert.AreSame( testFunc.NativeType, alias.NativeType );

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

        private static Function CreateSimpleVoidNopTestFunction( NativeModule module, string name )
        {
            var ctx = module.Context;
            Assert.IsNotNull( ctx );
            var testFunc = module.AddFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            testFunc.AppendBasicBlock( "entry" );
            var irBuilder = new InstructionBuilder( testFunc.EntryBlock );
            irBuilder.Return( );
            return testFunc;
        }

        private static Function CreateInvalidFunction( NativeModule module, string name )
        {
            var ctx = module.Context;

            var testFunc = module.AddFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            testFunc.AppendBasicBlock( "entry" );
            // UNTERMINATED BLOCK INTENTIONAL
            return testFunc;
        }
    }
}