// <copyright file="MDNodeTests.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.DebugInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    public class MDNodeTests
    {
        /*[TestMethod]
        //public void ResolveCyclesTest( )
        //{
        //    Assert.Inconclusive( );
        //}
        */

        /*[TestMethod]
        //public void ReplaceAllUsesWithTest( )
        //{
        //    Assert.Inconclusive( );
        //}
        */

        [TestMethod]
        public void AppendingModuleFlagsTest( )
        {
            using( var ctx = new Context( ) )
            using( var module = ctx.CreateBitcodeModule( "test.bc", SourceLanguage.C99, "test.c", "unit-tests" ) )
            {
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
        }

        [TestMethod]
        public void OperandsAreAccessibleTest()
        {
            const int CompositeTypeOperandCount = 9;
            var targetMachine = TargetTests.GetTargetMachine( );
            using( var ctx = new Context( ) )
            using( var module = ctx.CreateBitcodeModule( "test.bc", SourceLanguage.C99, "test.c", "unit-tests" ) )
            {
                module.Layout = targetMachine.TargetData;
                var intType = new DebugBasicType( module.Context.Int32Type, module, "int", DiTypeKind.Signed );
                var arrayType = new DebugArrayType(intType, module, 3u);
                Assert.IsNotNull( arrayType );

                var mdnode = arrayType.DIType;
                Assert.IsNotNull( mdnode.Operands );
                Assert.AreEqual( CompositeTypeOperandCount, mdnode.Operands.Count );

                Assert.IsNull( mdnode.File );
                Assert.IsNull( mdnode.Operands[ 0 ] );

                Assert.IsNull( mdnode.Scope );
                Assert.IsNull( mdnode.Operands[ 1 ] );

                Assert.IsTrue( string.IsNullOrEmpty( mdnode.Name ) );
                Assert.IsNull( mdnode.Operands[ 2 ] );

                Assert.IsNotNull( mdnode.BaseType );
                Assert.IsNotNull( mdnode.Operands[ 3 ] );

                Assert.IsNotNull( mdnode.Elements );
                Assert.IsNotNull( mdnode.Operands[ 4 ] );
                Assert.AreEqual( 1, mdnode.Elements.Count );
                Assert.AreEqual( 1, mdnode.Elements.Count );
                var subRange = mdnode.Elements[ 0 ] as DISubRange;
                Assert.IsNotNull( subRange );

                /* TODO: Test non-operand properties when available
                // Assert.AreEqual( 0, subRange.LowerBound );
                // Assert.AreEqual( 3, subRange.Length );
                */

                Assert.IsNull( mdnode.VTableHolder );
                Assert.IsNull( mdnode.Operands[ 5 ] );

                Assert.IsNull( mdnode.TemplateParameters );
                Assert.IsNull( mdnode.Operands[ 6 ] );

                Assert.IsTrue( string.IsNullOrEmpty( mdnode.Identifier ) );
                Assert.IsNull( mdnode.Operands[ 7 ] );

                Assert.IsNull( mdnode.Discriminator );
                Assert.IsNull( mdnode.Operands[ 8 ] );

                Assert.AreSame( intType.DIType, mdnode.BaseType );
            }
        }
    }
}
