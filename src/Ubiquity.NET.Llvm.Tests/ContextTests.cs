// -----------------------------------------------------------------------
// <copyright file="ContextTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

// warning SA1500: Braces for multi-line statements must not share line
#pragma warning disable SA1500

namespace Ubiquity.NET.Llvm.Tests
{
    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        [TestCategory( "Constructor" )]
        public void SimpleConstructorDisposeTest( )
        {
            var context = new Context( );
            using( context )
            {
                Assert.IsNotNull( context );
                Assert.IsFalse( context.IsDisposed );
            }

            Assert.IsTrue( context.IsDisposed );
        }

        [TestMethod]
        [TestCategory( "Primitive Types" )]
        public void GetPointerTypeForTest( )
        {
            using var context = new Context( );
            var int8PtrType = context.GetPointerTypeFor( context.Int8Type );
            Assert.IsNotNull( int8PtrType );
            Assert.AreSame( context.Int8Type, int8PtrType.ElementType );
            Assert.AreSame( context, int8PtrType.Context );

            var int8PtrTypeAlt = context.Int8Type.CreatePointerType( );
            Assert.AreSame( int8PtrType, int8PtrTypeAlt );
        }

        [TestMethod]
        [TestCategory( "Primitive Types" )]
        public void VoidTypePropertyTest( )
        {
            using var context = new Context( );
            var voidType = context.VoidType;
            Assert.AreEqual( TypeKind.Void, voidType.Kind );
            Assert.IsFalse( voidType.IsDouble );
            Assert.IsFalse( voidType.IsFloat );
            Assert.IsFalse( voidType.IsFloatingPoint );
            Assert.IsFalse( voidType.IsInteger );
            Assert.IsFalse( voidType.IsPointer );
            Assert.IsFalse( voidType.IsPointerPointer );
            Assert.IsFalse( voidType.IsSequence );
            Assert.IsFalse( voidType.IsStruct );
            Assert.IsFalse( voidType.IsSized );
            Assert.IsTrue( voidType.IsVoid );
            Assert.AreEqual( 0u, voidType.IntegerBitWidth );
        }

        [TestMethod]
        [TestCategory( "Primitive Types" )]
        public void IntegerTypePropertiesTest( )
        {
            using var context = new Context( );
            VerifyIntegerType( context.BoolType, 1 );
            VerifyIntegerType( context.Int8Type, 8 );
            VerifyIntegerType( context.Int16Type, 16 );
            VerifyIntegerType( context.Int32Type, 32 );
            VerifyIntegerType( context.Int64Type, 64 );
        }

        [TestMethod]
        [TestCategory( "Primitive Types" )]
        public void GetIntTypeTest( )
        {
            using var context = new Context( );
            var int8Type = context.GetIntType( 8 );
            Assert.AreSame( context.Int8Type, int8Type );
            Assert.AreSame( context, int8Type.Context );
            Assert.AreEqual( 8U, int8Type.IntegerBitWidth );

            var int16Type = context.GetIntType( 16 );
            Assert.AreSame( context.Int16Type, int16Type );
            Assert.AreSame( context, int16Type.Context );
            Assert.AreEqual( 16U, int16Type.IntegerBitWidth );

            var int32Type = context.GetIntType( 32 );
            Assert.AreSame( context.Int32Type, int32Type );
            Assert.AreSame( context, int32Type.Context );
            Assert.AreEqual( 32U, int32Type.IntegerBitWidth );

            var int64Type = context.GetIntType( 64 );
            Assert.AreSame( context.Int64Type, int64Type );
            Assert.AreSame( context, int64Type.Context );
            Assert.AreEqual( 64U, int64Type.IntegerBitWidth );

            var int128Type = context.GetIntType( 128 );
            Assert.AreSame( context, int128Type.Context );
            Assert.AreEqual( 128U, int128Type.IntegerBitWidth );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void GetFunctionTypeTest( )
        {
            using var context = new Context( );

            // i16 ( i32, float )
            var funcSig = context.GetFunctionType( context.Int16Type, context.Int32Type, context.FloatType );
            Assert.IsNotNull( funcSig );
            Assert.AreSame( context, funcSig.Context );

            Assert.AreEqual( TypeKind.Function, funcSig.Kind );
            Assert.AreSame( context.Int16Type, funcSig.ReturnType );
            Assert.AreEqual( 2, funcSig.ParameterTypes.Count );

            // verify additional properties created properly
            Assert.AreEqual( 0U, funcSig.IntegerBitWidth );
            Assert.IsFalse( funcSig.IsDouble );
            Assert.IsFalse( funcSig.IsFloat );
            Assert.IsFalse( funcSig.IsFloatingPoint );
            Assert.IsFalse( funcSig.IsInteger );
            Assert.IsFalse( funcSig.IsPointer );
            Assert.IsFalse( funcSig.IsPointerPointer );
            Assert.IsFalse( funcSig.IsSequence );
            Assert.IsFalse( funcSig.IsSized );
            Assert.IsFalse( funcSig.IsStruct );
            Assert.IsFalse( funcSig.IsVarArg );
            Assert.IsFalse( funcSig.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void CreateFunctionTypeTest( )
        {
            var targetMachine = TargetTests.GetTargetMachine( );
            using var context = new Context( );
            var module = context.CreateBitcodeModule( "test.bc", SourceLanguage.C99, "test.c", "unit-tests" );
            Assert.IsNotNull( module );
            module.Layout = targetMachine.TargetData;

            var i16 = new DebugBasicType( context.Int16Type, module, "int16", DiTypeKind.Signed );
            var i32 = new DebugBasicType( context.Int32Type, module, "int32", DiTypeKind.Signed );
            var f32 = new DebugBasicType( context.FloatType, module, "float", DiTypeKind.Float );

            // i16 ( i32, float )
            var funcSig = context.CreateFunctionType( module.DIBuilder, i16, i32, f32 );

            Assert.IsNotNull( funcSig );
            Assert.AreSame( context, funcSig.Context );

            Assert.AreEqual( TypeKind.Function, funcSig.Kind );
            Assert.AreSame( context.Int16Type, funcSig.ReturnType );
            Assert.AreEqual( 2, funcSig.ParameterTypes.Count );

            // verify additional properties created properly
            Assert.AreEqual( 0U, funcSig.IntegerBitWidth );
            Assert.IsFalse( funcSig.IsDouble );
            Assert.IsFalse( funcSig.IsFloat );
            Assert.IsFalse( funcSig.IsFloatingPoint );
            Assert.IsFalse( funcSig.IsInteger );
            Assert.IsFalse( funcSig.IsPointer );
            Assert.IsFalse( funcSig.IsPointerPointer );
            Assert.IsFalse( funcSig.IsSequence );
            Assert.IsFalse( funcSig.IsSized );
            Assert.IsFalse( funcSig.IsStruct );
            Assert.IsFalse( funcSig.IsVarArg );
            Assert.IsFalse( funcSig.IsVoid );

            Assert.IsNotNull( funcSig.DIType );
            DISubroutineType subroutineType = funcSig.DIType!;
            Assert.IsNotNull( subroutineType );
            Assert.AreSame( context, subroutineType.Context );
            Assert.AreEqual( DebugInfoFlags.None, subroutineType.DebugInfoFlags );

            // signatures, have no scope or file
            Assert.IsNull( subroutineType.Scope );
            Assert.IsNull( subroutineType.File );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void VerifyCreateFunctionTypeWithSameSigIsSameInstanceTest( )
        {
            var targetMachine = TargetTests.GetTargetMachine( );
            using var context = new Context( );
            var module = context.CreateBitcodeModule( "test.bc", SourceLanguage.C99, "test.cs", "unit-tests" );
            Assert.IsNotNull( module );
            module.Layout = targetMachine.TargetData;

            var i16 = new DebugBasicType( context.Int16Type, module, "int16", DiTypeKind.Signed );
            var i32 = new DebugBasicType( context.Int32Type, module, "int32", DiTypeKind.Signed );
            var f32 = new DebugBasicType( context.FloatType, module, "float", DiTypeKind.Float );

            // i16 ( i32, float )
            var funcSig = context.CreateFunctionType( module.DIBuilder, i16, i32, f32 );
            var funcSig2 = context.CreateFunctionType( module.DIBuilder, i16, i32, f32 );
            Assert.AreSame( funcSig.NativeType, funcSig2.NativeType );
            Assert.AreSame( funcSig.DIType, funcSig2.DIType );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void VerifySameFunctionSigRetrievesTheSameType( )
        {
            using var context = new Context( );

            // i16 ( i32, float )
            var funcSig = context.GetFunctionType( context.Int16Type, context.Int32Type, context.FloatType );
            var funcSig2 = context.GetFunctionType( context.Int16Type, context.Int32Type, context.FloatType );
            Assert.IsNotNull( funcSig );
            Assert.IsNotNull( funcSig2 );
            Assert.AreSame( funcSig, funcSig2 );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void GetFunctionTypeTest1( )
        {
            using var context = new Context( );
            Assert.IsNotNull( context );
            Assert.IsNotNull( context.Int32Type );
            Assert.IsNotNull( context.FloatType );
            Assert.IsNotNull( context.Int16Type );

            // i16 ( i32, float )
            var argTypes = new[ ] { context.Int32Type, context.FloatType };
            var funcSig = context.GetFunctionType( context.Int16Type, argTypes );
            Assert.IsNotNull( funcSig );
            Assert.AreSame( context, funcSig.Context );

            Assert.AreEqual( TypeKind.Function, funcSig.Kind );
            Assert.AreSame( context.Int16Type, funcSig.ReturnType );
            Assert.AreEqual( 2, funcSig.ParameterTypes.Count );

            // verify additional properties created properly
            Assert.AreEqual( 0U, funcSig.IntegerBitWidth );
            Assert.IsFalse( funcSig.IsDouble );
            Assert.IsFalse( funcSig.IsFloat );
            Assert.IsFalse( funcSig.IsFloatingPoint );
            Assert.IsFalse( funcSig.IsInteger );
            Assert.IsFalse( funcSig.IsPointer );
            Assert.IsFalse( funcSig.IsPointerPointer );
            Assert.IsFalse( funcSig.IsSequence );
            Assert.IsFalse( funcSig.IsSized );
            Assert.IsFalse( funcSig.IsStruct );
            Assert.IsFalse( funcSig.IsVarArg );
            Assert.IsFalse( funcSig.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void GetFunctionTypeTest2( )
        {
            using var context = new Context( );

            // i16 ( i32, float )
            var argTypes = new List<ITypeRef> { context.Int32Type, context.FloatType };
            var funcSig = context.GetFunctionType( context.Int16Type, argTypes, true );
            Assert.IsNotNull( funcSig );
            Assert.AreSame( context, funcSig.Context );

            Assert.AreEqual( TypeKind.Function, funcSig.Kind );
            Assert.AreSame( context.Int16Type, funcSig.ReturnType );
            Assert.AreEqual( 2, funcSig.ParameterTypes.Count );
            Assert.IsTrue( funcSig.IsVarArg );

            // verify additional properties created properly
            Assert.AreEqual( 0U, funcSig.IntegerBitWidth );
            Assert.IsFalse( funcSig.IsDouble );
            Assert.IsFalse( funcSig.IsFloat );
            Assert.IsFalse( funcSig.IsFloatingPoint );
            Assert.IsFalse( funcSig.IsInteger );
            Assert.IsFalse( funcSig.IsPointer );
            Assert.IsFalse( funcSig.IsPointerPointer );
            Assert.IsFalse( funcSig.IsSequence );
            Assert.IsFalse( funcSig.IsSized );
            Assert.IsFalse( funcSig.IsStruct );
            Assert.IsFalse( funcSig.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateStructTypeTest( )
        {
            using var context = new Context( );
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 0, type.Members.Count );

            // with no elements the type should not be considered sized
            Assert.IsFalse( type.IsSized );
            Assert.IsTrue( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousStructTypeTestWithOneMemberUnpacked( )
        {
            using var context = new Context( );
            var type = context.CreateStructType( false, context.Int16Type );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.IsNull( type.Name );
            Assert.AreEqual( 1, type.Members.Count );
            Assert.AreSame( context.Int16Type, type.Members[ 0 ] );
            Assert.IsFalse( type.IsPacked );

            // with at least one element the type should be considered sized
            Assert.IsTrue( type.IsSized );
            Assert.IsFalse( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousStructTypeTestWithOneMemberPacked( )
        {
            using var context = new Context( );
            var type = context.CreateStructType( true, context.Int16Type );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.IsNull( type.Name );
            Assert.AreEqual( 1, type.Members.Count );
            Assert.AreSame( context.Int16Type, type.Members[ 0 ] );
            Assert.IsTrue( type.IsPacked );

            // with at least one element the type should be considered sized
            Assert.IsTrue( type.IsSized );
            Assert.IsFalse( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousStructTypeTestWithMultipleMembersUnpacked( )
        {
            using var context = new Context( );
            var type = context.CreateStructType( false, context.Int16Type, context.Int32Type );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.IsNull( type.Name );
            Assert.AreEqual( 2, type.Members.Count );
            Assert.AreSame( context.Int16Type, type.Members[ 0 ] );
            Assert.AreSame( context.Int32Type, type.Members[ 1 ] );
            Assert.IsFalse( type.IsPacked );

            // with at least one element the type should be considered sized
            Assert.IsTrue( type.IsSized );
            Assert.IsFalse( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousStructTypeTestWithMultipleMembersPacked( )
        {
            using var context = new Context( );
            var type = context.CreateStructType( true, context.Int16Type, context.Int32Type );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.IsNull( type.Name );
            Assert.AreEqual( 2, type.Members.Count );
            Assert.AreSame( context.Int16Type, type.Members[ 0 ] );
            Assert.AreSame( context.Int32Type, type.Members[ 1 ] );
            Assert.IsTrue( type.IsPacked );

            // with at least one element the type should be considered sized
            Assert.IsTrue( type.IsSized );
            Assert.IsFalse( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateNamedStructTypeTestWithOneMemberUnpacked( )
        {
            using var context = new Context( );
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName, false, context.Int16Type );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 1, type.Members.Count );
            Assert.AreSame( context.Int16Type, type.Members[ 0 ] );
            Assert.IsFalse( type.IsPacked );

            // with at least one element the type should be considered sized
            Assert.IsTrue( type.IsSized );
            Assert.IsFalse( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateNamedStructTypeTestWithOneMemberPacked( )
        {
            using var context = new Context( );
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName, true, context.Int16Type );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 1, type.Members.Count );
            Assert.AreSame( context.Int16Type, type.Members[ 0 ] );
            Assert.IsTrue( type.IsPacked );

            // with at least one element the type should be considered sized
            Assert.IsTrue( type.IsSized );
            Assert.IsFalse( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateNamedStructTypeTestWithMultipleMembersUnpacked( )
        {
            using var context = new Context( );
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName, false, context.Int16Type, context.Int32Type );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 2, type.Members.Count );
            Assert.AreSame( context.Int16Type, type.Members[ 0 ] );
            Assert.AreSame( context.Int32Type, type.Members[ 1 ] );
            Assert.IsFalse( type.IsPacked );

            // with at least one element the type should be considered sized
            Assert.IsTrue( type.IsSized );
            Assert.IsFalse( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateNamedStructTypeTestWithMultipleMembersPacked( )
        {
            using var context = new Context( );
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName, true, context.Int16Type, context.Int32Type );
            Assert.IsNotNull( type );
            Assert.AreSame( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 2, type.Members.Count );
            Assert.AreSame( context.Int16Type, type.Members[ 0 ] );
            Assert.AreSame( context.Int32Type, type.Members[ 1 ] );
            Assert.IsTrue( type.IsPacked );

            // with at least one element the type should be considered sized
            Assert.IsTrue( type.IsSized );
            Assert.IsFalse( type.IsOpaque );

            // verify additional properties created properly
            Assert.AreEqual( 0U, type.IntegerBitWidth );
            Assert.IsFalse( type.IsDouble );
            Assert.IsFalse( type.IsFloat );
            Assert.IsFalse( type.IsFloatingPoint );
            Assert.IsFalse( type.IsInteger );
            Assert.IsFalse( type.IsPointer );
            Assert.IsFalse( type.IsPointerPointer );
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousPackedConstantStructUsingParamsTest( )
        {
            using var context = new Context( );
            var value = context.CreateConstantStruct( true
                                                    , context.CreateConstant( ( byte )1)
                                                    , context.CreateConstant( 2.0f )
                                                    , context.CreateConstant( ( short )-3 )
                                                    );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousConstantNestedStructTest( )
        {
            using var context = new Context( );
            var nestedValue = context.CreateConstantStruct( false
                                                          , context.CreateConstant( 5 )
                                                          , context.CreateConstantString("Hello")
                                                          , context.CreateConstant( 6 )
                                                          );
            var value = context.CreateConstantStruct( true
                                                    , context.CreateConstant( ( byte )1)
                                                    , context.CreateConstant( 2.0f )
                                                    , nestedValue
                                                    , context.CreateConstant( ( short )-3 )
                                                    );
            Assert.AreEqual( 4, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantStruct ) );
            Assert.IsInstanceOfType( value.Operands[ 3 ], typeof( ConstantInt ) );
            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 3 ]! ).SignExtendedValue );

            // verify the nested struct is generated correctly
            var nestedConst = ( Constant )value.Operands[ 2 ]!;
            Assert.IsInstanceOfType( nestedConst.Operands[ 0 ]!, typeof( ConstantInt ) );
            Assert.IsInstanceOfType( nestedConst.Operands[ 1 ]!, typeof( ConstantDataArray ) );
            Assert.IsInstanceOfType( nestedConst.Operands[ 2 ]!, typeof( ConstantInt ) );

            Assert.AreEqual( 5, ( ( ConstantInt )nestedConst.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( "Hello", ( ( ConstantDataSequential )nestedConst.Operands[ 1 ]! ).ExtractAsString( ) );
            Assert.AreEqual( 6, ( ( ConstantInt )nestedConst.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestCategory( "Anonymous Structs" )]
        [TestMethod]
        public void CreateAnonymousUnpackedConstantStructUsingParamsTest( )
        {
            using var context = new Context( );
            var value = context.CreateConstantStruct( false
                                                    , context.CreateConstant( ( byte )1)
                                                    , context.CreateConstant( 2.0f )
                                                    , context.CreateConstant( ( short )-3 )
                                                    );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousPackedConstantStructUsingEnumerableTest( )
        {
            using var context = new Context( );
            var fields = new List<Constant>
                    { context.CreateConstant( ( byte )1 )
                    , context.CreateConstant( 2.0f )
                    , context.CreateConstant( ( short )-3 )
                    };

            var value = context.CreateConstantStruct( true, fields );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousUnpackedConstantStructUsingEnumerableTest( )
        {
            using var context = new Context( );
            var fields = new List<Constant> { context.CreateConstant( ( byte )1 )
                                            , context.CreateConstant( 2.0f )
                                            , context.CreateConstant( ( short )-3 )
                                            };
            var value = context.CreateConstantStruct( false, fields );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateNamedConstantPackedStructTestUsingEnumerable( )
        {
            using var context = new Context( );
            var structType = context.CreateStructType( "struct.test", true, context.Int8Type, context.FloatType, context.Int16Type );
            var fields = new List<Constant> { context.CreateConstant( ( byte )1 )
                                            , context.CreateConstant( 2.0f )
                                            , context.CreateConstant( ( short )-3 )
                                            };
            var value = context.CreateNamedConstantStruct( structType, fields );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateNamedConstantUnpackedStructTestUsingEnumerable( )
        {
            using var context = new Context( );
            var structType = context.CreateStructType( "struct.test"
                                                     , false
                                                     , context.Int8Type
                                                     , context.FloatType
                                                     , context.Int16Type
                                                     );
            var fields = new List<Constant> { context.CreateConstant( ( byte )1 )
                                            , context.CreateConstant( 2.0f )
                                            , context.CreateConstant( ( short )-3 )
                                            };
            var value = context.CreateNamedConstantStruct( structType, fields );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateNamedConstantPackedStructTestUsingParams( )
        {
            using var context = new Context( );
            var structType = context.CreateStructType( "struct.test"
                                                     , true
                                                     , context.Int8Type
                                                     , context.FloatType
                                                     , context.Int16Type
                                                     );
            var value = context.CreateNamedConstantStruct( structType
                                                         , context.CreateConstant( ( byte )1 )
                                                         , context.CreateConstant( 2.0f )
                                                         , context.CreateConstant( ( short )-3 )
                                                         );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateNamedConstantUnpackedStructTestUsingParams( )
        {
            using var context = new Context( );
            var structType = context.CreateStructType( "struct.test"
                                                     , false
                                                     , context.Int8Type
                                                     , context.FloatType
                                                     , context.Int16Type
                                                     );
            var value = context.CreateNamedConstantStruct( structType
                                                         , context.CreateConstant( ( byte )1 )
                                                         , context.CreateConstant( 2.0f )
                                                         , context.CreateConstant( ( short )-3 )
                                                         );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
            Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
            Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

            Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ]! ).SignExtendedValue );
            Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ]! ).Value );
            Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ]! ).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsPointerPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Metadata String" )]
        public void CreateMetadataStringTest( )
        {
            using var context = new Context( );
            const string content = "Test MDString";
            var mdstring = context.CreateMetadataString( content );
            Assert.IsNotNull( mdstring );
            Assert.AreEqual( content, mdstring.ToString( ) );
        }

        [TestMethod]
        [TestCategory( "Metadata String" )]
        public void CreateMetadataStringWithEmptyArgTest( )
        {
            using var context = new Context( );
            var mdstring = context.CreateMetadataString( string.Empty );
            Assert.IsNotNull( mdstring );
            Assert.AreEqual( string.Empty, mdstring.ToString( ) );
        }

        [TestMethod]
        [TestCategory( "Metadata String" )]
        public void CreateMetadataStringWithNullArgTest( )
        {
            using var context = new Context( );
            var mdstring = context.CreateMetadataString( null );
            Assert.IsNotNull( mdstring );
            Assert.AreEqual( string.Empty, mdstring.ToString( ) );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateConstantCStringTest( )
        {
            using var context = new Context( );
            string str = "hello world";
            ConstantDataArray value = context.CreateConstantString( str );
            Assert.IsNotNull( value );
            Assert.IsTrue( value.IsString ); // Has terminating null, so it is a C String
            Assert.IsTrue( value.IsI8Sequence );

            Assert.IsFalse( value.IsNull );
            Assert.IsFalse( value.IsUndefined );
            Assert.IsFalse( value.IsInstruction );
            Assert.IsFalse( value.IsFunction );
            Assert.IsFalse( value.IsCallSite );
            Assert.IsFalse( value.IsZeroValue );

            Assert.AreSame( string.Empty, value.Name );
            Assert.IsNotNull( value.NativeType );

            var arrayType = value.NativeType as IArrayType;
            Assert.IsNotNull( arrayType );
            Assert.AreSame( context, arrayType!.Context );
            Assert.AreSame( context.Int8Type, arrayType.ElementType );
            Assert.AreEqual( ( uint )str.Length + 1 , arrayType.Length ); // +1 for terminating \0
            string valueStr = value.ExtractAsString( );
            Assert.IsFalse( string.IsNullOrWhiteSpace( valueStr ) );
            Assert.AreEqual( str, valueStr );

            var span = value.RawData;
            byte[ ] strBytes = System.Text.Encoding.ASCII.GetBytes(str);

            Assert.AreEqual( strBytes.Length + 1, span.Length ); // +1 for terminating \0
            for( int i = 0; i < strBytes.Length; ++i )
            {
                Assert.AreEqual( strBytes[ i ], span[ i ], $"At index {i}" );
            }

            Assert.AreEqual( 0, span[ ^1 ] );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateConstantStringTest( )
        {
            using var context = new Context( );
            string str = "hello world";
            ConstantDataArray value = context.CreateConstantString( str, false );
            Assert.IsNotNull( value );
            Assert.IsFalse( value.IsString ); // No terminating null, so it is not a C String
            Assert.IsTrue( value.IsI8Sequence );

            Assert.IsFalse( value.IsNull );
            Assert.IsFalse( value.IsUndefined );
            Assert.IsFalse( value.IsInstruction );
            Assert.IsFalse( value.IsFunction );
            Assert.IsFalse( value.IsCallSite );
            Assert.IsFalse( value.IsZeroValue );

            Assert.AreSame( string.Empty, value.Name );
            Assert.IsNotNull( value.NativeType );

            var arrayType = value.NativeType as IArrayType;
            Assert.IsNotNull( arrayType );
            Assert.AreSame( context, arrayType!.Context );
            Assert.AreSame( context.Int8Type, arrayType.ElementType );
            Assert.AreEqual( ( uint )str.Length, arrayType.Length );
            string valueStr = value.ExtractAsString( );
            Assert.IsFalse( string.IsNullOrWhiteSpace( valueStr ) );
            Assert.AreEqual( str, valueStr );

            var span = value.RawData;
            byte[ ] strBytes = System.Text.Encoding.ASCII.GetBytes(str);

            Assert.AreEqual( strBytes.Length, span.Length );
            for( int i = 0; i < strBytes.Length; ++i )
            {
                Assert.AreEqual( strBytes[ i ], span[ i ], $"At index {i}" );
            }
        }

        private static void VerifyIntegerType( ITypeRef integerType, uint bitWidth )
        {
            Assert.AreEqual( TypeKind.Integer, integerType.Kind );
            Assert.IsFalse( integerType.IsDouble );
            Assert.IsFalse( integerType.IsFloat );
            Assert.IsFalse( integerType.IsFloatingPoint );
            Assert.IsTrue( integerType.IsInteger );
            Assert.IsFalse( integerType.IsPointer );
            Assert.IsFalse( integerType.IsPointerPointer );
            Assert.IsFalse( integerType.IsSequence );
            Assert.IsFalse( integerType.IsStruct );
            Assert.IsTrue( integerType.IsSized );
            Assert.IsFalse( integerType.IsVoid );
            Assert.AreEqual( bitWidth, integerType.IntegerBitWidth );
        }
    }
}
