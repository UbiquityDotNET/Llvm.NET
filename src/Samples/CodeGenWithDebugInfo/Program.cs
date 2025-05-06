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
            if(!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            string srcPath = args[ 1 ];
            if( !File.Exists( srcPath ) )
            {
                Console.Error.WriteLine( "Src file not found: '{0}'", srcPath );
                return;
            }

            srcPath = Path.GetFullPath( srcPath );
            #endregion

            #region TargetABISelection
            using var targetABI = AbiFactory(args[0]);
            if (targetABI is null)
            {
                ShowUsage( );
                return;
            }

            string moduleName = $"test_{targetABI.ShortName}.bc";
            #endregion

            #region CreatingModule
            using var context = new Context( );
            using var module = context.CreateBitcodeModule( moduleName );
            using var diBuilder = new DIBuilder(module);
            DICompileUnit compilationUnit = diBuilder.CreateCompileUnit(SourceLanguage.C99, srcPath, VersionIdentString);
            module.SourceFileName = Path.GetFileName( srcPath );
            using var targetMachine = targetABI.CreateTargetMachine();
            module.TargetTriple = targetMachine.Triple;
            using var layout = targetMachine.CreateTargetData();
            module.Layout = layout;

            var abiAttributes = targetABI.BuildTargetDependentFunctionAttributes( context );
            #endregion

            Debug.Assert(compilationUnit.File is not null, "File was set in creation, should NOT be null");
            DIFile diFile = compilationUnit.File;

            #region CreatingBasicTypesWithDebugInfo
            // Create basic types used in this compilation
            var i32 = new DebugBasicType( module.Context.Int32Type, in diBuilder, "int", DiTypeKind.Signed );
            var f32 = new DebugBasicType( module.Context.FloatType, in diBuilder, "float", DiTypeKind.Float );
            var voidType = DebugType.Create( module.Context.VoidType, (DIType?)null );
            var i32Array_0_32 = i32.CreateArrayType( in diBuilder, 0, 32 );
            #endregion

            #region CreatingStructureTypes
            // create the LLVM structure type and body with full debug information
            var fooBody = new DebugMemberInfo[ ]
                {
                    new( 0, "a", diFile, 3, i32 ),
                    new( 1, "b", diFile, 4, f32 ),
                    new( 2, "c", diFile, 5, i32Array_0_32 ),
                };

            var fooType = new DebugStructType( in diBuilder, "struct.foo", compilationUnit, "foo", diFile, 1, DebugInfoFlags.None, fooBody );
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
            bar.AddDebugInfo( diBuilder.CreateGlobalVariableExpression( compilationUnit, "bar", string.Empty, diFile, 8, fooType.DebugInfoType, false, null ) );

            var baz = module.AddGlobal( fooType, false, Linkage.Common, Constant.NullValueFor( fooType ), "baz" );
            baz.Alignment = module.Layout.AbiAlignmentOf( fooType );
            baz.AddDebugInfo( diBuilder.CreateGlobalVariableExpression( compilationUnit, "baz", string.Empty, diFile, 9, fooType.DebugInfoType, false, null ) );

            // add module flags and compiler identifiers...
            // this can technically occur at any point, though placing it here makes
            // comparing against clang generated files easier
            AddModuleFlags(targetABI, module );
            #endregion

            #region CreatingQualifiedTypes
            // create types for function args
            var constFoo = diBuilder.CreateQualifiedType( fooType.DebugInfoType, QualifiedTypeTag.Const );
            var fooPtr = new DebugPointerType( fooType, in diBuilder );
            #endregion

            // Create the functions
            // NOTE: The declaration ordering is reversed from that of the sample code file (test.c)
            //       However, this is what Clang ends up doing for some reason so it is
            //       replicated here to aid in comparing the generated LL files.
            Function doCopyFunc = DeclareDoCopyFunc( in diBuilder, diFile, voidType, abiAttributes );
            Function copyFunc = DeclareCopyFunc(targetABI, in diBuilder, diFile, voidType, constFoo, fooPtr, abiAttributes );

            CreateCopyFunctionBody( in diBuilder, copyFunc, diFile, fooType, fooPtr, constFoo );
            CreateDoCopyFunctionBody( module, doCopyFunc, fooType, bar, baz, copyFunc );

            // finalize the debug information
            // all temporaries must be replaced by now, this resolves any remaining
            // forward declarations and marks the builder to prevent adding any
            // nodes that are not completely resolved.
            diBuilder.Finish( );

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
                targetMachine.EmitToFile( module, Path.Combine( outputPath, "test.o" ), CodeGenFileKind.ObjectFile );
                targetMachine.EmitToFile( module, Path.Combine( outputPath, "test.s" ), CodeGenFileKind.AssemblySource );
                Console.WriteLine( $"Generated test.bc, test.ll, test.o, and test.s to {outputPath}" );
            }
        }

        private static void ShowUsage( )
        {
            Console.Error.WriteLine( "Usage: CodeGenWithDebugInfo [X64|M3] <source file path>" );
        }

        private static ITargetABI? AbiFactory(string arg)
        {
            return arg.ToUpperInvariant() switch
            {
                "M3" => new CortexM3ABI(),
                "X64" => new X64ABI(),
                _ => null,
            };
        }

        #region FunctionDeclarations
        private static Function DeclareDoCopyFunc(
            ref readonly DIBuilder diBuilder,
            DIFile diFile,
            IDebugType<ITypeRef, DIType> voidType,
            IEnumerable<AttributeValue> abiAttributes
            )
        {
            var module = diBuilder.OwningModule;
            var doCopySig = module.Context.CreateFunctionType( in diBuilder, voidType );

            var doCopyFunc = module.CreateFunction( in diBuilder
                                                  , scope: diFile
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
                                                  ).AddAttributes( FunctionAttributeIndex.Function, "noinline", "nounwind", "optimizenone")
                                                   .AddAttributes( FunctionAttributeIndex.Function, abiAttributes );
            return doCopyFunc;
        }

        private static Function DeclareCopyFunc( ITargetABI abi
                                               , ref readonly DIBuilder diBuilder
                                               , DIFile diFile
                                               , IDebugType<ITypeRef, DIType> voidType
                                               , DIDerivedType constFoo
                                               , DebugPointerType fooPtr
                                               , IEnumerable<AttributeValue> abiAttributes
                                               )
        {
            var module = diBuilder.OwningModule;

            // Since the first parameter is passed by value
            // using the pointer + alloca + memcopy pattern, the actual
            // source, and therefore debug, signature is NOT a pointer.
            // However, that usage would create a signature with two
            // pointers as the arguments, which doesn't match the source
            // To get the correct debug info signature this inserts an
            // explicit DebugType<> that overrides the default behavior
            // to pair the LLVM pointer type with the original source type.
            var copySig = module.Context.CreateFunctionType( in diBuilder
                                                           , voidType
                                                           , DebugType.Create( fooPtr, constFoo )
                                                           , fooPtr
                                                           );

            var copyFunc = module.CreateFunction( in diBuilder
                                                , scope: diFile
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
                                                 .AddAttributes( FunctionAttributeIndex.Function, "nounwind"u8, "noinline"u8, "optimizenone"u8 )
                                                 .AddAttributes( FunctionAttributeIndex.Function, abiAttributes );

            Debug.Assert( !fooPtr.IsOpaque(), "Expected the debug info for a pointer was created with a valid ElementType");
            abi.AddAttributesForByValueStructure( copyFunc, copySig, 0 );
            return copyFunc;
        }
        #endregion

        #region AddModuleFlags
        private static void AddModuleFlags(ITargetABI abi, Module module )
        {
            module.AddModuleFlag( ModuleFlagBehavior.Warning, Module.DwarfVersionValue, 4 );
            module.AddModuleFlag( ModuleFlagBehavior.Warning, Module.DebugVersionValue, Module.DebugMetadataVersion );
            abi.AddModuleFlags( module );
            module.AddVersionIdentMetadata( VersionIdentString );
        }
        #endregion

        private static void CreateCopyFunctionBody( ref readonly DIBuilder diBuilder
                                                  , Function copyFunc
                                                  , DIFile diFile
                                                  , ITypeRef foo
                                                  , DebugPointerType fooPtr
                                                  , DIType constFooType
                                                  )
        {
            Debug.Assert( copyFunc.DISubProgram != null, "Expected function with a valid debug subprogram" );
            IModule module = diBuilder.OwningModule;

            copyFunc.Parameters[ 0 ].Name = "src";
            copyFunc.Parameters[ 1 ].Name = "pDst";

            // create block for the function body, only need one for this simple sample
            var blk = copyFunc.AppendBasicBlock( "entry" );

            // create instruction builder to build the body
            using var instBuilder = new InstructionBuilder( blk );

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
                                     .SetAlignment( ptrAlign );

            bool hasParam0ByVal = copyFunc.FindAttribute(FunctionAttributeIndex.Parameter0, "byval"u8) is not null;
            if( hasParam0ByVal )
            {
                diBuilder.InsertDeclare( copyFunc.Parameters[ 0 ]
                                       , paramSrc
                                       , new DILocation( module.Context, 11, 43, copyFunc.DISubProgram )
                                       , blk
                                       );
            }

            instBuilder.Store( copyFunc.Parameters[ 1 ], dstAddr )
                       .SetAlignment( ptrAlign );

            // insert attach debug record to the local declarations
            diBuilder.InsertDeclare( dstAddr, paramDst, new DILocation( module.Context, 12, 38, copyFunc.DISubProgram ), blk );

            if( !hasParam0ByVal )
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
                                       .SetAlignment( ptrAlign );

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

        private static void CreateDoCopyFunctionBody( Module module
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
            Debug.Assert( doCopyFunc.DISubProgram != null, "Expected non null subProgram" );

            // create instruction builder to build the body
            using var instBuilder = new InstructionBuilder( blk );
            bool hasParam0ByVal = doCopyFunc.FindAttribute(FunctionAttributeIndex.Parameter0, "byval"u8) is not null;
            if( !hasParam0ByVal )
            {
                // create a temp local copy of the global structure
                var dstAddr = instBuilder.Alloca( foo )
                                         .RegisterName( "agg.tmp" )
                                         .SetAlignment( module.Layout.CallFrameAlignmentOf( foo ) );

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

        // obviously this is not clang but using an identical name helps in text comparisons with actual clang output
        private const string VersionIdentString = "clang version 5.0.0 (tags/RELEASE_500/rc4)";
    }
}
