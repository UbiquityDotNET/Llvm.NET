using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Values.Tests
{
    [TestClass]
    public class AttributeValueTests
    {
        private const string TestTargetDependentAttributeName = "TestCustom";

        [TestMethod]
        public void AttributeValueTestEnum( )
        {
            using( var ctx = new Context( ) )
            {
                var value = ctx.CreateAttribute( AttributeKind.AlwaysInline );
                Assert.IsFalse( value.IntegerValue.HasValue );
                Assert.IsFalse( value.IsInt );
                Assert.IsFalse( value.IsString );
                Assert.AreEqual( "alwaysinline", value.Name );
                Assert.IsNull( value.StringValue );
                Assert.IsTrue( value.IsEnum );
                Assert.AreEqual( AttributeKind.AlwaysInline, value.Kind );
            }
        }

        [TestMethod]
        public void AttributeValueEnumInt( )
        {
            using( var ctx = new Context( ) )
            {
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
        }

        [TestMethod]
        public void AttributeValueEnumString( )
        {
            using( var ctx = new Context( ) )
            {
                var value = ctx.CreateAttribute( TestTargetDependentAttributeName );
                Assert.IsFalse( value.IntegerValue.HasValue );
                Assert.IsFalse( value.IsInt );
                Assert.IsTrue( value.IsString );
                Assert.AreEqual( TestTargetDependentAttributeName, value.Name );
                Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue ) );
                Assert.IsFalse( value.IsEnum );
                Assert.AreEqual( AttributeKind.None, value.Kind );
            }
        }

        [TestMethod]
        public void ImplicitCastAttributeKindToAttributeValueTest()
        {
            using( var ctx = new Context( ) )
            {
                AttributeValue value = ctx.CreateAttribute( AttributeKind.NoInline );
                Assert.IsFalse( value.IntegerValue.HasValue );
                Assert.IsFalse( value.IsInt );
                Assert.IsFalse( value.IsString );
                Assert.AreEqual( "noinline", value.Name );
                Assert.IsNull( value.StringValue );
                Assert.IsTrue( value.IsEnum );
                Assert.AreEqual( AttributeKind.NoInline, value.Kind );
            }
        }

        [TestMethod]
        public void ImplicitCastStringToAttributeValueTest( )
        {
            using( var ctx = new Context( ) )
            {
                AttributeValue value = ctx.CreateAttribute( TestTargetDependentAttributeName );
                Assert.IsFalse( value.IntegerValue.HasValue );
                Assert.IsFalse( value.IsInt );
                Assert.IsTrue( value.IsString );
                Assert.AreEqual( TestTargetDependentAttributeName, value.Name );
                Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue ) );
                Assert.IsFalse( value.IsEnum );
                Assert.AreEqual( AttributeKind.None, value.Kind );
            }
        }

        // test all int value parameters to ensure that a value is provided (implicit casting from enum should provide default value of 0)
    }
}