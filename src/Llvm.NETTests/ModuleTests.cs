// <copyright file="ModuleTests.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using Llvm.NET.Instructions;
using Llvm.NET.Values;
using Llvm.NETTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    public class ModuleTests
    {
        private const string StructTestName = "struct.Test";
        private const string TestModuleName = "test";

        // To validate transformation to correct newline formatting
        // this must explicitly setup the string, using a file source
        // would cause git line ending transforms to impact the results
        // In order to have consistent indexed source symbols the automated
        // builds standardize on the single LineFeed character so the test
        // file would end up containing incorrect line endings for the test
        private const string TestModuleTemplate = "; ModuleID = '{1}'{0}"
                                                + "source_filename = \"test\"{0}"
                                                + "{0}"
                                                + "define void @foo() {{{0}"
                                                + "entry:{0}"
                                                + "  ret void{0}"
                                                + "}}{0}";

        [TestMethod]
        public void DefaultConstructorTest( )
        {
            using( var module = new BitcodeModule( ) )
            {
                Assert.AreSame( string.Empty, module.Name );
                Assert.AreSame( string.Empty, module.SourceFileName );
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
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                Assert.AreEqual( TestModuleName, module.Name );
                Assert.AreEqual( TestModuleName, module.SourceFileName );
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
            using( var module = new BitcodeModule( TestModuleName, DebugInfo.SourceLanguage.C99, "test.c", "unitTest", false, string.Empty, 0 ) )
            {
                Assert.AreEqual( TestModuleName, module.Name );
                Assert.AreEqual( "test.c", module.SourceFileName );
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
            using( var module = new BitcodeModule( TestModuleName ) )
            {
            }
        }

        [TestMethod]
        public void BasicLinkTest( )
        {
            // verifies linked modules can be disposed
            using( var ctx = new Context( ) )
            using( var module = new BitcodeModule( TestModuleName, ctx ) )
            using( var otherModule = new BitcodeModule( "Other", ctx ) )
            {
                module.Link( otherModule );
                Assert.IsTrue( otherModule.IsDisposed );
            }
        }

        [TestMethod]
        [ExpectedArgumentException( "otherModule", ExpectedExceptionMessage = "Linking modules from different contexts is not allowed" )]
        public void MultiContextLinkTest( )
        {
            using( var mergedMod = new BitcodeModule( ) )
            using( var m1 = CreateSimpleModule( "module1" ) )
            using( var m2 = CreateSimpleModule( "module2" ) )
            {
                Assert.AreNotSame( mergedMod.Context, m1.Context );
                Assert.AreNotSame( mergedMod.Context, m2.Context );
                mergedMod.Link( m1 ); // exception expected here.
            }
        }

        [TestMethod]
        public void ModuleCloneInContextTest( )
        {
            var m1 = CreateSimpleModule( "module1" );
            using( var context = new Context( ) )
            {
                var m2 = m1.Clone( context );
                Assert.AreNotSame( context, m1 );
                Assert.IsNotNull( m2 );
                Assert.AreSame( context, m2.Context );
            }
        }

        [TestMethod]
        public void MultiContextCloneLinkTest( )
        {
            using( var mergedMod = new BitcodeModule( ) )
            using( var m1 = CreateSimpleModule( "module1" ) )
            using( var m2 = CreateSimpleModule( "module2" ) )
            {
                Assert.AreNotSame( mergedMod.Context, m1.Context );
                Assert.AreNotSame( mergedMod.Context, m2.Context );
                var clone1 = m1.Clone( mergedMod.Context );
                var clone2 = m2.Clone( mergedMod.Context );
                GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced, true );
                mergedMod.Link( clone1 );
                GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced, true );
                Assert.IsTrue( mergedMod.Verify( out string errMsg ), errMsg );
                Assert.AreEqual( 1, mergedMod.Functions.Count( ) );
                mergedMod.Link( clone2 );
                GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced, true );
                Assert.IsTrue( mergedMod.Verify( out errMsg ), errMsg );
                Assert.AreEqual( 2, mergedMod.Functions.Count( ) );
            }
        }

        [TestMethod]
        public void VerifyValidModuleTest( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                Function testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );

                // verify basics
                Assert.IsNotNull( testFunc );
                bool isValid = module.Verify( out string msg );
                Assert.IsTrue( isValid );
                Assert.AreEqual( string.Empty, msg );
            }
        }

        [TestMethod]
        public void VerifyInvalidModuleTest( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                Function testFunc = CreateInvalidFunction( module, "badfunc" );

                // verify basics
                Assert.IsNotNull( testFunc );
                bool isValid = module.Verify( out string msg );
                Assert.IsFalse( isValid );
                Assert.IsNotNull( msg );
                /* while verifying the contents of the message might be a normal test
                   it comes from the underlying wrapped LLVM and is subject to change
                   by the LLVM team and is therefore outside the control of LLVM.NET
                */
            }
        }

        [TestMethod]
        public void AddFunctionGetFunctionTest( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
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
            string path = Path.GetTempFileName( );
            try
            {
                using( var module = new BitcodeModule( TestModuleName ) )
                {
                    Function testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );
                    module.WriteToFile( path );
                }

                using( var ctx = new Context( ) )
                using( var module2 = BitcodeModule.LoadFrom( path, ctx ) )
                {
                    Function testFunc = module2.GetFunction( "foo" );

                    // verify basics
                    Assert.IsNotNull( testFunc );
                    string txt = module2.WriteToString( );
                    Assert.IsFalse( string.IsNullOrWhiteSpace( txt ) );
                    string expectedText = string.Format( TestModuleTemplate, Environment.NewLine, path );
                    Assert.AreEqual( expectedText, txt );
                }
            }
            finally
            {
                File.Delete( path );
            }
        }

        [TestMethod]
        public void AsStringTest( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                Function testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );

                // verify basics
                Assert.IsNotNull( testFunc );
                string txt = module.WriteToString( );
                Assert.IsFalse( string.IsNullOrWhiteSpace( txt ) );
                string expectedText = string.Format( TestModuleTemplate, Environment.NewLine, "test" );
                Assert.AreEqual( expectedText, txt );
            }
        }

        [TestMethod]
        public void AddAliasGetAliasTest( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
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
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                module.AddGlobal( module.Context.Int32Type, "TestInt" );
                GlobalVariable globalVar = module.GetNamedGlobal( "TestInt" );
                Assert.AreEqual( "TestInt", globalVar.Name );
                Assert.AreSame( module.Context.Int32Type.CreatePointerType( ), globalVar.NativeType );
            }
        }

        [TestMethod]
        public void AddGlobalTest1( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                // unnamed global
                module.AddGlobal( module.Context.Int32Type, true, Linkage.WeakODR, module.Context.CreateConstant( 0x12345678 ) );
                var globalVar = module.Globals.First( ) as GlobalVariable;
                Assert.IsNotNull( globalVar );
                Assert.IsTrue( string.IsNullOrWhiteSpace( globalVar.Name ) );
                Assert.AreSame( module.Context.Int32Type.CreatePointerType( ), globalVar.NativeType );
                Assert.AreSame( module.Context.Int32Type, globalVar.Initializer.NativeType );
                Assert.AreEqual( Linkage.WeakODR, globalVar.Linkage );
                Assert.IsTrue( globalVar.IsConstant );
                Assert.IsInstanceOfType( globalVar.Initializer, typeof( ConstantInt ) );
                var constInt = ( ConstantInt )globalVar.Initializer;
                Assert.AreEqual( 0x12345678, constInt.SignExtendedValue );
            }
        }

        [TestMethod]
        public void AddGlobalTest2( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                module.AddGlobal( module.Context.Int32Type, true, Linkage.WeakODR, module.Context.CreateConstant( 0x12345678 ), "TestInt" );
                GlobalVariable globalVar = module.GetNamedGlobal( "TestInt" );
                Assert.AreEqual( "TestInt", globalVar.Name );
                Assert.AreSame( module.Context.Int32Type.CreatePointerType( ), globalVar.NativeType );
                Assert.AreSame( module.Context.Int32Type, globalVar.Initializer.NativeType );
                Assert.AreEqual( Linkage.WeakODR, globalVar.Linkage );
                Assert.IsTrue( globalVar.IsConstant );
                Assert.IsInstanceOfType( globalVar.Initializer, typeof( ConstantInt ) );
                var constInt = ( ConstantInt )globalVar.Initializer;
                Assert.AreEqual( 0x12345678, constInt.SignExtendedValue );
            }
        }

        [TestMethod]
        public void GetTypeByNameTest( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
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
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                module.AddModuleFlag( ModuleFlagBehavior.Warning, BitcodeModule.DwarfVersionValue, 4 );
                module.AddModuleFlag( ModuleFlagBehavior.Warning, BitcodeModule.DebugVersionValue, BitcodeModule.DebugMetadataVersion );
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

        [TestMethod]
        public void ComdatDataTest( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                const string comdatName = "testcomdat";
                const string globalName = "globalwithcomdat";

                module.Comdats.InsertOrUpdate( comdatName, ComdatKind.SameSize );
                Assert.AreEqual( 1, module.Comdats.Count );
                module.AddGlobal( module.Context.Int32Type, globalName )
                      .Linkage( Linkage.LinkOnceAny )
                      .Comdat( globalName );

                Assert.AreEqual( 2, module.Comdats.Count, "Unsaved module should have all comdats even if unused" );
                Assert.IsTrue( module.Comdats.Contains( comdatName ) );
                Assert.IsTrue( module.Comdats.Contains( globalName ) );
                Assert.AreEqual( comdatName, module.Comdats[ comdatName ].Name );
                Assert.AreEqual( globalName, module.Comdats[ globalName ].Name );
                Assert.AreEqual( ComdatKind.SameSize, module.Comdats[ comdatName ].Kind );
                Assert.AreEqual( ComdatKind.Any, module.Comdats[ globalName ].Kind );

                using( var context2 = new Context( ) )
                {
                    var clone = module.Clone( context2 );
                    Assert.AreEqual( 1, clone.Comdats.Count, "Comdat count should contain the one and only referenced comdat after save/clone" );
                    Assert.IsTrue( clone.Comdats.Contains( globalName ), "Cloned module should have the referenced comdat" );
                    var clonedGlobal = clone.GetNamedGlobal( globalName );
                    Assert.AreEqual( globalName, clonedGlobal.Comdat.Name, "Name of the comdat on the cloned global should match the one set in the original module" );
                    Assert.AreEqual( ComdatKind.Any, module.Comdats[ globalName ].Kind );
                }
            }
        }

        [TestMethod]
        public void ComdatFunctionTest( )
        {
            using( var module = new BitcodeModule( TestModuleName ) )
            {
                const string comdatName = "testcomdat";
                const string globalName = "globalwithcomdat";

                Comdat comdat = module.Comdats.InsertOrUpdate( comdatName, ComdatKind.SameSize );
                Assert.AreEqual( comdatName, comdat.Name );
                Assert.AreEqual( ComdatKind.SameSize, comdat.Kind );
                Assert.AreEqual( 1, module.Comdats.Count );
                CreateSimpleVoidNopTestFunction( module, globalName )
                    .Linkage( Linkage.LinkOnceODR )
                    .Comdat( globalName );

                Assert.AreEqual( 2, module.Comdats.Count, "Unsaved module should have all comdats even if unused" );
                Assert.IsTrue( module.Comdats.Contains( comdatName ) );
                Assert.IsTrue( module.Comdats.Contains( globalName ) );
                Assert.AreEqual( comdatName, module.Comdats[ comdatName ].Name );
                Assert.AreEqual( globalName, module.Comdats[ globalName ].Name );
                Assert.AreEqual( ComdatKind.SameSize, module.Comdats[ comdatName ].Kind );
                Assert.AreEqual( ComdatKind.Any, module.Comdats[ globalName ].Kind );

                using( var context2 = new Context( ) )
                {
                    var clone = module.Clone( context2 );
                    Assert.AreEqual( 1, clone.Comdats.Count, "Comdat count should contain the one and only referenced comdat after save/clone" );
                    Assert.IsTrue( clone.Comdats.Contains( globalName ), "Cloned module should have the referenced comdat" );
                    var clonedGlobal = clone.GetFunction( globalName );
                    Assert.AreEqual( globalName, clonedGlobal.Comdat.Name, "Name of the comdat on the cloned global should match the one set in the original module" );
                    Assert.AreEqual( ComdatKind.Any, module.Comdats[ globalName ].Kind );
                }
            }
        }

        private BitcodeModule CreateSimpleModule( string name, Context ctx = null )
        {
            var retVal = new BitcodeModule( name, ctx );
            CreateSimpleVoidNopTestFunction( retVal, name );
            return retVal;
        }

        private static Function CreateSimpleVoidNopTestFunction( BitcodeModule module, string name )
        {
            var ctx = module.Context;
            Assert.IsNotNull( ctx );
            var testFunc = module.AddFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            testFunc.AppendBasicBlock( "entry" );
            var irBuilder = new InstructionBuilder( testFunc.EntryBlock );
            irBuilder.Return( );
            return testFunc;
        }

        private static Function CreateInvalidFunction( BitcodeModule module, string name )
        {
            var ctx = module.Context;

            var testFunc = module.AddFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            testFunc.AppendBasicBlock( "entry" );

            // UNTERMINATED BLOCK INTENTIONAL
            return testFunc;
        }
    }
}
