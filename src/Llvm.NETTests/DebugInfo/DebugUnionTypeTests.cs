// <copyright file="DebugUnionTypeTests.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using Llvm.NET.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// warning SA1500: Braces for multi-line statements must not share line
#pragma warning disable SA1500

namespace Llvm.NET.DebugInfo.Tests
{
    // TODO: Standard arg checks and error handling in general
    // for now focusing on verifying it does the right thing
    // when given valid inputs (e.g. the "Golden" path)
    [TestClass]
    public class DebugUnionTypeTests
    {
        [TestMethod]
        public void DebugUnionTypeTest( )
        {
            using( var context = new Context( ) )
            using( var module = new BitcodeModule( context ) )
            {
                const string nativeUnionName = "union.testUnion";
                const string unionSymbolName = "testUnion";
                var testFile = module.DIBuilder.CreateFile( "test" );

                var union = new DebugUnionType( module, nativeUnionName, null, unionSymbolName, module.DIBuilder.CreateFile("test") );
                Assert.IsNotNull( union );

                Assert.IsTrue( module.Verify( out string errMsg ), errMsg );

                Assert.IsNotNull( union.DIType );
                Assert.IsNotNull( union.NativeType );

                Assert.AreEqual( Tag.UnionType, union.DIType.Tag );
                Assert.AreEqual( nativeUnionName, union.Name );
                Assert.AreEqual( nativeUnionName, union.NativeType.Name );
                Assert.AreEqual( unionSymbolName, union.DIType.Name );
                Assert.IsNull( union.DIType.Scope );
                Assert.AreEqual( testFile, union.DIType.File );
                Assert.AreEqual( 0U, union.DIType.Line );

                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Private ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Protected ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Public ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.ForwardDeclaration ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.AppleBlock ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.BlockByrefStruct ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Virtual ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Artificial ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.ObjectPointer ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.ObjcClassComplete ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Vector ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.StaticMember ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.LValueReference ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.RValueReference ) );

                // test the wrapped native type created correctly
                Assert.IsFalse( union.NativeType.IsSized );
                Assert.IsTrue( union.NativeType.IsStruct );
                Assert.AreEqual( TypeKind.Struct, union.NativeType.Kind );
                Assert.IsFalse( union.NativeType.IsInteger );
                Assert.IsFalse( union.NativeType.IsFloat );
                Assert.IsFalse( union.NativeType.IsDouble );
                Assert.IsFalse( union.NativeType.IsVoid );
                Assert.IsFalse( union.NativeType.IsPointer );
                Assert.IsFalse( union.NativeType.IsSequence );
                Assert.IsFalse( union.NativeType.IsFloatingPoint );
                Assert.IsFalse( union.NativeType.IsPointerPointer );
                Assert.AreEqual( 0U, union.NativeType.IntegerBitWidth );

                // test the wrapping interface reflects the wrapped native type
                Assert.IsFalse( union.IsSized );
                Assert.IsTrue( union.IsStruct );
                Assert.AreEqual( TypeKind.Struct, union.Kind );
                Assert.IsFalse( union.IsInteger );
                Assert.IsFalse( union.IsFloat );
                Assert.IsFalse( union.IsDouble );
                Assert.IsFalse( union.IsVoid );
                Assert.IsFalse( union.IsPointer );
                Assert.IsFalse( union.IsSequence );
                Assert.IsFalse( union.IsFloatingPoint );
                Assert.IsFalse( union.IsPointerPointer );
                Assert.AreEqual( 0U, union.IntegerBitWidth );
            }
        }

        [TestMethod]
        public void DebugUnionTypeTest2( )
        {
            var testTriple = new Triple( "thumbv7m-none--eabi" );
            var targetMachine = new TargetMachine( testTriple );
            using( var ctx = new Context( ) )
            using( var module = new BitcodeModule( ctx, "testModule" ) { Layout= targetMachine.TargetData } )
            {
                const string nativeUnionName = "union.testUnion";
                const string unionSymbolName = "testUnion";

                var diFile = module.DIBuilder.CreateFile( "test.c" );
                var diCompileUnit = module.DIBuilder.CreateCompileUnit( SourceLanguage.C, "test.c", "unit-test", false, string.Empty, 0 );

                // Create basic types used in this compilation
                var i32 = new DebugBasicType( module.Context.Int32Type, module, "int", DiTypeKind.Signed );
                var i16 = new DebugBasicType( module.Context.Int16Type, module, "short", DiTypeKind.Signed );
                var f32 = new DebugBasicType( module.Context.FloatType, module, "float", DiTypeKind.Float );

                var members = new[ ]
                    { new DebugMemberInfo { File = diFile, Line = 3, Name = "a", DebugType = i32, Index = 0 }
                    , new DebugMemberInfo { File = diFile, Line = 4, Name = "b", DebugType = i16, Index = 1 }
                    , new DebugMemberInfo { File = diFile, Line = 5, Name = "c", DebugType = f32, Index = 2 }
                    };

                var llvmType = module.Context.CreateStructType( nativeUnionName );
                var union = new DebugUnionType( llvmType, module, diCompileUnit, unionSymbolName, diFile, 0, DebugInfoFlags.None, members );
                Assert.IsNotNull( union );
                Assert.IsNotNull( union.DIType );
                Assert.IsNotNull( union.NativeType );
                Assert.AreEqual( Tag.UnionType, union.DIType.Tag );
                Assert.AreEqual( nativeUnionName, union.Name );
                Assert.AreEqual( nativeUnionName, union.NativeType.Name );
                Assert.AreEqual( unionSymbolName, union.DIType.Name );
                Assert.IsNull( union.DIType.Scope );
                Assert.AreEqual( diFile, union.DIType.File );
                Assert.AreEqual( 0U, union.DIType.Line );

                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Private ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Protected ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Public ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.ForwardDeclaration ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.AppleBlock ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.BlockByrefStruct ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Virtual ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Artificial ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.ObjectPointer ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.ObjcClassComplete ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.Vector ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.StaticMember ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.LValueReference ) );
                Assert.IsFalse( union.DIType.DebugInfoFlags.HasFlag( DebugInfoFlags.RValueReference ) );

                // test the wrapped native type created correctly
                Assert.IsTrue( union.NativeType.IsSized );
                Assert.IsTrue( union.NativeType.IsStruct );
                Assert.AreEqual( TypeKind.Struct, union.NativeType.Kind );
                Assert.IsFalse( union.NativeType.IsInteger );
                Assert.IsFalse( union.NativeType.IsFloat );
                Assert.IsFalse( union.NativeType.IsDouble );
                Assert.IsFalse( union.NativeType.IsVoid );
                Assert.IsFalse( union.NativeType.IsPointer );
                Assert.IsFalse( union.NativeType.IsSequence );
                Assert.IsFalse( union.NativeType.IsFloatingPoint );
                Assert.IsFalse( union.NativeType.IsPointerPointer );
                Assert.AreEqual( 0U, union.NativeType.IntegerBitWidth );

                // test the wrapping interface reflects the wrapped native type
                Assert.IsTrue( union.IsSized );
                Assert.IsTrue( union.IsStruct );
                Assert.AreEqual( TypeKind.Struct, union.Kind );
                Assert.IsFalse( union.IsInteger );
                Assert.IsFalse( union.IsFloat );
                Assert.IsFalse( union.IsDouble );
                Assert.IsFalse( union.IsVoid );
                Assert.IsFalse( union.IsPointer );
                Assert.IsFalse( union.IsSequence );
                Assert.IsFalse( union.IsFloatingPoint );
                Assert.IsFalse( union.IsPointerPointer );
                Assert.AreEqual( 0U, union.IntegerBitWidth );

                Assert.AreEqual( 1, union.NativeType.Members.Count );
                Assert.AreSame( ctx.Int32Type, union.NativeType.Members[ 0 ] );

                Assert.IsNotNull( union.DIType.Elements );
                Assert.AreEqual( members.Length, union.DIType.Elements.Count );
                for( int i = 0; i< members.Length; ++i )
                {
                    var memberType = union.DIType.Elements[ i ] as DIDerivedType;
                    Assert.IsNotNull( memberType );
                    Assert.AreEqual( Tag.Member, memberType.Tag );
                    Assert.AreEqual( members[ i ].Name, memberType.Name );
                    Assert.AreEqual( members[ i ].DebugType.DIType, memberType.BaseType );
                }
            }
        }
    }
}
