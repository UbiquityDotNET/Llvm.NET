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
                var newAttribs = func.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline );
                func.Attributes = newAttribs;
                var newParamAttribs = func.Attributes.Add( FunctionAttributeIndex.Parameter0, new AttributeValue(AttributeKind.Alignment, 4 ) );
                func.Attributes = newParamAttribs;
                Assert.IsTrue( func.Attributes.Has(FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.AreEqual( 4ul, func.Attributes.GetAttributeValue( FunctionAttributeIndex.Parameter0, AttributeKind.Alignment ) );
            }
        }

        [TestMethod( )]
        public void AddTest1( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void AddTest2( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod( )]
        public void AddTest3( )
        {
            Assert.Inconclusive( );
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