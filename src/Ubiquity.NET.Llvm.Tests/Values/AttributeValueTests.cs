// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
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
            var attribInfo = AttributeInfo.From("alwaysinline");
            var value = ctx.CreateAttribute( "alwaysinline");
            Assert.AreEqual( AttributeArgKind.None, value.AttributeInfo.ArgKind );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "alwaysinline", value.Name.ToString() );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsEnum );
            Assert.AreEqual( attribInfo.ID, value.Id );
        }

        [TestMethod]
        public void AttributeValueEnumInt( )
        {
            using var ctx = new Context( );
            var attribInfo = AttributeInfo.From("dereferenceable_or_null");
            var value = ctx.CreateAttribute( "dereferenceable_or_null", 1234ul );
            Assert.AreEqual( AttributeArgKind.Int, value.AttributeInfo.ArgKind );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "dereferenceable_or_null", value.Name.ToString() );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsInt );
            Assert.AreEqual( attribInfo.ID, value.Id );
            Assert.AreEqual( 1234ul, value.IntegerValue );
        }

        [TestMethod]
        public void AttributeValueEnumString( )
        {
            using var ctx = new Context( );
            var value = ctx.CreateAttribute( TestTargetDependentAttributeName );
            Assert.IsFalse( value.IsEnum );
            Assert.IsFalse( value.IsType );
            Assert.IsTrue( value.IsString );

            Assert.AreEqual( AttributeArgKind.String, value.AttributeInfo.ArgKind );
            Assert.AreEqual( TestTargetDependentAttributeName, value.Name.ToString() );
            Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue?.ToString() ) );
            Assert.IsFalse( value.IsEnum );
            Assert.AreEqual( 0u, value.Id );
        }

        [TestMethod]
        public void ImplicitCastAttributeKindToAttributeValueTest( )
        {
            using var ctx = new Context( );
            var attribInfo = AttributeInfo.From("noinline");
            AttributeValue value = ctx.CreateAttribute( "noinline" );
            Assert.IsTrue( value.IsEnum );
            Assert.IsFalse( value.IsInt );
            Assert.IsFalse( value.IsType );
            Assert.IsFalse( value.IsString );
            Assert.AreEqual( "noinline", value.Name.ToString() );
            Assert.IsNull( value.StringValue );
            Assert.IsTrue( value.IsEnum );
            Assert.AreEqual( attribInfo.ID, value.Id );
        }

        [TestMethod]
        public void ImplicitCastStringToAttributeValueTest( )
        {
            using var ctx = new Context( );
            AttributeValue value = ctx.CreateAttribute( TestTargetDependentAttributeName );
            Assert.IsFalse( value.IsInt );
            Assert.IsFalse( value.IsType );
            Assert.IsTrue( value.IsString );
            Assert.AreEqual( TestTargetDependentAttributeName, value.Name.ToString() );
            Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue?.ToString() ) );
            Assert.IsFalse( value.IsEnum );
            Assert.AreEqual( 0u, value.Id );
        }

        // test all attributes for an index are available and reflect attributes set
        // (verifies [In,Out] array marshaling is functioning correctly)
        [TestMethod]
        [SuppressMessage( "StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "byValInt32Attrib is NOT Hungarian" )]
        public void AttributeIndexTest( )
        {
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( );
            var nestAttrib = ctx.CreateAttribute("nest");
            var byValInt32Attrib = ctx.CreateAttribute("byval", ctx.Int32Type);

            var signature = ctx.GetFunctionType( ctx.Int32Type , ctx.DoubleType, ctx.Int8Type.CreatePointerType( ));
            var function = module.CreateFunction( "test", signature );

            function.Parameters[ 0 ].AddAttributes( nestAttrib, byValInt32Attrib );
            var attributes = function.GetAttributesAtIndex( FunctionAttributeIndex.Parameter0 ).ToArray( );
            Assert.AreEqual( 2, attributes.Length );
            Assert.IsTrue( attributes.Contains( nestAttrib ) );
            Assert.IsTrue( attributes.Contains( byValInt32Attrib ) );
        }
    }
}
