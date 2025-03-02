// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Ubiquity.NET.Llvm;
using Ubiquity.NET.Llvm.DebugInfo;
using Ubiquity.NET.Llvm.Instructions;

using Ubiquity.NET.Llvm.Types;
using Ubiquity.NET.Llvm.Values;

using static Ubiquity.NET.Llvm.Interop.Library;

[assembly: SuppressMessage( "StyleCop.CSharp.DocumentationRules", "SA1652:Enable XML documentation output", Justification = "Sample application" )]

#pragma warning disable SA1512, SA1513, SA1515 // single line comments used to tag regions for extraction into docs
#pragma warning disable CA1506 // warning CA1506: 'Main' is coupled with '51' different types from '12' different namespaces. Rewrite or refactor the code to decrease its class coupling below '41'.

namespace CodeGenWithDebugInfo
{
    /// <summary>Program to test/demonstrate Aspects of debug information generation with Ubiquity.NET.Llvm</summary>
    public static class Program
    {
        /// <summary>Creates a test LLVM module with debug information</summary>
        /// <param name="args">ignored</param>
        /// <remarks>
        /// <code language="c" title="Example code generated" source="test.c" />
        /// </remarks>
        [SuppressMessage( "Design", "CA1062:Validate arguments of public methods", Justification = "Provided by platform" )]
        public static void Main( string[ ] args )
        {
            #region CommandlineArguments
            if( args.Length < 2 || args.Length > 3 )
            {
                ShowUsage( );
                return;
            }

            string outputPath = args.Length == 3 ? args[2] : Environment.CurrentDirectory;

            string srcPath = args[ 1 ];
            if( !File.Exists( srcPath ) )
            {
                Console.Error.WriteLine( "Src file not found: '{0}'", srcPath );
                return;
            }

            srcPath = Path.GetFullPath( srcPath );
            #endregion

            using var libLLVM = InitializeLLVM( );

            #region TargetDetailsSelection
            switch( args[ 0 ].ToUpperInvariant( ) )
            {
            case "M3":
                TargetDetails = new CortexM3Details( libLLVM );
                break;

            case "X64":
                TargetDetails = new X64Details( libLLVM );
                break;

            default:
                ShowUsage( );
                return;
            }

            string moduleName = $"test_{TargetDetails.ShortName}.bc";
            #endregion

            #region CreatingModule
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( moduleName, SourceLanguage.C99, srcPath, VersionIdentString );
            module.SourceFileName = Path.GetFileName( srcPath );
            module.TargetTriple = TargetDetails.TargetMachine.Triple;
            module.Layout = TargetDetails.TargetMachine.TargetData;
            Debug.Assert( module.DICompileUnit is not null, "Expected module with non-null compile unit" );

            TargetDependentAttributes = TargetDetails.BuildTargetDependentFunctionAttributes( context );
            #endregion

            var diFile = module.DIBuilder.CreateFile( srcPath );

            #region CreatingBasicTypesWithDebugInfo
            // Create basic types used in this compilation
            var i32 = new DebugBasicType( module.Context.Int32Type, module, "int", DiTypeKind.Signed );
            var f32 = new DebugBasicType( module.Context.FloatType, module, "float", DiTypeKind.Float );
            var voidType = DebugType.Create( module.Context.VoidType, (DIType?)null );
            var i32Array_0_32 = i32.CreateArrayType( module, 0, 32 );
            #endregion

            #region CreatingStructureTypes
            // create the LLVM structure type and body with full debug information
            var fooBody = new[ ]
                {
                    new DebugMemberInfo( 0, "a", diFile, 3, i32 ),
                    new DebugMemberInfo( 1, "b", diFile, 4, f32 ),
                    new DebugMemberInfo( 2, "c", diFile, 5, i32Array_0_32 ),
                };

            var fooType = new DebugStructType( module, "struct.foo", module.DICompileUnit, "foo", diFile, 1, DebugInfoFlags.None, fooBody );
            #endregion

            #region CreatingGlobalsAndMetadata
            // add global variables and constants
            var constArray = ConstantArray.From( i32, 32, module.Context.CreateConstant( 3 ), module.Context.CreateConstant( 4 ) );
            var barValue = module.Context.CreateNamedConstantStruct( fooType
                                                                    , module.Context.CreateConstant( 1 )
                                                                    , module.Context.CreateConstant( 2.0f )
                                                                    , constArray
                                                                    );

            var bar = module.AddGlobal( fooType, false, 0, barValue, "bar" );
            bar.Alignment = module.Layout.AbiAlignmentOf( fooType );
            bar.AddDebugInfo( module.DIBuilder.CreateGlobalVariableExpression( module.DICompileUnit, "bar", string.Empty, diFile, 8, fooType.DebugInfoType, false, null ) );

            var baz = module.AddGlobal( fooType, false, Linkage.Common, Constant.NullValueFor( fooType ), "baz" );
            baz.Alignment = module.Layout.AbiAlignmentOf( fooType );
            baz.AddDebugInfo( module.DIBuilder.CreateGlobalVariableExpression( module.DICompileUnit, "baz", string.Empty, diFile, 9, fooType.DebugInfoType, false, null ) );

            // add module flags and compiler identifiers...
            // this can technically occur at any point, though placing it here makes
            // comparing against clang generated files easier
            AddModuleFlags( module );
            #endregion

            #region CreatingQualifiedTypes
            // create types for function args
            var constFoo = module.DIBuilder.CreateQualifiedType( fooType.DebugInfoType, QualifiedTypeTag.Const );
            var fooPtr = new DebugPointerType( fooType, module );
            #endregion

            // Create the functions
            // NOTE: The declaration ordering is reversed from that of the sample code file (test.c)
            //       However, this is what Clang ends up doing for some reason so it is
            //       replicated here to aid in comparing the generated LL files.
            IrFunction doCopyFunc = DeclareDoCopyFunc( module, diFile, voidType );
            IrFunction copyFunc = DeclareCopyFunc( module, diFile, voidType, constFoo, fooPtr );

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
                // Module is good, so generate the output files
                module.WriteToFile( Path.Combine( outputPath, "test.bc" ) );
                File.WriteAllText( Path.Combine( outputPath, "test.ll" ), module.WriteToString( ) );
                TargetDetails.TargetMachine.EmitToFile( module, Path.Combine( outputPath, "test.o" ), CodeGenFileKind.ObjectFile );
                TargetDetails.TargetMachine.EmitToFile( module, Path.Combine( outputPath, "test.s" ), CodeGenFileKind.AssemblySource );
                Console.WriteLine( "Generated test.bc, test.ll, test.o, and test.s" );
            }
        }

        private static void ShowUsage( )
        {
            Console.Error.WriteLine( "Usage: {0} [X64|M3] <source file path>", Path.GetFileName( System.Reflection.Assembly.GetExecutingAssembly( ).Location ) );
        }

        #region FunctionDeclarations
        private static IrFunction DeclareDoCopyFunc( BitcodeModule module, DIFile diFile, IDebugType<ITypeRef, DIType> voidType )
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

        private static IrFunction DeclareCopyFunc( BitcodeModule module
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

            Debug.Assert( !fooPtr.IsOpaquePtr(), "Expected the debug info for a pointer was created with a valid ElementType");
            TargetDetails.AddABIAttributesForByValueStructure( copyFunc, 0 );
            return copyFunc;
        }
        #endregion

        #region AddModuleFlags
        private static void AddModuleFlags( BitcodeModule module )
        {
            module.AddModuleFlag( ModuleFlagBehavior.Warning, BitcodeModule.DwarfVersionValue, 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Warning, BitcodeModule.DebugVersionValue, BitcodeModule.DebugMetadataVersion );
            TargetDetails.AddModuleFlags( module );
            module.AddVersionIdentMetadata( VersionIdentString );
        }
        #endregion

        private static void CreateCopyFunctionBody( BitcodeModule module
                                                  , IrFunction copyFunc
                                                  , DIFile diFile
                                                  , ITypeRef foo
                                                  , DebugPointerType fooPtr
                                                  , DIType constFooType
                                                  )
        {
            Debug.Assert( copyFunc.DISubProgram != null, "Expected function with a valid debug subprogram" );
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
            var paramDst = diBuilder.CreateArgument( copyFunc.DISubProgram, "pDst", diFile, 12, fooPtr.DebugInfoType, false, 0, 2 );

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
                                       , diBuilder.CreateExpression( ExpressionOp.Deref )
                                       , new DILocation( module.Context, 11, 43, copyFunc.DISubProgram )
                                       , blk
                                       );
            }

            var loadedDst = instBuilder.SetDebugLocation( 15, 6, copyFunc.DISubProgram )
                                       .Load( fooPtr, dstAddr )
                                       .Alignment( ptrAlign );

            instBuilder.SetDebugLocation( 15, 13, copyFunc.DISubProgram );
            var dstPtr = instBuilder.BitCast( loadedDst, module.Context.Int8Type.CreatePointerType( ) );
            var srcPtr = instBuilder.BitCast( copyFunc.Parameters[ 0 ], module.Context.Int8Type.CreatePointerType( ) );

            uint pointerSize = module.Layout.IntPtrType( module.Context ).IntegerBitWidth;
            instBuilder.MemCpy( dstPtr
                              , srcPtr
                              , module.Context.CreateConstant( pointerSize, module.Layout.ByteSizeOf( foo ), false )
                              , false
                              );
            instBuilder.SetDebugLocation( 16, 1, copyFunc.DISubProgram )
                       .Return( );
        }

        private static void CreateDoCopyFunctionBody( BitcodeModule module
                                                    , IrFunction doCopyFunc
                                                    , IStructType foo
                                                    , GlobalVariable bar
                                                    , GlobalVariable baz
                                                    , IrFunction copyFunc
                                                    )
        {
            var bytePtrType = module.Context.Int8Type.CreatePointerType( );

            // create block for the function body, only need one for this simple sample
            var blk = doCopyFunc.AppendBasicBlock( "entry" );
            Debug.Assert( doCopyFunc.DISubProgram != null, "Expected non null subProgram" );

            // create instruction builder to build the body
            var instBuilder = new InstructionBuilder( blk );

            bool param0ByVal = copyFunc.Attributes[ FunctionAttributeIndex.Parameter0 ].Contains( AttributeKind.ByVal );
            if( !param0ByVal )
            {
                // create a temp local copy of the global structure
                var dstAddr = instBuilder.Alloca( foo )
                                         .RegisterName( "agg.tmp" )
                                         .Alignment( module.Layout.CallFrameAlignmentOf( foo ) );

                instBuilder.SetDebugLocation( 25, 11, doCopyFunc.DISubProgram );
                var bitCastDst = instBuilder.BitCast( dstAddr, bytePtrType );
                var bitCastSrc = instBuilder.BitCast( bar, bytePtrType );

                instBuilder.MemCpy( bitCastDst
                                  , bitCastSrc
                                  , module.Context.CreateConstant( module.Layout.ByteSizeOf( foo ) )
                                  , false
                                  );

                instBuilder.SetDebugLocation( 25, 5, doCopyFunc.DISubProgram )
                           .Call( copyFunc, dstAddr, baz );
            }
            else
            {
                instBuilder.SetDebugLocation( 25, 5, doCopyFunc.DISubProgram )
                           .Call( copyFunc, bar, baz )
                           .AddAttributes( FunctionAttributeIndex.Parameter0, copyFunc.Parameters[ 0 ].Attributes );
            }

            instBuilder.SetDebugLocation( 26, 1, doCopyFunc.DISubProgram )
                       .Return( );
        }

        // obviously this is not clang but using an identical name helps in comparisons with actual clang output
        private const string VersionIdentString = "clang version 5.0.0 (tags/RELEASE_500/rc4)";

        // these fields are initialized in main before being used elsewhere
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private static ITargetDependentDetails TargetDetails;

        private static IEnumerable<AttributeValue> TargetDependentAttributes { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
