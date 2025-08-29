// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Instructions;
using Ubiquity.NET.Llvm.Values;

namespace Ubiquity.NET.Llvm.UT.DebugInfo
{
    [TestClass]
    public class DebugRecordTests
    {
        [TestMethod]
        public void TestDebugRecordsNoCrash( )
        {
            using var ctx = new Context();
            using var module = ctx.CreateBitcodeModule("testModule");
            using var diBuilder = new DIBuilder(module);
            DICompileUnit compilationUnit = diBuilder.CreateCompileUnit(SourceLanguage.CSharp, "test.cs", "test method 0.0.0");

            Assert.IsNotNull( compilationUnit, "Just created CU should not be null" );
            Assert.IsNotNull( compilationUnit.File, "CU should not have null file, it was provided in creation" );

            var i32 = new DebugBasicType( module.Context.Int32Type, diBuilder, "int", DiTypeKind.Signed );
            var i32Ptr = i32.CreatePointerType(diBuilder, 0);
            var testFuncSig = ctx.CreateFunctionType(diBuilder, i32Ptr);
            var testFunc = module.CreateFunction( diBuilder
                                                , scope: compilationUnit.File
                                                , name: "DoCopy"
                                                , linkageName: null
                                                , file: compilationUnit.File
                                                , line: 23 // made up source line location for testing
                                                , signature: testFuncSig
                                                , isLocalToUnit: false
                                                , isDefinition: true
                                                , scopeLine: 24
                                                , debugFlags: DebugInfoFlags.None
                                                , isOptimized: false
                                                );

            Assert.IsNotNull( testFunc.DISubProgram, "Function creation provided debug info, subprogram should NOT be null" );

            var entryBlock = testFunc.AppendBasicBlock("entry");
            using var instBuilder = new InstructionBuilder(entryBlock);
            ConstantInt constOne = ctx.CreateConstant(1);
            Value addInst = instBuilder.Add(constOne, constOne);
            Assert.IsFalse( addInst is Instruction, "Constant folding should make this a constant, not a real instruction" );

            var loc = new DILocation(ctx, 25, 1, testFunc.DISubProgram);

            // NOTE: register name not really needed here but matches the lower level interop test
            var mallocValue = instBuilder.Malloc(i32).RegisterName("malloc_1");
            Assert.IsTrue( mallocValue is Instruction );
            var mallocInst = (Instruction)mallocValue;
            Assert.IsFalse( mallocInst.HasDebugRecords );

            // Now attach some records to the instruction and test again.
            var diVarInfo = diBuilder.CreateLocalVariable(testFunc.DISubProgram, "testmallocVal", compilationUnit.File, 25, i32Ptr);
            diBuilder.InsertDeclare( mallocInst, diVarInfo, new DILocation( ctx, 25, 1, testFunc.DISubProgram ), mallocInst );
            Assert.IsTrue( mallocInst.HasDebugRecords );

            // Test enumeration of records works (doesn't crash at least)
            DebugRecord[] records = [ .. mallocInst.DebugRecords ];
            Assert.AreEqual( 1, records.Length );
            Assert.IsFalse( records[ 0 ].IsNull, "Should not enumerate a null record" );
            Assert.IsTrue( records[ 0 ].NextRecord.IsNull, "Next record should be null (only one record expected)" );
        }
    }
}
