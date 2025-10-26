// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Types;

namespace Ubiquity.NET.Llvm.UT
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
            using var module = llvmContext.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(module);
            _ = diBuilder.CreateCompileUnit( SourceLanguage.C, TestSrcFileName, "unit tests" );
            Assert.IsTrue( module.CompileUnits.Any() );
            Assert.IsNotNull( diBuilder.CompileUnit );

            using var hostTriple = Triple.GetHostTriple();
            using var tm = TargetMachine.FromTriple( hostTriple );
            module.TargetTriple = tm.Triple;

            using var layout = tm.CreateTargetData();
            module.Layout = layout;

            var doubleType = new DebugBasicType( llvmContext.DoubleType, diBuilder, "double", DiTypeKind.Float );
            var voidType = DebugType.Create<ITypeRef, DIType>( module.Context.VoidType, null );

            var printDecl = module.CreateFunction( diBuilder, PrintFuncName, false, voidType, doubleType );

            using(var bldr = CreateFunctionAndGetBuilder( diBuilder, module, doubleType, AddFuncName, AddSectionName, 0 ))
            {
                bldr.CurrentDebugLocation = new DILocation( llvmContext, 2, 1, bldr.InsertFunction!.DISubProgram! );
                var result = bldr.FAdd( bldr.InsertFunction.Parameters[ 0 ], bldr.InsertFunction.Parameters[ 1 ] );
                _ = bldr.Call( printDecl, result );
                bldr.Return( result );
            }

            using(var bldr = CreateFunctionAndGetBuilder( diBuilder, module, doubleType, SubFuncName, SubSectionName, 5 ))
            {
                bldr.CurrentDebugLocation = new DILocation( llvmContext, 7, 1, bldr.InsertFunction!.DISubProgram! );
                var result = bldr.FSub( bldr.InsertFunction.Parameters[ 0 ], bldr.InsertFunction.Parameters[ 1 ] );
                _ = bldr.Call( printDecl, result );
                bldr.Return( result );
            }

            using(var bldr = CreateFunctionAndGetBuilder( diBuilder, module, doubleType, MulFuncName, MulSectionName, 10 ))
            {
                bldr.CurrentDebugLocation = new DILocation( llvmContext, 12, 1, bldr.InsertFunction!.DISubProgram! );
                var result = bldr.FMul( bldr.InsertFunction.Parameters[ 0 ], bldr.InsertFunction.Parameters[ 1 ] );
                _ = bldr.Call( printDecl, result );
                bldr.Return( result );
            }

            using(var bldr = CreateFunctionAndGetBuilder( diBuilder, module, doubleType, DivFuncName, DivSectionName, 15 ))
            {
                bldr.CurrentDebugLocation = new DILocation( llvmContext, 17, 1, bldr.InsertFunction!.DISubProgram! );
                var result = bldr.FDiv( bldr.InsertFunction.Parameters[ 0 ], bldr.InsertFunction.Parameters[ 1 ] );
                _ = bldr.Call( printDecl, result );
                bldr.Return( result );
            }

            Debug.WriteLine( module.WriteToString() );
            tm.EmitToFile( module, TestObjFileName, CodeGenFileKind.ObjectFile );
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

            foreach(var sec in declaredSections)
            {
                Assert.AreEqual( PrintFuncName, sec.Relocations.First().Symbol.Name );
            }
        }

        [ClassCleanup]
        public static void Cleanup( )
        {
            System.IO.File.Delete( TestObjFileName );
        }

        private static InstructionBuilder CreateFunctionAndGetBuilder(
            IDIBuilder diBuilder,
            Module module,
            DebugBasicType doubleType,
            LazyEncodedString name,
            LazyEncodedString section,
            uint line
            )
        {
            DIFile file = diBuilder.CreateFile(TestSrcFileName);

            DebugFunctionType signature = module.Context.CreateFunctionType( diBuilder, doubleType, doubleType, doubleType );
            var func = module.CreateFunction(
                diBuilder,
                scope: diBuilder.CompileUnit,
                name: name,
                linkageName: name,
                file: file,
                line: line,
                signature: signature,
                isLocalToUnit: true,
                isDefinition: true,
                scopeLine: line + 1,
                debugFlags: DebugInfoFlags.None,
                isOptimized: false
                );
            func.Section = section;
            var entry = func.AppendBasicBlock( "entry" );
            return new InstructionBuilder( entry );
        }
    }
}
