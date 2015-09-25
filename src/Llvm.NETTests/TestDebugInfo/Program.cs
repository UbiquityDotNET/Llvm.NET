using System;
using System.IO;
using System.Linq;
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
        const string Triple = "x86_64-pc-windows-msvc18.0.0";
        const string Cpu = "x86-64";
        const string Features = "+sse,+sse2";
        static readonly AttributeValue[] TargetDependentAttributes =
        {
            new AttributeValue( "disable-tail-calls", "false" ),
            new AttributeValue( "less-precise-fpmad", "false" ),
            new AttributeValue( "no-frame-pointer-elim", "false" ),
            new AttributeValue( "no-infs-fp-math", "false" ),
            new AttributeValue( "no-nans-fp-math", "false" ),
            new AttributeValue( "stack-protector-buffer-size", "8" ),
            new AttributeValue( "target-cpu", "x86-64" ),
            new AttributeValue( "target-features", "+sse,+sse2" ),
            new AttributeValue( "unsafe-fp-math", "false" ),
            new AttributeValue( "use-soft-float", "false" )
        };

        /// <summary>Creates a test LLVM module with debug information</summary>
        /// <param name="args">ignored</param>
        /// <remarks>
        /// <code language="C99" title="Example code generated">
        /// struct foo
        /// {
        ///     int a;
        ///     float b;
        ///     int c[2];
        /// };
        /// 
        /// struct foo bar = { 1, 2.0, { 3, 4 } };
        /// struct foo baz;
        /// 
        /// inline static void copy( struct foo src     // function line here
        ///                        , struct foo* pDst
        ///                        )
        /// { // function's ScopeLine here
        ///     *pDst = src;
        /// }
        /// 
        /// //void OtherSig( struct foo const* pSrc, struct foo* pDst )
        /// //{
        /// //    copy( *pSrc, pDst );
        /// //}
        /// //
        /// void DoCopy( )
        /// {
        ///     copy( bar, &baz );
        /// }
        /// </code>
        /// </remarks>
        static void Main( string[ ] args )
        {
            var srcPath = args[0];
            if( !File.Exists( srcPath ) )
            {
                Console.Error.WriteLine( "Src file not found: '{0}'", srcPath );
                return;
            }

            StaticState.RegisterAll( );
            var target = Target.FromTriple( Triple );
            using( var targetMachine = target.CreateTargetMachine( Triple, Cpu, Features, CodeGenOpt.Aggressive, Reloc.Default, CodeModel.Small ) )
            using( var module = new Module( "test_x86.bc" ) )
            {
                var targetData = targetMachine.TargetData;

                module.TargetTriple = targetMachine.Triple;
                module.DataLayoutString = targetMachine.TargetData.ToString( );

                // create compile unit and file as the top level scope for everything
                var cu = module.DIBuilder.CreateCompileUnit( SourceLanguage.C99
                                                            , Path.GetFileName( srcPath )
                                                            , Path.GetDirectoryName( srcPath )
                                                            , "clang version 3.7.0 " // obviously this is not clang but helps in diff with actual clang output
                                                            , false
                                                            , ""
                                                            , 0
                                                            );
                var diFile = module.DIBuilder.CreateFile( srcPath );

                // Create basic types used in this compilation
                module.Context.Int32Type.CreateDIType( module, "int", cu );
                module.Context.FloatType.CreateDIType( module, "float", cu );
                var i32 = module.Context.Int32Type;
                var f32 = module.Context.FloatType;
                var i32Array_0_2 = i32.CreateArrayType( module, 0, 2 );

                // create the LLVM structure type and body
                // The full body is used with targetMachine.TargetData to determine size, alignment and element offsets
                // in a target independent manner.
                var fooType = module.Context.CreateStructType( "struct.foo" );
                fooType.CreateDIType( module, "foo", cu );
                fooType.SetBody( false, i32, f32, i32Array_0_2 );

                // add global variables and constants
                var constArray = ConstantArray.From( i32, module.Context.CreateConstant( 3 ), module.Context.CreateConstant( 4 ));
                var barValue = module.Context.CreateNamedConstantStruct( fooType
                                                                       , module.Context.CreateConstant( 1 )
                                                                       , module.Context.CreateConstant( 2.0f )
                                                                       , constArray
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
                module.AddVersionIdentMetadata( "clang version 3.7.0 " );

                // create types for function args
                var constFoo = module.DIBuilder.CreateQualifiedType( fooType.DIType, QualifiedTypeTag.Const );
                var fooPtr = fooType.CreatePointerType( module );
                // Create function signatures

                // Since the the first parameter is passed by value 
                // using the pointer+alloca+memcopy pattern, the actual
                // source, and therefore debug, signature is NOT a pointer.
                // However, that usage would create a signature with two
                // pointers as the arguments, which doesn't match the source
                // To get the correct debug info signature this inserts an
                // explicit ParameterTypePair that overrides the default
                // behavior to pair LLVM pointer type with the original
                // source type.
                var copySig = module.Context.CreateFunctionType( module.DIBuilder
                                                               , diFile
                                                               , module.Context.VoidType
                                                               , new DebugTypePair<DIDerivedType>( fooPtr, constFoo )
                                                               , fooPtr
                                                               );
                var doCopySig = module.Context.CreateFunctionType( module.DIBuilder, diFile, module.Context.VoidType );

                // Create the functions
                // NOTE: The declaration ordering is reveresd from that of the sample code file (test.c)
                //       However, this is what Clang ends up doing for some reason so it is
                //       replicated here to aid in comparing the generated LL files.
                var doCopyFunc = module.CreateFunction( scope: diFile
                                                      , name: "DoCopy"
                                                      , linkageName: null
                                                      , file: diFile
                                                      , line: 23
                                                      , signature: doCopySig
                                                      , isLocalToUnit: false
                                                      , isDefinition: true
                                                      , scopeLine: 24
                                                      , flags: 0
                                                      , isOptimized: false
                                                      ).AddAttributes( AttributeKind.NoUnwind, AttributeKind.UWTable )
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
                                                    ).Linkage( Linkage.Internal ) // static function
                                                     .AddAttributes( AttributeKind.NoUnwind, AttributeKind.UWTable, AttributeKind.InlineHint )
                                                     .AddAttributes( TargetDependentAttributes );

                CreateDoCopyFunctionBody( module, targetData, doCopyFunc, fooType, bar, baz, copyFunc );
                CreateCopyFunctionBody( module, targetData, copyFunc, diFile, fooType, fooPtr, constFoo );

                // fill in the debug info body for type foo
                FinalizeFooDebugInfo( module.DIBuilder, targetData, cu, diFile, fooType );

                // finalize the debug information
                // all temporaries must be replaced by now, this resolves any remaining
                // forward declarations and marks the builder to prevent adding any
                // nodes that are not completely resolved.
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
                    File.WriteAllText( "test.ll", module.AsString( ) );
                    targetMachine.EmitToFile( module, "test.o", CodeGenFileType.ObjectFile );
                    targetMachine.EmitToFile( module, "test.s", CodeGenFileType.AssemblySource );
                }
            }
        }

        private static void FinalizeFooDebugInfo( DebugInfoBuilder diBuilder
                                                , TargetData layout
                                                , DICompileUnit cu
                                                , DIFile diFile
                                                , IStructType foo
                                                )
        {
            // Create concrete DIType and RAUW of the opaque one with the complete version
            // While this two phase approach might not seem strictly necessary in this sample
            // it actually is, due to how the member's scope requires a DIType. So this
            // is creating members that are linked to the temporary DIType as the parent scope
            // but attached to the newly created concrete type. The discrepency is resolved
            // when ReplaceAllUsesOfDebugTypeWith() is called to replace all instances of the
            // temporary type with the final concrete type ending up with the members properly
            // referring to the containing type as the scope.
            var diFields = new DIType[ ]
                { diBuilder.CreateMemberType( scope: foo.DIType
                                            , name: "a"
                                            , file: diFile
                                            , line: 3
                                            , bitSize: layout.BitSizeOf( foo.Members[0] )
                                            , bitAlign: layout.AbiBitAlignmentOf( foo.Members[0] )
                                            , bitOffset: layout.BitOffsetOfElement( foo, 0 )
                                            , flags: 0
                                            , type: foo.Members[0].DIType
                                            )
                , diBuilder.CreateMemberType( scope: foo.DIType
                                            , name: "b"
                                            , file: diFile
                                            , line: 4
                                            , bitSize: layout.BitSizeOf( foo.Members[1] )
                                            , bitAlign: layout.AbiBitAlignmentOf( foo.Members[1] )
                                            , bitOffset: layout.BitOffsetOfElement( foo, 1 )
                                            , flags: 0
                                            , type: foo.Members[1].DIType
                                            )
                , diBuilder.CreateMemberType( scope: foo.DIType
                                            , name: "c"
                                            , file: diFile
                                            , line: 5
                                            , bitSize: layout.BitSizeOf( foo.Members[2] )
                                            , bitAlign: layout.AbiBitAlignmentOf( foo.Members[2] )
                                            , bitOffset: layout.BitOffsetOfElement( foo, 2 )
                                            , flags: 0
                                            , type: foo.Members[2].DIType
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
                                                        , elements: diFields.AsEnumerable() );
            foo.ReplaceAllUsesOfDebugTypeWith( fooConcrete );
        }

        private static void CreateCopyFunctionBody( Module module
                                                  , TargetData layout
                                                  , Function copyFunc
                                                  , DIFile diFile
                                                  , IStructType foo
                                                  , IPointerType fooPtr
                                                  , DIDerivedType constFooType
                                                  )
        {
            var diBuilder = module.DIBuilder;

            // ByVal pointers indicate by value semantics. The actual semantics are along the lines of
            // "pass the arg as copy on the arguments stack and set parameter implicitly to that copy's address"
            // (src: https://github.com/ldc-developers/ldc/issues/937 )
            //
            // LLVM recognizes this pattern and has a pass to map to an efficient register usage whenever plausible.
            // Though it seems Clang doesn't apply the attribute...
            //copyFunc.Parameters[ 0 ].AddAttribute( FunctionAttributeIndex.Parameter0, AttributeKind.ByVal );
            //copyFunc.Parameters[ 0 ].SetAlignment( layout.AbiAlignmentOf( foo ) );
            copyFunc.Parameters[ 0 ].Name = "src";
            copyFunc.Parameters[ 1 ].Name = "pDst";

            // create block for the function body, only need one for this simple sample
            var blk = copyFunc.AppendBasicBlock( "entry" );

            // create instruction builder to build the body
            var instBuilder = new InstructionBuilder( blk );

            // create debug info locals for the arguments
            // NOTE: Debug parameter indeces are 1 based!
            var paramSrc = diBuilder.CreateArgument( copyFunc.DISubProgram, "src", diFile, 11, constFooType, false, 0, 1 );
            var paramDst = diBuilder.CreateArgument( copyFunc.DISubProgram, "pDst", diFile, 12, fooPtr.DIType, false, 0, 2 );

            var ptrAlign = layout.CallFrameAlignmentOf( fooPtr );
            
            // create Locals
            // NOTE: There's no debug location attatched to these instructions.
            //       The debug info will come from the declare instrinsic below.
            var dstAddr = instBuilder.Alloca( fooPtr )
                                     .RegisterName( "pDst.addr" )
                                     .Alignment( ptrAlign );

            instBuilder.Store( copyFunc.Parameters[ 1 ], dstAddr)
                       .Alignment( ptrAlign );

            // insert declare pseudo instruction to attach debug info to the local declarations
            diBuilder.InsertDeclare( dstAddr, paramDst, new DILocation( module.Context, 12, 38, copyFunc.DISubProgram ), blk );

            // since the function's LLVM signature uses a pointer, which is copied locally
            // inform the debugger to treat it as the value by dereferencing the pointer
            diBuilder.InsertDeclare( copyFunc.Parameters[ 0 ]
                                   , paramSrc
                                   , diBuilder.CreateExpression( ExpressionOp.deref )
                                   , new DILocation( module.Context, 11, 43, copyFunc.DISubProgram )
                                   , blk
                                   );

            var loadedDst = instBuilder.Load( dstAddr )
                                       .Alignment( ptrAlign )
                                       .SetDebugLocation( 15, 6, copyFunc.DISubProgram );

            var dstPtr = instBuilder.BitCast( loadedDst, module.Context.Int8Type.CreatePointerType( ) )
                                    .SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            var srcPtr = instBuilder.BitCast( copyFunc.Parameters[ 0 ], module.Context.Int8Type.CreatePointerType( ) )
                                    .SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            instBuilder.MemCpy( module
                              , dstPtr
                              , srcPtr
                              , module.Context.CreateConstant( layout.ByteSizeOf( foo ) )
                              , ( int )layout.AbiAlignmentOf( foo )
                              , false
                              ).SetDebugLocation( 15, 13, copyFunc.DISubProgram );

            instBuilder.Return( )
                       .SetDebugLocation( 16, 1, copyFunc.DISubProgram );
        }

        private static void CreateDoCopyFunctionBody( Module module
                                                    , TargetData layout
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

            // create a temp local copy of the global structure
            var dstAddr = instBuilder.Alloca( foo )
                                     .RegisterName( "agg.tmp" )
                                     .Alignment( layout.CallFrameAlignmentOf( foo ) );

            var bitCastDst = instBuilder.BitCast( dstAddr, bytePtrType )
                                        .SetDebugLocation( 25, 11, doCopyFunc.DISubProgram );

            var bitCastSrc = instBuilder.BitCast( bar, bytePtrType )
                                        .SetDebugLocation( 25, 11, doCopyFunc.DISubProgram );

            instBuilder.MemCpy( module
                              , bitCastDst
                              , bitCastSrc
                              , module.Context.CreateConstant( layout.ByteSizeOf( foo ) )
                              , ( int )layout.CallFrameAlignmentOf( foo )
                              , false
                              ).SetDebugLocation( 25, 11, doCopyFunc.DISubProgram );

            instBuilder.Call( copyFunc, dstAddr, baz )
                       .SetDebugLocation( 25, 5, doCopyFunc.DISubProgram );

            instBuilder.Return( )
                       .SetDebugLocation( 26, 1, doCopyFunc.DISubProgram );
        }
    }
}
