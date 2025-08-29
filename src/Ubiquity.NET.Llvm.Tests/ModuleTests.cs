// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Metadata;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class ModuleTests
    {
        private static readonly LazyEncodedString StructTestName = "struct.Test"u8;
        private static readonly LazyEncodedString TestModuleName = "test"u8;
        private const string LlvmNewLine = "\n";

        // To validate transformation to correct newline formatting
        // this must explicitly setup the string. Using a file source
        // would cause git line ending transforms to impact the results
        // In order to have consistent indexed source symbols the automated
        // builds standardize on the single LineFeed character so the test
        // file would end up containing incorrect line endings for the test
        private const string TestModuleTemplate = """
            ; ModuleID = '{0}'
            source_filename = "test"

            define void @foo() {{
            entry:
              ret void
            }}

            """;

        [TestMethod]
        public void DefaultConstructorTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( );

            Assert.AreSame( LazyEncodedString.Empty, module.Name );
            Assert.AreSame( LazyEncodedString.Empty, module.SourceFileName );
            Assert.IsNotNull( module );
            Assert.IsNotNull( module.Context );
            Assert.IsNotNull( module.DataLayoutString );
            Assert.IsTrue( module.DataLayoutString.IsEmpty );
            Assert.IsNotNull( module.Layout );
            Assert.AreSame( LazyEncodedString.Empty, module.TargetTriple );

            // Functions collection should be valid but empty
            Assert.IsNotNull( module.Functions );
            Assert.IsFalse( module.Functions.Any() );

            // Globals collection should be valid but empty
            Assert.IsNotNull( module.Globals );
            Assert.IsFalse( module.Globals.Any() );
        }

        [TestMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Design", "MSTEST0032:Assertion condition is always true", Justification = "BS! This VERIFIES the claim!" )]
        public void ConstructorTestWithName( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            Assert.AreEqual( TestModuleName, module.Name );
            Assert.AreEqual( TestModuleName, module.SourceFileName );
            Assert.IsNotNull( module );
            Assert.IsNotNull( module.Context );
            Assert.IsTrue( module.DataLayoutString.IsEmpty );
            Assert.IsNotNull( module.Layout );
            Assert.AreSame( LazyEncodedString.Empty, module.TargetTriple );

            // Functions collection should be valid but empty
            Assert.IsNotNull( module.Functions );
            Assert.IsFalse( module.Functions.Any() );

            // Globals collection should be valid but empty
            Assert.IsNotNull( module.Globals );
            Assert.IsFalse( module.Globals.Any() );
        }

        [TestMethod]
        public void ConstructorTestWithNameAndCompileUnit( )
        {
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( TestModuleName );
            using var diBuilder = new DIBuilder( module );
            DICompileUnit cu = diBuilder.CreateCompileUnit( SourceLanguage.C99, "test.c", "unitTest", false, string.Empty );

            Assert.AreEqual( TestModuleName, module.Name );
            Assert.AreEqual( TestModuleName, module.SourceFileName );
            Assert.IsNotNull( module );
            Assert.IsNotNull( module.Context );
            Assert.IsNotNull( module.DataLayoutString );
            Assert.IsTrue( module.DataLayoutString.IsEmpty );
            Assert.IsNotNull( module.Layout );
            Assert.AreSame( LazyEncodedString.Empty, module.TargetTriple );

            // Functions collection should be valid but empty
            Assert.IsNotNull( module.Functions );
            Assert.IsFalse( module.Functions.Any() );

            // Globals collection should be valid but empty
            Assert.IsNotNull( module.Globals );
            Assert.IsFalse( module.Globals.Any() );
        }

        [TestMethod]
        public void DisposeTest( )
        {
            using(var context = new Context())
            using(context.CreateBitcodeModule( TestModuleName ))
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
            var ex = Assert.ThrowsExactly<ArgumentException>(()=>
                        mergedMod.Link( m1 )
                     );
            Assert.AreEqual( "srcModule", ex.ParamName );

            // full message includes the name of the parameter, but that's .NET functionality not tested
            Assert.IsTrue( ex.Message.StartsWith( "Linking modules from different contexts is not allowed" ) );
        }

        [TestMethod]
        public void ModuleCloneInContextTest( )
        {
            using var context1 = new Context( );
            using var m1 = CreateSimpleModule( context1, "module1" );

            using var context2 = new Context( );
            using var m2 = m1.Clone( context2 );

            Assert.AreNotSame( context2, m1.Context );
            Assert.IsNotNull( m2 );
            Assert.AreEqual( context2, m2.Context );
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

            using var clone1 = m1.Clone( mergedMod.Context );
            using var clone2 = m2.Clone( mergedMod.Context );
            GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced, true );
            mergedMod.Link( clone1 );
            GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced, true );
            Assert.IsTrue( mergedMod.Verify( out string errMsg ), errMsg );
            Assert.AreEqual( 1, mergedMod.Functions.Count() );
            mergedMod.Link( clone2 );
            GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced, true );
            Assert.IsTrue( mergedMod.Verify( out errMsg ), errMsg );
            Assert.AreEqual( 2, mergedMod.Functions.Count() );
        }

        [TestMethod]
        public void VerifyValidModuleTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            Function testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );

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
            Function testFunc = CreateInvalidFunction( module, "badfunc" );

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
            Function testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );

            // verify basics
            Assert.IsNotNull( testFunc );
            Assert.AreEqual( module, testFunc.ParentModule );
            Assert.AreEqual( "foo", testFunc.Name );

            // Verify the function is in the module, and getting it retrieves the same instance
            Assert.IsTrue( module.TryGetFunction( "foo", out Function? funcFromModule ) );
            Assert.AreEqual( testFunc, funcFromModule );
        }

        [TestMethod]
        public void WriteToFileTest( )
        {
            string tmpFileName = Path.GetTempFileName( );
            try
            {
                using(var context = new Context())
                using(var module = context.CreateBitcodeModule( TestModuleName ))
                {
                    _ = CreateSimpleVoidNopTestFunction( module, "foo" );
                    module.WriteToFile( tmpFileName );
                }

                using var ctx = new Context( );
                using var module2 = Module.LoadFrom( tmpFileName, ctx );

                // force a GC to ensure buffer created in LoadFrom is handled correctly
                GC.Collect( GC.MaxGeneration );
                Assert.IsTrue( module2.TryGetFunction( "foo", out Function? testFunc ) );

                // verify basics
                Assert.IsNotNull( testFunc );
                string? txt = module2.WriteToString( );
                Assert.IsFalse( string.IsNullOrWhiteSpace( txt ) );
                string expectedText = GetExpectedModuleText(tmpFileName);
                Assert.AreEqual( expectedText, txt );
            }
            finally
            {
                File.Delete( tmpFileName );
            }
        }

        [TestMethod]
        public void AsStringTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );

            Function? testFunc = CreateSimpleVoidNopTestFunction( module, "foo" );

            // verify basics
            Assert.IsNotNull( testFunc );
            string? txt = module.WriteToString( );
            Assert.IsFalse( string.IsNullOrWhiteSpace( txt ) );
            string expectedText = GetExpectedModuleText("test");
            Assert.AreEqual( expectedText, txt );
        }

        [TestMethod]
        public void AddAliasGetAliasTest( )
        {
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );
            Function testFunc = CreateSimpleVoidNopTestFunction( module, "_test" );

            var alias = module.AddAlias( testFunc, TestModuleName );
            Assert.AreEqual( alias, module.GetAlias( TestModuleName ) );
            Assert.AreEqual( module, alias.ParentModule );
            Assert.AreEqual( testFunc, alias.Aliasee );
            Assert.AreEqual( TestModuleName, alias.Name );
            Assert.AreEqual( Linkage.External, alias.Linkage );
            Assert.AreEqual( testFunc.NativeType, alias.NativeType );

            Assert.AreEqual( 1, alias.Operands.Count );
            Assert.AreEqual( testFunc, alias.Aliasee );

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
            Assert.AreEqual( module.Context.Int32Type.CreatePointerType(), globalVar.NativeType );
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
            Assert.IsTrue( LazyEncodedString.IsNullOrWhiteSpace( globalVar.Name ) );
            Assert.AreEqual( module.Context.Int32Type.CreatePointerType(), globalVar.NativeType );
            Assert.AreEqual( module.Context.Int32Type, globalVar.Initializer!.NativeType );
            Assert.AreEqual( Linkage.WeakODR, globalVar.Linkage );
            Assert.IsTrue( globalVar.IsConstant );
            Assert.IsInstanceOfType<ConstantInt>( globalVar.Initializer );

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
            Assert.AreEqual( module.Context.Int32Type.CreatePointerType(), globalVar.NativeType );
            Assert.AreEqual( module.Context.Int32Type, globalVar.Initializer!.NativeType );
            Assert.AreEqual( Linkage.WeakODR, globalVar.Linkage );
            Assert.IsTrue( globalVar.IsConstant );
            Assert.IsInstanceOfType<ConstantInt>( globalVar.Initializer );

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
            Assert.AreEqual( expectedType, actualType );
        }

        [TestMethod]
        public void AddModuleFlagTest( )
        {
            Assert.IsNotNull(ModuleFixtures.LibLLVM);

            using var context = new Context( );
            using var module = context.CreateBitcodeModule( TestModuleName );

            module.AddModuleFlag( ModuleFlagBehavior.Warning, Module.DwarfVersionValue, 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Warning, Module.DebugVersionValue, ModuleFixtures.LibLLVM.DebugMetadataVersion );
            module.AddModuleFlag( ModuleFlagBehavior.Error, "wchar_size", 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Error, "min_enum_size", 4 );
            module.AddVersionIdentMetadata( "unit-tests 1.0" );

            Assert.AreEqual( 4, module.ModuleFlags.Count );
            Assert.IsTrue( module.ModuleFlags.ContainsKey( Module.DwarfVersionValue ) );
            Assert.IsTrue( module.ModuleFlags.ContainsKey( Module.DebugVersionValue ) );
            Assert.IsTrue( module.ModuleFlags.ContainsKey( "wchar_size" ) );
            Assert.IsTrue( module.ModuleFlags.ContainsKey( "min_enum_size" ) );

            var dwarfVerFlag = module.ModuleFlags[ Module.DwarfVersionValue ];
            Assert.AreEqual( ModuleFlagBehavior.Warning, dwarfVerFlag.Behavior );
            Assert.AreEqual( Module.DwarfVersionValue, dwarfVerFlag.Name );
            Assert.IsInstanceOfType<ConstantAsMetadata>( dwarfVerFlag.Metadata );

            var dwarfVerConst = ( ( ConstantAsMetadata )dwarfVerFlag.Metadata ).Constant;
            Assert.IsInstanceOfType<ConstantInt>( dwarfVerConst );
            Assert.AreEqual( 4UL, ((ConstantInt)dwarfVerConst).ZeroExtendedValue );

            var debugVerFlag = module.ModuleFlags[ Module.DebugVersionValue ];
            Assert.AreEqual( ModuleFlagBehavior.Warning, debugVerFlag.Behavior );
            Assert.AreEqual( Module.DebugVersionValue, debugVerFlag.Name );
            Assert.IsInstanceOfType<ConstantAsMetadata>( debugVerFlag.Metadata );

            var debugVerConst = ( ( ConstantAsMetadata )debugVerFlag.Metadata ).Constant;
            Assert.IsInstanceOfType<ConstantInt>( debugVerConst );
            Assert.AreEqual( ModuleFixtures.LibLLVM.DebugMetadataVersion, ((ConstantInt)debugVerConst).ZeroExtendedValue );

            var wcharSizeFlag = module.ModuleFlags[ "wchar_size" ];
            Assert.AreEqual( ModuleFlagBehavior.Error, wcharSizeFlag.Behavior );
            Assert.AreEqual( "wchar_size", wcharSizeFlag.Name );
            Assert.IsInstanceOfType<ConstantAsMetadata>( wcharSizeFlag.Metadata );

            var wcharSizeConst = ( ( ConstantAsMetadata )wcharSizeFlag.Metadata ).Constant;
            Assert.IsInstanceOfType<ConstantInt>( wcharSizeConst );
            Assert.AreEqual( 4UL, ((ConstantInt)wcharSizeConst).ZeroExtendedValue );

            var minEnumSizeFlag = module.ModuleFlags[ "wchar_size" ];
            Assert.AreEqual( ModuleFlagBehavior.Error, minEnumSizeFlag.Behavior );
            Assert.AreEqual( "wchar_size", minEnumSizeFlag.Name );
            Assert.IsInstanceOfType<ConstantAsMetadata>( minEnumSizeFlag.Metadata );

            var minEnumSizeConst = ( ( ConstantAsMetadata )minEnumSizeFlag.Metadata ).Constant;
            Assert.IsInstanceOfType<ConstantInt>( minEnumSizeConst );
            Assert.AreEqual( 4UL, ((ConstantInt)minEnumSizeConst).ZeroExtendedValue );
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

            module.Comdats.AddOrUpdate( comdatName, ComdatKind.SameSize );
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
            using var clone = module.Clone( context2 );
            Assert.AreEqual( 1, clone.Comdats.Count, "Comdat count should contain the one and only referenced comdat after save/clone" );
            Assert.IsTrue( clone.Comdats.Contains( globalName ), "Cloned module should have the referenced comdat" );

            var clonedGlobal = clone.GetNamedGlobal( globalName );
            Assert.IsNotNull( clonedGlobal );
            Assert.IsFalse( clonedGlobal.Comdat.IsNull );

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

            Comdat comdat = module.Comdats.AddOrUpdate( comdatName, ComdatKind.SameSize );
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
            using var clone = module.Clone( context2 );
            Assert.AreEqual( 1, clone.Comdats.Count, "Comdat count should contain the one and only referenced comdat after save/clone" );
            Assert.IsTrue( clone.Comdats.Contains( globalName ), "Cloned module should have the referenced comdat" );

            Assert.IsTrue( clone.TryGetFunction( globalName, out Function? clonedGlobal ) );
            Assert.IsNotNull( clonedGlobal );
            Assert.IsFalse( clonedGlobal.Comdat.IsNull );
            Assert.AreEqual( globalName, clonedGlobal.Comdat!.Name, "Name of the comdat on the cloned global should match the one set in the original module" );
            Assert.AreEqual( ComdatKind.Any, module.Comdats[ globalName ].Kind );
        }

        [SuppressMessage( "Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Test does not assume single implementation of interface" )]
        private static Module CreateSimpleModule( IContext ctx, LazyEncodedString name )
        {
            // ownership transferred out of this helper
            var retVal = ctx.CreateBitcodeModule( name );
            CreateSimpleVoidNopTestFunction( retVal, name );
            return retVal;
        }

        private static Function CreateSimpleVoidNopTestFunction( Module module, LazyEncodedString name )
        {
            var ctx = module.Context;
            Assert.IsNotNull( ctx );

            var testFunc = module.CreateFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            var entryBlock = testFunc.AppendBasicBlock( "entry" );
            Assert.IsNotNull( testFunc.EntryBlock );
            Assert.AreEqual( entryBlock, testFunc.EntryBlock );

            using var irBuilder = new InstructionBuilder( testFunc.EntryBlock! );
            irBuilder.Return();
            return testFunc;
        }

        private static Function CreateInvalidFunction( Module module, LazyEncodedString name )
        {
            var ctx = module.Context;

            var testFunc = module.CreateFunction( name, ctx.GetFunctionType( ctx.VoidType ) );
            testFunc.AppendBasicBlock( "entry" );

            // UNTERMINATED BLOCK INTENTIONAL
            return testFunc;
        }

        private static string GetExpectedModuleText( LazyEncodedString moduleName )
        {
            string expectedText = string.Format( CultureInfo.InvariantCulture, TestModuleTemplate, moduleName );
            if(Environment.NewLine != LlvmNewLine)
            {
                // Normalize expected text to use LLVM's line endings as it
                // is NOT the same on Windows platforms at least.
                expectedText = expectedText.ReplaceLineEndings( LlvmNewLine );
            }

            return expectedText;
        }
    }
}
