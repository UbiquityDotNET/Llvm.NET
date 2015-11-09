using System.Collections.Generic;
using Llvm.NET.Types;
using Llvm.NET.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Llvm.NET.Tests
{
    [TestClass]
    [DeploymentItem("LibLLVM.dll")]
    public class ContextTests
    {
        [TestMethod]
        public void DisposeTest( )
        {
            using( var module = new NativeModule("test") )
            {
                Assert.IsNotNull( module.Context );
            }
        }

        [TestMethod]
        public void GetPointerTypeForTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var int8PtrType = module.Context.GetPointerTypeFor( module.Context.Int8Type );
                Assert.IsNotNull( int8PtrType );
                Assert.AreSame( module.Context.Int8Type, int8PtrType.ElementType );
                Assert.AreSame( module.Context, int8PtrType.Context );

                var int8PtrTypeAlt = module.Context.Int8Type.CreatePointerType( );
                Assert.AreSame( int8PtrType, int8PtrTypeAlt );
            }
        }

        [TestMethod]
        public void GetIntTypeTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var int8Type = module.Context.GetIntType( 8 );
                Assert.AreSame( module.Context.Int8Type, int8Type );
                Assert.AreSame( module.Context, int8Type.Context );
                Assert.AreEqual( 8U, int8Type.IntegerBitWidth );

                var int16Type = module.Context.GetIntType( 16 );
                Assert.AreSame( module.Context.Int16Type, int16Type );
                Assert.AreSame( module.Context, int16Type.Context );
                Assert.AreEqual( 16U, int16Type.IntegerBitWidth );

                var int32Type = module.Context.GetIntType( 32 );
                Assert.AreSame( module.Context.Int32Type, int32Type );
                Assert.AreSame( module.Context, int32Type.Context );
                Assert.AreEqual( 32U, int32Type.IntegerBitWidth );

                var int64Type = module.Context.GetIntType( 64 );
                Assert.AreSame( module.Context.Int64Type, int64Type );
                Assert.AreSame( module.Context, int64Type.Context );
                Assert.AreEqual( 64U, int64Type.IntegerBitWidth );

                var int128Type = module.Context.GetIntType( 128 );
                Assert.AreSame( module.Context, int128Type.Context );
                Assert.AreEqual( 128U, int128Type.IntegerBitWidth ); 
            }
        }

        [TestMethod]
        public void GetFunctionTypeTest( )
        {
            using( var module = new NativeModule("test") )
            {
                // i16 ( i32, float )
                var funcSig = module.Context.GetFunctionType( module.Context.Int16Type, module.Context.Int32Type, module.Context.FloatType );
                Assert.IsNotNull( funcSig );
                Assert.AreSame( module.Context, funcSig.Context );

                Assert.AreEqual( TypeKind.Function, funcSig.Kind );
                Assert.AreSame( module.Context.Int16Type, funcSig.ReturnType );
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
            using( var module = new NativeModule("test") )
            {
                Assert.IsNotNull( module.Context );
                Assert.IsNotNull( module.Context.Int32Type );
                Assert.IsNotNull( module.Context.FloatType );
                Assert.IsNotNull( module.Context.Int16Type );

                // i16 ( i32, float )
                var argTypes = new ITypeRef[ ] { module.Context.Int32Type, module.Context.FloatType };
                var funcSig = module.Context.GetFunctionType( module.Context.Int16Type, argTypes );
                Assert.IsNotNull( funcSig );
                Assert.AreSame( module.Context, funcSig.Context );

                Assert.AreEqual( TypeKind.Function, funcSig.Kind );
                Assert.AreSame( module.Context.Int16Type, funcSig.ReturnType );
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
            using( var module = new NativeModule("test") )
            {
                // i16 ( i32, float )
                var argTypes = new List<ITypeRef> { module.Context.Int32Type, module.Context.FloatType };
                var funcSig = module.Context.GetFunctionType( module.Context.Int16Type, argTypes, true );
                Assert.IsNotNull( funcSig );
                Assert.AreSame( module.Context, funcSig.Context );

                Assert.AreEqual( TypeKind.Function, funcSig.Kind );
                Assert.AreSame( module.Context.Int16Type, funcSig.ReturnType );
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
            using( var module = new NativeModule("test") )
            {
                var typeName = "struct.test";
                var type = module.Context.CreateStructType( typeName );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

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
            using( var module = new NativeModule("test") )
            {
                var type = module.Context.CreateStructType( false, module.Context.Int16Type );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.IsNull( type.Name );
                Assert.AreEqual( 1, type.Members.Count );
                Assert.AreSame( module.Context.Int16Type, type.Members[ 0 ] );
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
            using( var module = new NativeModule("test") )
            {
                var type = module.Context.CreateStructType( true, module.Context.Int16Type );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.IsNull( type.Name );
                Assert.AreEqual( 1, type.Members.Count );
                Assert.AreSame( module.Context.Int16Type, type.Members[ 0 ] );
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
            using( var module = new NativeModule("test") )
            {
                var type = module.Context.CreateStructType( false, module.Context.Int16Type, module.Context.Int32Type );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.IsNull( type.Name );
                Assert.AreEqual( 2, type.Members.Count );
                Assert.AreSame( module.Context.Int16Type, type.Members[ 0 ] );
                Assert.AreSame( module.Context.Int32Type, type.Members[ 1 ] );
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
            using( var module = new NativeModule("test") )
            {
                var type = module.Context.CreateStructType( true, module.Context.Int16Type, module.Context.Int32Type );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.IsNull( type.Name );
                Assert.AreEqual( 2, type.Members.Count );
                Assert.AreSame( module.Context.Int16Type, type.Members[ 0 ] );
                Assert.AreSame( module.Context.Int32Type, type.Members[ 1 ] );
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
            using( var module = new NativeModule("test") )
            {
                var typeName = "struct.test";
                var type = module.Context.CreateStructType( typeName, false, module.Context.Int16Type );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName, type.Name );
                Assert.AreEqual( 1, type.Members.Count );
                Assert.AreSame( module.Context.Int16Type, type.Members[ 0 ] );
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
            using( var module = new NativeModule("test") )
            {
                var typeName = "struct.test";
                var type = module.Context.CreateStructType( typeName, true, module.Context.Int16Type );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName, type.Name );
                Assert.AreEqual( 1, type.Members.Count );
                Assert.AreSame( module.Context.Int16Type, type.Members[ 0 ] );
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
            using( var module = new NativeModule("test") )
            {
                var typeName = "struct.test";
                var type = module.Context.CreateStructType( typeName, false, module.Context.Int16Type, module.Context.Int32Type );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName, type.Name );
                Assert.AreEqual( 2, type.Members.Count );
                Assert.AreSame( module.Context.Int16Type, type.Members[ 0 ] );
                Assert.AreSame( module.Context.Int32Type, type.Members[ 1 ] );
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
            using( var module = new NativeModule("test") )
            {
                var typeName = "struct.test";
                var type = module.Context.CreateStructType( typeName, true, module.Context.Int16Type, module.Context.Int32Type );
                Assert.IsNotNull( type );
                Assert.AreSame( module.Context, type.Context );

                // verify type specific attributes
                Assert.AreEqual( TypeKind.Struct, type.Kind );
                Assert.IsTrue( type.IsStruct );
                Assert.AreEqual( typeName, type.Name );
                Assert.AreEqual( 2, type.Members.Count );
                Assert.AreSame( module.Context.Int16Type, type.Members[ 0 ] );
                Assert.AreSame( module.Context.Int32Type, type.Members[ 1 ] );
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
            using( var module = new NativeModule("test") )
            {
                var value = module.Context.CreateConstantStruct( true
                                                               , module.Context.CreateConstant( ( byte )1)
                                                               , module.Context.CreateConstant( 2.0f )
                                                               , module.Context.CreateConstant( ( short )-3 )
                                                               );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousConstantNestedStructTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var nestedValue = module.Context.CreateConstantStruct( false
                                                                     , module.Context.CreateConstant( 5 )
                                                                     , module.Context.CreateConstantString("Hello")
                                                                     , module.Context.CreateConstant( 6 )
                                                                     );
                var value = module.Context.CreateConstantStruct( true
                                                               , module.Context.CreateConstant( ( byte )1)
                                                               , module.Context.CreateConstant( 2.0f )
                                                               , nestedValue
                                                               , module.Context.CreateConstant( ( short )-3 )
                                                               );
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
                Assert.AreEqual( "Hello", ( ( ConstantDataSequential )nestedConst.Operands[ 1 ] ).ExtractAsString() );
                Assert.AreEqual( 6, ( ( ConstantInt )nestedConst.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestCategory("Anonymous Structs")]
        [TestMethod]
        public void CreateAnonymousUnpackedConstantStructUsingParamsTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var value = module.Context.CreateConstantStruct( false
                                                               , module.Context.CreateConstant( ( byte )1)
                                                               , module.Context.CreateConstant( 2.0f )
                                                               , module.Context.CreateConstant( ( short )-3 )
                                                               );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousPackedConstantStructUsingEnumerableTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var fields = new List<Constant> { module.Context.CreateConstant( ( byte )1 ), module.Context.CreateConstant( 2.0f ), module.Context.CreateConstant( ( short )-3 ) };
                var value = module.Context.CreateConstantStruct( true, fields );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestMethod]
        [TestCategory("Anonymous Structs")]
        public void CreateAnonymousUnpackedConstantStructUsingEnumerableTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var fields = new List<Constant> { module.Context.CreateConstant( ( byte )1 )
                                                , module.Context.CreateConstant( 2.0f )
                                                , module.Context.CreateConstant( ( short )-3 )
                                                };
                var value = module.Context.CreateConstantStruct( false, fields );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestMethod]
        public void CreateNamedConstantPackedStructTestUsingEnumerable( )
        {                                                          
            using( var module = new NativeModule("test") )
            {
                var structType = module.Context.CreateStructType( "struct.test", true, module.Context.Int8Type, module.Context.FloatType, module.Context.Int16Type );
                var fields = new List<Constant> { module.Context.CreateConstant( ( byte )1 )
                                                , module.Context.CreateConstant( 2.0f )
                                                , module.Context.CreateConstant( ( short )-3 )
                                                };
                var value = module.Context.CreateNamedConstantStruct( structType, fields );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestMethod]
        public void CreateNamedConstantUnpackedStructTestUsingEnumerable( )
        {                                                          
            using( var module = new NativeModule("test") )
            {
                var structType = module.Context.CreateStructType( "struct.test"
                                                                , false
                                                                , module.Context.Int8Type
                                                                , module.Context.FloatType
                                                                , module.Context.Int16Type
                                                                );
                var fields = new List<Constant> { module.Context.CreateConstant( ( byte )1 )
                                                , module.Context.CreateConstant( 2.0f )
                                                , module.Context.CreateConstant( ( short )-3 )
                                                };
                var value = module.Context.CreateNamedConstantStruct( structType, fields );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestMethod]
        public void CreateNamedConstantPackedStructTestUsingParams( )
        {                                                          
            using( var module = new NativeModule("test") )
            {
                var structType = module.Context.CreateStructType( "struct.test"
                                                                , true
                                                                , module.Context.Int8Type
                                                                , module.Context.FloatType
                                                                , module.Context.Int16Type
                                                                );
                var value = module.Context.CreateNamedConstantStruct( structType
                                                                    , module.Context.CreateConstant( ( byte )1 )
                                                                    , module.Context.CreateConstant( 2.0f )
                                                                    , module.Context.CreateConstant( ( short )-3 )
                                                                    );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestMethod]
        public void CreateNamedConstantUnpackedStructTestUsingParams( )
        {                                                          
            using( var module = new NativeModule("test") )
            {
                var structType = module.Context.CreateStructType( "struct.test"
                                                                , false
                                                                , module.Context.Int8Type
                                                                , module.Context.FloatType
                                                                , module.Context.Int16Type
                                                                );
                var value = module.Context.CreateNamedConstantStruct( structType
                                                                    , module.Context.CreateConstant( ( byte )1 )
                                                                    , module.Context.CreateConstant( 2.0f )
                                                                    , module.Context.CreateConstant( ( short )-3 )
                                                                    );
                Assert.AreEqual( 3, value.Operands.Count );
                Assert.IsInstanceOfType( value.Operands[ 0 ], typeof( ConstantInt ) );
                Assert.IsInstanceOfType( value.Operands[ 1 ], typeof( ConstantFP ) );
                Assert.IsInstanceOfType( value.Operands[ 2 ], typeof( ConstantInt ) );

                Assert.AreEqual( 1L, ( ( ConstantInt )value.Operands[ 0 ] ).SignExtendedValue );
                Assert.AreEqual( 2.0, ( ( ConstantFP )value.Operands[ 1 ] ).Value );
                Assert.AreEqual( -3L, ( ( ConstantInt )value.Operands[ 2 ] ).SignExtendedValue );

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
        }

        [TestMethod]
        public void CreateMetadataStringTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var content = "Test MDString";
                var mdstring = module.Context.CreateMetadataString( content );
                Assert.IsNotNull( mdstring );
                Assert.AreEqual( content, mdstring.ToString( ) );
            }
        }

        [TestMethod]
        public void CreateMetadataStringWithEmptyArgTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var mdstring = module.Context.CreateMetadataString( string.Empty );
                Assert.IsNotNull( mdstring );
                Assert.AreEqual( string.Empty, mdstring.ToString( ) );
            }
        }

        [TestMethod]
        public void CreateMetadataStringWithNullArgTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var mdstring = module.Context.CreateMetadataString( null );
                Assert.IsNotNull( mdstring );
                Assert.AreEqual( string.Empty, mdstring.ToString( ) );
            }
        }

        [TestMethod]
        public void CreateConstantStringTest( )
        {
            using( var module = new NativeModule("test") )
            {
                var str = "hello world";
                ConstantDataArray value = module.Context.CreateConstantString( str );
                Assert.IsNotNull( value );
                Assert.IsTrue( value.IsString );
                Assert.IsFalse( value.IsNull );
                Assert.IsFalse( value.IsUndefined );
                Assert.AreSame( string.Empty, value.Name );
                Assert.IsNotNull( value.NativeType );
                var arrayType = value.NativeType as IArrayType;
                Assert.IsNotNull( arrayType );
                Assert.AreSame( module.Context, arrayType.Context );
                Assert.AreSame( module.Context.Int8Type, arrayType.ElementType );
                Assert.AreEqual( ( uint )str.Length, arrayType.Length );
                var valueStr = value.ExtractAsString( );
                Assert.IsFalse( string.IsNullOrWhiteSpace( valueStr ) );
                Assert.AreEqual( str, valueStr );
            }
        }
    }
}