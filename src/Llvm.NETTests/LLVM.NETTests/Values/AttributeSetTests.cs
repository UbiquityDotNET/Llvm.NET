using Microsoft.VisualStudio.TestTools.UnitTesting;
using Llvm.NET.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llvm.NET.Values.Tests
{
    [TestClass( )]
    public class AttributeSetTests
    {
        [TestMethod( )]
        public void ParameterAttributesTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void AsStringTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void AddTest( )
        {
            using( var ctx = new Context( ) )
            using( var module = new NativeModule( "test", ctx ) )
            {
                var structType = ctx.CreateStructType( "testT", false, ctx.Int32Type, ctx.Int8Type.CreateArrayType( 32 ) );
                var func = module.AddFunction( "test", ctx.GetFunctionType( ctx.VoidType, structType.CreatePointerType() ) );
                // shouldn't be created with an attribute
                Assert.IsFalse( func.Attributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );

                var newAttribs = func.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline );
                // creating the new attributeset shouldn't modify the original
                Assert.IsFalse( func.Attributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );

                func.Attributes = newAttribs;
                // replaced attributes, so function should now have the inline attribute
                Assert.IsTrue( func.Attributes.Has(FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );

                var newParamAttribs = func.Attributes.Add( FunctionAttributeIndex.Parameter0, new AttributeValue(AttributeKind.Alignment, 4 ) );
                func.Attributes = newParamAttribs;
                Assert.AreEqual( 4ul, func.Attributes.GetAttributeValue( FunctionAttributeIndex.Parameter0, AttributeKind.Alignment ) );

                // function should still have the inline attribute (e.g. adding the param attribute didn't erase the function attribute)
                Assert.IsTrue( func.Attributes.Has(FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );

                // Add another function attribute
                func.Attributes = func.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.NoUnwind );
                Assert.IsTrue( func.Attributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoUnwind ) );

                // function should still have the previous attributes
                Assert.IsTrue( func.Attributes.Has(FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.AreEqual( 4ul, func.Attributes.GetAttributeValue( FunctionAttributeIndex.Parameter0, AttributeKind.Alignment ) );
            }
        }

        [TestMethod( )]
        public void RemoveTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void RemoveTest1( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void GetAttributeValueTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void HasAnyTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void HasTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void HasTest1( )
        {
            Assert.Inconclusive( );
        }
    }
}