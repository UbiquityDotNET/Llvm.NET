// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.InteropHelpers;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Metadata;

namespace Ubiquity.NET.Llvm.UT.DebugInfo
{
    [TestClass]
    [SuppressMessage( "StyleCop.CSharp.ReadabilityRules", "SA1124:Do not use regions", Justification = "Groups related test methods by category" )]
    public class DebugInfoBuilderTests
    {
        #region DIBuilder.CreateFile
        [TestMethod]
        [TestCategory( "DIBuilder.CreateFile" )]
        public void CreateFile_with_null_path_should_succeed( )
        {
            using var context = new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            // testing how API handles NULL
            var file = diBuilder.CreateFile( null );
            Assert.IsNotNull( file );
            ValidateDIFileProperties( context, file, LazyEncodedString.Empty, LazyEncodedString.Empty );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateFile" )]
        public void CreateFile_with_empty_path_should_succeed( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            var file = diBuilder.CreateFile( LazyEncodedString.Empty );
            Assert.IsNotNull( file );
            ValidateDIFileProperties( context, file, LazyEncodedString.Empty, LazyEncodedString.Empty );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateFile" )]
        public void CreateFile_with_valid_path_should_succeed( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            string path = "test.foo";

            var file = diBuilder.CreateFile( path );
            Assert.IsNotNull( file );
            ValidateDIFileProperties( context, file, LazyEncodedString.Empty, path );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateFile" )]
        public void CreateFile_with_null_directory_should_succeed( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            string fileName = $"{nameof(CreateFile_with_null_directory_should_succeed)}.foo";

            var file = diBuilder.CreateFile( fileName, null );
            ValidateDIFileProperties( context, file, LazyEncodedString.Empty, fileName );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateFile" )]
        public void CreateFile_with_empty_directory_should_succeed( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            string fileName = $"{nameof(CreateFile_with_null_directory_should_succeed)}.foo";

            var file = diBuilder.CreateFile( fileName, LazyEncodedString.Empty );
            ValidateDIFileProperties( context, file, LazyEncodedString.Empty, fileName );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateFile" )]
        public void CreateFile_with_valid_directory_and_filename_should_succeed( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            string fileName = $"{nameof(CreateFile_with_null_directory_should_succeed)}.foo";
            string directory = nameof(DebugInfoBuilderTests);

            var file = diBuilder.CreateFile( fileName, directory );
            ValidateDIFileProperties( context, file, directory, fileName );
        }
        #endregion

        #region DIBuilder.CreateCompileUnit
        [TestMethod]
        [TestCategory( "DIBuilder.CreateCompileUnit" )]
        public void CreateCompileUnit_with_null_sourcefilepath_should_throw( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            var ex = Assert.ThrowsExactly<ArgumentNullException>(()=>
            {
                using var diBuilder = new DIBuilder(testModule);
                _ = diBuilder.CreateCompileUnit( language: default
                                               , sourceFilePath: null! // should trigger exception
                                               , producer: null
                                               , optimized: false
                                               , compilationFlags: null
                                               , runtimeVersion: 0
                                               );
            });
            Assert.AreEqual( "sourceFilePath", ex.ParamName );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateCompileUnit" )]
        public void CreateCompileUnit_with_empty_sourcefilepath_should_throw( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );

            var ex = Assert.ThrowsExactly<ArgumentException>(()=>
            {
                using var diBuilder = new DIBuilder(testModule);

                _ = diBuilder.CreateCompileUnit( language: default
                                               , sourceFilePath: LazyEncodedString.Empty // should trigger exception
                                               , producer: null
                                               , optimized: false
                                               , compilationFlags: null
                                               , runtimeVersion: 0
                                               );
            });
            Assert.AreEqual( "sourceFilePath", ex.ParamName );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateCompileUnit" )]
        public void CreateCompileUnit_with_valid_sourcefilepath_should_be_valid( )
        {
            string directory = Path.Combine( "folder1", "folder2" );
            string fileName = @"test.foo";
            string sourceFilePath = Path.Combine( directory, fileName );

            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            var unit = diBuilder.CreateCompileUnit( language: default
                                                  , sourceFilePath: sourceFilePath
                                                  , producer: LazyEncodedString.Empty
                                                  , optimized: false
                                                  , compilationFlags: null
                                                  , runtimeVersion: 0
                                                  );

            // validate unit itself, based on input
            ValidateConstructedCompileUnit( unit, context, directory, fileName );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateCompileUnit" )]
        public void CreateCompileUnit_with_null_producer_should_not_throw( )
        {
            string sourceFilePath = "test.foo";

            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            var unit = diBuilder.CreateCompileUnit( language: default
                                                  , sourceFilePath: sourceFilePath
                                                  , producer: null
                                                  , optimized: false
                                                  , compilationFlags: null
                                                  , runtimeVersion: 0
                                                  );
            ValidateConstructedCompileUnit( unit, context, LazyEncodedString.Empty, sourceFilePath );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateCompileUnit" )]
        public void CreateCompileUnit_with_empty_producer_should_not_throw( )
        {
            string sourceFilePath = "test.foo";

            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            var unit = diBuilder.CreateCompileUnit( language: default
                                                  , sourceFilePath: sourceFilePath
                                                  , producer: LazyEncodedString.Empty
                                                  , optimized: false
                                                  , compilationFlags: null
                                                  , runtimeVersion: 0
                                                  );
            ValidateConstructedCompileUnit( unit, context, LazyEncodedString.Empty, sourceFilePath );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateCompileUnit" )]
        public void CreateCompileUnit_with_valid_producer_should_not_throw( )
        {
            string sourceFilePath = "test.foo";
            string producer = nameof(CreateCompileUnit_with_valid_producer_should_not_throw);
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            var unit = diBuilder.CreateCompileUnit( language: default
                                                  , sourceFilePath: sourceFilePath
                                                  , producer: producer
                                                  , optimized: false
                                                  , compilationFlags: null
                                                  , runtimeVersion: 0
                                                  );
            ValidateConstructedCompileUnit( unit, context, LazyEncodedString.Empty, sourceFilePath, producer );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateCompileUnit" )]
        public void CreateCompileUnit_with_empty_compilationFlags_should_not_throw( )
        {
            string sourceFilePath = "test.foo";

            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            var unit = diBuilder.CreateCompileUnit( language: default
                                                  , sourceFilePath: sourceFilePath
                                                  , producer: LazyEncodedString.Empty
                                                  , optimized: false
                                                  , compilationFlags: LazyEncodedString.Empty
                                                  , runtimeVersion: 0
                                                  );

            ValidateConstructedCompileUnit( unit, context, LazyEncodedString.Empty, sourceFilePath, LazyEncodedString.Empty, LazyEncodedString.Empty );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateCompileUnit" )]
        public void CreateCompileUnit_with_valid_compilationFlags_should_not_throw( )
        {
            string sourceFilePath = "test.foo";
            string compilationFlags = "Flag1, Flag2,Flag3";
            string producer = nameof(CreateCompileUnit_with_valid_compilationFlags_should_not_throw);

            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            var unit = diBuilder.CreateCompileUnit( language: default
                                                  , sourceFilePath: sourceFilePath
                                                  , producer: producer
                                                  , optimized: false
                                                  , compilationFlags: compilationFlags
                                                  , runtimeVersion: 0
                                                  );

            ValidateConstructedCompileUnit( unit, context, LazyEncodedString.Empty, sourceFilePath, producer, compilationFlags );
        }
        #endregion

        #region DIBuilder.CreateSubRange
        [TestMethod]
        [TestCategory( "DIBuilder.CreateSubRange" )]
        public void CreateSubRange_create_empty_subrange_should_succeed( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            long lowerBound = 0;
            long count = 0;

            var subRange = diBuilder.CreateSubRange(lowerBound, count);
            Assert.IsNotNull( subRange );
            Assert.AreEqual( context, subRange.Context );
            Assert.IsNull( subRange.VariableCount );
            Assert.IsTrue( subRange.ConstantCount.HasValue );
            Assert.AreEqual( lowerBound, subRange.LowerBound );
            Assert.AreEqual( count, subRange.ConstantCount!.Value );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateSubRange" )]
        public void CreateSubRange_create_subrange_with_nonzero_length_should_succeed( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            long lowerBound = 10;
            long count = 20;

            var subRange = diBuilder.CreateSubRange(lowerBound, count);
            Assert.IsNotNull( subRange );
            Assert.AreEqual( context, subRange.Context );
            Assert.IsNull( subRange.VariableCount );
            Assert.IsTrue( subRange.ConstantCount.HasValue );
            Assert.AreEqual( lowerBound, subRange.LowerBound );
            Assert.AreEqual( count, subRange.ConstantCount!.Value );
        }
        #endregion

        #region DIBuilder.CreateTempMacroFile
        [TestMethod]
        [TestCategory( "DIBuilder.CreateTempMacroFile" )]
        public void CreateTempMacroFile_with_nulls_succeeds( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            DIMacroFile? parent = null;
            uint line = 0;
            DIFile? file = null;

            var result = diBuilder.CreateTempMacroFile( parent
                                                      , line
                                                      , file
                                                      );

            Assert.IsNotNull( result );
            Assert.AreEqual( context, result.Context );
            Assert.AreEqual( 0, result.Elements.Count );
            Assert.IsNull( result.File );
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateTempMacroFile" )]
        public void CreateTempMacroFile_with_nonnull_parent_succeeds( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            DIMacroFile parent = diBuilder.CreateTempMacroFile( null
                                                              , 1
                                                              , null
                                                              );
            uint line = 0;
            DIFile? file = null;

            var result = diBuilder.CreateTempMacroFile( parent
                                                      , line
                                                      , file
                                                      );

            Assert.IsNotNull( result );
            Assert.AreEqual( MetadataKind.DIMacroFile, result.Kind );
            Assert.AreEqual( context, result.Context );
            Assert.AreEqual( 0, result.Elements.Count );
            Assert.IsNull( result.File );
            /* LLVM doesn't provide a way to access the parent, child relationship of a DIMacroFile (even at the C++ level) */
        }

        [TestMethod]
        [TestCategory( "DIBuilder.CreateTempMacroFile" )]
        public void CreateTempMacroFile_with_nonnull_parent_and_file_succeeds( )
        {
            using var context =new Context( );
            using var testModule = context.CreateBitcodeModule( "test" );
            using var diBuilder = new DIBuilder(testModule);

            DIMacroFile parent = diBuilder.CreateTempMacroFile( null
                                                              , 1
                                                              , null
                                                              );
            DIFile file = diBuilder.CreateFile( "test.inc" );

            uint line = 123;

            var result = diBuilder.CreateTempMacroFile( parent
                                                      , line
                                                      , file
                                                      );

            Assert.IsNotNull( result );
            Assert.AreEqual( context, result.Context );
            Assert.AreEqual( MetadataKind.DIMacroFile, result.Kind );
            Assert.AreEqual( 0, result.Elements.Count );
            Assert.AreEqual( file, result.File );
            /* LLVM doesn't provide a way to access the parent : child relationship of a DIMacroFile (even at the C++ level) */
        }
        #endregion

#if AUTO_GENERATED_NOT_YET_READY
        [TestMethod]
        public void CreateMacro_StateUnderTest_ExpectedBehavior( )
        {
            DIMacroFile parentFile = null;
            uint line = 0;
            MacroKind kind = default;
            string name = null;
            string value = null;

            var result = DiBuilder.CreateMacro( parentFile
                                              , line
                                              , kind
                                              , name
                                              , value
                                              );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateNamespace_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            bool exportSymbols = false;

            var result = DiBuilder.CreateNamespace( scope
                                                  , name
                                                  , exportSymbols
                                                  );

            Assert.Inconclusive( );
        }


        [TestMethod]
        public void CreateLexicalBlock_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            DIFile file = null;
            uint line = 0;
            uint column = 0;

            var result = DiBuilder.CreateLexicalBlock( scope
                                                     , file
                                                     , line
                                                     , column
                                                     );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateLexicalBlockFile_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            DIFile file = null;
            uint discriminator = 0;

            var result = DiBuilder.CreateLexicalBlockFile( scope
                                                         , file
                                                         , discriminator
                                                         );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateFunction_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            string mangledName = null;
            DIFile file = null;
            uint line = 0;
            DISubroutineType signatureType = null;
            bool isLocalToUnit = false;
            bool isDefinition = false;
            uint scopeLine = 0;
            DebugInfoFlags debugFlags = default;
            bool isOptimized = false;
            IrFunction function = null;

            var result = DiBuilder.CreateFunction( scope
                                                 , name
                                                 , mangledName
                                                 , file
                                                 , line
                                                 , signatureType
                                                 , isLocalToUnit
                                                 , isDefinition
                                                 , scopeLine
                                                 , debugFlags
                                                 , isOptimized
                                                 , function
                                                 );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void ForwardDeclareFunction_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            string mangledName = null;
            DIFile file = null;
            uint line = 0;
            DISubroutineType subroutineType = null;
            bool isLocalToUnit = false;
            bool isDefinition = false;
            uint scopeLine = 0;
            DebugInfoFlags debugFlags = default;
            bool isOptimized = false;

            var result = DiBuilder.ForwardDeclareFunction( scope
                                                         , name
                                                         , mangledName
                                                         , file
                                                         , line
                                                         , subroutineType
                                                         , isLocalToUnit
                                                         , isDefinition
                                                         , scopeLine
                                                         , debugFlags
                                                         , isOptimized
                                                         );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateLocalVariable_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            DIType type = null;
            bool alwaysPreserve = false;
            DebugInfoFlags debugFlags = default;
            uint alignInBits = 0;

            var result = DiBuilder.CreateLocalVariable( scope
                                                      , name
                                                      , file
                                                      , line
                                                      , type
                                                      , alwaysPreserve
                                                      , debugFlags
                                                      , alignInBits
                                                      );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateArgument_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            DIType type = null;
            bool alwaysPreserve = false;
            DebugInfoFlags debugFlags = default;
            ushort argNo = 0;

            var result = DiBuilder.CreateArgument( scope
                                                 , name
                                                 , file
                                                 , line
                                                 , type
                                                 , alwaysPreserve
                                                 , debugFlags
                                                 , argNo
                                                 );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateBasicType_StateUnderTest_ExpectedBehavior( )
        {
            string name = null;
            UInt64 bitSize = 0;
            DiTypeKind encoding = default;
            DebugInfoFlags diFlags = default;

            var result = DiBuilder.CreateBasicType( name
                                                  , bitSize
                                                  , encoding
                                                  , diFlags
                                                  );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreatePointerType_StateUnderTest_ExpectedBehavior( )
        {
            DIType pointeeType = null;
            string name = null;
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            uint addressSpace = 0;

            var result = DiBuilder.CreatePointerType( pointeeType
                                                    , name
                                                    , bitSize
                                                    , bitAlign
                                                    , addressSpace
                                                    );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateQualifiedType_StateUnderTest_ExpectedBehavior( )
        {
            DIType baseType = null;
            QualifiedTypeTag tag = default;

            var result = DiBuilder.CreateQualifiedType( baseType, tag );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateTypeArray_StateUnderTest_ExpectedBehavior( )
        {
            DIType[ ] types = null;

            var result = DiBuilder.CreateTypeArray( types );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateTypeArray_StateUnderTest_ExpectedBehavior1( )
        {
            IEnumerable<DIType> types = null;

            var result = DiBuilder.CreateTypeArray( types );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateSubroutineType_StateUnderTest_ExpectedBehavior( )
        {
            DebugInfoFlags debugFlags = default;
            DIType[ ] types = null;

            var result = DiBuilder.CreateSubroutineType( debugFlags, types );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateSubroutineType_StateUnderTest_ExpectedBehavior1( )
        {
            DebugInfoFlags debugFlags = default;
            IEnumerable<DIType> types = null;

            var result = DiBuilder.CreateSubroutineType( debugFlags, types );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateSubroutineType_StateUnderTest_ExpectedBehavior2( )
        {
            DebugInfoFlags debugFlags = default;

            var result = DiBuilder.CreateSubroutineType( debugFlags );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateSubroutineType_StateUnderTest_ExpectedBehavior3( )
        {
            DebugInfoFlags debugFlags = default;
            DIType returnType = null;
            IEnumerable<DIType> types = null;

            var result = DiBuilder.CreateSubroutineType( debugFlags, returnType, types );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateStructType_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DebugInfoFlags debugFlags = default;
            DIType derivedFrom = null;
            DINode[ ] elements = null;

            var result = DiBuilder.CreateStructType( scope
                                                   , name
                                                   , file
                                                   , line
                                                   , bitSize
                                                   , bitAlign
                                                   , debugFlags
                                                   , derivedFrom
                                                   , elements
                                                   );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateStructType_StateUnderTest_ExpectedBehavior1( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DebugInfoFlags debugFlags = default;
            DIType derivedFrom = null;
            IEnumerable<DINode> elements = null;
            uint runTimeLang = 0;
            DIType vTableHolder = null;
            string uniqueId = null;

            var result = DiBuilder.CreateStructType( scope
                                                   , name
                                                   , file
                                                   , line
                                                   , bitSize
                                                   , bitAlign
                                                   , debugFlags
                                                   , derivedFrom
                                                   , elements
                                                   , runTimeLang
                                                   , vTableHolder
                                                   , uniqueId
                                                   );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateUnionType_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DebugInfoFlags debugFlags = default;
            DINodeArray elements = null;

            var result = DiBuilder.CreateUnionType( scope
                                                  , name
                                                  , file
                                                  , line
                                                  , bitSize
                                                  , bitAlign
                                                  , debugFlags
                                                  , elements
                                                  );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateUnionType_StateUnderTest_ExpectedBehavior1( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DebugInfoFlags debugFlags = default;
            DINode[ ] elements = null;

            var result = DiBuilder.CreateUnionType( scope
                                                  , name
                                                  , file
                                                  , line
                                                  , bitSize
                                                  , bitAlign
                                                  , debugFlags
                                                  , elements
                                                  );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateUnionType_StateUnderTest_ExpectedBehavior2( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DebugInfoFlags debugFlags = default;
            IEnumerable<DINode> elements = null;
            uint runTimeLang = 0;
            string uniqueId = null;

            var result = DiBuilder.CreateUnionType(
                scope,
                name,
                file,
                line,
                bitSize,
                bitAlign,
                debugFlags,
                elements,
                runTimeLang,
                uniqueId );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateMemberType_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            UInt64 bitOffset = 0;
            DebugInfoFlags debugFlags = default;
            DIType type = null;

            var result = DiBuilder.CreateMemberType(
                scope,
                name,
                file,
                line,
                bitSize,
                bitAlign,
                bitOffset,
                debugFlags,
                type );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateArrayType_StateUnderTest_ExpectedBehavior( )
        {
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DIType elementType = null;
            DINodeArray subscripts = null;

            var result = DiBuilder.CreateArrayType(
                bitSize,
                bitAlign,
                elementType,
                subscripts );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateArrayType_StateUnderTest_ExpectedBehavior1( )
        {
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DIType elementType = null;
            DINode[ ] subscripts = null;

            var result = DiBuilder.CreateArrayType(
                bitSize,
                bitAlign,
                elementType,
                subscripts );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateArrayType_StateUnderTest_ExpectedBehavior2( )
        {
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DIType elementType = null;
            IEnumerable<DINode> subscripts = null;

            var result = DiBuilder.CreateArrayType(
                bitSize,
                bitAlign,
                elementType,
                subscripts );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateVectorType_StateUnderTest_ExpectedBehavior( )
        {
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DIType elementType = null;
            DINodeArray subscripts = null;

            var result = DiBuilder.CreateVectorType(
                bitSize,
                bitAlign,
                elementType,
                subscripts );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateVectorType_StateUnderTest_ExpectedBehavior1( )
        {
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DIType elementType = null;
            DINode[ ] subscripts = null;

            var result = DiBuilder.CreateVectorType(
                bitSize,
                bitAlign,
                elementType,
                subscripts );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateVectorType_StateUnderTest_ExpectedBehavior2( )
        {
            UInt64 bitSize = 0;
            UInt32 bitAlign = 0;
            DIType elementType = null;
            IEnumerable<DINode> subscripts = null;

            var result = DiBuilder.CreateVectorType(
                bitSize,
                bitAlign,
                elementType,
                subscripts );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateTypedef_StateUnderTest_ExpectedBehavior( )
        {
            DIType type = null;
            string name = null;
            DIFile file = null;
            uint line = 0;
            DINode context = null;
            UInt32 alignInBits = 0;

            var result = DiBuilder.CreateTypedef(
                type,
                name,
                file,
                line,
                context,
                alignInBits );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateSubRange_StateUnderTest_ExpectedBehavior( )
        {
            long lowerBounds = 0;
            long count = 0;

            var result = DiBuilder.CreateSubRange(
                lowerBounds,
                count );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void GetOrCreateArray_StateUnderTest_ExpectedBehavior( )
        {
            IEnumerable<DINode> elements = null;

            var result = DiBuilder.GetOrCreateArray(
                elements );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void GetOrCreateTypeArray_StateUnderTest_ExpectedBehavior( )
        {
            DIType[ ] types = null;

            var result = DiBuilder.GetOrCreateTypeArray(
                types );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void GetOrCreateTypeArray_StateUnderTest_ExpectedBehavior1( )
        {
            IEnumerable<DIType> types = null;

            var result = DiBuilder.GetOrCreateTypeArray(
                types );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateEnumeratorValue_StateUnderTest_ExpectedBehavior( )
        {
            string name = null;
            long value = 0;
            bool isUnsigned = false;

            var result = DiBuilder.CreateEnumeratorValue(
                name,
                value,
                isUnsigned );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateEnumerationType_StateUnderTest_ExpectedBehavior( )
        {
            DIScope scope = null;
            string name = null;
            DIFile file = null;
            uint lineNumber = 0;
            UInt64 sizeInBits = 0;
            UInt32 alignInBits = 0;
            IEnumerable<DIEnumerator> elements = null;
            DIType underlyingType = null;

            var result = DiBuilder.CreateEnumerationType(
                scope,
                name,
                file,
                lineNumber,
                sizeInBits,
                alignInBits,
                elements,
                underlyingType );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateGlobalVariableExpression_StateUnderTest_ExpectedBehavior( )
        {
            DINode scope = null;
            string name = null;
            string linkageName = null;
            DIFile file = null;
            uint lineNo = 0;
            DIType type = null;
            bool isLocalToUnit = false;
            DIExpression value = null;
            DINode declaration = null;
            UInt32 bitAlign = 0;

            var result = DiBuilder.CreateGlobalVariableExpression(
                scope,
                name,
                linkageName,
                file,
                lineNo,
                type,
                isLocalToUnit,
                value,
                declaration,
                bitAlign );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void Finish_StateUnderTest_ExpectedBehavior( )
        {
            DISubProgram subProgram = null;

            DiBuilder.Finish(
                subProgram );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void Finish_StateUnderTest_ExpectedBehavior1( )
        {
            DiBuilder.Finish( );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void InsertDeclare_StateUnderTest_ExpectedBehavior( )
        {
            Value storage = null;
            DILocalVariable varInfo = null;
            DILocation location = null;
            Instruction insertBefore = null;

            var result = DiBuilder.InsertDeclare(
                storage,
                varInfo,
                location,
                insertBefore );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void InsertDeclare_StateUnderTest_ExpectedBehavior1( )
        {
            Value storage = null;
            DILocalVariable varInfo = null;
            DIExpression expression = null;
            DILocation location = null;
            Instruction insertBefore = null;

            var result = DiBuilder.InsertDeclare(
                storage,
                varInfo,
                expression,
                location,
                insertBefore );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void InsertDeclare_StateUnderTest_ExpectedBehavior2( )
        {
            Value storage = null;
            DILocalVariable varInfo = null;
            DILocation location = null;
            BasicBlock insertAtEnd = null;

            var result = DiBuilder.InsertDeclare(
                storage,
                varInfo,
                location,
                insertAtEnd );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void InsertDeclare_StateUnderTest_ExpectedBehavior3( )
        {
            Value storage = null;
            DILocalVariable varInfo = null;
            DIExpression expression = null;
            DILocation location = null;
            BasicBlock insertAtEnd = null;

            var result = DiBuilder.InsertDeclare(
                storage,
                varInfo,
                expression,
                location,
                insertAtEnd );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void InsertValue_StateUnderTest_ExpectedBehavior( )
        {
            Value value = null;
            DILocalVariable varInfo = null;
            DILocation location = null;
            Instruction insertBefore = null;

            var result = DiBuilder.InsertValue(
                value,
                varInfo,
                location,
                insertBefore );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void InsertValue_StateUnderTest_ExpectedBehavior1( )
        {
            Value value = null;
            DILocalVariable varInfo = null;
            DIExpression expression = null;
            DILocation location = null;
            Instruction insertBefore = null;

            var result = DiBuilder.InsertValue(
                value,
                varInfo,
                expression,
                location,
                insertBefore );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void InsertValue_StateUnderTest_ExpectedBehavior2( )
        {
            Value value = null;
            DILocalVariable varInfo = null;
            DILocation location = null;
            BasicBlock insertAtEnd = null;

            var result = DiBuilder.InsertValue(
                value,
                varInfo,
                location,
                insertAtEnd );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void InsertValue_StateUnderTest_ExpectedBehavior3( )
        {
            Value value = null;
            DILocalVariable varInfo = null;
            DIExpression expression = null;
            DILocation location = null;
            BasicBlock insertAtEnd = null;

            var result = DiBuilder.InsertValue(
                value,
                varInfo,
                expression,
                location,
                insertAtEnd );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateExpression_StateUnderTest_ExpectedBehavior( )
        {
            ExpressionOp[ ] operations = null;

            var result = DiBuilder.CreateExpression(
                operations );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateExpression_StateUnderTest_ExpectedBehavior1( )
        {
            IEnumerable<ExpressionOp> operations = null;

            var result = DiBuilder.CreateExpression( operations );

            Assert.Inconclusive( );
        }

        [TestMethod]
        public void CreateReplaceableCompositeType_StateUnderTest_ExpectedBehavior( )
        {
            Tag tag = default;
            string name = null;
            DIScope scope = null;
            DIFile file = null;
            uint line = 0;
            uint lang = 0;
            UInt64 sizeInBits = 0;
            UInt32 alignBits = 0;
            DebugInfoFlags flags = default;
            string uniqueId = null;

            var result = DiBuilder.CreateReplaceableCompositeType(
                tag,
                name,
                scope,
                file,
                line,
                lang,
                sizeInBits,
                alignBits,
                flags,
                uniqueId );

            Assert.Inconclusive( );
        }
#endif
        private static void ValidateConstructedCompileUnit( DICompileUnit unit
                                                          , IContext context
                                                          , LazyEncodedString? directory
                                                          , LazyEncodedString? fileName
                                                          , LazyEncodedString? producer = null
                                                          , LazyEncodedString? compilationFlags = null
                                                          , LazyEncodedString? splitDebugFileName = null
                                                          )
        {
            Assert.IsNotNull( unit );
            Assert.AreEqual( Tag.CompileUnit, unit.Tag );
            Assert.AreEqual( context, unit.Context, "Expect matching context" );

            // direct string properties that are set with 'null' should 'get' as LazyEncodedString.Empty
            // The raw operand remains as null [see checks below].
            Assert.AreEqual( compilationFlags ?? LazyEncodedString.Empty, unit.Flags, "Expect matching compilation flags" );
            Assert.AreEqual( producer ?? LazyEncodedString.Empty, unit.Producer, "Expect matching producer" );
            Assert.AreEqual( splitDebugFileName ?? LazyEncodedString.Empty, unit.SplitDebugFileName, "Expect matching SplitDebugFileName" );

            Assert.IsNotNull( unit.EnumTypes, "Expect non-null EnumType" );
            Assert.AreEqual( 0, unit.EnumTypes.Count );

            Assert.IsNotNull( unit.GlobalVariables );
            Assert.AreEqual( 0, unit.GlobalVariables.Count );

            Assert.IsNotNull( unit.ImportedEntities );
            Assert.AreEqual( 0, unit.ImportedEntities.Count );

            Assert.IsNotNull( unit.Macros );
            Assert.AreEqual( 0, unit.Macros.Count );

            Assert.IsNotNull( unit.Operands );

            // validate Operand enumeration works
            // operands
            // 0 - DIFile
            // 1 - Producer
            // 2 - Flags
            // 3 - SplitDebugFileName
            // 4 - EnumTypes
            // 5 - RetainedTypes
            // 6 - GlobalVariables
            // 7 - ImportedEntities
            // 8 - Macros
            // 9 - ????
            // 10 - ????
            const int expectedOperandCount = 11;

            Assert.AreEqual( expectedOperandCount, unit.Operands.Count, "CompileUnit should have correct number of operands" );
            var operandList = unit.Operands.ToList();
            Assert.AreEqual( expectedOperandCount, operandList.Count, "Enumerating CompileUnit.Operands should have correct number of elements" );
            Assert.IsNotNull( operandList[ 0 ], "Expect DIFile operand not null" );

            ValidateMetadataStringOperand( nameof( producer ), producer, operandList[ 1 ] );
            ValidateMetadataStringOperand( nameof( compilationFlags ), compilationFlags, operandList[ 2 ] );
            ValidateMetadataStringOperand( nameof( splitDebugFileName ), splitDebugFileName, operandList[ 3 ] );

            for(int opIndex = 4; opIndex < unit.Operands.Count; ++opIndex)
            {
                Assert.IsNull( unit.Operands[ opIndex ], $"Expected Operand[{opIndex}]==null" );
            }

            Assert.IsFalse( unit.IsDeleted, "CompileUnit should not be deleted" );
            Assert.IsTrue( unit.IsDistinct, "CompileUnit should be a distinct node" );
            Assert.IsTrue( unit.IsResolved, "CompileUnit should not have any unresolved operands" );
            Assert.IsFalse( unit.IsTemporary, "CompileUnit should not be a temporary node" );
            Assert.IsFalse( unit.IsUniqued, "CompileUnit should not be uniqued" );
            /*NOT YET EXPOSED AS A PROPERTY: Assert.AreEqual( 0, unit.RuntimeVersion ); */
            /*NOT YET EXPOSED AS A PROPERTY: Assert.IsFalse( unit.IsOptimized ); */

            // validate the file property
            Assert.IsNotNull( unit.File );
            ValidateDIFileProperties( context, unit.File!, directory ?? LazyEncodedString.Empty, fileName ?? LazyEncodedString.Empty );
        }

        private static void ValidateMetadataStringOperand( LazyEncodedString operandName, LazyEncodedString? expected, IrMetadata? md )
        {
            // use IsNullOrEmpty here (NOT IsNullOrWhitespace or just 'is null' as LLVM is supposed
            // to transform an empty string to a null operand.
            expected = LazyEncodedString.IsNullOrEmpty( expected ) ? null : expected;
            bool isMdString = false;
            LazyEncodedString? actual = null;
            if(md is MDString mdstring)
            {
                isMdString = true;
                actual = mdstring.ToString();
            }

            Assert.IsTrue( isMdString || expected == null, $"Expect an MDString unless the expected value is null for {operandName}" );
            Assert.AreEqual( expected, actual, $"Expect matching operand value for {operandName}" );
        }

        private static void ValidateDIFileProperties( IContext context
                                                    , DIFile file
                                                    , LazyEncodedString expectedDirectory
                                                    , LazyEncodedString expecteFileName
                                                    , LazyEncodedString? expectedCheckSum = null
                                                    , LazyEncodedString? expectedSourceText = null
                                                    )
        {
            /* NOTE: string properties return LazyEncodedString.Empty instead of null
            |  however, direct retrieval of operands returns null
            */

            string sourceFilePath = Path.Combine(expectedDirectory, expecteFileName);
            Assert.AreEqual( expectedCheckSum ?? LazyEncodedString.Empty, file.CheckSum );
            /* NOT YET AVAILABLE: Assert.AreEqual( expectedChecksumKind, file.ChecksumKind */
            Assert.AreEqual( context, file.Context );
            Assert.AreEqual( expectedDirectory, file.Directory );
            Assert.AreEqual( file, file.File );
            Assert.AreEqual( expecteFileName, file.FileName );
            Assert.IsFalse( file.IsDeleted );
            Assert.IsFalse( file.IsDistinct );
            Assert.IsTrue( file.IsResolved, "Should not have unresolved elements" );
            Assert.IsFalse( file.IsTemporary );
            Assert.IsTrue( file.IsUniqued, "DIFile should be uniqued" );
            Assert.AreEqual( MetadataKind.DIFile, file.Kind );
            Assert.AreEqual( LazyEncodedString.Empty, file.Name );
            Assert.IsNotNull( file.Operands );

            // validate Operands
            // operands
            // 0 - FileName[MDSrting?]
            // 1 - Directory[MDString?]
            // 2 - Checksum[MDString?] {NOTE: checksum algo type - is NOT stored in the operands, even though it exists in the binary IR}
            // 3 - SourceText[MDString?]
            Assert.AreEqual( 4, file.Operands.Count, "DIFile should have correct number of operands" );
            var operandList = file.Operands.ToList();
            Assert.AreEqual( file.Operands.Count, operandList.Count, "Enumerating DIFile.Operands should have correct number of elements" );
            ValidateMetadataStringOperand( "FileName", expecteFileName, operandList[ 0 ] );
            ValidateMetadataStringOperand( "Directory", expectedDirectory, operandList[ 1 ] );
            ValidateMetadataStringOperand( "CheckSum", expectedCheckSum, operandList[ 2 ] );
            ValidateMetadataStringOperand( "SourceText", expectedSourceText, operandList[ 3 ] );

            Assert.AreEqual( sourceFilePath, file.Path );
            Assert.IsNull( file.Scope );
            Assert.AreEqual( expectedSourceText ?? LazyEncodedString.Empty, file.Source );
            Assert.AreEqual( Tag.FileType, file.Tag );
        }
    }
}
