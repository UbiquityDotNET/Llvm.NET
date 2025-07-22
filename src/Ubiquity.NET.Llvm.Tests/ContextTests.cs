// -----------------------------------------------------------------------
// <copyright file="ContextTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

// warning SA1500: Braces for multi-line statements must not share line
#pragma warning disable SA1500

namespace Ubiquity.NET.Llvm.UT
{
    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        [TestCategory( "Constructor" )]
        public void SimpleConstructorDisposeTest( )
        {
            var context = new Context();
            using(context)
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
            using var context = new Context();
            var int8PtrType = context.Int8Type.CreatePointerType();
            Assert.IsNotNull( int8PtrType );
            Assert.AreEqual( context, int8PtrType.Context );

            var int8PtrTypeAlt = context.Int8Type.CreatePointerType( );
            Assert.IsTrue( int8PtrType.Equals( int8PtrTypeAlt ) );
        }

        [TestMethod]
        [TestCategory( "Primitive Types" )]
        public void VoidTypePropertyTest( )
        {
            using var context = new Context();
            var voidType = context.VoidType;
            Assert.AreEqual( TypeKind.Void, voidType.Kind );
            Assert.IsFalse( voidType.IsDouble );
            Assert.IsFalse( voidType.IsFloat );
            Assert.IsFalse( voidType.IsFloatingPoint );
            Assert.IsFalse( voidType.IsInteger );
            Assert.IsFalse( voidType.IsPointer );
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
            using var context = new Context();
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
            using var context = new Context();
            var int8Type = context.GetIntType( 8 );
            Assert.AreEqual( context.Int8Type, int8Type );
            Assert.AreEqual( context, int8Type.Context );
            Assert.AreEqual( 8U, int8Type.IntegerBitWidth );

            var int16Type = context.GetIntType( 16 );
            Assert.AreEqual( context.Int16Type, int16Type );
            Assert.AreEqual( context, int16Type.Context );
            Assert.AreEqual( 16U, int16Type.IntegerBitWidth );

            var int32Type = context.GetIntType( 32 );
            Assert.AreEqual( context.Int32Type, int32Type );
            Assert.AreEqual( context, int32Type.Context );
            Assert.AreEqual( 32U, int32Type.IntegerBitWidth );

            var int64Type = context.GetIntType( 64 );
            Assert.AreEqual( context.Int64Type, int64Type );
            Assert.AreEqual( context, int64Type.Context );
            Assert.AreEqual( 64U, int64Type.IntegerBitWidth );

            var int128Type = context.GetIntType( 128 );
            Assert.AreEqual( context, int128Type.Context );
            Assert.AreEqual( 128U, int128Type.IntegerBitWidth );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void GetFunctionTypeTest( )
        {
            using var context = new Context();

            // i16 ( i32, float )
            var funcSig = context.GetFunctionType( context.FloatType , context.Int16Type, context.Int32Type);
            Assert.IsNotNull( funcSig );
            Assert.AreEqual( context, funcSig.Context );

            Assert.AreEqual( TypeKind.Function, funcSig.Kind );
            Assert.AreEqual( context.FloatType, funcSig.ReturnType );
            Assert.AreEqual( 2, funcSig.ParameterTypes.Count );

            // verify additional properties created properly
            Assert.AreEqual( 0U, funcSig.IntegerBitWidth );
            Assert.IsFalse( funcSig.IsDouble );
            Assert.IsFalse( funcSig.IsFloat );
            Assert.IsFalse( funcSig.IsFloatingPoint );
            Assert.IsFalse( funcSig.IsInteger );
            Assert.IsFalse( funcSig.IsPointer );
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
            using var targetMachine = TargetTests.GetTargetMachine( );
            using var context = new Context();
            using var module = context.CreateBitcodeModule( "test.bc" );
            using var diBuilder = new DIBuilder(module);
            DICompileUnit cu = diBuilder.CreateCompileUnit( SourceLanguage.C99, "test.c", "unit-tests" );

            Assert.IsNotNull( module );
            using var layout = targetMachine.CreateTargetData();
            module.Layout = layout;

            var i16 = new DebugBasicType( context.Int16Type, diBuilder, "int16", DiTypeKind.Signed );
            var i32 = new DebugBasicType( context.Int32Type, diBuilder, "int32", DiTypeKind.Signed );
            var f32 = new DebugBasicType( context.FloatType, diBuilder, "float", DiTypeKind.Float );

            // i16 ( i32, float )
            var funcSig = context.CreateFunctionType( diBuilder, i16, i32, f32 );

            Assert.IsNotNull( funcSig );
            Assert.AreEqual( context, funcSig.Context );

            Assert.AreEqual( TypeKind.Function, funcSig.Kind );
            Assert.AreEqual( context.Int16Type, funcSig.ReturnType );
            Assert.AreEqual( 2, funcSig.ParameterTypes.Count );

            // verify additional properties created properly
            Assert.AreEqual( 0U, funcSig.IntegerBitWidth );
            Assert.IsFalse( funcSig.IsDouble );
            Assert.IsFalse( funcSig.IsFloat );
            Assert.IsFalse( funcSig.IsFloatingPoint );
            Assert.IsFalse( funcSig.IsInteger );
            Assert.IsFalse( funcSig.IsPointer );
            Assert.IsFalse( funcSig.IsSequence );
            Assert.IsFalse( funcSig.IsSized );
            Assert.IsFalse( funcSig.IsStruct );
            Assert.IsFalse( funcSig.IsVarArg );
            Assert.IsFalse( funcSig.IsVoid );

            Assert.IsNotNull( funcSig.DebugInfoType );
            DISubroutineType subroutineType = funcSig.DebugInfoType!;
            Assert.IsNotNull( subroutineType );
            Assert.AreEqual( context, subroutineType.Context );
            Assert.AreEqual( DebugInfoFlags.None, subroutineType.DebugInfoFlags );

            // signatures, have no scope or file
            Assert.IsNull( subroutineType.Scope );
            Assert.IsNull( subroutineType.File );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void VerifyCreateFunctionTypeWithSameSigIsSameInstanceTest( )
        {
            using var targetMachine = TargetTests.GetTargetMachine( );
            using var context = new Context();
            using var module = context.CreateBitcodeModule( "test.bc" );
            using var diBuilder = new DIBuilder(module);
            DICompileUnit cu = diBuilder.CreateCompileUnit( SourceLanguage.C99, "test.c", "unit-tests" );

            Assert.IsNotNull( module );
            using var layout = targetMachine.CreateTargetData();
            module.Layout = layout;

            var i16 = new DebugBasicType( context.Int16Type, diBuilder, "int16", DiTypeKind.Signed );
            var i32 = new DebugBasicType( context.Int32Type, diBuilder, "int32", DiTypeKind.Signed );
            var f32 = new DebugBasicType( context.FloatType, diBuilder, "float", DiTypeKind.Float );

            // i16 ( i32, float )
            var funcSig = context.CreateFunctionType( diBuilder, i16, i32, f32 );
            var funcSig2 = context.CreateFunctionType( diBuilder, i16, i32, f32 );
            Assert.IsTrue( funcSig.NativeType.Equals( funcSig2.NativeType ) );
            Assert.IsNotNull( funcSig.DebugInfoType );
            Assert.IsTrue( funcSig.HasDebugInfo() );

            Assert.IsTrue( funcSig.DebugInfoType.Equals( funcSig2.DebugInfoType ) );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void VerifySameFunctionSigRetrievesTheSameType( )
        {
            using var context = new Context();

            // i16 ( i32, float )
            var funcSig = context.GetFunctionType( context.FloatType , context.Int16Type, context.Int32Type);
            var funcSig2 = context.GetFunctionType( context.FloatType , context.Int16Type, context.Int32Type);
            Assert.IsNotNull( funcSig );
            Assert.IsNotNull( funcSig2 );
            Assert.IsTrue( funcSig.Equals( funcSig2 ) );
        }

        [TestMethod]
        [TestCategory( "Function Type" )]
        public void GetFunctionTypeTest1( )
        {
            using var context = new Context();
            Assert.IsNotNull( context );
            Assert.IsNotNull( context.Int32Type );
            Assert.IsNotNull( context.FloatType );
            Assert.IsNotNull( context.Int16Type );

            // i16 ( i32, float )
            var argTypes = new[ ] { context.Int32Type, context.FloatType };
            var funcSig = context.GetFunctionType( returnType: context.Int16Type, args: argTypes );
            Assert.IsNotNull( funcSig );
            Assert.AreEqual( context, funcSig.Context );

            Assert.AreEqual( TypeKind.Function, funcSig.Kind );
            Assert.AreEqual( context.Int16Type, funcSig.ReturnType );
            Assert.AreEqual( 2, funcSig.ParameterTypes.Count );

            // verify additional properties created properly
            Assert.AreEqual( 0U, funcSig.IntegerBitWidth );
            Assert.IsFalse( funcSig.IsDouble );
            Assert.IsFalse( funcSig.IsFloat );
            Assert.IsFalse( funcSig.IsFloatingPoint );
            Assert.IsFalse( funcSig.IsInteger );
            Assert.IsFalse( funcSig.IsPointer );
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
            using var context = new Context();

            // i16 ( i32, float )
            var argTypes = new List<ITypeRef> { context.Int32Type, context.FloatType };
            var funcSig = context.GetFunctionType( true , context.Int16Type, argTypes);
            Assert.IsNotNull( funcSig );
            Assert.AreEqual( context, funcSig.Context );

            Assert.AreEqual( TypeKind.Function, funcSig.Kind );
            Assert.AreEqual( context.Int16Type, funcSig.ReturnType );
            Assert.AreEqual( 2, funcSig.ParameterTypes.Count );
            Assert.IsTrue( funcSig.IsVarArg );

            // verify additional properties created properly
            Assert.AreEqual( 0U, funcSig.IntegerBitWidth );
            Assert.IsFalse( funcSig.IsDouble );
            Assert.IsFalse( funcSig.IsFloat );
            Assert.IsFalse( funcSig.IsFloatingPoint );
            Assert.IsFalse( funcSig.IsInteger );
            Assert.IsFalse( funcSig.IsPointer );
            Assert.IsFalse( funcSig.IsSequence );
            Assert.IsFalse( funcSig.IsSized );
            Assert.IsFalse( funcSig.IsStruct );
            Assert.IsFalse( funcSig.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateStructTypeTest( )
        {
            using var context = new Context();
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        [SuppressMessage( "Design", "MSTEST0032:Assertion condition is always true", Justification = "BS! Test VERIFIES claim" )]
        public void CreateAnonymousStructTypeTestWithOneMemberUnpacked( )
        {
            using var context = new Context();
            var type = context.CreateStructType( false, context.Int16Type );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.IsNotNull( type.Name );
            Assert.IsEmpty( type.Name );
            Assert.AreEqual( 1, type.Members.Count );
            Assert.AreEqual( context.Int16Type, type.Members[ 0 ] );
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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        [SuppressMessage( "Design", "MSTEST0032:Assertion condition is always true", Justification = "BS! Test VERIFIES claim" )]
        public void CreateAnonymousStructTypeTestWithOneMemberPacked( )
        {
            using var context = new Context();
            var type = context.CreateStructType( true, context.Int16Type );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.IsNotNull( type.Name );
            Assert.IsEmpty( type.Name );
            Assert.AreEqual( 1, type.Members.Count );
            Assert.AreEqual( context.Int16Type, type.Members[ 0 ] );
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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        [SuppressMessage( "Design", "MSTEST0032:Assertion condition is always true", Justification = "BS! Test VERIFIES claim" )]
        public void CreateAnonymousStructTypeTestWithMultipleMembersUnpacked( )
        {
            using var context = new Context();
            var type = context.CreateStructType( false, context.Int16Type, context.Int32Type );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.IsNotNull( type.Name );
            Assert.IsEmpty( type.Name );
            Assert.AreEqual( 2, type.Members.Count );
            Assert.AreEqual( context.Int16Type, type.Members[ 0 ] );
            Assert.AreEqual( context.Int32Type, type.Members[ 1 ] );
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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        [SuppressMessage( "Design", "MSTEST0032:Assertion condition is always true", Justification = "BS! Test VERIFIES claim" )]
        public void CreateAnonymousStructTypeTestWithMultipleMembersPacked( )
        {
            using var context = new Context();
            var type = context.CreateStructType( true, context.Int16Type, context.Int32Type );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.IsNotNull( type.Name );
            Assert.IsEmpty( type.Name );
            Assert.AreEqual( 2, type.Members.Count );
            Assert.AreEqual( context.Int16Type, type.Members[ 0 ] );
            Assert.AreEqual( context.Int32Type, type.Members[ 1 ] );
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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateNamedStructTypeTestWithOneMemberUnpacked( )
        {
            using var context = new Context();
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName, false, context.Int16Type );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 1, type.Members.Count );
            Assert.AreEqual( context.Int16Type, type.Members[ 0 ] );
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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateNamedStructTypeTestWithOneMemberPacked( )
        {
            using var context = new Context();
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName, true, context.Int16Type );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 1, type.Members.Count );
            Assert.AreEqual( context.Int16Type, type.Members[ 0 ] );
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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateNamedStructTypeTestWithMultipleMembersUnpacked( )
        {
            using var context = new Context();
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName, false, context.Int16Type, context.Int32Type );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 2, type.Members.Count );
            Assert.AreEqual( context.Int16Type, type.Members[ 0 ] );
            Assert.AreEqual( context.Int32Type, type.Members[ 1 ] );
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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Named Structs" )]
        public void CreateNamedStructTypeTestWithMultipleMembersPacked( )
        {
            using var context = new Context();
            string typeName = "struct.test";
            var type = context.CreateStructType( typeName, true, context.Int16Type, context.Int32Type );
            Assert.IsNotNull( type );
            Assert.AreEqual( context, type.Context );

            // verify type specific attributes
            Assert.AreEqual( TypeKind.Struct, type.Kind );
            Assert.IsTrue( type.IsStruct );
            Assert.AreEqual( typeName, type.Name );
            Assert.AreEqual( 2, type.Members.Count );
            Assert.AreEqual( context.Int16Type, type.Members[ 0 ] );
            Assert.AreEqual( context.Int32Type, type.Members[ 1 ] );
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
            Assert.IsFalse( type.IsSequence );
            Assert.IsFalse( type.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousPackedConstantStructUsingParamsTest( )
        {
            using var context = new Context();
            var value = context.CreateConstantStruct( true
                                                    , context.CreateConstant( ( byte )1)
                                                    , context.CreateConstant( 2.0f )
                                                    , context.CreateConstant( ( short )-3 )
                                                    );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 2 ] );

            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousConstantNestedStructTest( )
        {
            using var context = new Context();
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
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantStruct>( value.Operands[ 2 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 3 ] );
            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 3 ]!).SignExtendedValue );

            // verify the nested struct is generated correctly
            var nestedConst = ( Constant )value.Operands[ 2 ]!;
            Assert.IsInstanceOfType( nestedConst.Operands[ 0 ]!, typeof( ConstantInt ) );
            Assert.IsInstanceOfType( nestedConst.Operands[ 1 ]!, typeof( ConstantDataArray ) );
            Assert.IsInstanceOfType( nestedConst.Operands[ 2 ]!, typeof( ConstantInt ) );

            Assert.AreEqual( 5, ((ConstantInt)nestedConst.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( "Hello", ((ConstantDataSequential)nestedConst.Operands[ 1 ]!).ExtractAsString() );
            Assert.AreEqual( 6, ((ConstantInt)nestedConst.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestCategory( "Anonymous Structs" )]
        [TestMethod]
        public void CreateAnonymousUnpackedConstantStructUsingParamsTest( )
        {
            using var context = new Context();
            var value = context.CreateConstantStruct( false
                                                    , context.CreateConstant( ( byte )1)
                                                    , context.CreateConstant( 2.0f )
                                                    , context.CreateConstant( ( short )-3 )
                                                    );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 2 ] );

            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousPackedConstantStructUsingEnumerableTest( )
        {
            using var context = new Context();
            var fields = new List<Constant>
                    { context.CreateConstant( ( byte )1 )
                    , context.CreateConstant( 2.0f )
                    , context.CreateConstant( ( short )-3 )
                    };

            var value = context.CreateConstantStruct( true, fields );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 2 ] );

            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Anonymous Structs" )]
        public void CreateAnonymousUnpackedConstantStructUsingEnumerableTest( )
        {
            using var context = new Context();
            var fields = new List<Constant> { context.CreateConstant( ( byte )1 )
                                            , context.CreateConstant( 2.0f )
                                            , context.CreateConstant( ( short )-3 )
                                            };
            var value = context.CreateConstantStruct( false, fields );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 2 ] );

            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateNamedConstantPackedStructTestUsingEnumerable( )
        {
            using var context = new Context();
            var structType = context.CreateStructType( "struct.test", true, context.Int8Type, context.FloatType, context.Int16Type );
            var fields = new List<Constant> { context.CreateConstant( ( byte )1 )
                                            , context.CreateConstant( 2.0f )
                                            , context.CreateConstant( ( short )-3 )
                                            };
            var value = context.CreateNamedConstantStruct( structType, fields );
            Assert.AreEqual( 3, value.Operands.Count );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 2 ] );

            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateNamedConstantUnpackedStructTestUsingEnumerable( )
        {
            using var context = new Context();
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
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 2 ] );

            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateNamedConstantPackedStructTestUsingParams( )
        {
            using var context = new Context();
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
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 2 ] );

            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateNamedConstantUnpackedStructTestUsingParams( )
        {
            using var context = new Context();
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
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 0 ] );
            Assert.IsInstanceOfType<ConstantFP>( value.Operands[ 1 ] );
            Assert.IsInstanceOfType<ConstantInt>( value.Operands[ 2 ] );

            Assert.AreEqual( 1L, ((ConstantInt)value.Operands[ 0 ]!).SignExtendedValue );
            Assert.AreEqual( 2.0, ((ConstantFP)value.Operands[ 1 ]!).Value );
            Assert.AreEqual( -3L, ((ConstantInt)value.Operands[ 2 ]!).SignExtendedValue );

            // verify additional properties created properly
            Assert.AreEqual( 0U, value.NativeType.IntegerBitWidth );
            Assert.IsFalse( value.NativeType.IsDouble );
            Assert.IsFalse( value.NativeType.IsFloat );
            Assert.IsFalse( value.NativeType.IsFloatingPoint );
            Assert.IsFalse( value.NativeType.IsInteger );
            Assert.IsFalse( value.NativeType.IsPointer );
            Assert.IsFalse( value.NativeType.IsSequence );
            Assert.IsFalse( value.NativeType.IsVoid );
        }

        [TestMethod]
        [TestCategory( "IrMetadata String" )]
        public void CreateMetadataStringTest( )
        {
            using var context = new Context();
            const string content = "Test MDString";
            var mdstring = context.CreateMetadataString( content );
            Assert.IsNotNull( mdstring );
            Assert.AreEqual( content, mdstring.ToString() );
        }

        [TestMethod]
        [TestCategory( "IrMetadata String" )]
        public void CreateMetadataStringWithEmptyArgTest( )
        {
            using var context = new Context();
            var mdstring = context.CreateMetadataString( string.Empty );
            Assert.IsNotNull( mdstring );
            Assert.AreEqual( string.Empty, mdstring.ToString() );
        }

        [TestMethod]
        [TestCategory( "IrMetadata String" )]
        public void CreateMetadataStringWithNullArgTest( )
        {
            using var context = new Context();
            var mdstring = context.CreateMetadataString( null );
            Assert.IsNotNull( mdstring );
            Assert.AreEqual( string.Empty, mdstring.ToString() );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateConstantCStringTest( )
        {
            using var context = new Context();
            string str = "hello world";
            ConstantDataArray value = context.CreateConstantString( str );
            Assert.IsTrue( value.IsString ); // Has terminating null, so it is a C String
            Assert.IsTrue( value.IsI8Sequence );

            Assert.IsFalse( value.IsNull );
            Assert.IsFalse( value.IsUndefined );
            Assert.IsFalse( value.IsInstruction );
            Assert.IsFalse( value.IsFunction );
            Assert.IsFalse( value.IsCallSite );
            Assert.IsFalse( value.IsZeroValue );

            Assert.AreSame( LazyEncodedString.Empty, value.Name );

            var arrayType = value.NativeType as IArrayType;
            Assert.IsNotNull( arrayType );
            Assert.AreEqual( context, arrayType.Context );
            Assert.AreEqual( context.Int8Type, arrayType.ElementType );
            Assert.AreEqual( (uint)str.Length + 1, arrayType.Length ); // +1 for terminating \0
            string valueStr = value.ExtractAsString( );
            Assert.IsFalse( string.IsNullOrWhiteSpace( valueStr ) );
            Assert.AreEqual( str, valueStr );

            var span = value.RawData;
            byte[ ] strBytes = System.Text.Encoding.ASCII.GetBytes(str);

            Assert.AreEqual( strBytes.Length + 1, span.Length ); // +1 for terminating \0
            for(int i = 0; i < strBytes.Length; ++i)
            {
                Assert.AreEqual( strBytes[ i ], span[ i ], $"At index {i}" );
            }

            Assert.AreEqual( 0, span[ ^1 ] );
        }

        [TestMethod]
        [TestCategory( "Constants" )]
        public void CreateConstantStringTest( )
        {
            using var context = new Context();
            string str = "hello world";
            ConstantDataArray value = context.CreateConstantString( str, false );
            Assert.IsFalse( value.IsString ); // No terminating null, so it is not a C String
            Assert.IsTrue( value.IsI8Sequence );

            Assert.IsFalse( value.IsNull );
            Assert.IsFalse( value.IsUndefined );
            Assert.IsFalse( value.IsInstruction );
            Assert.IsFalse( value.IsFunction );
            Assert.IsFalse( value.IsCallSite );
            Assert.IsFalse( value.IsZeroValue );

            Assert.AreSame( LazyEncodedString.Empty, value.Name );

            var arrayType = value.NativeType as IArrayType;
            Assert.IsNotNull( arrayType );
            Assert.AreEqual( context, arrayType.Context );
            Assert.AreEqual( context.Int8Type, arrayType.ElementType );
            Assert.AreEqual( (uint)str.Length, arrayType.Length );
            string valueStr = value.ExtractAsString( );
            Assert.IsFalse( LazyEncodedString.IsNullOrWhiteSpace( valueStr ) );
            Assert.AreEqual( str, valueStr );

            var span = value.RawData;
            byte[ ] strBytes = System.Text.Encoding.ASCII.GetBytes(str);

            Assert.AreEqual( strBytes.Length, span.Length );
            for(int i = 0; i < strBytes.Length; ++i)
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
            Assert.IsFalse( integerType.IsSequence );
            Assert.IsFalse( integerType.IsStruct );
            Assert.IsTrue( integerType.IsSized );
            Assert.IsFalse( integerType.IsVoid );
            Assert.AreEqual( bitWidth, integerType.IntegerBitWidth );
        }
    }
}
