// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Types;

// warning SA1500: Braces for multi-line statements must not share line
#pragma warning disable SA1500
#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Ubiquity.NET.Llvm.UT
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
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( );
            using var diBuilder = new DIBuilder(module);

            const string nativeUnionName = "union.testUnion";
            const string unionSymbolName = "testUnion";
            var testFile = diBuilder.CreateFile( "test" );

            var union = new DebugUnionType( diBuilder, nativeUnionName, null, unionSymbolName, diBuilder.CreateFile("test") );
            Assert.IsNotNull( union );

            Assert.IsTrue( module.Verify( out string errMsg ), errMsg );

            Assert.IsNotNull( union!.DebugInfoType );
            Assert.IsNotNull( union.NativeType );

            Assert.AreEqual( Tag.UnionType, union.DebugInfoType!.Tag );
            Assert.AreEqual( nativeUnionName, union.Name );
            Assert.AreEqual( nativeUnionName, union.NativeType.Name );
            Assert.AreEqual<string>( unionSymbolName, union.DebugInfoType.Name );
            Assert.IsNull( union.DebugInfoType.Scope );
            Assert.AreEqual( testFile, union.DebugInfoType.File );
            Assert.AreEqual( 0U, union.DebugInfoType.Line );

            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Private ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Protected ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Public ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.ForwardDeclaration ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.AppleBlock ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Virtual ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Artificial ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.ObjectPointer ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.ObjcClassComplete ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Vector ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.StaticMember ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.LValueReference ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.RValueReference ) );

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
            Assert.AreEqual( 0U, union.IntegerBitWidth );
        }

#pragma warning disable CA1506
        [TestMethod]
        public void DebugUnionTypeTest2( )
        {
            const string nativeUnionName = "union.testUnion";
            const string unionSymbolName = "testUnion";

            using var testTriple = new Triple( "thumbv7m-none--eabi" );
            using var targetMachine = new TargetMachine( testTriple );
            using var ctx = new Context( );
            using var module = ctx.CreateBitcodeModule( "testModule" );
            using var diBuilder = new DIBuilder(module);

            using var layout = targetMachine.CreateTargetData();
            module.Layout = layout;
            var diFile = diBuilder.CreateFile( "test.cs" );
            var diCompileUnit = diBuilder.CreateCompileUnit( SourceLanguage.CSharp, "test.cs", "unit-test", false, string.Empty, 0 );

            // Create basic types used in this compilation
            var i32 = new DebugBasicType( module.Context.Int32Type, diBuilder, "int", DiTypeKind.Signed );
            var i16 = new DebugBasicType( module.Context.Int16Type, diBuilder, "short", DiTypeKind.Signed );
            var f32 = new DebugBasicType( module.Context.FloatType, diBuilder, "float", DiTypeKind.Float );

            var members = new[ ]
                    { new DebugMemberInfo( 0, "a", diFile, 3, i32 )
                    , new DebugMemberInfo( 1, "b", diFile, 4, i16 )
                    , new DebugMemberInfo( 2, "c", diFile, 5, f32 )
                    };

            var llvmType = module.Context.CreateStructType( nativeUnionName );
            var union = new DebugUnionType( llvmType, diBuilder, diCompileUnit, unionSymbolName, diFile, 0, DebugInfoFlags.None, members );
            Assert.IsNotNull( union );
            Assert.IsNotNull( union!.DebugInfoType );
            Assert.IsNotNull( union.NativeType );
            Assert.AreEqual( Tag.UnionType, union.DebugInfoType!.Tag );
            Assert.AreEqual( nativeUnionName, union.Name );
            Assert.AreEqual( nativeUnionName, union.NativeType!.Name );
            Assert.AreEqual<string>( unionSymbolName, union.DebugInfoType.Name );
            Assert.IsNull( union.DebugInfoType.Scope );
            Assert.AreEqual( diFile, union.DebugInfoType.File );
            Assert.AreEqual( 0U, union.DebugInfoType.Line );

            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Private ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Protected ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Public ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.ForwardDeclaration ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.AppleBlock ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Virtual ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Artificial ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.ObjectPointer ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.ObjcClassComplete ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.Vector ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.StaticMember ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.LValueReference ) );
            Assert.IsFalse( union.DebugInfoType.DebugInfoFlags.HasFlag( DebugInfoFlags.RValueReference ) );

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
            Assert.AreEqual( 0U, union.IntegerBitWidth );

            Assert.HasCount( 1, union.NativeType.Members );
            Assert.AreEqual( ctx.Int32Type, union.NativeType.Members[ 0 ] );

            Assert.IsNotNull( union.DebugInfoType.Elements );
            Assert.AreEqual( members.Length, union.DebugInfoType.Elements.Count );
            for(int i = 0; i < members.Length; ++i)
            {
                var memberType = union.DebugInfoType.Elements[ i ] as DIDerivedType;
                Assert.IsNotNull( memberType );
                Assert.AreEqual( Tag.Member, memberType!.Tag );
                Assert.AreEqual<string>( members[ i ].Name, memberType.Name );
                Assert.AreEqual( members[ i ].DebugType.DebugInfoType, memberType.BaseType );
            }
        }
#pragma warning restore CA1506
    }
}
