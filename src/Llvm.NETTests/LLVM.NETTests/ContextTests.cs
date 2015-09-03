using System;
using System.Collections.Generic;
using System.Linq;
using Llvm.NET.Types;
using Llvm.NET.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void DisposeTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                Assert.IsNotNull( ctx );
            }
        }

        [TestMethod]
        public void GetPointerTypeForTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var int8PtrType = ctx.GetPointerTypeFor( ctx.Int8Type );
                Assert.IsNotNull( int8PtrType );
                Assert.AreSame( ctx.Int8Type, int8PtrType.ElementType );
                Assert.AreSame( ctx, int8PtrType.Context );

                var int8PtrTypeAlt = ctx.Int8Type.CreatePointerType( );
                Assert.AreSame( int8PtrType, int8PtrTypeAlt );
            }
        }

        [TestMethod]
        public void GetIntTypeTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var int8Type = ctx.GetIntType( 8 );
                Assert.AreSame( ctx.Int8Type, int8Type );
                Assert.AreSame( ctx, int8Type.Context );
                Assert.AreEqual( 8U, int8Type.IntegerBitWidth );

                var int16Type = ctx.GetIntType( 16 );
                Assert.AreSame( ctx.Int16Type, int16Type );
                Assert.AreSame( ctx, int16Type.Context );
                Assert.AreEqual( 16U, int16Type.IntegerBitWidth );

                var int32Type = ctx.GetIntType( 32 );
                Assert.AreSame( ctx.Int32Type, int32Type );
                Assert.AreSame( ctx, int32Type.Context );
                Assert.AreEqual( 32U, int32Type.IntegerBitWidth );

                var int64Type = ctx.GetIntType( 64 );
                Assert.AreSame( ctx.Int64Type, int64Type );
                Assert.AreSame( ctx, int64Type.Context );
                Assert.AreEqual( 64U, int64Type.IntegerBitWidth );

                var int128Type = ctx.GetIntType( 128 );
                Assert.AreSame( ctx, int128Type.Context );
                Assert.AreEqual( 128U, int128Type.IntegerBitWidth ); 
            }
        }

        [TestMethod]
        public void GetFunctionTypeTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                // i16 ( i32, float )
                var funcSig = ctx.GetFunctionType( ctx.Int16Type, ctx.Int32Type, ctx.FloatType );
                Assert.IsNotNull( funcSig );
                Assert.AreSame( ctx, funcSig.Context );

                Assert.AreEqual( TypeKind.Function, funcSig.Kind );
                Assert.AreSame( ctx.Int16Type, funcSig.ReturnType );
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
        }

        [TestMethod]
        public void GetFunctionTypeTest1( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                // i16 ( i32, float )
                var argTypes = new TypeRef[ ] { ctx.Int32Type, ctx.FloatType };
                var funcSig = ctx.GetFunctionType( ctx.Int16Type, argTypes );
                Assert.IsNotNull( funcSig );
                Assert.AreSame( ctx, funcSig.Context );

                Assert.AreEqual( TypeKind.Function, funcSig.Kind );
                Assert.AreSame( ctx.Int16Type, funcSig.ReturnType );
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
        }

        [TestMethod]
        public void GetFunctionTypeTest2( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                // i16 ( i32, float )
                var argTypes = new List<TypeRef> { ctx.Int32Type, ctx.FloatType };
                var funcSig = ctx.GetFunctionType( ctx.Int16Type, argTypes, true );
                Assert.IsNotNull( funcSig );
                Assert.AreSame( ctx, funcSig.Context );

                Assert.AreEqual( TypeKind.Function, funcSig.Kind );
                Assert.AreSame( ctx.Int16Type, funcSig.ReturnType );
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
        }

        [TestMethod]
        [TestCategory("Named Structs")]
        public void CreateStructTypeTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var typeName = "struct.test";
                var type = ctx.CreateStructType( typeName );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName , type.Name );
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
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousStructTypeTestWithOneMemberUnpacked( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var type = ctx.CreateStructType( false, ctx.Int16Type );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.IsNull( type.Name );
                Assert.AreEqual( 1, type.Members.Count );
                Assert.AreSame( ctx.Int16Type, type.Members[ 0 ] );
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
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousStructTypeTestWithOneMemberPacked( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var type = ctx.CreateStructType( true, ctx.Int16Type );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.IsNull( type.Name );
                Assert.AreEqual( 1, type.Members.Count );
                Assert.AreSame( ctx.Int16Type, type.Members[ 0 ] );
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
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousStructTypeTestWithMultipleMembersUnpacked( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var type = ctx.CreateStructType( false, ctx.Int16Type, ctx.Int32Type );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.IsNull( type.Name );
                Assert.AreEqual( 2, type.Members.Count );
                Assert.AreSame( ctx.Int16Type, type.Members[ 0 ] );
                Assert.AreSame( ctx.Int32Type, type.Members[ 1 ] );
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
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousStructTypeTestWithMultipleMembersPacked( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var type = ctx.CreateStructType( true, ctx.Int16Type, ctx.Int32Type );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.IsNull( type.Name );
                Assert.AreEqual( 2, type.Members.Count );
                Assert.AreSame( ctx.Int16Type, type.Members[ 0 ] );
                Assert.AreSame( ctx.Int32Type, type.Members[ 1 ] );
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
        }

        [TestMethod]
        [TestCategory("Named Structs")]
        public void CreateNamedStructTypeTestWithOneMemberUnpacked( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var typeName = "struct.test";
                var type = ctx.CreateStructType( typeName, false, ctx.Int16Type );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName, type.Name );
                Assert.AreEqual( 1, type.Members.Count );
                Assert.AreSame( ctx.Int16Type, type.Members[ 0 ] );
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
        }

        [TestMethod]
        [TestCategory("Named Structs")]
        public void CreateNamedStructTypeTestWithOneMemberPacked( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var typeName = "struct.test";
                var type = ctx.CreateStructType( typeName, true, ctx.Int16Type );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName, type.Name );
                Assert.AreEqual( 1, type.Members.Count );
                Assert.AreSame( ctx.Int16Type, type.Members[ 0 ] );
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
        }

        [TestMethod]
        [TestCategory("Named Structs")]
        public void CreateNamedStructTypeTestWithMultipleMembersUnpacked( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var typeName = "struct.test";
                var type = ctx.CreateStructType( typeName, false, ctx.Int16Type, ctx.Int32Type );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName, type.Name );
                Assert.AreEqual( 2, type.Members.Count );
                Assert.AreSame( ctx.Int16Type, type.Members[ 0 ] );
                Assert.AreSame( ctx.Int32Type, type.Members[ 1 ] );
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
        }

        [TestMethod]
        [TestCategory("Named Structs")]
        public void CreateNamedStructTypeTestWithMultipleMembersPacked( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var typeName = "struct.test";
                var type = ctx.CreateStructType( typeName, true, ctx.Int16Type, ctx.Int32Type );
                Assert.IsNotNull( type );
                Assert.AreSame( ctx, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName, type.Name );
                Assert.AreEqual( 2, type.Members.Count );
                Assert.AreSame( ctx.Int16Type, type.Members[ 0 ] );
                Assert.AreSame( ctx.Int32Type, type.Members[ 1 ] );
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
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousPackedConstantStructUsingParamsTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var value = ctx.CreateConstantStruct( true, ConstantInt.From( ( byte )1), ConstantFP.From( 2.0f ), ConstantInt.From( ( short )-3 ) );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousConstantNestedStructTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var nestedValue = ctx.CreateConstantStruct( false, ConstantInt.From( 5 ), ctx.CreateConstantString("Hello"), ConstantInt.From( 6 ) );
                var value = ctx.CreateConstantStruct( true, ConstantInt.From( ( byte )1), ConstantFP.From( 2.0f ), nestedValue, ConstantInt.From( ( short )-3 ) );
                Assert.AreEqual( 4, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantStruct ) );
                Assert.IsInstanceOfType( value.Operands[ 3 ], typeof( ConstantInt ) );
                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 3 ] ).SignExtendedValue );

                // verify the nested struct is genrated correctly
                var nestedConst = ( Constant )value.Operands[ 2 ];
                Assert.IsInstanceOfType( nestedConst.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( nestedConst.Operands[ 1 ], typeof( ConstantDataArray ) );
                Assert.IsInstanceOfType( nestedConst.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 5, ( ( ConstantInt )nestedConst.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( "Hello", ( ( ConstantDataSequential )nestedConst.Operands[ 1 ] ).GetAsString() );
                Assert.AreEqual( 6, ( ( ConstantInt )nestedConst.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestCategory("Anonymous Structs")]
        [TestMethod]
        public void CreateAnonymousUnpackedConstantStructUsingParamsTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var value = ctx.CreateConstantStruct( false, ConstantInt.From( ( byte )1), ConstantFP.From( 2.0f ), ConstantInt.From( ( short )-3 ) );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousPackedConstantStructUsingEnumerableTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var fields = new List<Constant> { ConstantInt.From( ( byte )1 ), ConstantFP.From( 2.0f ), ConstantInt.From( ( short )-3 ) };
                var value = ctx.CreateConstantStruct( true, fields );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousUnpackedConstantStructUsingEnumerableTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var fields = new List<Constant> { ConstantInt.From( ( byte )1 ), ConstantFP.From( 2.0f ), ConstantInt.From( ( short )-3 ) };
                var value = ctx.CreateConstantStruct( false, fields );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestMethod]
        public void CreateNamedConstantPackedStructTestUsingEnumerable( )
        {                                                          
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var structType = ctx.CreateStructType( "struct.test", true, ctx.Int8Type, ctx.FloatType, ctx.Int16Type );
                var fields = new List<Constant> { ConstantInt.From( ( byte )1 ), ConstantFP.From( 2.0f ), ConstantInt.From( ( short )-3 ) };
                var value = ctx.CreateNamedConstantStruct( structType, fields );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestMethod]
        public void CreateNamedConstantUnpackedStructTestUsingEnumerable( )
        {                                                          
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var structType = ctx.CreateStructType( "struct.test", false, ctx.Int8Type, ctx.FloatType, ctx.Int16Type );
                var fields = new List<Constant> { ConstantInt.From( ( byte )1 ), ConstantFP.From( 2.0f ), ConstantInt.From( ( short )-3 ) };
                var value = ctx.CreateNamedConstantStruct( structType, fields );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestMethod]
        public void CreateNamedConstantPackedStructTestUsingParams( )
        {                                                          
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var structType = ctx.CreateStructType( "struct.test", true, ctx.Int8Type, ctx.FloatType, ctx.Int16Type );
                var value = ctx.CreateNamedConstantStruct( structType, ConstantInt.From( ( byte )1 ), ConstantFP.From( 2.0f ), ConstantInt.From( ( short )-3 )  );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestMethod]
        public void CreateNamedConstantUnpackedStructTestUsingParams( )
        {                                                          
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var structType = ctx.CreateStructType( "struct.test", false, ctx.Int8Type, ctx.FloatType, ctx.Int16Type );
                var value = ctx.CreateNamedConstantStruct( structType, ConstantInt.From( ( byte )1 ), ConstantFP.From( 2.0f ), ConstantInt.From( ( short )-3 )  );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

                // verify additional properties created properly
                Assert.AreEqual( 0U, value.Type.IntegerBitWidth );
                Assert.IsFalse( value.Type.IsDouble );
                Assert.IsFalse( value.Type.IsFloat );
                Assert.IsFalse( value.Type.IsFloatingPoint );
                Assert.IsFalse( value.Type.IsInteger );
                Assert.IsFalse( value.Type.IsPointer );
                Assert.IsFalse( value.Type.IsPointerPointer );
                Assert.IsFalse( value.Type.IsSequence );
                Assert.IsFalse( value.Type.IsVoid );
            }
        }

        [TestMethod]
        public void CreateModuleWithEmptyNameTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                using( var module = ctx.CreateModule( string.Empty ) )
                {
                    Assert.AreSame( ctx, module.Context );
                    Assert.AreSame( string.Empty, module.Name );
                    Assert.AreSame( string.Empty, module.TargetTriple );
                    Assert.AreSame( string.Empty, module.DataLayout );
                    Assert.IsNotNull( module.Functions );
                    Assert.IsFalse( module.Functions.Any( ) );
                    Assert.IsFalse( module.Globals.Any( ) );
                }
            }
        }

        [TestMethod]
        public void CreateModuleWithNameTest( )
        {
            var moduleName = "testModule";
            using( var ctx = Context.CreateThreadContext( ) )
            {
                using( var module = ctx.CreateModule( moduleName ) )
                {
                    Assert.AreSame( ctx, module.Context );
                    Assert.AreEqual( moduleName, module.Name );
                    Assert.AreSame( string.Empty, module.TargetTriple );
                    Assert.AreSame( string.Empty, module.DataLayout );
                    Assert.IsNotNull( module.Functions );
                    Assert.IsFalse( module.Functions.Any( ) );
                    Assert.IsFalse( module.Globals.Any( ) );
                }
            }
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateModuleWithNullNameTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                using( var module = ctx.CreateModule( null ) )
                {
                }
            }
        }

        [TestMethod]
        public void CreateMetadataStringTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var content = "Test MDString";
                var mdstring = ctx.CreateMetadataString( content );
                Assert.IsNotNull( mdstring );
                Assert.AreEqual( content, mdstring.ToString( ) );
            }
        }

        [TestMethod]
        public void CreateMetadataStringWithEmptyArgTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var mdstring = ctx.CreateMetadataString( string.Empty );
                Assert.IsNotNull( mdstring );
                Assert.AreEqual( string.Empty, mdstring.ToString( ) );
            }
        }

        [TestMethod]
        public void CreateMetadataStringWithNullArgTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var mdstring = ctx.CreateMetadataString( null );
                Assert.IsNotNull( mdstring );
                Assert.AreEqual( string.Empty, mdstring.ToString( ) );
            }
        }

        [TestMethod]
        public void CreateConstantStringTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                var str = "hello world";
                ConstantDataArray value = ctx.CreateConstantString( str );
                Assert.IsNotNull( value );
                Assert.IsTrue( value.IsString );
                Assert.IsFalse( value.IsNull );
                Assert.IsFalse( value.IsUndefined );
                Assert.AreSame( string.Empty, value.Name );
                Assert.IsNotNull( value.Type );
                var arrayType = value.Type as Types.ArrayType;
                Assert.IsNotNull( arrayType );
                Assert.AreSame( ctx, arrayType.Context );
                Assert.AreSame( ctx.Int8Type, arrayType.ElementType );
                Assert.AreEqual( ( uint )str.Length, arrayType.Length );
                var valueStr = value.GetAsString( );
                Assert.IsFalse( string.IsNullOrWhiteSpace( valueStr ) );
                Assert.AreEqual( str, valueStr );
            }
        }

        [TestMethod]
        public void CreateThreadContextTest( )
        {
            using( var ctx = Context.CreateThreadContext( ) )
            {
                Assert.IsNotNull( ctx );
                Assert.AreSame( ctx, Context.CurrentContext ); 
            }
        }
    }
}