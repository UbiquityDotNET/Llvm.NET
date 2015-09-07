using System;
using System.Collections.Generic;
using System.IO;
using Llvm.NET;
using Llvm.NET.DebugInfo;
using Llvm.NET.Instructions;
using Llvm.NET.Types;
using Llvm.NET.Values;

namespace TestDebugInfo
{
    /// <summary>Program to test/demonstrate Aspects of debug information generation with Llvm.NET</summary>
    class Program
    {
        const string Triple = "x86_64-pc-windows-msvc18.0.0"; //"thumbv7m-none-eabi";
        const string Cpu = "x86-64";//"cortex-m3";
        const string Features = "+sse,+sse2";
        static readonly Dictionary<string, string> TargetDependentAttributes = new Dictionary<string, string>
        {
            [ "disable-tail-calls" ] = "false",
            [ "less-precise-fpmad" ] = "false",
            [ "no-frame-pointer-elim" ] = "false",
            [ "no-infs-fp-math" ] = "false",
            [ "no-nans-fp-math" ] = "false",
            [ "stack-protector-buffer-size" ] = "8",
            [ "target-cpu" ] = "x86-64",
            [ "target-features" ] = "+sse,+sse2",
            [ "unsafe-fp-math" ] = "false",
            [ "use-soft-float" ] = "false",
        };

        /// <summary>Creates a test LLVM module with debug information</summary>
        /// <param name="args">ignored</param>
        /// <remarks>
        /// <code language="C++" title="Example code generated">
        /// struct foo                                        // 1
        /// {                                                 // 2
        ///     int a;                                        // 3
        ///     float b;                                      // 4
        ///     int c[2];                                     // 5
        /// };                                                // 6
        ///                                                   // 7
        /// struct foo bar = { 1, 2.0, { 3, 4 } };            // 8
        /// struct foo baz;                                   // 9
        ///                                                   // 10
        /// void copy( struct foo src, struct foo* pDst )     // 11
        /// {                                                 // 12
        ///     *pDst = src;                                  // 13
        /// }                                                 // 14
        ///                                                   // 15
        /// void DoCopy( )                                    // 16
        /// {                                                 // 17
        ///     copy( bar, &amp;baz );                        // 18
        /// }                                                 // 19
        /// </code>
        /// </remarks>
        static void Main( string[ ] args )
        {
            using( var context = Context.CreateThreadContext( ) )
            {
                StaticState.RegisterAll( );
                var target = Target.FromTriple( Triple );
                using( var targetMachine = target.CreateTargetMachine( Triple, Cpu, Features, CodeGenOpt.Aggressive, Reloc.Default, CodeModel.Small ) )
                using( var module = context.CreateModule( "test.bc" ) )
                {
                    var targetData = targetMachine.TargetData;

                    module.TargetTriple = targetMachine.Triple;
                    module.DataLayout = targetMachine.TargetData.ToString( );

                    // create compile unit and file as the top level scope for everything
                    var srcPath = @"C:\Github\NETMF\Llvm.NET\src\Llvm.NETTests\TestDebugInfo\test.c";
                    var cu = module.DIBuilder.CreateCompileUnit( SourceLanguage.C99
                                                               , Path.GetFileName( srcPath )
                                                               , Path.GetDirectoryName( srcPath )
                                                               , "TestDebugInfo"
                                                               , false
                                                               , ""
                                                               , 0
                                                               );
                    var diFile = module.DIBuilder.CreateFile( srcPath );

                    // Create basic types used in this compilation
                    context.Int32Type.CreateDIType( module.DIBuilder, targetData, "int", cu );
                    context.FloatType.CreateDIType( module.DIBuilder, targetData, "float", cu );
                    var i32 = context.Int32Type;
                    var f32 = context.FloatType;
                    var i32Array_0_2 = i32.CreateArrayType( module.DIBuilder, targetData, 0, 2 );

                    // create the LLVM structure type and body
                    // The full body is used with targetMachine.TargetData to determine size, alignment and element offsets
                    // in a target independent manner.
                    var fooType = context.CreateStructType( "struct.foo" );
                    fooType.CreateDIType( module.DIBuilder, targetData, "foo", cu );
                    fooType.SetBody( false, context.Int32Type, context.FloatType, i32Array_0_2 );

                    // add global variables and constants
                    var barValue = context.CreateNamedConstantStruct( fooType
                                                                    , ConstantInt.From( 1 )
                                                                    , ConstantFP.From( 2.0f )
                                                                    , ConstantArray.From( i32, ConstantInt.From( 3 ), ConstantInt.From( 4 ) )
                                                                    );

                    var bar = module.AddGlobal( fooType, false, 0, barValue, "bar" );
                    bar.Alignment = targetData.AbiAlignmentOf( fooType );
                    module.DIBuilder.CreateGlobalVariable( cu, "bar", string.Empty, diFile, 8, fooType.DIType, false, bar );

                    var baz = module.AddGlobal( fooType, false, Linkage.Common, Constant.NullValueFor( fooType ), "baz" );
                    baz.Alignment = targetData.AbiAlignmentOf( fooType );
                    module.DIBuilder.CreateGlobalVariable( cu, "baz", string.Empty, diFile, 9, fooType.DIType, false, baz );

                    // add module flags and ident
                    // this can technically occur at any point, though placing it here makes
                    // comparing against clang generated files simpler
                    module.AddModuleFlag( ModuleFlagBehavior.Warning, Module.DwarfVersionValue, 4 );
                    module.AddModuleFlag( ModuleFlagBehavior.Warning, Module.DebugVersionValue, Module.DebugMetadataVersion );
                    module.AddModuleFlag( ModuleFlagBehavior.Error, "PIC Level", 2 );
                    module.AddVersionIdentMetadata( "testdebuginfo 0.0.1-test" );

                    // create types for function args
                    var fooPtr = fooType.CreatePointerType( module.DIBuilder, targetData );
                    // WARNING: This would generate the wrong debug info signature!
                    //          Since the the first parameter is passed by value 
                    //          using the pointer+alloca+memcopy pattern, the actual
                    //          source, and therefore debug, signature is NOT a pointer.
                    //          However, this usage will create a signature with two
                    //          pointers as the arguments and thus the correct approach
                    //          is to insert an explicit ParameterTypePair that overrides
                    //          the default behavior to pair LLVM pointer type with the
                    //          original source type.
                    //var copySig = context.CreateFunctionType( module.DIBuilder, diFile, context.VoidType, fooPtr, fooType.DIType, fooPtr );
                    var copySig = context.CreateFunctionType( module.DIBuilder, diFile, context.VoidType, new ParameterTypePair( fooPtr, fooType.DIType ), fooPtr );
                    var doCopySig = context.CreateFunctionType( module.DIBuilder, diFile, context.VoidType );

                    // Create the functions
                    // NOTE: The ordering is reveresd from that of the sample code file (test.c)
                    //       However this is what Clang ends up doing for some reason so it is
                    //       replicated here to aid in comparing the generated LL files.
                    var doCopyFunc = module.CreateFunction( scope: diFile
                                                          , name: "DoCopy"
                                                          , linkageName: null
                                                          , file: diFile
                                                          , line: 18
                                                          , signature: doCopySig
                                                          , isLocalToUnit: false
                                                          , isDefinition: true
                                                          , scopeLine: 19
                                                          , flags: 0
                                                          , isOptimized: false
                                                          ).AddAttributes( Attributes.NoUnwind | Attributes.UnwindTable )
                                                           .AddAttributes( TargetDependentAttributes );

                    var copyFunc = module.CreateFunction( scope: diFile
                                                         , name: "copy"
                                                         , linkageName: null
                                                         , file: diFile
                                                         , line: 11
                                                         , signature: copySig
                                                         , isLocalToUnit: true
                                                         , isDefinition: true
                                                         , scopeLine: 14
                                                         , flags: DebugInfoFlags.Prototyped
                                                         , isOptimized: false
                                                         ).AddAttributes( Attributes.NoUnwind | Attributes.UnwindTable | Attributes.InlineHint )
                                                          .AddAttributes( TargetDependentAttributes )
                                                          .Linkage( Linkage.Internal ); // static function

                    CreateDoCopyFunctionBody( module, targetData, doCopyFunc, fooType, bar, baz, copyFunc );
                    CreateCopyFunctionBody( module, targetData, copyFunc, diFile, fooType, fooPtr );
                    finalizeFooDebugInfo( module.DIBuilder, targetData, cu, diFile, i32, i32Array_0_2, f32, fooType );

                    // finalize the debug information
                    module.DIBuilder.Finish( );

                    // verify the module is still good and print any errors found
                    string msg;
                    if( !module.Verify( out msg ) )
                    {
                        Console.Error.WriteLine( "ERROR: {0}", msg );
                    }
                    else
                    {
                        // Module is good, so generate the output files
                        module.WriteToFile( "test.bc" );
                        var lltxt = module.AsString( );
                        System.IO.File.WriteAllText( "test.ll", lltxt );
                        targetMachine.EmitToFile( module, "test.o", CodeGenFileType.ObjectFile );
                        targetMachine.EmitToFile( module, "test.s", CodeGenFileType.AssemblySource );
                    }
                }
            }
        }

        private static void finalizeFooDebugInfo( DebugInfoBuilder diBuilder
                                                , TargetData layout
                                                , DICompileUnit cu
                                                , DIFile diFile
                                                , TypeRef i32
                                                , TypeRef i32Array_0_2
                                                , TypeRef f32
                                                , StructType foo
                                                )
        {
            // Create concrete DIType and RAUW of the opaque one with the complete version
            // While this two phase approach isn't strictly necessary in this sample it
            // isn't an uncommon case in the real world so this example demonstrates how
            // to use forward type decalarations and replace them with a complete type when
            // all of the type information is available
            var diFields = new DIType[ ]
                { diBuilder.CreateMemberType( scope: foo.DIType
                                            , name: "a"
                                            , file: diFile
                                            , line: 3
                                            , bitSize: layout.BitSizeOf( i32 )
                                            , bitAlign: layout.AbiBitAlignmentOf( i32 )
                                            , bitOffset: layout.BitOffsetOfElement( foo, 0 )
                                            , flags: 0
                                            , type: i32.DIType
                                            )
                , diBuilder.CreateMemberType( scope: foo.DIType
                                            , name: "b"
                                            , file: diFile
                                            , line: 4
                                            , bitSize: layout.BitSizeOf( f32 )
                                            , bitAlign: layout.AbiBitAlignmentOf( f32 )
                                            , bitOffset: layout.BitOffsetOfElement( foo, 1 )
                                            , flags: 0
                                            , type: f32.DIType
                                            )
                , diBuilder.CreateMemberType( scope: foo.DIType
                                            , name: "c"
                                            , file: diFile
                                            , line: 5
                                            , bitSize: layout.BitSizeOf( i32Array_0_2 )
                                            , bitAlign: layout.AbiBitAlignmentOf( i32Array_0_2 )
                                            , bitOffset: layout.BitOffsetOfElement( foo, 2 )
                                            , flags: 0
                                            , type: i32Array_0_2.DIType
                                            )
                };
            var fooConcrete = diBuilder.CreateStructType( scope: cu
                                                        , name: "foo"
                                                        , file: diFile
                                                        , line: 1
                                                        , bitSize: layout.BitSizeOf( foo )
                                                        , bitAlign: layout.AbiBitAlignmentOf( foo )
                                                        , flags: 0
                                                        , derivedFrom: null
                                                        , elements: diFields );
            foo.ReplaceAllUsesOfDebugTypeWith( fooConcrete );
        }

        private static void CreateCopyFunctionBody( Module module
                                                  , TargetData layout
                                                  , Function copyFunc
                                                  , DIFile diFile
                                                  , StructType foo
                                                  , PointerType fooPtr
                                                  )
        {
            var diBuilder = module.DIBuilder;

            // ByVal pointers indicate by value semantics. LLVM recognizes this pattern and has a pass to map
            // to an efficient register usage whenever plausible.
            // Though it seems unnecessary as Clang doesn't apply the attribute...
            //copyFunc.Parameters[ 0 ].AddAttributes( Attributes.ByVal );
            //copyFunc.Parameters[ 0 ].SetAlignment( foo.AbiAlignment );
            copyFunc.Parameters[ 0 ].Name = "src";
            copyFunc.Parameters[ 1 ].Name = "pDst";

            // create block for the function body, only need one for this simple sample
            var blk = copyFunc.AppendBasicBlock( "entry" );

            // create instruction builder to build the body
            var instBuilder = new InstructionBuilder( blk );

            // create debug info locals for the arguments
            // NOTE: Debug parameter indeces are 1 based!
            var paramSrc = diBuilder.CreateArgument( copyFunc.DISubProgram, "src", diFile, 11, foo.DIType, false, 0, 1 );
            var paramDst = diBuilder.CreateArgument( copyFunc.DISubProgram, "pDst", diFile, 12, fooPtr.DIType, false, 0, 2 );

            var ptrAlign = layout.CallFrameAlignmentOf( fooPtr );
            
            // create Locals
            // NOTE: There's no debug location attatched to these instructions.
            //       The debug info will come from the declare instrinsic below.
            var dstAddr = instBuilder.Alloca( fooPtr, "pDst.addr" )
                                     .Alignment( ptrAlign );

            var dstStore = instBuilder.Store( copyFunc.Parameters[ 1 ], dstAddr)
                                      .Alignment( ptrAlign );

            // insert declare pseudo instruction to attach debug info to the local declarations
            var dstDeclare = diBuilder.InsertDeclare( dstAddr, paramDst, new DILocation( 12, 38, copyFunc.DISubProgram ), blk );

            // since the function's LLVM signature uses a pointer, which is copied locally
            // inform the debugger to treat it as the value by dereferencing the pointer
            var srcDeclare = diBuilder.InsertDeclare( copyFunc.Parameters[ 0 ]
                                                           , paramSrc
                                                           , diBuilder.CreateExpression( ExpressionOp.deref )
                                                           , new DILocation( 11, 37, copyFunc.DISubProgram )
                                                           , blk
                                                           );

            var loadedDst = instBuilder.Load( dstAddr )
                                       .Alignment( ptrAlign )
                                       .SetDebugLocation( 15, 6, copyFunc.DISubProgram );

            var dstPtr = instBuilder.BitCast( loadedDst, module.Context.Int8Type.CreatePointerType( ) )
                                    .SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            var srcPtr = instBuilder.BitCast( copyFunc.Parameters[ 0 ], module.Context.Int8Type.CreatePointerType( ) )
                                    .SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            var memCpy = instBuilder.MemCpy( module
                                           , dstPtr
                                           , srcPtr
                                           , ConstantInt.From( layout.ByteSizeOf( foo ) )
                                           , ( int )layout.AbiAlignmentOf( foo )
                                           , false
                                           ).SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            var ret = instBuilder.Return( )
                                 .SetDebugLocation( 16, 1, copyFunc.DISubProgram );
        }

        private static void CreateDoCopyFunctionBody( Module module
                                                    , TargetData layout
                                                    , Function doCopyFunc
                                                    , StructType foo
                                                    , GlobalVariable bar
                                                    , GlobalVariable baz
                                                    , Function copyFunc
                                                    )
        {
            var diBuilder = module.DIBuilder;

            var bytePtrType = module.Context.Int8Type.CreatePointerType( );

            // create block for the function body, only need one for this simple sample
            var blk = doCopyFunc.AppendBasicBlock( "entry" );

            // create instruction builder to build the body
            var instBuilder = new InstructionBuilder( blk );

            // create a temp local copy of the global structure
            var dstAddr = instBuilder.Alloca( foo, "agg.tmp" )
                                     .Alignment( layout.CallFrameAlignmentOf( foo ) );

            var bitCastDst = instBuilder.BitCast( dstAddr, bytePtrType )
                                        .SetDebugLocation( 20, 11, doCopyFunc.DISubProgram );

            var bitCastSrc = instBuilder.BitCast( bar, bytePtrType )
                                        .SetDebugLocation( 20, 11, doCopyFunc.DISubProgram );

            var memCpy = instBuilder.MemCpy( module
                                           , bitCastDst
                                           , bitCastSrc
                                           , ConstantInt.From( layout.ByteSizeOf( foo ) )
                                           , ( int )layout.CallFrameAlignmentOf( foo )
                                           , false
                                           ).SetDebugLocation( 20, 11, doCopyFunc.DISubProgram );

            var callCopy = instBuilder.Call( copyFunc, dstAddr, baz )
                                      .SetDebugLocation( 20, 5, doCopyFunc.DISubProgram );

            var ret =instBuilder.Return( )
                                .SetDebugLocation( 21, 1, doCopyFunc.DISubProgram );
        }
    }
}
