﻿// -----------------------------------------------------------------------
// <copyright file="ModuleTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Values;
using Ubiquity.NET.LlvmTests;

namespace Ubiquity.NET.Llvm.Tests
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
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( );
            Assert.AreSame( string.Empty, module.Name );
            Assert.AreSame( string.Empty, module.SourceFileName );
            Assert.IsNotNull( module );
            Assert.IsNotNull( module.Context );
            Assert.AreSame( string.Empty, module.DataLayoutString );
            Assert.IsNotNull( module.Layout );
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

        [TestMethod]
        public void ConstructorTestWithName( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            Assert.AreEqual( TestModuleName, module.Name );
            Assert.AreEqual( TestModuleName, module.SourceFileName );
            Assert.IsNotNull( module );
            Assert.IsNotNull( module.Context );
            Assert.AreSame( string.Empty, module.DataLayoutString );
            Assert.IsNotNull( module.Layout );
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

        [TestMethod]
        public void ConstructorTestWithNameAndCompileUnit( )
        {
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( TestModuleName, SourceLanguage.C99, "test.c", "unitTest", false, string.Empty );
            Assert.AreEqual( TestModuleName, module.Name );
            Assert.AreEqual( "test.c", module.SourceFileName );
            Assert.IsNotNull( module );
            Assert.IsNotNull( module.Context );
            Assert.AreSame( string.Empty, module.DataLayoutString );
            Assert.IsNotNull( module.Layout );
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

        [TestMethod]
        public void DisposeTest( )
        {
            using( var context = new Context( ) )
            using( context.CreateBitcodeModule( TestModuleName ) )
            {
            }
        }

        [TestMethod]
        public void BasicLinkTest( )
        {
            // verifies linked modules can be disposed
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( TestModuleName );
            using var otherModule = ctx.CreateBitcodeModule( "Other" );
            module.Link( otherModule );
            Assert.IsTrue( otherModule.IsDisposed );
        }

        [TestMethod]
        [ExpectedArgumentException( "otherModule", ExpectedExceptionMessage = "Linking modules from different contexts is not allowed" )]
        public void MultiContextLinkTest( )
        {
            using var context = new Context( );
            using var mergedMod = context.CreateBitcodeModule( );
            using var contextM1 = new Context( );
            using var m1 = CreateSimpleModule( contextM1, "module1" );
            using var contextM2 = new Context( );
            using var m2 = CreateSimpleModule( contextM2, "module2" );
            Assert.AreNotSame( mergedMod.Context, m1.Context );
            Assert.AreNotSame( mergedMod.Context, m2.Context );
            mergedMod.Link( m1 ); // exception expected here.
        }

        [TestMethod]
        public void ModuleCloneInContextTest( )
        {
            using var context1 = new Context( );
            using var m1 = CreateSimpleModule( context1, "module1" );
            using var context2 = new Context( );
            var m2 = m1.Clone( context2 );
            Assert.AreNotSame( context2, m1 );
            Assert.IsNotNull( m2 );
            Assert.AreSame( context2, m2.Context );
        }

        [TestMethod]
        public void MultiContextCloneLinkTest( )
        {
            using var context = new Context( );
            using var mergedMod = context.CreateBitcodeModule( );
            using var contextM1 = new Context( );
            using var m1 = CreateSimpleModule( contextM1, "module1" );
            using var contextM2 = new Context( );
            using var m2 = CreateSimpleModule( contextM2, "module2" );
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

        [TestMethod]
        public void VerifyValidModuleTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            IrFunction testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );

            // verify basics
            Assert.IsNotNull( testFunc );
            bool isValid = module.Verify( out string msg );
            Assert.IsTrue( isValid );
            Assert.AreEqual( string.Empty, msg );
        }

        [TestMethod]
        public void VerifyInvalidModuleTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            IrFunction testFunc = CreateInvalidFunction( module, "badfunc" );

            // verify basics
            Assert.IsNotNull( testFunc );
            bool isValid = module.Verify( out string msg );
            Assert.IsFalse( isValid );
            Assert.IsNotNull( msg );
        }

        [TestMethod]
        public void AddFunctionGetFunctionTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            IrFunction testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );

            // verify basics
            Assert.IsNotNull( testFunc );
            Assert.AreSame( module, testFunc.ParentModule );
            Assert.AreEqual( "foo", testFunc.Name );

            // Verify the function is in the module, and getting it retrieves the same instance
            Assert.IsTrue( module.TryGetFunction( "foo", out IrFunction? funcFromModule ) );
            Assert.AreSame( testFunc, funcFromModule );
        }

        [TestMethod]
        public void WriteToFileTest( )
        {
            string path = Path.GetTempFileName( );
            try
            {
                using( var context = new Context( ) )
                using( var module = context.CreateBitcodeModule( TestModuleName ) )
                {
                    _ = CreateSimpleVoidNopTestFunction( module, "foo" );
                    module.WriteToFile( path );
                }

                using var ctx = new Context( );
                using var module2 = BitcodeModule.LoadFrom( path, ctx );

                // force a GC to ensure buffer created in LoadFrom is handled correctly
                GC.Collect( GC.MaxGeneration );
                Assert.IsTrue( module2.TryGetFunction( "foo", out IrFunction? testFunc ) );

                // verify basics
                Assert.IsNotNull( testFunc );
                string txt = module2.WriteToString( );
                Assert.IsFalse( string.IsNullOrWhiteSpace( txt ) );
                string expectedText = string.Format( CultureInfo.InvariantCulture, TestModuleTemplate, Environment.NewLine, path );
                Assert.AreEqual( expectedText, txt );
            }
            finally
            {
                File.Delete( path );
            }
        }

        [TestMethod]
        public void AsStringTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );

            IrFunction testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );

            // verify basics
            Assert.IsNotNull( testFunc );
            string txt = module.WriteToString( );
            Assert.IsFalse( string.IsNullOrWhiteSpace( txt ) );
            string expectedText = string.Format( CultureInfo.InvariantCulture, TestModuleTemplate, Environment.NewLine, "test" );
            Assert.AreEqual( expectedText, txt );
        }

        [TestMethod]
        public void AddAliasGetAliasTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            IrFunction testFunc = CreateSimpleVoidNopTestFunction( module, "_test" );

            var alias = module.AddAlias( testFunc, TestModuleName );
            Assert.AreSame( alias, module.GetAlias( TestModuleName ) );
            Assert.AreSame( module, alias.ParentModule );
            Assert.AreSame( testFunc, alias.Aliasee );
            Assert.AreEqual( TestModuleName, alias.Name );
            Assert.AreEqual( Linkage.External, alias.Linkage );
            Assert.AreSame( testFunc.NativeType, alias.NativeType );

            Assert.AreEqual( 1, alias.Operands.Count );
            Assert.AreSame( testFunc, alias.Aliasee );

            Assert.IsFalse( alias.IsNull );
            Assert.IsFalse( alias.IsUndefined );
            Assert.IsFalse( alias.IsZeroValue );
        }

        [TestMethod]
        public void AddGlobalTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            Assert.IsNotNull( module );

            module.AddGlobal( module.Context.Int32Type, "TestInt" );
            GlobalVariable? globalVar = module.GetNamedGlobal( "TestInt" );
            Assert.IsNotNull( globalVar );

            Assert.AreEqual( "TestInt", globalVar!.Name );
            Assert.AreSame( module.Context.Int32Type.CreatePointerType( ), globalVar.NativeType );
        }

        [TestMethod]
        public void AddGlobalTest1( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );

            // unnamed global
            module.AddGlobal( module.Context.Int32Type, true, Linkage.WeakODR, module.Context.CreateConstant( 0x12345678 ) );
            var globalVar = module.Globals.First( );
            Assert.IsNotNull( globalVar );
            Assert.IsNotNull( globalVar.Initializer );
            Assert.IsTrue( string.IsNullOrWhiteSpace( globalVar.Name ) );
            Assert.AreSame( module.Context.Int32Type.CreatePointerType( ), globalVar.NativeType );
            Assert.AreSame( module.Context.Int32Type, globalVar.Initializer!.NativeType );
            Assert.AreEqual( Linkage.WeakODR, globalVar.Linkage );
            Assert.IsTrue( globalVar.IsConstant );
            Assert.IsInstanceOfType( globalVar.Initializer, typeof( ConstantInt ) );

            var constInt = ( ConstantInt )globalVar.Initializer;
            Assert.AreEqual( 0x12345678, constInt.SignExtendedValue );
        }

        [TestMethod]
        public void AddGlobalTest2( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );

            module.AddGlobal( module.Context.Int32Type, true, Linkage.WeakODR, module.Context.CreateConstant( 0x12345678 ), "TestInt" );
            GlobalVariable? globalVar = module.GetNamedGlobal( "TestInt" );
            Assert.IsNotNull( globalVar );
            Assert.IsNotNull( globalVar!.Initializer );

            Assert.AreEqual( "TestInt", globalVar.Name );
            Assert.AreSame( module.Context.Int32Type.CreatePointerType( ), globalVar.NativeType );
            Assert.AreSame( module.Context.Int32Type, globalVar.Initializer!.NativeType );
            Assert.AreEqual( Linkage.WeakODR, globalVar.Linkage );
            Assert.IsTrue( globalVar.IsConstant );
            Assert.IsInstanceOfType( globalVar.Initializer, typeof( ConstantInt ) );

            var constInt = ( ConstantInt )globalVar.Initializer;
            Assert.AreEqual( 0x12345678, constInt.SignExtendedValue );
        }

        [TestMethod]
        public void GetTypeByNameTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );

            // while GetTypeByName is exposed on the module it isn't really specific to the module
            // That is, the type belongs to the context and GetTypeByName() is just a convenience
            // wrapper to access types for a module.
            var type = module.GetTypeByName( StructTestName );
            Assert.IsNull( type );

            var expectedType = module.Context.CreateStructType( StructTestName );
            var actualType = module.GetTypeByName( StructTestName );
            Assert.AreSame( expectedType, actualType );
        }

        [TestMethod]
        public void AddModuleFlagTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );

            module.AddModuleFlag( ModuleFlagBehavior.Warning, BitcodeModule.DwarfVersionValue, 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Warning, BitcodeModule.DebugVersionValue, BitcodeModule.DebugMetadataVersion );
            module.AddModuleFlag( ModuleFlagBehavior.Error, "wchar_size", 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Error, "min_enum_size", 4 );
            module.AddVersionIdentMetadata( "unit-tests 1.0" );

            Assert.AreEqual( 4, module.ModuleFlags.Count );
            Assert.IsTrue( module.ModuleFlags.ContainsKey( BitcodeModule.DwarfVersionValue ) );
            Assert.IsTrue( module.ModuleFlags.ContainsKey( BitcodeModule.DebugVersionValue ) );
            Assert.IsTrue( module.ModuleFlags.ContainsKey( "wchar_size" ) );
            Assert.IsTrue( module.ModuleFlags.ContainsKey( "min_enum_size" ) );

            var dwarfVerFlag = module.ModuleFlags[ BitcodeModule.DwarfVersionValue ];
            Assert.AreEqual( ModuleFlagBehavior.Warning, dwarfVerFlag.Behavior );
            Assert.AreEqual( BitcodeModule.DwarfVersionValue, dwarfVerFlag.Name );
            Assert.IsInstanceOfType( dwarfVerFlag.Metadata, typeof( ConstantAsMetadata ) );

            var dwarfVerConst = ( ( ConstantAsMetadata )dwarfVerFlag.Metadata ).Constant;
            Assert.IsInstanceOfType( dwarfVerConst, typeof( ConstantInt ) );
            Assert.AreEqual( 4UL, ( ( ConstantInt )dwarfVerConst ).ZeroExtendedValue );

            var debugVerFlag = module.ModuleFlags[ BitcodeModule.DebugVersionValue ];
            Assert.AreEqual( ModuleFlagBehavior.Warning, debugVerFlag.Behavior );
            Assert.AreEqual( BitcodeModule.DebugVersionValue, debugVerFlag.Name );
            Assert.IsInstanceOfType( debugVerFlag.Metadata, typeof( ConstantAsMetadata ) );

            var debugVerConst = ( ( ConstantAsMetadata )debugVerFlag.Metadata ).Constant;
            Assert.IsInstanceOfType( debugVerConst, typeof( ConstantInt ) );
            Assert.AreEqual( BitcodeModule.DebugMetadataVersion, ( ( ConstantInt )debugVerConst ).ZeroExtendedValue );

            var wcharSizeFlag = module.ModuleFlags[ "wchar_size" ];
            Assert.AreEqual( ModuleFlagBehavior.Error, wcharSizeFlag.Behavior );
            Assert.AreEqual( "wchar_size", wcharSizeFlag.Name );
            Assert.IsInstanceOfType( wcharSizeFlag.Metadata, typeof( ConstantAsMetadata ) );

            var wcharSizeConst = ( ( ConstantAsMetadata )wcharSizeFlag.Metadata ).Constant;
            Assert.IsInstanceOfType( wcharSizeConst, typeof( ConstantInt ) );
            Assert.AreEqual( 4UL, ( ( ConstantInt )wcharSizeConst ).ZeroExtendedValue );

            var minEnumSizeFlag = module.ModuleFlags[ "wchar_size" ];
            Assert.AreEqual( ModuleFlagBehavior.Error, minEnumSizeFlag.Behavior );
            Assert.AreEqual( "wchar_size", minEnumSizeFlag.Name );
            Assert.IsInstanceOfType( minEnumSizeFlag.Metadata, typeof( ConstantAsMetadata ) );

            var minEnumSizeConst = ( ( ConstantAsMetadata )minEnumSizeFlag.Metadata ).Constant;
            Assert.IsInstanceOfType( minEnumSizeConst, typeof( ConstantInt ) );
            Assert.AreEqual( 4UL, ( ( ConstantInt )minEnumSizeConst ).ZeroExtendedValue );
        }

        /*
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
        */
        [TestMethod]
        public void ComdatDataTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
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

            using var context2 = new Context( );
            var clone = module.Clone( context2 );
            Assert.AreEqual( 1, clone.Comdats.Count, "Comdat count should contain the one and only referenced comdat after save/clone" );
            Assert.IsTrue( clone.Comdats.Contains( globalName ), "Cloned module should have the referenced comdat" );

            var clonedGlobal = clone.GetNamedGlobal( globalName );
            Assert.IsNotNull( clonedGlobal );
            Assert.IsNotNull( clonedGlobal!.Comdat );

            Assert.AreEqual( globalName, clonedGlobal.Comdat!.Name, "Name of the comdat on the cloned global should match the one set in the original module" );
            Assert.AreEqual( ComdatKind.Any, module.Comdats[ globalName ].Kind );
        }

        [TestMethod]
        public void ComdatFunctionTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
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

            using var context2 = new Context( );
            var clone = module.Clone( context2 );
            Assert.AreEqual( 1, clone.Comdats.Count, "Comdat count should contain the one and only referenced comdat after save/clone" );
            Assert.IsTrue( clone.Comdats.Contains( globalName ), "Cloned module should have the referenced comdat" );

            Assert.IsTrue( clone.TryGetFunction( globalName, out IrFunction? clonedGlobal ) );
            Assert.IsNotNull( clonedGlobal );
            Assert.IsNotNull( clonedGlobal!.Comdat );
            Assert.AreEqual( globalName, clonedGlobal.Comdat!.Name, "Name of the comdat on the cloned global should match the one set in the original module" );
            Assert.AreEqual( ComdatKind.Any, module.Comdats[ globalName ].Kind );
        }

        private static BitcodeModule CreateSimpleModule( Context ctx, string name )
        {
            var retVal = ctx.CreateBitcodeModule( name );
            CreateSimpleVoidNopTestFunction( retVal, name );
            return retVal;
        }

        private static IrFunction CreateSimpleVoidNopTestFunction( BitcodeModule module, string name )
        {
            var ctx = module.Context;
            Assert.IsNotNull( ctx );

            var testFunc = module.CreateFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            var entryBlock = testFunc.AppendBasicBlock( "entry" );
            Assert.IsNotNull( testFunc.EntryBlock );
            Assert.AreSame( entryBlock, testFunc.EntryBlock );

            var irBuilder = new InstructionBuilder( testFunc.EntryBlock! );
            irBuilder.Return( );
            return testFunc;
        }

        private static IrFunction CreateInvalidFunction( BitcodeModule module, string name )
        {
            var ctx = module.Context;

            var testFunc = module.CreateFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            testFunc.AppendBasicBlock( "entry" );

            // UNTERMINATED BLOCK INTENTIONAL
            return testFunc;
        }
    }
}
