// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Types;

namespace Ubiquity.NET.Llvm.UT.DebugInfo
{
    [TestClass]
    public class DebugStructTypeTests
    {
        [TestMethod]
        public void DebugStructType_constructing_empty_struct_succeeds( )
        {
            using var context = new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            string sourceName = "empty_struct";
            string linkageName = "_empty_struct";

            var structType = new DebugStructType( diBuilder
                                                , nativeName: linkageName
                                                , scope: null
                                                , sourceName: sourceName
                                                , file: null
                                                , line: 0
                                                , debugFlags: default
                                                , members: []
                                                ); // rest of args use defaults...

            Assert.IsTrue( structType.IsSized );
            Assert.IsFalse( structType.IsPacked );
            Assert.IsTrue( structType.IsStruct );
            Assert.AreEqual( linkageName, structType.Name );
            Assert.AreEqual( sourceName, structType.SourceName );
        }

        [TestMethod]
        public void DebugStructType_constructing_empty_anonymous_struct_succeeds( )
        {
            using var context = new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            string sourceName = string.Empty;
            string linkageName = string.Empty;

            var structType = new DebugStructType( diBuilder
                                                , nativeName: linkageName
                                                , scope: null
                                                , sourceName: sourceName
                                                , file: null
                                                , line: 0
                                                , debugFlags: default
                                                , members: []
                                                ); // rest of args use defaults...

            Assert.IsTrue( structType.IsSized );
            Assert.IsFalse( structType.IsPacked );
            Assert.IsTrue( structType.IsStruct );
            Assert.AreEqual( linkageName, structType.Name );
            Assert.AreEqual( sourceName, structType.SourceName );
        }

        [TestMethod]
        public void SetBody_with_native_elements_succeeds( )
        {
            using var context = new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            var file = diBuilder.CreateFile( "test.foo" );
            uint line = 1234;
            string sourceName = string.Empty;
            string linkageName = string.Empty;

            var debugStructType = new DebugStructType( diBuilder, linkageName, file, sourceName, file, line );
            bool packed = false;
            var elements = new ITypeRef[] { context.Float128Type, context.Int32Type, context.Int64Type };

            debugStructType.SetBody(
                packed,
                elements );

            Assert.HasCount( elements.Length, debugStructType.Members );
            for(int i = 0; i < elements.Length; ++i)
            {
                Assert.AreEqual( elements[ i ], debugStructType.Members[ i ] );
            }
        }

#if NOT_YET_READY_GENERATED

        [TestMethod]
        public void SetBody_StateUnderTest_ExpectedBehavior1( )
        {
            var debugStructType = new DebugStructType( TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO );
            bool packed = false;
            Module module = null;
            DIScope? scope = null;
            DIFile? file = null;
            uint line = 0;
            DebugInfoFlags debugFlags = default( global::Ubiquity.NET.Llvm.DebugInfo.DebugInfoFlags );
            IEnumerable debugElements = null;

            debugStructType.SetBody(
                packed,
                module,
                scope,
                file,
                line,
                debugFlags,
                debugElements );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void SetBody_StateUnderTest_ExpectedBehavior2( )
        {
            var debugStructType = new DebugStructType( TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO, TODO );
            bool packed = false;
            Module module = null;
            DIScope? scope = null;
            DIFile? file = null;
            uint line = 0;
            DebugInfoFlags debugFlags = default( global::Ubiquity.NET.Llvm.DebugInfo.DebugInfoFlags );
            IEnumerable nativeElements = null;
            IEnumerable debugElements = null;
            DIType? derivedFrom = null;
            uint? bitSize = null;
            uint bitAlignment = 0;

            debugStructType.SetBody(
                packed,
                module,
                scope,
                file,
                line,
                debugFlags,
                nativeElements,
                debugElements,
                derivedFrom,
                bitSize,
                bitAlignment );

            Assert.Inconclusive( );
        }
#endif
    }
}
