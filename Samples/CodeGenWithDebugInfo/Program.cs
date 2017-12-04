// <copyright file="Program.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Llvm.NET;
using Llvm.NET.DebugInfo;
using Llvm.NET.Instructions;
using Llvm.NET.Transforms;
using Llvm.NET.Types;
using Llvm.NET.Values;

using static Llvm.NET.StaticState;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

namespace TestDebugInfo
{
    /// <summary>Program to test/demonstrate Aspects of debug information generation with Llvm.NET</summary>
    public static class Program
    {
        /// <summary>Creates a test LLVM module with debug information</summary>
        /// <param name="args">ignored</param>
        /// <remarks>
        /// <code language="c" title="Example code generated" source="test.c" />
        /// </remarks>
        public static void Main( string[ ] args )
        {
            #region Commandline Arguments
            if( args.Length != 2 )
            {
                ShowUsage( );
                return;
            }

            string srcPath = args[ 1 ];
            if( !File.Exists( srcPath ) )
            {
                Console.Error.WriteLine( "Src file not found: '{0}'", srcPath );
                return;
            }

            srcPath = Path.GetFullPath( srcPath );
            #endregion

            using( InitializeLLVM() )
            {
                // <TargetDetailsSelection>
                switch( args[ 0 ] )
                {
                case "M3":
                    TargetDetails = new CortexM3Details( );
                    break;

                case "X64":
                    TargetDetails = new X64Details( );
                    break;

                default:
                    ShowUsage( );
                    return;
                }

                string moduleName = $"test_{TargetDetails.ShortName}.bc";
                // </TargetDetailsSelection>

                // <CreatingModule>
                using( var context = new Context( ) )
                using( var module = new BitcodeModule( context, moduleName ) )
                {
                    module.SourceFileName = Path.GetFileName( srcPath );
                    module.TargetTriple = TargetDetails.TargetMachine.Triple;
                    module.Layout = TargetDetails.TargetMachine.TargetData;

                    TargetDependentAttributes = TargetDetails.BuildTargetDependentFunctionAttributes( context );
                    // </CreatingModule>

                    // <CreatingCompileUnit>
                    // create compile unit and file as the top level scope for everything
                    var cu = module.DIBuilder.CreateCompileUnit( SourceLanguage.C99
                                                               , srcPath
                                                               , VersionIdentString
                                                               , optimized: false
                                                               , compilationFlags: string.Empty
                                                               , runtimeVersion: 0
                                                               );
                    var diFile = module.DIBuilder.CreateFile( srcPath );
                    // </CreatingCompileUnit>

                    // <CreatingBasicTypesWithDebugInfo>
                    // Create basic types used in this compilation
                    var i32 = new DebugBasicType( module.Context.Int32Type, module, "int", DiTypeKind.Signed );
                    var f32 = new DebugBasicType( module.Context.FloatType, module, "float", DiTypeKind.Float );
                    var voidType = DebugType.Create( module.Context.VoidType, ( DIType )null );
                    var i32Array_0_32 = i32.CreateArrayType( module, 0, 32 );
                    // </CreatingBasicTypesWithDebugInfo>

#pragma warning disable SA1500 // "Warning SA1500  Braces for multi - line statements must not share line" (simple table format)
                    // <CreatingStructureTypes>
                    // create the LLVM structure type and body with full debug information
                    var fooBody = new[ ]
                        { new DebugMemberInfo { File = diFile, Line = 3, Name = "a", DebugType = i32, Index = 0 }
                        , new DebugMemberInfo { File = diFile, Line = 4, Name = "b", DebugType = f32, Index = 1 }
                        , new DebugMemberInfo { File = diFile, Line = 5, Name = "c", DebugType = i32Array_0_32, Index = 2 }
                        };

                    var fooType = new DebugStructType( module, "struct.foo", cu, "foo", diFile, 1, DebugInfoFlags.None, fooBody );
                    // </CreatingStructureTypes>
#pragma warning restore
                    // <CreatingGlobalsAndMetadata>
                    // add global variables and constants
                    var constArray = ConstantArray.From( i32, 32, module.Context.CreateConstant( 3 ), module.Context.CreateConstant( 4 ) );
                    var barValue = module.Context.CreateNamedConstantStruct( fooType
                                                                           , module.Context.CreateConstant( 1 )
                                                                           , module.Context.CreateConstant( 2.0f )
                                                                           , constArray
                                                                           );

                    var bar = module.AddGlobal( fooType, false, 0, barValue, "bar" );
                    bar.Alignment = module.Layout.AbiAlignmentOf( fooType );
                    bar.AddDebugInfo( module.DIBuilder.CreateGlobalVariableExpression( cu, "bar", string.Empty, diFile, 8, fooType.DIType, false, null ) );

                    var baz = module.AddGlobal( fooType, false, Linkage.Common, Constant.NullValueFor( fooType ), "baz" );
                    baz.Alignment = module.Layout.AbiAlignmentOf( fooType );
                    baz.AddDebugInfo( module.DIBuilder.CreateGlobalVariableExpression( cu, "baz", string.Empty, diFile, 9, fooType.DIType, false, null ) );

                    // add module flags and compiler identifiers...
                    // this can technically occur at any point, though placing it here makes
                    // comparing against clang generated files easier
                    AddModuleFlags( module );
                    // </CreatingGlobalsAndMetadata>

                    // <CreatingQualifiedTypes>
                    // create types for function args
                    var constFoo = module.DIBuilder.CreateQualifiedType( fooType.DIType, QualifiedTypeTag.Const );
                    var fooPtr = new DebugPointerType( fooType, module );
                    // </CreatingQualifiedTypes>

                    // Create the functions
                    // NOTE: The declaration ordering is reversed from that of the sample code file (test.c)
                    //       However, this is what Clang ends up doing for some reason so it is
                    //       replicated here to aid in comparing the generated LL files.
                    Function doCopyFunc = DeclareDoCopyFunc( module, diFile, voidType );
                    Function copyFunc = DeclareCopyFunc( module, diFile, voidType, constFoo, fooPtr );

                    CreateCopyFunctionBody( module, copyFunc, diFile, fooType, fooPtr, constFoo );
                    CreateDoCopyFunctionBody( module, doCopyFunc, fooType, bar, baz, copyFunc );

                    // finalize the debug information
                    // all temporaries must be replaced by now, this resolves any remaining
                    // forward declarations and marks the builder to prevent adding any
                    // nodes that are not completely resolved.
                    module.DIBuilder.Finish( );

                    // verify the module is still good and print any errors found
                    if( !module.Verify( out string msg ) )
                    {
                        Console.Error.WriteLine( "ERROR: {0}", msg );
                    }
                    else
                    {
                        // test optimization works, but don't save it as that makes it harder to do a diff with official clang builds
                        {// force a GC to verify callback delegate for diagnostics is still valid, this is for test only and wouldn't
                         // normally be done in production code.
                            GC.Collect( GC.MaxGeneration );
                            var modForOpt = module.Clone( );

                            // NOTE:
                            // The ordering of passes can matter depending on the pass and passes may be added more than once
                            // the caller has full control of ordering, this is just a sample of effectively randomly picked
                            // passes and not necessarily a reflection of any particular use case.
                            var pm = new ModulePassManager( )
                                     .AddAlwaysInlinerPass( )
                                     .AddAggressiveDCEPass( )
                                     .AddArgumentPromotionPass( )
                                     .AddBasicAliasAnalysisPass( )
                                     .AddBitTrackingDCEPass( )
                                     .AddCFGSimplificationPass( )
                                     .AddConstantMergePass( )
                                     .AddConstantPropagationPass( )
                                     .AddFunctionInliningPass( )
                                     .AddGlobalOptimizerPass( )
                                     .AddInstructionCombiningPass( );

                            pm.Run( modForOpt );
                        }

                        // Module is good, so generate the output files
                        module.WriteToFile( "test.bc" );
                        File.WriteAllText( "test.ll", module.WriteToString( ) );
                        TargetDetails.TargetMachine.EmitToFile( module, "test.o", CodeGenFileType.ObjectFile );
                        TargetDetails.TargetMachine.EmitToFile( module, "test.s", CodeGenFileType.AssemblySource );
                    }
                }
            }
        }

        private static void ShowUsage( )
        {
            Console.Error.WriteLine( "Usage: {0} [X64|M3] <source file path>", Path.GetFileName( System.Reflection.Assembly.GetExecutingAssembly( ).CodeBase ) );
        }

        // <FunctionDeclarations>
        private static Function DeclareDoCopyFunc( BitcodeModule module, DIFile diFile, IDebugType<ITypeRef, DIType> voidType )
        {
            var doCopySig = module.Context.CreateFunctionType( module.DIBuilder, voidType );

            var doCopyFunc = module.CreateFunction( scope: diFile
                                                  , name: "DoCopy"
                                                  , linkageName: null
                                                  , file: diFile
                                                  , line: 23
                                                  , signature: doCopySig
                                                  , isLocalToUnit: false
                                                  , isDefinition: true
                                                  , scopeLine: 24
                                                  , debugFlags: DebugInfoFlags.None
                                                  , isOptimized: false
                                                  ).AddAttributes( FunctionAttributeIndex.Function, AttributeKind.NoInline, AttributeKind.NoUnwind, AttributeKind.OptimizeNone )
                                                   .AddAttributes( FunctionAttributeIndex.Function, TargetDependentAttributes );
            return doCopyFunc;
        }

        private static Function DeclareCopyFunc( BitcodeModule module
                                               , DIFile diFile
                                               , IDebugType<ITypeRef, DIType> voidType
                                               , DIDerivedType constFoo
                                               , DebugPointerType fooPtr
                                               )
        {
            // Since the first parameter is passed by value
            // using the pointer + alloca + memcopy pattern, the actual
            // source, and therefore debug, signature is NOT a pointer.
            // However, that usage would create a signature with two
            // pointers as the arguments, which doesn't match the source
            // To get the correct debug info signature this inserts an
            // explicit DebugType<> that overrides the default behavior
            // to pair the LLVM pointer type with the original source type.
            var copySig = module.Context.CreateFunctionType( module.DIBuilder
                                                           , voidType
                                                           , DebugType.Create( fooPtr, constFoo )
                                                           , fooPtr
                                                           );

            var copyFunc = module.CreateFunction( scope: diFile
                                                , name: "copy"
                                                , linkageName: null
                                                , file: diFile
                                                , line: 11
                                                , signature: copySig
                                                , isLocalToUnit: true
                                                , isDefinition: true
                                                , scopeLine: 14
                                                , debugFlags: DebugInfoFlags.Prototyped
                                                , isOptimized: false
                                                ).Linkage( Linkage.Internal ) // static function
                                                 .AddAttributes( FunctionAttributeIndex.Function, AttributeKind.NoUnwind, AttributeKind.NoInline, AttributeKind.OptimizeNone )
                                                 .AddAttributes( FunctionAttributeIndex.Function, TargetDependentAttributes );

            TargetDetails.AddABIAttributesForByValueStructure( copyFunc, 0 );
            return copyFunc;
        }
        // </FunctionDeclarations>

        // <AddModuleFlags>
        private static void AddModuleFlags( BitcodeModule module )
        {
            module.AddModuleFlag( ModuleFlagBehavior.Warning, BitcodeModule.DwarfVersionValue, 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Warning, BitcodeModule.DebugVersionValue, BitcodeModule.DebugMetadataVersion );
            TargetDetails.AddModuleFlags( module );
            module.AddVersionIdentMetadata( VersionIdentString );
        }
        // </AddModuleFlags>

        private static void CreateCopyFunctionBody( BitcodeModule module
                                                  , Function copyFunc
                                                  , DIFile diFile
                                                  , ITypeRef foo
                                                  , DebugPointerType fooPtr
                                                  , DIType constFooType
                                                  )
        {
            var diBuilder = module.DIBuilder;

            copyFunc.Parameters[ 0 ].Name = "src";
            copyFunc.Parameters[ 1 ].Name = "pDst";

            // create block for the function body, only need one for this simple sample
            var blk = copyFunc.AppendBasicBlock( "entry" );

            // create instruction builder to build the body
            var instBuilder = new InstructionBuilder( blk );

            // create debug info locals for the arguments
            // NOTE: Debug parameter indices are 1 based!
            var paramSrc = diBuilder.CreateArgument( copyFunc.DISubProgram, "src", diFile, 11, constFooType, false, 0, 1 );
            var paramDst = diBuilder.CreateArgument( copyFunc.DISubProgram, "pDst", diFile, 12, fooPtr.DIType, false, 0, 2 );

            uint ptrAlign = module.Layout.CallFrameAlignmentOf( fooPtr );

            // create Locals
            // NOTE: There's no debug location attached to these instructions.
            //       The debug info will come from the declare intrinsic below.
            var dstAddr = instBuilder.Alloca( fooPtr )
                                     .RegisterName( "pDst.addr" )
                                     .Alignment( ptrAlign );

            bool param0ByVal = copyFunc.Attributes[ FunctionAttributeIndex.Parameter0 ].Contains( AttributeKind.ByVal );
            if( param0ByVal )
            {
                diBuilder.InsertDeclare( copyFunc.Parameters[ 0 ]
                                       , paramSrc
                                       , new DILocation( module.Context, 11, 43, copyFunc.DISubProgram )
                                       , blk
                                       );
            }

            instBuilder.Store( copyFunc.Parameters[ 1 ], dstAddr )
                       .Alignment( ptrAlign );

            // insert declare pseudo instruction to attach debug info to the local declarations
            diBuilder.InsertDeclare( dstAddr, paramDst, new DILocation( module.Context, 12, 38, copyFunc.DISubProgram ), blk );

            if( !param0ByVal )
            {
                // since the function's LLVM signature uses a pointer, which is copied locally
                // inform the debugger to treat it as the value by dereferencing the pointer
                diBuilder.InsertDeclare( copyFunc.Parameters[ 0 ]
                                       , paramSrc
                                       , diBuilder.CreateExpression( ExpressionOp.deref )
                                       , new DILocation( module.Context, 11, 43, copyFunc.DISubProgram )
                                       , blk
                                       );
            }

            var loadedDst = instBuilder.Load( dstAddr )
                                       .Alignment( ptrAlign )
                                       .SetDebugLocation( 15, 6, copyFunc.DISubProgram );

            var dstPtr = instBuilder.BitCast( loadedDst, module.Context.Int8Type.CreatePointerType( ) )
                                    .SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            var srcPtr = instBuilder.BitCast( copyFunc.Parameters[ 0 ], module.Context.Int8Type.CreatePointerType( ) )
                                    .SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            uint pointerSize = module.Layout.IntPtrType( module.Context ).IntegerBitWidth;
            instBuilder.MemCpy( module
                              , dstPtr
                              , srcPtr
                              , module.Context.CreateConstant( pointerSize, module.Layout.ByteSizeOf( foo ), false )
                              , ( int )module.Layout.AbiAlignmentOf( foo )
                              , false
                              ).SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            instBuilder.Return( )
                       .SetDebugLocation( 16, 1, copyFunc.DISubProgram );
        }

        private static void CreateDoCopyFunctionBody( BitcodeModule module
                                                    , Function doCopyFunc
                                                    , IStructType foo
                                                    , GlobalVariable bar
                                                    , GlobalVariable baz
                                                    , Function copyFunc
                                                    )
        {
            var bytePtrType = module.Context.Int8Type.CreatePointerType( );

            // create block for the function body, only need one for this simple sample
            var blk = doCopyFunc.AppendBasicBlock( "entry" );

            // create instruction builder to build the body
            var instBuilder = new InstructionBuilder( blk );

            bool param0ByVal = copyFunc.Attributes[ FunctionAttributeIndex.Parameter0 ].Contains( AttributeKind.ByVal );
            if( !param0ByVal )
            {
                // create a temp local copy of the global structure
                var dstAddr = instBuilder.Alloca( foo )
                                         .RegisterName( "agg.tmp" )
                                         .Alignment( module.Layout.CallFrameAlignmentOf( foo ) );

                var bitCastDst = instBuilder.BitCast( dstAddr, bytePtrType )
                                            .SetDebugLocation( 25, 11, doCopyFunc.DISubProgram );

                var bitCastSrc = instBuilder.BitCast( bar, bytePtrType )
                                            .SetDebugLocation( 25, 11, doCopyFunc.DISubProgram );

                instBuilder.MemCpy( module
                                  , bitCastDst
                                  , bitCastSrc
                                  , module.Context.CreateConstant( module.Layout.ByteSizeOf( foo ) )
                                  , ( int )module.Layout.CallFrameAlignmentOf( foo )
                                  , false
                                  ).SetDebugLocation( 25, 11, doCopyFunc.DISubProgram );

                instBuilder.Call( copyFunc, dstAddr, baz )
                           .SetDebugLocation( 25, 5, doCopyFunc.DISubProgram );
            }
            else
            {
                instBuilder.Call( copyFunc, bar, baz )
                           .SetDebugLocation( 25, 5, doCopyFunc.DISubProgram )
                           .AddAttributes( FunctionAttributeIndex.Parameter0, copyFunc.Parameters[0].Attributes );
            }

            instBuilder.Return( )
                       .SetDebugLocation( 26, 1, doCopyFunc.DISubProgram );
        }

        // obviously this is not clang but using an identical name helps in diff with actual clang output
        private const string VersionIdentString = "clang version 5.0.0 (tags/RELEASE_500/rc4)";

        private static ITargetDependentDetails TargetDetails;

        private static IEnumerable<AttributeValue> TargetDependentAttributes { get; set; }
    }
}
