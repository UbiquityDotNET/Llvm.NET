// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop.ABI.libllvm_c;
using Ubiquity.NET.Llvm.Interop.ABI.llvm_c;

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.ValueBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Core;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.DebugInfo;

namespace Ubiquity.NET.Llvm.Interop.UT
{
    [TestClass]
    public class DebugRecordTests
    {
        // This tests the very quirky/buggy behavior of LLVMGetFirstDbgRecord()
        // where it (As of LLVM 20.1.3) will just crash if the instruction has no
        // debug records attached. (Worse it lands in the dreaded Undefined Behavior
        // if the value is NOT an instruction since it blindly casts it assuming it
        // is - there's no guarantees on what will happen for any other type of Value...)
        // Thus, LibLLVMHasDbgRecords was introduced to help detect/avoid these problematic
        // cases.
        [TestMethod]
        public void DebugRecordEnumerationSucceeds( )
        {
            using LLVMContextRef ctx = LLVMContextCreate();
            using LLVMModuleRef module = LLVMModuleCreateWithNameInContext("testModule", ctx);
            LLVMTypeRef intType = LLVMInt32TypeInContext(ctx);
            LLVMTypeRef funcType = LLVMFunctionType(intType, [], 0, false);

            // declare a test func to work with and create an entry block for it...
            LLVMValueRef func = LLVMAddFunction(module,"TestFunc", funcType);
            LLVMBasicBlockRef testBlock = LLVMAppendBasicBlockInContext(ctx, func, "entry");

            // Create an instruction builder to work with and connect it to
            // a new entry block for the test function
            using LLVMBuilderRef testBuilder = LLVMCreateBuilderInContext(ctx);
            LLVMPositionBuilderAtEnd( testBuilder, testBlock );

            // Now build out some code...
            LLVMValueRef constOne = LLVMConstInt(intType, 1, false);
            LLVMValueRef r0 = LLVMBuildAdd(testBuilder, constOne, constOne, string.Empty);

            // Finally get to the test...
            // No debug records attached to this instruction.
            // (That is, calls to LLVMGetFirstDbgRecord() will crash with a null ref.)
            //
            // Add (from above) is constant folded to a const, so not an instruction... Tests if
            // LibLLVMHasDbgRecords still returns false, however
            Assert.AreEqual( LLVMValueKind.LLVMConstantIntValueKind, LLVMGetValueKind( r0 ) );
            Assert.IsFalse( LibLLVMHasDbgRecords( r0 ) );

            // Build a call to malloc that is guaranteed not to land in a const.
            LLVMValueRef mallocInst = LLVMBuildMalloc(testBuilder, intType, "malloc_1");
            Assert.AreEqual( LLVMValueKind.LLVMInstructionValueKind, LLVMGetValueKind( mallocInst ) );
            Assert.IsFalse( LibLLVMHasDbgRecords( mallocInst ) );

            // Now attach debug records to the malloc.
            // first create the information to attach (It's a lot...)
            using LLVMDIBuilderRef diBuilder = LLVMCreateDIBuilder(module);
            LLVMMetadataRef diFile = LLVMDIBuilderCreateFile(diBuilder, "testfile.cs"u8, Environment.CurrentDirectory);
            LLVMMetadataRef diCU = LLVMDIBuilderCreateCompileUnit(
                    diBuilder,
                    LLVMDWARFSourceLanguage.LLVMDWARFSourceLanguageC_sharp,
                    diFile,
                    Producer: null,
                    isOptimized: false,
                    Flags: null,
                    RuntimeVer: 0,
                    SplitName: null,
                    LLVMDWARFEmissionKind.LLVMDWARFEmissionFull,
                    DWOId: 0,
                    SplitDebugInlining: false,
                    DebugInfoForProfiling: false,
                    SysRoot: null,
                    SDK: null
                    );

            LLVMMetadataRef int32DiType = LLVMDIBuilderCreateBasicType(
                diBuilder,
                "int32_t"u8,
                32,
                (uint)LibLLVMDwarfAttributeEncoding.DW_ATE_signed,
                LLVMDIFlags.LLVMDIFlagPublic
            );

            LLVMMetadataRef int32PtrDiType = LLVMDIBuilderCreatePointerType(diBuilder, int32DiType, 32, 0, 0, "int*"u8);
            LLVMMetadataRef emptyExpression = LLVMDIBuilderCreateExpression(diBuilder, []);
            LLVMMetadataRef diFuncType = LLVMDIBuilderCreateSubroutineType(diBuilder, default, [], LLVMDIFlags.LLVMDIFlagPrivate);
            LLVMMetadataRef scope = LLVMDIBuilderCreateFunction(
                diBuilder,
                Scope: default,
                Name: "TestFunc"u8,
                LinkageName: "_TestFunc"u8,
                File: diFile,
                LineNo: 0,
                Ty: diFuncType,
                IsLocalToUnit: true,
                IsDefinition: true,
                ScopeLine: 0,
                LLVMDIFlags.LLVMDIFlagPrivate,
                IsOptimized: false
            );

            var diLocalVar = LLVMDIBuilderCreateAutoVariable(
                diBuilder,
                scope,
                Name: string.Empty,
                File: diFile,
                LineNo: 0,
                int32PtrDiType,
                AlwaysPreserve: false,
                LLVMDIFlags.LLVMDIFlagArtificial,
                4);

            LLVMMetadataRef diLocation = LLVMDIBuilderCreateDebugLocation(ctx, 123,4, scope, default);

            // Now attach the record to the result of the malloc call
            // and retest the status as it should be different now
            LLVMDbgRecordRef dbgRecord = LLVMDIBuilderInsertDbgValueRecordBefore(diBuilder, mallocInst, diLocalVar, emptyExpression, diLocation, mallocInst);
            Assert.IsTrue( LibLLVMHasDbgRecords( mallocInst ) );

            // this should not crash now... [It shouldn't ever but that's another story...]
            LLVMDbgRecordRef firstRecord = LLVMGetFirstDbgRecord(mallocInst);
            Assert.AreEqual( dbgRecord, firstRecord );
            Assert.IsFalse( firstRecord.IsNull );
            LLVMDbgRecordRef nxtRecord = LLVMGetNextDbgRecord(firstRecord);
            Assert.IsTrue( nxtRecord.IsNull );
        }
    }
}
