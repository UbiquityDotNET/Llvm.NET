// -----------------------------------------------------------------------
// <copyright file="MDNodeTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;

namespace Ubiquity.NET.Llvm.Tests
{
    [TestClass]
    public class MDNodeTests
    {
        [TestMethod]
        public void AppendingModuleFlagsTest( )
        {
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( "test.bc", SourceLanguage.C99, "test.c", "unit-tests" );
            module.AddModuleFlag( ModuleFlagBehavior.Append, "testMD", ctx.CreateMDNode( "testValue" ) );
            Assert.AreEqual( 1, module.ModuleFlags.Count );
            var flag = module.ModuleFlags[ "testMD" ];
            Assert.IsNotNull( flag );
            Assert.AreEqual( ModuleFlagBehavior.Append, flag.Behavior );
            Assert.AreEqual( "testMD", flag.Name );
            Assert.IsInstanceOfType( flag.Metadata, typeof( MDTuple ) );
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

            var targetMachine = TargetTests.GetTargetMachine( );
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( "test.bc", SourceLanguage.C99, "test.c", "unit-tests" );
            module.Layout = targetMachine.TargetData;
            var intType = new DebugBasicType( module.Context.Int32Type, module, "int", DiTypeKind.Signed );
            var arrayType = new DebugArrayType(intType, module, 3u);
            Assert.IsNotNull( arrayType );

            DICompositeType? mdnode = arrayType.DebugInfoType;
            Assert.IsNotNull( mdnode );

            // BS warning - this is here to VALIDATE the claim that it is always going to be true.
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

            // TODO: New operand [9] => DataLocation (Metadata)[DIVariable][DIExpression]
            // TODO: New Operand [10] => Associated (Metadata)
            // TODO: New Operand [11] => Allocated (Metadata)
            // TODO: New Operand [12] => Rank (Metadata)[ConstantInt][DIExpression]
            // TODO: New Operand [13] => Annotations (Metadata)[DINodeArray]
            // TODO: New Operand [14] => Specification (Metadata)[DebugInfoType]
            Assert.AreSame( intType.DebugInfoType, mdnode.BaseType );
        }
    }
}
