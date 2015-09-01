using System;
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
                    var diBuilder = new DebugInfoBuilder( module );

                    module.TargetTriple = targetMachine.Triple;
                    module.DataLayout = targetMachine.TargetData.ToString( );

                    // create compile unit and file as the top level scope for everything
                    var cu = diBuilder.CreateCompileUnit( SourceLanguage.C99, "test.c", Environment.CurrentDirectory, "TestDebugInfo", false, "", 0 );
                    var diFile = diBuilder.CreateFile( "test.c", Environment.CurrentDirectory );

                    // Create basic types used in this compilation
                    DebugTypeInfo i32 = new DebugTypeInfo( context.Int32Type, diBuilder, targetData, "int", cu );
                    DebugTypeInfo f32 = new DebugTypeInfo( context.FloatType, diBuilder, targetData, "float", cu );
                    DebugTypeInfo voidType = new DebugTypeInfo( context.VoidType, diBuilder, targetData, "void", cu );
                    var i32Array_0_2 = i32.AsArrayType( diBuilder, 0, 2 );

                    // create the LLVM structure type and body
                    // The full body is used with targetMachine.TargetData to determine size, alignment and element offsets
                    // in a target independent manner.
                    DebugTypeInfo foo = new DebugTypeInfo( context.CreateStructType( "struct.foo" ), diBuilder, targetData, "foo", cu );
                    ( ( StructType )foo.LlvmType ).SetBody( false, context.Int32Type, context.FloatType, i32Array_0_2.LlvmType );

                    // add global variables 
                    var barValue = context.CreateNamedConstantStruct( (StructType)foo.LlvmType
                                                                    , ConstantInt.From( 1 )
                                                                    , ConstantFP.From( 2.0f )
                                                                    , ConstantArray.From( i32.LlvmType, ConstantInt.From( 3 ), ConstantInt.From( 4 ) )
                                                                    );
                    
                    var bar = module.AddGlobal( foo.LlvmType, false, 0, barValue, "bar" );
                    bar.Alignment = foo.AbiAlignment;
                    diBuilder.CreateGlobalVariable( cu, "bar", string.Empty, diFile, 8, foo.DebugType, false, bar );

                    var baz = module.AddGlobal( foo.LlvmType, false, Linkage.Common, Constant.NullValueFor( foo.LlvmType ), "baz" );
                    baz.Alignment = foo.AbiAlignment;
                    diBuilder.CreateGlobalVariable( cu, "baz", string.Empty, diFile, 9, foo.DebugType, false, bar );

                    // add module flags and ident
                    // this can technically occur at any point, though placing it here makes
                    // comparing against clang generated files simpler
                    module.AddModuleFlag( ModuleFlagBehavior.Warning, Module.DwarfVersionValue, 4 );
                    module.AddModuleFlag( ModuleFlagBehavior.Warning, Module.DebugVersionValue, Module.DebugMetadataVersion );
                    module.AddModuleFlag( ModuleFlagBehavior.Error, "PIC Level", 2 );
                    module.AddVersionIdentMetadata( "testdebuginfo 0.0.1-test" );

                    // create types for function args
                    var fooPtr = foo.AsPointerType( diBuilder );
                    var copySig = DebugTypeInfo.CreateFunctionType( diBuilder, diFile, voidType, fooPtr, fooPtr );
                    var doCopySig = DebugTypeInfo.CreateFunctionType( diBuilder, diFile, voidType );

                    // Create the functions
                    var copyFunc = CreateCopyFunction( module, diBuilder, diFile, foo, fooPtr, copySig );
                    CreateDoCopyFunction( module, diBuilder, diFile, foo, fooPtr, doCopySig, bar, baz, copyFunc );
                    finalizeFooDebugInfo( diBuilder, cu, diFile, i32, i32Array_0_2, f32, foo );

                    // finalize the debug information
                    diBuilder.Finish( );

                    string msg;
                    if( !module.Verify( out msg ) )
                    {
                        Console.Error.WriteLine( "ERROR: {0}", msg );
                    }
                    else
                    {
                        // generate output files
                        module.WriteToFile( "test.bc" );
                        System.IO.File.WriteAllText( "test.ll", module.AsString( ) );
                        targetMachine.EmitToFile( module, "test.o", CodeGenFileType.ObjectFile );
                        targetMachine.EmitToFile( module, "test.s", CodeGenFileType.AssemblySource );
                    }
                }
            }
        }

        private static void finalizeFooDebugInfo( DebugInfoBuilder diBuilder
                                                , DICompileUnit cu
                                                , DIFile diFile
                                                , DebugTypeInfo i32
                                                , DebugTypeInfo i32Array_0_2
                                                , DebugTypeInfo f32
                                                , DebugTypeInfo foo
                                                )
        {
            // Create concrete DIType and RAUW of the opaque one with the complete version
            // While this two phase approach isn't strictly necessary in this sample it
            // isn't an uncommon case in the real world so this example demonstrates how
            // to use forward decalarations and replace them with a complete type when
            // all of the type information is available
            var diFields = new DIType[ ]
                { diBuilder.CreateMemberType( scope: foo.DebugType
                                            , name: "a"
                                            , file: diFile
                                            , line: 3
                                            , bitSize: i32.BitSize
                                            , bitAlign: i32.AbiBitAlignment
                                            , bitOffset: foo.BitOffsetOfElement( 0 )
                                            , flags: 0
                                            , type: i32.DebugType
                                            )
                , diBuilder.CreateMemberType( scope: foo.DebugType
                                            , name: "b"
                                            , file: diFile
                                            , line: 4
                                            , bitSize: f32.BitSize
                                            , bitAlign: f32.AbiBitAlignment
                                            , bitOffset: foo.BitOffsetOfElement( 1 )
                                            , flags: 0
                                            , type: f32.DebugType
                                            )
                , diBuilder.CreateMemberType( scope: foo.DebugType
                                            , name: "c"
                                            , file: diFile
                                            , line: 5
                                            , bitSize: i32Array_0_2.BitSize
                                            , bitAlign: i32Array_0_2.AbiBitAlignment
                                            , bitOffset: foo.BitOffsetOfElement( 2 )
                                            , flags: 0
                                            , type: i32Array_0_2.DebugType
                                            )
                };
            var fooConcrete = diBuilder.CreateStructType( scope: cu
                                                        , name: "foo"
                                                        , file: diFile
                                                        , line: 1
                                                        , bitSize: foo.BitSize
                                                        , bitAlign: foo.AbiBitAlignment
                                                        , flags: 0
                                                        , derivedFrom: null
                                                        , elements: diFields );
            foo.ReplaceDebugTypeWith( fooConcrete );
        }

        private static Function CreateCopyFunction( Module module
                                                  , DebugInfoBuilder diBuilder
                                                  , DIFile diFile
                                                  , DebugTypeInfo foo
                                                  , DebugTypeInfo fooPtr
                                                  , DebugTypeInfo copySig
                                                  )
        {
            var copyFunc = module.AddFunction( "copy", ( FunctionType )copySig.LlvmType );
            copyFunc.AddAttributes( Attributes.NoUnwind | Attributes.UnwindTable );
            var copyFuncDi = diBuilder.CreateFunction( scope: diFile
                                                     , name: "copy"
                                                     , mangledName: null
                                                     , file: diFile
                                                     , line: 11
                                                     , compositeType: ( DICompositeType )copySig.DebugType
                                                     , isLocalToUnit: false
                                                     , isDefinition: true
                                                     , scopeLine: 12
                                                     , flags: ( uint )DebugInfoFlags.Prototyped
                                                     , isOptimized: false
                                                     , function: copyFunc
                                                     );

            // ByVal pointers indicate by value semantics. LLVM recognizes this pattern and has a pass to map
            // to an efficient register usage whenever plausible.
            // Though it seems unnecessary as CLang doesn't apply the attribute...
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
            var paramSrc = diBuilder.CreateLocalVariable( ( uint )Tag.ArgVariable, copyFuncDi, "src", diFile, 11, foo.DebugType, false, 0, 1 );
            var paramDst = diBuilder.CreateLocalVariable( ( uint )Tag.ArgVariable, copyFuncDi, "pDst", diFile, 11, fooPtr.DebugType, false, 0, 2 );

            // create Locals
            var dstAddr = ( Instruction )instBuilder.Alloca( fooPtr.LlvmType, "pDst.addr" );
            dstAddr.Alignment = fooPtr.CallFrameAlignment;
            var dstStore = ( Instruction )instBuilder.Store( copyFunc.Parameters[ 1 ], dstAddr );
            dstStore.Alignment = fooPtr.CallFrameAlignment;
            
            // insert declare pseudo instruction to attach debug info to the local declarations
            var dstDeclare = ( Instruction )diBuilder.InsertDeclare( dstAddr, paramDst, new DILocation( 11, 40, copyFuncDi ), blk );
            
            // since the function's LLVM signature uses a pointer, which is copied locally
            // inform the debugger to treat it as the value by dereferencing the pointer
            var srcDeclare = ( Instruction )diBuilder.InsertDeclare( copyFunc.Parameters[ 0 ]
                                                                   , paramSrc
                                                                   , diBuilder.CreateExpression( ExpressionOp.deref )
                                                                   , new DILocation( 11, 23, copyFuncDi )
                                                                   , blk
                                                                   );

            var loadedDst = ( Instruction )instBuilder.Load( dstAddr );
            loadedDst.Alignment = fooPtr.CallFrameAlignment;
            loadedDst.SetDebugLocation( 13, 6, copyFuncDi );

            var dstPtr = ( Instruction )instBuilder.BitCast( loadedDst, module.Context.Int8Type.CreatePointerType( ) );
            dstPtr.SetDebugLocation( 13, 13, copyFuncDi );

            var srcPtr = ( Instruction )instBuilder.BitCast( copyFunc.Parameters[ 0 ], module.Context.Int8Type.CreatePointerType( ) );
            srcPtr.SetDebugLocation( 13, 13, copyFuncDi );

            var memCpy = ( Instruction )instBuilder.MemCpy( module, dstPtr, srcPtr, ConstantInt.From( ( int )foo.BitSize / 8 ), ( int )foo.AbiAlignment, false );
            memCpy.SetDebugLocation( 13, 13, copyFuncDi );

            var ret = ( Instruction )instBuilder.Return( );
            ret.SetDebugLocation( 14, 1, copyFuncDi );

            return copyFunc;
        }

        private static void CreateDoCopyFunction( Module module
                                                , DebugInfoBuilder diBuilder
                                                , DIFile diFile
                                                , DebugTypeInfo foo
                                                , DebugTypeInfo fooPtr
                                                , DebugTypeInfo doCopySig
                                                , GlobalVariable bar
                                                , GlobalVariable baz
                                                , Function copyFunc
                                                )
        {
            var doCopyFunc = module.AddFunction( "DoCopy", ( FunctionType )doCopySig.LlvmType );
            doCopyFunc.AddAttributes( Attributes.NoUnwind | Attributes.UnwindTable );
            var copyFuncDi = diBuilder.CreateFunction( scope: diFile
                                                     , name: "DoCopy"
                                                     , mangledName: null
                                                     , file: diFile
                                                     , line: 16
                                                     , compositeType: ( DICompositeType )doCopySig.DebugType
                                                     , isLocalToUnit: false
                                                     , isDefinition: true
                                                     , scopeLine: 17
                                                     , flags: 0
                                                     , isOptimized: false
                                                     , function: doCopyFunc
                                                     );

            // create block for the function body, only need one for this simple sample
            var blk = doCopyFunc.AppendBasicBlock( "entry" );

            // create instruction builder to build the body
            var instBuilder = new InstructionBuilder( blk );

            // create a temp local copy of the global structure
            var dstAddr = ( Instruction )instBuilder.Alloca( foo.LlvmType, "agg.tmp" );
            dstAddr.Alignment = foo.CallFrameAlignment;
            var bytePtrType = module.Context.Int8Type.CreatePointerType( );
            var bitCastDst = (Instruction)instBuilder.BitCast( dstAddr, bytePtrType );
            bitCastDst.SetDebugLocation( 18, 11, copyFuncDi );
            var bitCastSrc = instBuilder.BitCast( bar, bytePtrType );
            var memCpy = (Instruction)instBuilder.MemCpy( module, bitCastDst, bitCastSrc, ConstantInt.From( ( int )foo.BitSize / 8 ), ( int )foo.CallFrameAlignment, false );
            memCpy.SetDebugLocation( 18, 11, copyFuncDi );
            var callCopy = instBuilder.Call( copyFunc, dstAddr, baz );
            callCopy.SetDebugLocation( 18, 5, copyFuncDi );
            var ret = ( Instruction )instBuilder.Return( );
            ret.SetDebugLocation( 19, 1, copyFuncDi );
        }
    }
}
