using System;
using Llvm.NET;
using Llvm.NET.DebugInfo;
using Llvm.NET.Instructions;

namespace TestDebugInfo
{
    /// <summary>Program to test/demonstrate Aspects of debug information generation with Llvm.NET</summary>
    class Program
    {
        const string Triple = "thumbv7m-none-eabi";
        const string Cpu = "cortex-m3";
        const string Features = "";

        /// <summary>Creates a test LLVM module with debug information</summary>
        /// <param name="args">ignored</param>
        /// <remarks>
        /// <code language="C#" title="Example code generated">
        /// struct foo
        /// {
        ///     int a;
        ///     int b[0];
        /// };
        ///
        /// void copy( struct foo src, struct foo* pDst )
        /// {
        ///     *pDst = src;
        /// }
        /// </code>
        /// </remarks>
        static void Main( string[ ] args )
        {
            using( var context = Context.CreateThreadContext( ) )
            {
                StaticState.RegisterARM( );
                var target = Target.FromTriple( Triple );
                using( var targetMachine = target.CreateTargetMachine( Triple, Cpu, Features, CodeGenOpt.Aggressive, Reloc.Default, CodeModel.Small ) )
                using( var module = context.CreateModule( "test.bc" ) )
                {
                    module.TargetTriple = targetMachine.Triple;
                    var targetData = targetMachine.TargetData;

                    // add debug module flags to the module
                    module.AddModuleFlag( ModuleFlagBehavior.Override, Module.DebugVersionValue, Module.DebugMetadataVersion );
                    module.AddModuleFlag( ModuleFlagBehavior.Override, Module.DwarfVersionValue, 4 );
                    var diBuilder = new DebugInfoBuilder( module );
                    
                    // create compile unit and file as the scope for everything
                    var cu = diBuilder.CreateCompileUnit( SourceLanguage.C, "test.c", Environment.CurrentDirectory, "TestDebugInfo", false, "", 0 );
                    var diFile = diBuilder.CreateFile( "test.c", Environment.CurrentDirectory );

                    // create the LLVM structure type and body
                    // The full body is used with targetMachine.TargetData to determine size, alignment and element offsets
                    // in a target independent manner.
                    var foo = context.CreateStructType( "Struct.foo" );
                    foo.SetBody( false, context.Int32Type, context.Int32Type.CreateArrayType(0) );

                    // use targetMachine to get size and alignemnt of struct and fields
                    var fooBitSize = targetData.BitSizeOf( foo );
                    var fooAbiAlignment = targetData.AbiAlignmentOf( foo );
                    var int32BitSize = targetData.BitSizeOf( context.Int32Type );
                    var int32AbiAlignment = targetData.AbiAlignmentOf( context.Int32Type ) * 8;
                    
                    // get a debug info type for int32
                    var diInt32 = diBuilder.CreateBasicType( "int", int32BitSize, int32AbiAlignment, Llvm.NET.DebugInfo.TypeKind.Signed );
                    var diInt32ArrayType = diBuilder.CreateArrayType( 0, 32, diInt32, diBuilder.CreateSubrange( 0, 0) );

                    // Create debug info for the structure
                    // for testing purposes create an opaque type, later a new type is created with members
                    // and all uses of this one are replaced with the complete one
                    var fooOpaque = diBuilder.CreateReplaceableForwardDecl( Tag.StructureType, "foo", cu, diFile, 0 );

                    // create pointer types for function args
                    var fooPtr = foo.CreatePointerType( );
                    var fooDiPtr = diBuilder.CreatePointerType( fooOpaque, string.Empty, targetData.BitSizeOf( fooPtr ), targetData.AbiAlignmentOf( fooPtr ) );

                    // create function signature
                    var copySig = context.GetFunctionType( context.VoidType, fooPtr, fooPtr );
                    var copySigDi = diBuilder.CreateSubroutineType( diFile, null, fooOpaque, fooDiPtr );

                    // Create the function
                    var copyFunc = module.AddFunction( "copy", copySig );
                    copyFunc.AddAttributes( Attributes.NoUnwind );
                    var copyFuncDi = diBuilder.CreateFunction( diFile, "copy", null, diFile, 7, copySigDi, false, true, 7, (uint)DebugInfoFlags.Prototyped, false, copyFunc );

                    // create block for the function body, only need one for this simple sample
                    var blk = copyFunc.AppendBasicBlock( "entry" );

                    // create instruction builder to build the body
                    var instBuilder = new InstructionBuilder( blk );

                    // create parameter declaration instrinssic instructions
                    var paramSrc = diBuilder.CreateLocalVariable( (uint)Tag.ArgVariable, copyFuncDi, "src", diFile, 7, fooOpaque, false, 0, 0 );
                    var srcDeclare = (Instruction)diBuilder.InsertDeclare( copyFunc.Parameters[ 0 ], paramSrc, blk );
                    // NOTE: Scope for an instruction debug location must be the function or a lexical block 
                    //       not the file the line, column info refere to since it is assumed the file is the
                    //       same as the scope. For parmaters and local declarations this allows assigning both
                    //       line and column location information to the arguments declaration, which isn't
                    //       possible with CreateLocalVariable(...) alone
                    srcDeclare.SetDebugLocation( 7, 23, copyFuncDi );

                    var paramDst = diBuilder.CreateLocalVariable( ( uint )Tag.ArgVariable, copyFuncDi, "pDst", diFile, 7, fooDiPtr, false, 0, 1 );
                    var dstDeclare = (Instruction)diBuilder.InsertDeclare( copyFunc.Parameters[ 1 ], paramDst, blk );
                    dstDeclare.SetDebugLocation( 7, 40, copyFuncDi );

                    // create a store instruction to copy the src into the destination
                    var storeInst = (Instruction)instBuilder.Store( copyFunc.Parameters[ 0 ], copyFunc.Parameters[ 1 ] );
                    storeInst.SetDebugLocation( 9, 5, copyFuncDi );
                    instBuilder.Return( );

                    // Create concrete type and RAUW of the opaque one with the complete version
                    // despite what seems intuitively obvious the scope for the createMemberType isn't the type the member is a part of,
                    // since it hasn't been created yet. Instead, it's the file (or compile unit )
                    var diFields = new Llvm.NET.DebugInfo.Type[]
                        { diBuilder.CreateMemberType( scope: cu
                                                    , name: "a"
                                                    , file: diFile
                                                    , line: 3
                                                    , bitSize: int32BitSize
                                                    , bitAlign: int32AbiAlignment
                                                    , bitOffset: targetMachine.TargetData.OffsetOfElement( foo, 0 )
                                                    , flags: 0
                                                    , type: diInt32
                                                    )
                        , diBuilder.CreateMemberType( scope: cu
                                                    , name: "b"
                                                    , file: diFile
                                                    , line: 4
                                                    , bitSize: targetData.BitSizeOf( foo.Members[ 1 ] )
                                                    , bitAlign: targetData.AbiAlignmentOf( foo.Members[ 1 ] )
                                                    , bitOffset: targetMachine.TargetData.OffsetOfElement( foo, 1 )
                                                    , flags: 0
                                                    , type: diInt32ArrayType
                                                    )
                        };
                    var fooConcrete = diBuilder.CreateStructType( scope: cu
                                                                , name: "foo"
                                                                , file: diFile
                                                                , line: 1
                                                                , bitSize: fooBitSize
                                                                , bitAlign: fooAbiAlignment
                                                                , flags: 0
                                                                , derivedFrom: null
                                                                , elements: diFields );
                    fooOpaque.ReplaceAllUsesWith( context, fooConcrete );
                    // NOTE: Use of fooOpaque at this point will generate an error/aassert or crash from the LLVM native code layer
                    // so it should be set to null or simply re-assigned the new concreate value, this sample is making the names
                    // and instances distinct for illustration so the opaque value is set to null as it should not be used after RAUW
                    fooOpaque = null;

                    // finalize the debug information
                    diBuilder.Finish( );

                    string msg;
                    if( !module.Verify( out msg ) )
                        Console.Error.WriteLine( "ERROR: {0}", msg );
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
    }
}
