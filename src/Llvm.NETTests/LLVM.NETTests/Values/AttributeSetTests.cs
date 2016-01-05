using System.Collections.Generic;
using System.Linq;
using Llvm.NET;
using Llvm.NET.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NETTests
{
    [TestClass]
    public class AttributeSetTests
    {
        [ClassInitialize]
        public static void ClassInitialize( TestContext testContext )
        {

        }

        [TestMethod]
        public void ParameterAttributesTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                // add attributes to all indices of the function
                var attributes = function.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline, AttributeKind.OptimizeNone );
                attributes = attributes.Add( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull );
                attributes = attributes.Add( FunctionAttributeIndex.Parameter0, AttributeKind.InReg );

                var paramAttributes = attributes.ParameterAttributes( 0 );
                Assert.AreNotSame( attributes, paramAttributes );
                // parameterAttributes should only have attributes on the parameter index (e.g. ParameterAttributes(0) should filter them)
                Assert.AreEqual( "inreg", paramAttributes.AsString( FunctionAttributeIndex.Parameter0 ) );
                Assert.AreEqual( string.Empty, paramAttributes.AsString( FunctionAttributeIndex.Function ) );
                Assert.AreEqual( string.Empty, paramAttributes.AsString( FunctionAttributeIndex.ReturnType ) );
            }
        }

        [TestMethod]
        public void GetParameterWhenNoAttributessetForParameterTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type, module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                // add attributes to all indices of the function
                var attributes = function.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline, AttributeKind.OptimizeNone )
                                                    .Add( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull )
                                                    .Add( FunctionAttributeIndex.Parameter0, AttributeKind.InReg );

                var paramAttributes = attributes.ParameterAttributes( 0 );
                Assert.AreNotSame( attributes, paramAttributes );
                // parameterAttributes should only have attributes on the parameter index (e.g. ParameterAttributes(0) should filter them)
                Assert.AreEqual( "inreg", paramAttributes.AsString( FunctionAttributeIndex.Parameter0 ) );
                var shouldBeEmptyAttributes = attributes.ParameterAttributes( 1 );
                Assert.IsFalse( shouldBeEmptyAttributes.AllAttributes.Any( ) );
            }
        }

        [TestMethod]
        public void AsStringForEmptySetIsEmptyStringTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type, module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                Assert.AreEqual( string.Empty, function.Attributes.AsString( FunctionAttributeIndex.Function ) );
                Assert.AreEqual( string.Empty, function.Attributes.AsString( FunctionAttributeIndex.ReturnType ) );
                Assert.AreEqual( string.Empty, function.Attributes.AsString( FunctionAttributeIndex.Parameter0 ) );
            }
        }

        [TestMethod]
        public void AsStringWithOneFunctionAttributeTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type, module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.AddAttributes( AttributeKind.AlwaysInline );
                Assert.AreEqual( "alwaysinline", function.Attributes.AsString( FunctionAttributeIndex.Function ) );
                Assert.AreEqual( string.Empty, function.Attributes.AsString( FunctionAttributeIndex.ReturnType ) );
                Assert.AreEqual( string.Empty, function.Attributes.AsString( FunctionAttributeIndex.Parameter0 ) );
            }
        }

        [TestMethod]
        public void AsStringWithTwoFunctionAttributeTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type, module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.AddAttributes( AttributeKind.AlwaysInline, AttributeKind.OptimizeNone );
                Assert.AreEqual( "alwaysinline optnone", function.Attributes.AsString( FunctionAttributeIndex.Function ) );
                Assert.AreEqual( string.Empty, function.Attributes.AsString( FunctionAttributeIndex.ReturnType ) );
                Assert.AreEqual( string.Empty, function.Attributes.AsString( FunctionAttributeIndex.Parameter0 ) );
            }
        }

        [TestMethod]
        [Description("Verifies that mutation produces a new attribute set leaving the original unmodified across all indices")]
        public void AttributeSetMutationProducesNewAttributeSet( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                var funcAttributes = function.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.NoInline, AttributeKind.OptimizeNone );
                var retAttributes = funcAttributes.Add( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull );
                var paramAttributes = retAttributes.Add( FunctionAttributeIndex.Parameter0, AttributeKind.InReg );

                // None of the attributeSets returned should be the same instance they started from
                Assert.AreNotSame( function.Attributes, funcAttributes );
                Assert.AreNotSame( funcAttributes, retAttributes );
                Assert.AreNotSame( retAttributes, paramAttributes );

                // starting attribute set should remain un-modified
                Assert.IsFalse( function.Attributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoInline ) );
                Assert.IsFalse( function.Attributes.Has( FunctionAttributeIndex.Function, AttributeKind.OptimizeNone ) );
                Assert.IsFalse( function.Attributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull ) );
                Assert.IsFalse( function.Attributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.InReg ) );

                // ditto for each subsequent addition along all indices
                Assert.IsTrue( funcAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoInline ) );
                Assert.IsTrue( funcAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.OptimizeNone ) );
                Assert.IsFalse( funcAttributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull ) );
                Assert.IsFalse( funcAttributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.InReg ) );

                Assert.IsTrue( retAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoInline ) );
                Assert.IsTrue( retAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.OptimizeNone ) );
                Assert.IsTrue( retAttributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull ) );
                Assert.IsFalse( retAttributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.InReg ) );

                // full set of attributes should be available in final instance
                Assert.IsTrue( paramAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoInline ) );
                Assert.IsTrue( paramAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.OptimizeNone ) );
                Assert.IsTrue( paramAttributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull ) );
                Assert.IsTrue( paramAttributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.InReg ) );
            }
        }

        [TestMethod]
        public void AsStringWithMultipleAttributesTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.AddAttributes( AttributeKind.AlwaysInline, AttributeKind.OptimizeNone );
                function.AddAttributes( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull );
                function.AddAttributes( FunctionAttributeIndex.Parameter0, AttributeKind.InReg );
                Assert.AreEqual( "alwaysinline optnone", function.Attributes.AsString( FunctionAttributeIndex.Function ) );
                Assert.AreEqual( "nonnull", function.Attributes.AsString( FunctionAttributeIndex.ReturnType ) );
                Assert.AreEqual( "inreg", function.Attributes.AsString( FunctionAttributeIndex.Parameter0 ) );
            }
        }

        [TestMethod]
        [Description("Verifies that AttributeSet mutation with additional enum attributes into a new set propagates existing values into the new set")]
        public void EnumFunctionAttributeMutationPropagationTest( )
        {
            using( var ctx = new Context() )
            using( var module = new NativeModule( "test", ctx ) )
            {
                var structType = ctx.CreateStructType( "testT", false, ctx.Int32Type, ctx.Int8Type.CreateArrayType( 32 ) );
                var func = module.AddFunction( "test", ctx.GetFunctionType( ctx.VoidType, structType.CreatePointerType() ) );

                var originalAttributes = func.Attributes;
                // shouldn't be created with any attributes
                Assert.IsFalse( originalAttributes.HasAny( FunctionAttributeIndex.Function ) );

                var newAttribs = func.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline );
                // creating the new AttributeSet shouldn't modify the original
                Assert.IsFalse( originalAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );

                Assert.IsTrue( newAttribs.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );

                // Add another attribute
                var additionalAttributes = newAttribs.Add( FunctionAttributeIndex.Function, AttributeKind.NoUnwind );
                Assert.IsTrue( additionalAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoUnwind ) );

                // function should still have the previous attributes, but not the new ones
                Assert.IsTrue( newAttribs.Has(FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.IsFalse( originalAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoUnwind ) );
                Assert.IsFalse( newAttribs.Has( FunctionAttributeIndex.Function, AttributeKind.NoUnwind ) );
            }
        }

        [TestMethod]
        [Description("Verifies that AttributeSet mutation with additional enum attributes into a new set propagates existing values into the new set")]
        public void EnumReturnAttributeMutationPropagationTest( )
        {
            using( var ctx = new Context() )
            using( var module = new NativeModule( "test", ctx ) )
            {
                var structType = ctx.CreateStructType( "testT", false, ctx.Int32Type, ctx.Int8Type.CreateArrayType( 32 ) );
                var func = module.AddFunction( "test", ctx.GetFunctionType( ctx.VoidType, structType.CreatePointerType() ) );

                var originalAttributes = func.Attributes;
                // shouldn't be created with any attributes
                Assert.IsFalse( originalAttributes.HasAny( FunctionAttributeIndex.Function ) );

                var newAttribs = func.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline );
                // creating the new AttributeSet shouldn't modify the original
                Assert.IsFalse( originalAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );

                Assert.IsTrue( newAttribs.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );

                // Add another attribute
                var additionalAttributes = newAttribs.Add( FunctionAttributeIndex.Function, AttributeKind.NoUnwind );
                Assert.IsTrue( additionalAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoUnwind ) );

                // function should still have the previous attributes, but not the new ones
                Assert.IsTrue( newAttribs.Has(FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.IsFalse( originalAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.NoUnwind ) );
                Assert.IsFalse( newAttribs.Has( FunctionAttributeIndex.Function, AttributeKind.NoUnwind ) );
            }
        }

        //[TestMethod]
        //[ExpectedArgumentException( "index", ExpectedExceptionMessage = "Specified parameter index exceeds the number of parameters in the function")]
        //public void OutofRangeParameterIndexTest()
        //{
        //    using( var module = new NativeModule( "test" ) )
        //    {
        //        var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
        //        var function = module.AddFunction( "test", sig );
        //        function.Attributes.Add(FunctionAttributeIndex.Parameter0 + 1, new AttributeValue( AttributeKind.Alignment, 64 ) );
        //    }
        //}

        //[TestMethod]
        //[ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on functions")]
        //public void AlignmentNotSupportedOnFunctionTest()
        //{
        //    using( var module = new NativeModule( "test" ) )
        //    {
        //        var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
        //        var function = module.AddFunction( "test", sig );
        //        function.Attributes.Add( FunctionAttributeIndex.Function, new AttributeValue(AttributeKind.Alignment, 64) );
        //    }
        //}

        //[TestMethod]
        //[ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on function Return")]
        //public void StackAlignmentNotSupportedOnReturnTest()
        //{
        //    using( var module = new NativeModule( "test" ) )
        //    {
        //        var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
        //        var function = module.AddFunction( "test", sig );
        //        function.Attributes.Add( FunctionAttributeIndex.ReturnType, new AttributeValue(AttributeKind.StackAlignment, 64) );
        //    }
        //}

        //[TestMethod]
        //[ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on function parameter")]
        //public void StackAlignmentNotSupportedOnParameterTest()
        //{
        //    using( var module = new NativeModule( "test" ) )
        //    {
        //        var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
        //        var function = module.AddFunction( "test", sig );
        //        function.Attributes.Add( FunctionAttributeIndex.Parameter0, new AttributeValue(AttributeKind.StackAlignment, 64) );
        //    }
        //}

        //[TestMethod]
        //[ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on functions")]
        //public void DereferenceableNotSupportedOnFunctionTest()
        //{
        //    using( var module = new NativeModule( "test" ) )
        //    {
        //        var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
        //        var function = module.AddFunction( "test", sig );
        //        function.Attributes.Add( FunctionAttributeIndex.Function, new AttributeValue(AttributeKind.Dereferenceable, 64) );
        //    }
        //}

        //[TestMethod]
        //[ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on functions")]
        //public void DereferenceableOrNullNotSupportedOnFunctionTest()
        //{
        //    using( var module = new NativeModule( "test" ) )
        //    {
        //        var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
        //        var function = module.AddFunction( "test", sig );
        //        function.Attributes.Add( FunctionAttributeIndex.Function, new AttributeValue(AttributeKind.DereferenceableOrNull, 64) );
        //    }
        //}

        [TestMethod]
        public void AddAndGetParameterAlignmentAttributeTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                var attributes = function.Attributes.Add( FunctionAttributeIndex.Parameter0, new AttributeValue(AttributeKind.Alignment, 64) );
                var actual = attributes.GetAttributeValue( FunctionAttributeIndex.Parameter0, AttributeKind.Alignment );
                Assert.AreEqual( 64ul, actual );
            }
        }

        [TestMethod]
        public void AddAndGetParameterDereferenceableAttributeTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                var attributes = function.Attributes.Add( FunctionAttributeIndex.Parameter0, new AttributeValue(AttributeKind.Dereferenceable, 64) );
                var actual = attributes.GetAttributeValue( FunctionAttributeIndex.Parameter0, AttributeKind.Dereferenceable );
                Assert.AreEqual( 64ul, actual );
            }
        }

        [TestMethod]
        public void AddAndGetParameterDereferenceableOrNullAttributeTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                var attributes = function.Attributes.Add( FunctionAttributeIndex.Parameter0, new AttributeValue(AttributeKind.DereferenceableOrNull, 64) );
                var actual = attributes.GetAttributeValue( FunctionAttributeIndex.Parameter0, AttributeKind.DereferenceableOrNull );
                Assert.AreEqual( 64ul, actual );
            }
        }

        [TestMethod]
        public void AddAndGetFunctionStackAlignmentAttributeTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                var attributes = function.Attributes.Add( FunctionAttributeIndex.Function, new AttributeValue(AttributeKind.StackAlignment, 64) );
                var actual = attributes.GetAttributeValue( FunctionAttributeIndex.Function, AttributeKind.StackAlignment );
                Assert.AreEqual( 64ul, actual );
            }
        }

        // TODO: verify each applicability check for attributes and indices
        // e.g. attempt to apply all attributes in AttributeKind to all indices
        // and ensure invalid ones are flagged with an argument exception...

        // TODO: String attributes and values on each index...

        [TestMethod]
        public void RemoveTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                var attributes = function.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline, AttributeKind.OptimizeNone );
                attributes = attributes.Add( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull );
                attributes = attributes.Add( FunctionAttributeIndex.Parameter0, AttributeKind.InReg );

                // verify all the expected attributes are present before trying to remove one
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.InReg ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.OptimizeNone ) );

                attributes = attributes.Remove( FunctionAttributeIndex.Function, AttributeKind.OptimizeNone );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.InReg ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.IsFalse( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.OptimizeNone ) );
            }
        }

        [TestMethod]
        public void RemoveNamedAttributeTest1( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                var attributes = function.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline );
                attributes = attributes.Add( FunctionAttributeIndex.Function, "testattr" );
                attributes = attributes.Add( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull );
                attributes = attributes.Add( FunctionAttributeIndex.Parameter0, AttributeKind.InReg );

                // verify all the expected attributes are present before trying to remove one
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.InReg ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Function, "testattr" ) );

                attributes = attributes.Remove( FunctionAttributeIndex.Function, "testattr" );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.NonNull ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.InReg ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.IsFalse( attributes.Has( FunctionAttributeIndex.Function, "testattr" ) );
            }
        }

        [TestMethod]
        public void HasAnyTest( )
        {
            using( var ctx = new Context( ) )
            {
                var attributes = new AttributeSet( );
                Assert.IsFalse( attributes.HasAny( FunctionAttributeIndex.Function ) );
                Assert.IsFalse( attributes.HasAny( FunctionAttributeIndex.ReturnType ) );
                Assert.IsFalse( attributes.HasAny( FunctionAttributeIndex.Parameter0 ) );
                Assert.IsFalse( attributes.HasAny( FunctionAttributeIndex.Parameter0 + 1 ) );

                attributes = attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline, AttributeKind.Builtin )
                                       .Add( FunctionAttributeIndex.ReturnType, new AttributeValue( AttributeKind.DereferenceableOrNull, 1 ) )
                                       .Add( FunctionAttributeIndex.Parameter0, AttributeKind.ByVal )
                                       .Add( FunctionAttributeIndex.Parameter0 + 1, "TestCustom" );

                Assert.IsTrue( attributes.HasAny( FunctionAttributeIndex.Function ) );
                Assert.IsTrue( attributes.HasAny( FunctionAttributeIndex.ReturnType ) );
                Assert.IsTrue( attributes.HasAny( FunctionAttributeIndex.Parameter0 ) );
                Assert.IsTrue( attributes.HasAny( FunctionAttributeIndex.Parameter0 + 1 ) );
            }
        }

        [TestMethod]
        public void HasTest( )
        {
            using( var ctx = new Context( ) )
            {
                var attributes = new AttributeSet( );

                Assert.IsFalse( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.IsFalse( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.Builtin ) );
                Assert.IsFalse( attributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.DereferenceableOrNull ) );
                Assert.IsFalse( attributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.ByVal ) );
                Assert.IsFalse( attributes.Has( FunctionAttributeIndex.Parameter0 + 1, "TestCustom" ) );

                attributes = attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline, AttributeKind.Builtin )
                                       .Add( FunctionAttributeIndex.ReturnType, new AttributeValue( AttributeKind.DereferenceableOrNull, 1 ) )
                                       .Add( FunctionAttributeIndex.Parameter0, AttributeKind.ByVal )
                                       .Add( FunctionAttributeIndex.Parameter0 + 1, "TestCustom" );

                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Function, AttributeKind.Builtin ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.ReturnType, AttributeKind.DereferenceableOrNull ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Parameter0, AttributeKind.ByVal ) );
                Assert.IsTrue( attributes.Has( FunctionAttributeIndex.Parameter0 + 1, "TestCustom" ) );
            }
        }

        [TestMethod]
        public void GetAllowedindicesForAttributeTest( )
        {
            var parmOrReturn = FunctionIndexKinds.Parameter | FunctionIndexKinds.Return;
            Assert.AreEqual( parmOrReturn, AttributeKind.ZExt.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.SExt.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.InReg.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.ByVal.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.InAlloca.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.StructRet.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.Alignment.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.NoAlias.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.NoCapture.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.Nest.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.Returned.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.NonNull.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.Dereferenceable.GetAllowedindices( ) );
            Assert.AreEqual( parmOrReturn, AttributeKind.DereferenceableOrNull.GetAllowedindices( ) );

            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.AlwaysInline.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.Builtin.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.Cold.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.Convergent.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.InlineHint.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.JumpTable.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.MinSize.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.Naked.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.NoBuiltin.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.NoDuplicate.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.NoImplicitFloat.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.NoInline.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.NonLazyBind.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.NoRedZone.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.NoReturn.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.NoUnwind.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.OptimizeForSize.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.OptimizeNone.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.ReadNone.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.ReadOnly.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.ArgMemOnly.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.ReturnsTwice.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.StackAlignment.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.StackProtect.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.StackProtectReq.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.StackProtectStrong.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.SafeStack.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.SanitizeAddress.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.SanitizeThread.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.SanitizeMemory.GetAllowedindices( ) );
            Assert.AreEqual( FunctionIndexKinds.Function, AttributeKind.UWTable.GetAllowedindices( ) );
        }

        [TestMethod]
        [Description("Verifies that the AllAttributesProperty contains the attributes specified for an attribute set")]
        public void AttributeSetAllAttributesTest( )
        {
            using( var ctx = new Context( ) )
            {
                var attributes = new AttributeSet( ).Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline, AttributeKind.Builtin )
                                                    .Add( FunctionAttributeIndex.ReturnType, new AttributeValue( AttributeKind.DereferenceableOrNull, 1 ) )
                                                    .Add( FunctionAttributeIndex.Parameter0, AttributeKind.ByVal )
                                                    .Add( FunctionAttributeIndex.Parameter0 + 1, "TestCustom" );

                var expectedSet = new HashSet<IndexedAttributeValue>
                    { new IndexedAttributeValue( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline )
                    , new IndexedAttributeValue( FunctionAttributeIndex.Function, AttributeKind.Builtin )
                    , new IndexedAttributeValue( FunctionAttributeIndex.ReturnType, new AttributeValue(AttributeKind.DereferenceableOrNull, 1 ) )
                    , new IndexedAttributeValue( FunctionAttributeIndex.Parameter0, AttributeKind.ByVal )
                    , new IndexedAttributeValue( FunctionAttributeIndex.Parameter0 + 1, "TestCustom" )
                    };

                Assert.IsTrue( expectedSet.SetEquals( attributes.AllAttributes ) );
            }
        }
    }
}