// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Metadata;

namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class MDNodeTests
    {
        [TestMethod]
        public void AppendingModuleFlagsTest( )
        {
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( "test.bc" );
            using var diBuilder = new DIBuilder(module);
            DICompileUnit cu = diBuilder.CreateCompileUnit(SourceLanguage.C99, "test.c", "unit-tests");

            module.AddModuleFlag( ModuleFlagBehavior.Append, "testMD", ctx.CreateMDNode( "testValue" ) );
            Assert.AreEqual( 1, module.ModuleFlags.Count );
            var flag = module.ModuleFlags[ "testMD" ];
            Assert.IsNotNull( flag );
            Assert.AreEqual( ModuleFlagBehavior.Append, flag.Behavior );
            Assert.AreEqual( "testMD", flag.Name );
            Assert.IsInstanceOfType<MDTuple>( flag.Metadata );
            var tuple = ( MDTuple )flag.Metadata;
            Assert.AreEqual( "testValue", tuple.GetOperandString( 0 ) );
        }

        [TestMethod]
        public void OperandsAreAccessibleTest( )
        {
            // NOTE: THis is the expected count of operands for a given LLVM version
            // It can, and will, change as LLVM itself changes. Thus it is used to
            // validate that the number of operands is reflected correctly AND to
            // serve as a test failure should the underlying LLVM change (indicating
            // this test needs updating for new operands)
            const int CompositeTypeOperandCount = 15;

            using var targetMachine = TargetTests.GetTargetMachine( );
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( "test.bc" );
            using var diBuilder = new DIBuilder(module);
            DICompileUnit cu = diBuilder.CreateCompileUnit(SourceLanguage.C99, "test.c", "unit-tests");

            using var layout = targetMachine.CreateTargetData();
            module.Layout = layout;
            var intType = new DebugBasicType( module.Context.Int32Type, diBuilder, "int", DiTypeKind.Signed );
            var arrayType = new DebugArrayType(intType, diBuilder, 3u);
            Assert.IsNotNull( arrayType );

            DICompositeType? mdnode = arrayType.DebugInfoType;
            Assert.IsNotNull( mdnode );

            // BS warning - this is here to VALIDATE the CLAIM that it is always going to be true.
#pragma warning disable MSTEST0032 // Assertion condition is always true;
            Assert.IsNotNull( mdnode.Operands );
#pragma warning restore MSTEST0032 // Assertion condition is always true

            Assert.AreEqual( CompositeTypeOperandCount, mdnode.Operands.Count );

            Assert.IsNull( mdnode.File );
            Assert.IsNull( mdnode.Operands[ 0 ] );

            Assert.IsNull( mdnode.Scope );
            Assert.IsNull( mdnode.Operands[ 1 ] );

            Assert.IsTrue( string.IsNullOrEmpty( mdnode.Name ) );
            Assert.IsNull( mdnode.Operands[ 2 ] );

            Assert.IsNotNull( mdnode.BaseType );
            Assert.IsNotNull( mdnode.Operands[ 3 ] );

            // BS warning - this is here to VALIDATE the claim that it is always going to be true.
#pragma warning disable MSTEST0032 // Assertion condition is always true;
            Assert.IsNotNull( mdnode.Elements );
#pragma warning restore MSTEST0032 // Assertion condition is always true

            Assert.IsNotNull( mdnode.Operands[ 4 ] );
            Assert.AreEqual( 1, mdnode.Elements.Count );
            Assert.AreEqual( 1, mdnode.Elements.Count );
            var subRange = mdnode.Elements[ 0 ] as DISubRange;
            Assert.IsNotNull( subRange );

            Assert.IsNull( mdnode.VTableHolder );
            Assert.IsNull( mdnode.Operands[ 5 ] );

            Assert.IsNull( mdnode.TemplateParameters );
            Assert.IsNull( mdnode.Operands[ 6 ] );

            Assert.IsTrue( string.IsNullOrEmpty( mdnode.Identifier ) );
            Assert.IsNull( mdnode.Operands[ 7 ] );

            Assert.IsNull( mdnode.Discriminator );
            Assert.IsNull( mdnode.Operands[ 8 ] );

            // TODO: New operand [9] => DataLocation (IrMetadata)[DIVariable][DIExpression]
            // TODO: New Operand [10] => Associated (IrMetadata)
            // TODO: New Operand [11] => Allocated (IrMetadata)
            // TODO: New Operand [12] => Rank (IrMetadata)[ConstantInt][DIExpression]
            // TODO: New Operand [13] => Annotations (IrMetadata)[DINodeArray]
            // TODO: New Operand [14] => Specification (IrMetadata)[DebugInfoType]
            Assert.AreEqual( intType.DebugInfoType, mdnode.BaseType );
        }
    }
}
