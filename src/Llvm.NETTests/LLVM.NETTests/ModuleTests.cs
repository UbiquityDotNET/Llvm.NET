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
        private const string StructTestName = "struct.Test";
        private const string TestModuleName = "test";

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
            using( var module = new NativeModule( TestModuleName ) )
            {
                Assert.AreEqual( TestModuleName, module.Name );
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
            using( var module = new NativeModule( TestModuleName, DebugInfo.SourceLanguage.C99, "test.c", "unitTest", false, string.Empty, 0 ) )
            {
                Assert.AreEqual( TestModuleName, module.Name );
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
            using( var module = new NativeModule( TestModuleName ) )
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
            using( var module = new NativeModule( TestModuleName ) )
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
            using( var module = new NativeModule( TestModuleName ) )
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
            using( var module = new NativeModule( TestModuleName ) )
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
            using( var module = new NativeModule( TestModuleName ) )
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
            using( var module = new NativeModule( TestModuleName ) )
            {
                Function testFunc = CreateSimpleVoidNopTestFunction( module, "_test" );

                var alias = module.AddAlias( testFunc, TestModuleName );
                Assert.AreSame( alias, module.GetAlias( TestModuleName ) );
                Assert.AreSame( module, alias.ParentModule );
                Assert.AreSame( testFunc, alias.Aliasee );
                Assert.AreEqual( TestModuleName, alias.Name );
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
            using( var module = new NativeModule( TestModuleName ) )
            {
                module.AddGlobal( module.Context.Int32Type, "TestInt" );
                GlobalVariable globalVar = module.GetNamedGlobal( "TestInt" );
                Assert.AreEqual( "TestInt", globalVar.Name );
                Assert.AreSame( module.Context.Int32Type.CreatePointerType(), globalVar.NativeType );
            }
        }

        [TestMethod]
        public void AddGlobalTest1( )
        {
            using( var module = new NativeModule( TestModuleName ) )
            {
                // unnamed global
                module.AddGlobal( module.Context.Int32Type, true, Linkage.WeakODR, module.Context.CreateConstant( 0x12345678 ) );
                var globalVar = module.Globals.First( ) as GlobalVariable;
                Assert.IsNotNull( globalVar );
                Assert.IsTrue( string.IsNullOrWhiteSpace( globalVar.Name ) );
                Assert.AreSame( module.Context.Int32Type.CreatePointerType(), globalVar.NativeType );
                Assert.AreSame( module.Context.Int32Type, globalVar.Initializer.NativeType );
                Assert.AreEqual( Linkage.WeakODR, globalVar.Linkage );
                Assert.IsTrue( globalVar.IsConstant );
                Assert.IsInstanceOfType( globalVar.Initializer, typeof( ConstantInt ) );
                var constInt = ( ConstantInt )globalVar.Initializer;
                Assert.AreEqual( ( long )0x12345678, constInt.SignExtendedValue );
            }
        }

        [TestMethod]
        public void AddGlobalTest2( )
        {
            using( var module = new NativeModule( TestModuleName ) )
            {
                module.AddGlobal( module.Context.Int32Type, true, Linkage.WeakODR, module.Context.CreateConstant( 0x12345678 ), "TestInt" );
                GlobalVariable globalVar = module.GetNamedGlobal( "TestInt" );
                Assert.AreEqual( "TestInt", globalVar.Name );
                Assert.AreSame( module.Context.Int32Type.CreatePointerType(), globalVar.NativeType );
                Assert.AreSame( module.Context.Int32Type, globalVar.Initializer.NativeType );
                Assert.AreEqual( Linkage.WeakODR, globalVar.Linkage );
                Assert.IsTrue( globalVar.IsConstant );
                Assert.IsInstanceOfType( globalVar.Initializer, typeof( ConstantInt ) );
                var constInt = ( ConstantInt )globalVar.Initializer;
                Assert.AreEqual( ( long )0x12345678, constInt.SignExtendedValue );
            }
        }

        [TestMethod]
        public void GetTypeByNameTest( )
        {
            using( var module = new NativeModule( TestModuleName ) )
            {
                // while GetTypeByName is exposed on the module it isn't really specific to the module
                // That is, the type belongs to the context and GetTypeByName() is just a convenience
                // wrapper to access types for a module.
                var type = module.GetTypeByName( StructTestName );
                Assert.IsNull( type );
                var expectedType = module.Context.CreateStructType( StructTestName );
                var actualType = module.GetTypeByName( StructTestName );
                Assert.AreSame( expectedType, actualType );
            }
        }

        [TestMethod]
        public void AddModuleFlagTest( )
        {
            using( var module = new NativeModule( TestModuleName ) )
            {
                module.AddModuleFlag( ModuleFlagBehavior.Warning, NativeModule.DwarfVersionValue, 4 );
                module.AddModuleFlag( ModuleFlagBehavior.Warning, NativeModule.DebugVersionValue, NativeModule.DebugMetadataVersion );
                module.AddModuleFlag( ModuleFlagBehavior.Error, "wchar_size", 4 );
                module.AddModuleFlag( ModuleFlagBehavior.Error, "min_enum_size", 4 );
                module.AddVersionIdentMetadata( "unit-tests 1.0" );
                // currently no exposed means to get module level flags...
                // so at this point as long as adding the flags doesn't throw an exception
                // assume things are OK.
            }
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