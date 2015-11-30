using Llvm.NET;
using Llvm.NET.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NETTests
{
    [TestClass]
    public class AttributeSetTests
    {
        [TestMethod]
        public void ParameterAttributesTest( )
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                // add attributes to all indeces of the function
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
        [Description("Verifies that mutation produces a new attribute set leaving the original unmodified across all indeces")]
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

                // ditto for each subsequent addition along all indeces
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
        [Description("Verifies that AttributeSet mutation with additional enum attributes into a new set propogates existing values into the new set")]
        public void EnumFunctionAttributeMutationPropagationTest( )
        {
            using( var ctx = new Context() )
            using( var module = new NativeModule( "test", ctx ) )
            {
                var structType = ctx.CreateStructType( "testT", false, ctx.Int32Type, ctx.Int8Type.CreateArrayType( 32 ) );
                var func = module.AddFunction( "test", ctx.GetFunctionType( ctx.VoidType, structType.CreatePointerType() ) );
                Assert.AreSame( func, func.Attributes.TargetFunction );

                var originalAttributes = func.Attributes;
                // shouldn't be created with any attributes
                Assert.IsFalse( originalAttributes.HasAny( FunctionAttributeIndex.Function ) );

                var newAttribs = func.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline );
                // creating the new attributeset shouldn't modify the original
                Assert.IsFalse( originalAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.AreSame( func, newAttribs.TargetFunction );

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
        [Description("Verifies that AttributeSet mutation with additional enum attributes into a new set propogates existing values into the new set")]
        public void EnumReturnAttributeMutationPropagationTest( )
        {
            using( var ctx = new Context() )
            using( var module = new NativeModule( "test", ctx ) )
            {
                var structType = ctx.CreateStructType( "testT", false, ctx.Int32Type, ctx.Int8Type.CreateArrayType( 32 ) );
                var func = module.AddFunction( "test", ctx.GetFunctionType( ctx.VoidType, structType.CreatePointerType() ) );
                Assert.AreSame( func, func.Attributes.TargetFunction );

                var originalAttributes = func.Attributes;
                // shouldn't be created with any attributes
                Assert.IsFalse( originalAttributes.HasAny( FunctionAttributeIndex.Function ) );

                var newAttribs = func.Attributes.Add( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline );
                // creating the new attributeset shouldn't modify the original
                Assert.IsFalse( originalAttributes.Has( FunctionAttributeIndex.Function, AttributeKind.AlwaysInline ) );
                Assert.AreSame( func, newAttribs.TargetFunction );

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
        [ExpectedArgumentException( "index", ExpectedExceptionMessage = "Specified parameter index exceeds the number of parameters in the function")]
        public void OutofRangeParameterIndexTest()
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.Attributes.Add(FunctionAttributeIndex.Parameter0 + 1, new AttributeValue( AttributeKind.Alignment, 64 ) );
            }
        }

        [TestMethod]
        [ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on functions")]
        public void AlignmentNotSupportedOnFunctionTest()
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.Attributes.Add(FunctionAttributeIndex.Function, new AttributeValue(AttributeKind.Alignment, 64) );
            }
        }

        [TestMethod]
        [ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on function Return")]
        public void StackAlignmentNotSupportedOnReturnTest()
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.Attributes.Add( FunctionAttributeIndex.ReturnType, new AttributeValue(AttributeKind.StackAlignment, 64) );
            }
        }

        [TestMethod]
        [ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on function parameter")]
        public void StackAlignmentNotSupportedOnParameterTest()
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.Attributes.Add( FunctionAttributeIndex.Parameter0, new AttributeValue(AttributeKind.StackAlignment, 64) );
            }
        }

        [TestMethod]
        [ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on functions")]
        public void DereferenceableNotSupportedOnFunctionTest()
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.Attributes.Add( FunctionAttributeIndex.Function, new AttributeValue(AttributeKind.Dereferenceable, 64) );
            }
        }

        [TestMethod]
        [ExpectedArgumentException( "index", ExpectedExceptionMessage = "Attribute not allowed on functions")]
        public void DereferenceableOrNullNotSupportedOnFunctionTest()
        {
            using( var module = new NativeModule( "test" ) )
            {
                var sig = module.Context.GetFunctionType( module.Context.Int8Type.CreatePointerType(), module.Context.Int32Type );
                var function = module.AddFunction( "test", sig );
                function.Attributes.Add( FunctionAttributeIndex.Function, new AttributeValue(AttributeKind.DereferenceableOrNull, 64) );
            }
        }

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

        // TODO: verify each applicability check for attributes and indeces
        // e.g. attempt to apply all attributes in AttributeKind to all indeces
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
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void HasTest( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void HasTest1( )
        {
            Assert.Inconclusive( );
        }

        [TestMethod]
        public void GetAllowedIndecesForAttributeTest( )
        {
            var parmOrReturn = FunctionIndexKind.Parameter | FunctionIndexKind.Return;
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.ZExt ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.SExt ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.InReg ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.ByVal ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.InAlloca ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.StructRet ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.Alignment ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoAlias ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoCapture ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.Nest ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.Returned ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NonNull ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.Dereferenceable ) );
            Assert.AreEqual( parmOrReturn, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.DereferenceableOrNull ) );

            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.AlwaysInline ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.Builtin ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.Cold ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.Convergent ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.InlineHint ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.JumpTable ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.MinSize ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.Naked ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoBuiltin ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoDuplicate ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoImplicitFloat ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoInline ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NonLazyBind ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoRedZone ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoReturn ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.NoUnwind ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.OptimizeForSize ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.OptimizeNone ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.ReadNone ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.ReadOnly ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.ArgMemOnly ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.ReturnsTwice ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.StackAlignment ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.StackProtect ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.StackProtectReq ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.StackProtectStrong ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.SafeStack ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.SanitizeAddress ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.SanitizeThread ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.SanitizeMemory ) );
            Assert.AreEqual( FunctionIndexKind.Function, AttributeSet.GetAllowedIndecesForAttribute( AttributeKind.UWTable ) );
        }
    }
}