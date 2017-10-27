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
        [TestMethod]
        public void ResolveCyclesTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void ReplaceAllUsesWithTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void OperandsAreAccessibleTest()
        {
            using( var ctx = new Context( ) )
            using( var module = new BitcodeModule( ctx, "test.bc", SourceLanguage.CSharp, "test.cs", "unittests" ) )
            using( var targetMachine = TargetTests.GetTargetMachine( ) )
            {
                module.Layout = targetMachine.TargetData;
                var intType = new DebugBasicType( module.Context.Int32Type, module, "int", DiTypeKind.Signed );
                var arrayType = new DebugArrayType( intType, module, 3u);
                Assert.IsNotNull( arrayType );

                var mdnode = arrayType.DIType as DICompositeType;
                Assert.IsNotNull( mdnode.Operands );
                Assert.AreEqual( 8, mdnode.Operands.Count );

                Assert.IsNotNull( mdnode.Operands[ 0 ] ); // File
                Assert.AreSame( mdnode, mdnode.Operands[ 0 ].OwningNode );
                Assert.IsNull( mdnode.Operands[ 0 ].Metadata );

                Assert.IsNotNull( mdnode.Operands[ 1 ] ); // Scope
                Assert.AreSame( mdnode, mdnode.Operands[ 1 ].OwningNode );
                Assert.IsNull( mdnode.Operands[ 1 ].Metadata );

                Assert.IsNotNull( mdnode.Operands[ 2 ] ); // Name
                Assert.AreSame( mdnode, mdnode.Operands[ 2 ].OwningNode );
                Assert.IsNull( mdnode.Operands[ 2 ].Metadata );

                Assert.IsNotNull( mdnode.Operands[ 3 ] ); // BaseType
                Assert.AreSame( mdnode, mdnode.Operands[ 3 ].OwningNode );
                Assert.IsNotNull( mdnode.Operands[ 3 ].Metadata );

                Assert.IsNotNull( mdnode.Operands[ 4 ] ); // Elements
                Assert.AreSame( mdnode, mdnode.Operands[ 4 ].OwningNode );
                Assert.IsNotNull( mdnode.Operands[ 4 ].Metadata );

                Assert.IsNotNull( mdnode.Operands[ 5 ] ); // VTableHolder
                Assert.AreSame( mdnode, mdnode.Operands[ 5 ].OwningNode );
                Assert.IsNull( mdnode.Operands[ 5 ].Metadata );

                Assert.IsNotNull( mdnode.Operands[ 6 ] ); // TemplateParams
                Assert.AreSame( mdnode, mdnode.Operands[ 6 ].OwningNode );
                Assert.IsNull( mdnode.Operands[ 6 ].Metadata );

                Assert.IsNotNull( mdnode.Operands[ 7 ] ); // Identifier
                Assert.AreSame( mdnode, mdnode.Operands[ 7 ].OwningNode );
                Assert.IsNull( mdnode.Operands[ 7 ].Metadata );

                Assert.AreSame( intType.DIType, mdnode.BaseType );
                Assert.AreEqual( 1, mdnode.Elements.Count );
                var subRange = mdnode.Elements[ 0 ] as DISubRange;
                Assert.IsNotNull( subRange );
            }
        }
    }
}
