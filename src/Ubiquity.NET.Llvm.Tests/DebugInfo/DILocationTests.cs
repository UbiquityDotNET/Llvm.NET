// -----------------------------------------------------------------------
// <copyright file="DILocationTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.DebugInfo;

namespace Ubiquity.NET.Llvm.UT.DebugInfo
{
    [TestClass]
    public class DILocationTests
    {
        [TestMethod]
        public void DILocation_ctor_with_null_scope_throws( )
        {
            using var context = new Context();
            var ex = Assert.ThrowsExactly<ArgumentNullException>(()=>
                _ = new DILocation( context, 0, 0, null!, null )
                );
            Assert.AreEqual("scope", ex.ParamName);
        }

        [TestMethod]
        public void DILocation_ctor_with_valid_args_succeeds( )
        {
            // Create a function to use as scope for a new DILocation
            using var context = new Context();
            using var module = context.CreateBitcodeModule("test");
            using var diBuilder = new DIBuilder(module);
            DICompileUnit cu = diBuilder.CreateCompileUnit( SourceLanguage.C, "test.c", "UnitTest" );

            var i32 = new DebugBasicType( module.Context.Int32Type, in diBuilder, "int", DiTypeKind.Signed );
            var function = module.CreateFunction( in diBuilder
                                                , scope: cu
                                                , name: "test"
                                                , linkageName: null
                                                , file: cu.File
                                                , line: 1
                                                , signature: context.CreateFunctionType(in diBuilder, i32)
                                                , isLocalToUnit: false
                                                , isDefinition: true
                                                , scopeLine: 1
                                                , debugFlags: DebugInfoFlags.None
                                                , isOptimized: false
                                                );
            Assert.IsNotNull( function.DISubProgram );

            // Create and test the DILocation
            var location = new DILocation( context, 12, 34, function.DISubProgram! );
            Assert.AreEqual( 12U, location.Line );
            Assert.AreEqual( 34U, location.Column );
            Assert.AreEqual( context, location.Context );
            Assert.AreEqual( function.DISubProgram, location.Scope );
        }
    }
}
