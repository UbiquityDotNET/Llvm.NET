using Llvm.NETTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Values.Tests
{
    [TestClass]
    public class AttributeValueTests
    {
        private const string TestTargetDependentAttributeName = "TestCustom";

        [TestMethod]
        public void AttributeValueTest( )
        {
            var value = new AttributeValue( );
            Assert.IsFalse( value.IntegerValue.HasValue );
            Assert.IsFalse( value.IsEnum );
            Assert.IsFalse( value.IsInt );
            Assert.IsFalse( value.IsString );
            Assert.IsNull( value.Name );
            Assert.IsNull( value.StringValue );
            Assert.AreEqual( AttributeKind.None, value.Kind );
        }

        [TestMethod]
        [ExpectedArgumentException("context", ExpectedExceptionMessage = "Provided context cannot be null or disposed")]
        public void AttributeValueConstructorNoContextTest( )
        {
            var value = new AttributeValue( null, AttributeKind.AlwaysInline );
        }

        [TestMethod]
        public void AttributeValueTestEnum( )
        {
            using( var ctx = new Context( ) )
            {
                var value = new AttributeValue(ctx, AttributeKind.AlwaysInline );
                Assert.IsFalse( value.IntegerValue.HasValue );
                Assert.IsFalse( value.IsInt );
                Assert.IsFalse( value.IsString );
                Assert.IsNull( value.Name );
                Assert.IsNull( value.StringValue );
                Assert.IsTrue( value.IsEnum );
                Assert.IsTrue( value.Kind.HasValue );
                Assert.AreEqual( AttributeKind.AlwaysInline, value.Kind.Value );
            }
        }

        [TestMethod]
        public void AttributeValueEnumInt( )
        {
            using( var ctx = new Context( ) )
            {
                var value = new AttributeValue(ctx, AttributeKind.DereferenceableOrNull, 1234ul );
                Assert.IsTrue( value.IntegerValue.HasValue );
                Assert.IsTrue( value.IsInt );
                Assert.IsFalse( value.IsString );
                Assert.IsNull( value.Name );
                Assert.IsNull( value.StringValue );
                Assert.IsTrue( value.IsEnum );
                Assert.IsTrue( value.Kind.HasValue );
                Assert.AreEqual( AttributeKind.DereferenceableOrNull, value.Kind.Value );
                Assert.AreEqual( value.IntegerValue, 1234ul );
            }
        }

        [TestMethod]
        public void AttributeValueEnumString( )
        {
            using( var ctx = new Context( ) )
            {
                var value = new AttributeValue( ctx, TestTargetDependentAttributeName );
                Assert.IsFalse( value.IntegerValue.HasValue );
                Assert.IsFalse( value.IsInt );
                Assert.IsTrue( value.IsString );
                Assert.AreEqual( TestTargetDependentAttributeName, value.Name );
                Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue ) );
                Assert.IsFalse( value.IsEnum );
                Assert.IsFalse( value.Kind.HasValue );
            }
        }

        [TestMethod]
        public void ImplicitCastAttributeKindToAttributeValueTest()
        {
            using( var ctx = new Context( ) )
            {
                AttributeValue value = AttributeKind.NoInline.ToAttributeValue( ctx );
                Assert.IsFalse( value.IntegerValue.HasValue );
                Assert.IsFalse( value.IsInt );
                Assert.IsFalse( value.IsString );
                Assert.IsNull( value.Name );
                Assert.IsNull( value.StringValue );
                Assert.IsTrue( value.IsEnum );
                Assert.IsTrue( value.Kind.HasValue );
                Assert.AreEqual( AttributeKind.NoInline, value.Kind.Value );
            }
        }

        [TestMethod]
        public void ImplicitCastStringToAttributeValueTest( )
        {
            using( var ctx = new Context( ) )
            {
                AttributeValue value = TestTargetDependentAttributeName.ToAttributeValue( ctx );
                Assert.IsFalse( value.IntegerValue.HasValue );
                Assert.IsFalse( value.IsInt );
                Assert.IsTrue( value.IsString );
                Assert.AreEqual( TestTargetDependentAttributeName, value.Name );
                Assert.IsTrue( string.IsNullOrWhiteSpace( value.StringValue ) );
                Assert.IsFalse( value.IsEnum );
                Assert.IsFalse( value.Kind.HasValue );
            }
        }

        // test all int value parameters to ensure that a value is provided (implicit casting from enum should provide default value of 0)
    }
}