// -----------------------------------------------------------------------
// <copyright file="AttributeValueTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Llvm.NET.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/* ReSharper disable StringLiteralTypo */
namespace Llvm.NET.Tests
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
            Assert.IsFalse( value.IntegerValue.HasValue );
            Assert.IsFalse( value.IsInt );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "alwaysinline", value.Name );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsEnum );
            Assert.AreEqual( AttributeKind.AlwaysInline, value.Kind );
        }

        [TestMethod]
        public void AttributeValueEnumInt( )
        {
            using var ctx = new Context( );
            var value = ctx.CreateAttribute( AttributeKind.DereferenceableOrNull, 1234ul );
            Assert.IsTrue( value.IntegerValue.HasValue );
            Assert.IsTrue( value.IsInt );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "dereferenceable_or_null", value.Name );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsEnum );
            Assert.AreEqual( AttributeKind.DereferenceableOrNull, value.Kind );
            Assert.AreEqual( value.IntegerValue, 1234ul );
        }

        [TestMethod]
        public void AttributeValueEnumString( )
        {
            using var ctx = new Context( );
            var value = ctx.CreateAttribute( TestTargetDependentAttributeName );
            Assert.IsFalse( value.IntegerValue.HasValue );
            Assert.IsFalse( value.IsInt );
            Assert.IsTrue( value.IsString );
            Assert.AreEqual( TestTargetDependentAttributeName, value.Name );
            Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue ) );
            Assert.IsFalse( value.IsEnum );
            Assert.AreEqual( AttributeKind.None, value.Kind );
        }

        [TestMethod]
        public void ImplicitCastAttributeKindToAttributeValueTest( )
        {
            using var ctx = new Context( );
            AttributeValue value = ctx.CreateAttribute( AttributeKind.NoInline );
            Assert.IsFalse( value.IntegerValue.HasValue );
            Assert.IsFalse( value.IsInt );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "noinline", value.Name );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsEnum );
            Assert.AreEqual( AttributeKind.NoInline, value.Kind );
        }

        [TestMethod]
        public void ImplicitCastStringToAttributeValueTest( )
        {
            using var ctx = new Context( );
            AttributeValue value = ctx.CreateAttribute( TestTargetDependentAttributeName );
            Assert.IsFalse( value.IntegerValue.HasValue );
            Assert.IsFalse( value.IsInt );
            Assert.IsTrue( value.IsString );
            Assert.AreEqual( TestTargetDependentAttributeName, value.Name );
            Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue ) );
            Assert.IsFalse( value.IsEnum );
            Assert.AreEqual( AttributeKind.None, value.Kind );
        }

        // test all attributes for an index are available and reflect attributes set
        // (verifies [In,Out] array marshaling is functioning correctly)
        [TestMethod]
        public void AttributeIndexTest( )
        {
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( );
            var signature = ctx.GetFunctionType( ctx.DoubleType, ctx.Int8Type.CreatePointerType( ), ctx.Int32Type );
            var function = module.AddFunction( "test", signature );
            function.Parameters[ 0 ].AddAttributes( AttributeKind.Nest, AttributeKind.ByVal );
            var attributes = function.GetAttributesAtIndex( FunctionAttributeIndex.Parameter0 ).ToArray( );
            Assert.AreEqual( 2, attributes.Length );
            Assert.IsTrue( attributes.Contains( AttributeKind.Nest ) );
            Assert.IsTrue( attributes.Contains( AttributeKind.ByVal ) );
        }
    }
}
