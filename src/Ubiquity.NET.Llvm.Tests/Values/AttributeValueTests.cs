// -----------------------------------------------------------------------
// <copyright file="AttributeValueTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Values;

/* ReSharper disable StringLiteralTypo */
namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class AttributeValueTests
    {
        private const string TestTargetDependentAttributeName = "TestCustom";

        [TestMethod]
        public void AttributeValueTestEnum( )
        {
            using var ctx = new Context( );
            var value = ctx.CreateAttribute( AttributeKind.AlwaysInline );
            Assert.IsFalse( value.Id.IsIntKind() );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "alwaysinline", value.Name );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsEnum );
            Assert.AreEqual( AttributeKind.AlwaysInline, value.Id );
        }

        [TestMethod]
        public void AttributeValueEnumInt( )
        {
            using var ctx = new Context( );
            var value = ctx.CreateAttribute( AttributeKind.DereferenceableOrNull, 1234ul );
            Assert.IsTrue( value.Id.IsIntKind() );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "dereferenceable_or_null", value.Name );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsEnum );
            Assert.AreEqual( AttributeKind.DereferenceableOrNull, value.Id);
            Assert.AreEqual( 1234ul, value.IntegerValue);
        }

        [TestMethod]
        public void AttributeValueEnumString( )
        {
            using var ctx = new Context( );
            var value = ctx.CreateAttribute( TestTargetDependentAttributeName );
            Assert.IsFalse( value.Id.IsIntKind() );
            Assert.IsFalse( value.Id.IsEnumKind() );
            Assert.IsFalse( value.Id.IsTypeKind() );
            Assert.IsTrue( value.IsString );
            Assert.AreEqual( TestTargetDependentAttributeName, value.Name );
            Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue ) );
            Assert.IsFalse( value.IsEnum );
            Assert.AreEqual( AttributeKind.None, value.Id );
        }

        [TestMethod]
        public void ImplicitCastAttributeKindToAttributeValueTest( )
        {
            using var ctx = new Context( );
            AttributeValue value = ctx.CreateAttribute( AttributeKind.NoInline );
            Assert.IsFalse( value.Id.IsIntKind() );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "noinline", value.Name );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsEnum );
            Assert.AreEqual( AttributeKind.NoInline, value.Id );
        }

        [TestMethod]
        public void ImplicitCastStringToAttributeValueTest( )
        {
            using var ctx = new Context( );
            AttributeValue value = ctx.CreateAttribute( TestTargetDependentAttributeName );
            Assert.IsFalse( value.Id.IsIntKind() );
            Assert.IsTrue( value.IsString );
            Assert.AreEqual( TestTargetDependentAttributeName, value.Name );
            Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue ) );
            Assert.IsFalse( value.IsEnum );
            Assert.AreEqual( AttributeKind.None, value.Id );
        }

        // test all attributes for an index are available and reflect attributes set
        // (verifies [In,Out] array marshaling is functioning correctly)
        [TestMethod]
        public void AttributeIndexTest( )
        {
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( );
            var signature = ctx.GetFunctionType( ctx.DoubleType, ctx.Int8Type.CreatePointerType( ), ctx.Int32Type );
            var function = module.CreateFunction( "test", signature );
            function.Parameters[ 0 ].AddAttributes( AttributeKind.Nest, AttributeKind.ByVal );
            var attributes = function.GetAttributesAtIndex( FunctionAttributeIndex.Parameter0 ).ToArray( );
            Assert.AreEqual( 2, attributes.Length );
            Assert.IsTrue( attributes.Contains( AttributeKind.Nest ) );
            Assert.IsTrue( attributes.Contains( AttributeKind.ByVal ) );
        }
    }
}
