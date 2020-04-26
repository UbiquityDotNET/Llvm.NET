// -----------------------------------------------------------------------
// <copyright file="TestObjectFile.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Types;

// signatures required by test framework
#pragma warning disable IDE0060 // Remove unused parameter

namespace Ubiquity.NET.Llvm.Tests
{
    [TestClass]
    public class TestObjectFile
    {
        private const string AddSectionName = "$func$Add";
        private const string SubSectionName = "$func$Sub";
        private const string MulSectionName = "$func$Mul";
        private const string DivSectionName = "$func$Div";
        private const string AddFuncName = "Add";
        private const string SubFuncName = "Sub";
        private const string MulFuncName = "Mul";
        private const string DivFuncName = "Div";
        private const string PrintFuncName = "Print";
        private const string TestObjFileName = "TestObjectFile.o";
        private const string TestSrcFileName = "test.c";

        [ClassInitialize]
        public static void Initialize( TestContext ctx )
        {
            _ = ctx; // unused
            using var llvmContext = new Context( );
            using var module = llvmContext.CreateBitcodeModule( "test", SourceLanguage.C, TestSrcFileName, "unit tests" );
            var tm = TargetMachine.FromTriple( Triple.HostTriple );
            module.TargetTriple = tm.Triple;
            module.Layout = tm.TargetData;

            var doubleType = new DebugBasicType( llvmContext.DoubleType, module, "double", DiTypeKind.Float );
            var voidType = DebugType.Create<ITypeRef, DIType>( module.Context.VoidType, null );

            var printDecl = module.CreateFunction( PrintFuncName, false, voidType, doubleType );

            var bldr = CreateFunctionAndGetBuilder(module, doubleType, AddFuncName, AddSectionName, 0);
            bldr.CurrentDebugLocation = new DILocation( llvmContext, 2, 1, bldr.InsertFunction!.DISubProgram! );
            var result = bldr.FAdd( bldr.InsertFunction.Parameters[ 0 ], bldr.InsertFunction.Parameters[ 1 ] );
            _ = bldr.Call( printDecl, result );
            bldr.Return( result );

            bldr = CreateFunctionAndGetBuilder( module, doubleType, SubFuncName, SubSectionName, 5 );
            bldr.CurrentDebugLocation = new DILocation( llvmContext, 7, 1, bldr.InsertFunction!.DISubProgram! );
            result = bldr.FSub( bldr.InsertFunction.Parameters[ 0 ], bldr.InsertFunction.Parameters[ 1 ] );
            _ = bldr.Call( printDecl, result );
            bldr.Return( result );

            bldr = CreateFunctionAndGetBuilder( module, doubleType, MulFuncName, MulSectionName, 10 );
            bldr.CurrentDebugLocation = new DILocation( llvmContext, 12, 1, bldr.InsertFunction!.DISubProgram! );
            result = bldr.FMul( bldr.InsertFunction.Parameters[ 0 ], bldr.InsertFunction.Parameters[ 1 ] );
            _ = bldr.Call( printDecl, result );
            bldr.Return( result );

            bldr = CreateFunctionAndGetBuilder( module, doubleType, DivFuncName, DivSectionName, 15 );
            bldr.CurrentDebugLocation = new DILocation( llvmContext, 17, 1, bldr.InsertFunction!.DISubProgram! );
            result = bldr.FDiv( bldr.InsertFunction.Parameters[ 0 ], bldr.InsertFunction.Parameters[ 1 ] );
            _ = bldr.Call( printDecl, result );
            bldr.Return( result );

            Debug.WriteLine( module.WriteToString( ) );
            tm.EmitToFile( module, TestObjFileName, CodeGenFileType.ObjectFile );
        }

        [TestMethod]
        public void LoadObjFileTest( )
        {
            using var llvmContext = new Context( );
            using var obj = llvmContext.OpenBinary( TestObjFileName );
        }

        [TestMethod]
        [Description( "All the declared section names should exist" )]
        public void DeclaredSectionsTest( )
        {
            using var llvmContext = new Context( );
            using var obj = llvmContext.OpenBinary( TestObjFileName );

            // all the declared section names should be present (There may be additional obj format specific sections as well)
            Assert.IsTrue( obj.Sections.SingleOrDefault( s => s.Name == AddSectionName ) != default );
            Assert.IsTrue( obj.Sections.SingleOrDefault( s => s.Name == SubSectionName ) != default );
            Assert.IsTrue( obj.Sections.SingleOrDefault( s => s.Name == MulSectionName ) != default );
            Assert.IsTrue( obj.Sections.SingleOrDefault( s => s.Name == DivSectionName ) != default );
        }

        [TestMethod]
        [Description( "Symbols should exist for all the declared functions" )]
        public void DeclaredSymbolsTest( )
        {
            using var llvmContext = new Context( );
            using var obj = llvmContext.OpenBinary( TestObjFileName );

            // symbols should be present for all the declared functions
            Assert.IsTrue( obj.Symbols.SingleOrDefault( s => s.Name == AddFuncName ) != default );
            Assert.IsTrue( obj.Symbols.SingleOrDefault( s => s.Name == SubFuncName ) != default );
            Assert.IsTrue( obj.Symbols.SingleOrDefault( s => s.Name == MulFuncName ) != default );
            Assert.IsTrue( obj.Symbols.SingleOrDefault( s => s.Name == DivFuncName ) != default );
        }

        [TestMethod]
        [Description( "Declared sections should have one relocation for the declared function" )]
        public void DeclaredFunctionRelocationTest( )
        {
            using var llvmContext = new Context( );
            using var obj = llvmContext.OpenBinary( TestObjFileName );

            // all the declared section names should be present (There may be additional obj format specific sections as well)
            var declaredSections = from sec in obj.Sections
                                   where sec.Name == AddSectionName
                                          || sec.Name == SubSectionName
                                          || sec.Name == MulSectionName
                                          || sec.Name == DivSectionName
                                   select sec;

            foreach( var sec in declaredSections )
            {
                Assert.AreEqual( PrintFuncName, sec.Relocations.First( ).Symbol.Name );
            }
        }

        [ClassCleanup]
        public static void Cleanup( )
        {
            System.IO.File.Delete( TestObjFileName );
        }

        private static InstructionBuilder CreateFunctionAndGetBuilder( BitcodeModule module, DebugBasicType doubleType, string name, string section, uint line )
        {
            DIFile file = module.DIBuilder.CreateFile(TestSrcFileName);

            DebugFunctionType signature = module.Context.CreateFunctionType( module.DIBuilder, doubleType, doubleType, doubleType );
            var func = module.CreateFunction(module.DICompileUnit!, name, name, file, line, signature, true, true, line + 1, DebugInfoFlags.None, false);
            func.Section = section;
            var entry = func.AppendBasicBlock( "entry" );
            return new InstructionBuilder( entry );
        }
    }
}
