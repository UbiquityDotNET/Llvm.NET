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
        ///     int b;
        /// };
        ///
        /// void copy( foo src, foo* pDst )
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
                using( var module = context.CreateModule( "test" ) )
                {
                    module.TargetTriple = targetMachine.Triple;

                    // add debug module flags to the module
                    module.AddModuleFlag( ModuleFlagBehavior.Override, Module.DebugVersionValue, Module.DebugMetadataVersion );
                    module.AddModuleFlag( ModuleFlagBehavior.Override, Module.DwarfVersionValue, 4 );
                    var diBuilder = new DebugInfoBuilder( module );
                    
                    // create compile unit and file as the scope for everything
                    var cu = diBuilder.CreateCompileUnit( SourceLanguage.C, "test.c", Environment.CurrentDirectory, "TestDebugInfo", false, "", 0 );
                    var diFile = diBuilder.CreateFile( "test.c" );

                    // create the LLVM structure type and body
                    // The full body is used with targetMachine.TargetData to determine size, alignment and element offsets
                    // in a target independent manner.
                    var foo = context.CreateStructType( "Struct.foo" );
                    foo.SetBody( false, context.Int32Type, context.Int32Type );

                    // use targetMachine to get size and alignemnt of struct and fields
                    var fooBitSize = targetMachine.TargetData.BitSizeOf( foo );
                    var fooAbiAlignment = targetMachine.TargetData.AbiAlignmentOf( foo );
                    var int32BitSize = targetMachine.TargetData.BitSizeOf( context.Int32Type );
                    var int32AbiAlignment = targetMachine.TargetData.AbiAlignmentOf( context.Int32Type ) * 8;
                    
                    // get a debug info type for int32
                    var diInt32 = diBuilder.CreateBasicType( "int32", int32BitSize, int32AbiAlignment, Llvm.NET.DebugInfo.TypeKind.Signed );

                    // Create debug info for the structure
                    // for testing purposes create an opaque type, later a new type is created with members
                    // and all uses of this one are replaced with the complete one
                    var fooOpaque = diBuilder.CreateForwardDecl( Tag.StructureType, "foo", cu, diFile, 0 );

                    // create pointer types for function args
                    var fooPtr = foo.CreatePointerType( );
                    var fooDiPtr = diBuilder.CreatePointerType( fooOpaque, "foo*", targetMachine.TargetData.BitSizeOf( fooPtr ), targetMachine.TargetData.AbiAlignmentOf( fooPtr ) );

                    // create debug info void type for signature return
                    var voidDiType = diBuilder.CreateBasicType( "void", 0, 0, Llvm.NET.DebugInfo.TypeKind.Unsigned );

                    // create function signature
                    var copySig = context.GetFunctionType( context.VoidType, foo, fooPtr );
                    var copySigDi = diBuilder.CreateSubroutineType( diFile, voidDiType, fooOpaque, fooDiPtr );

                    // Create the function
                    var copyFunc = module.AddFunction( "copy", copySig );
                    var copyFuncDi = diBuilder.CreateFunction( cu, "foo", "foo", diFile, 7, copySigDi, true, true, 7, 0, false, copyFunc );

                    // create block for the function body, only need one for this simple sample
                    var blk = copyFunc.AppendBasicBlock( "entry" );
                    var lblk = diBuilder.CreateLexicalBlock( copyFuncDi, diFile, 9, 1 );

                    // create the body of the function
                    var instBuilder = new InstructionBuilder( blk );
                    var storeInst = (Instruction)instBuilder.Store( copyFunc.Parameters[ 0 ], copyFunc.Parameters[ 1 ] );
                    storeInst.SetDebugLocation( 9, 5, lblk );
                    instBuilder.Return( );

                    // Create concrete type and RAUW of the opaque one with the complete version
                    // despite what seems intuitively obvious the scope for the createMemberType isn't the type the member is a part of,
                    // since it hasn't been created yet. Instead, it's the file (or compile unit )
                    var diFieldA = diBuilder.CreateMemberType( cu, "a", null, 3, int32BitSize, int32AbiAlignment, targetMachine.TargetData.OffsetOfElement( foo, 0 ), 0, diInt32 );
                    var diFieldB = diBuilder.CreateMemberType( cu, "b", null, 4, int32BitSize, int32AbiAlignment, targetMachine.TargetData.OffsetOfElement( foo, 1 ), 0, diInt32 );
                    var fooConcrete = diBuilder.CreateStructType( cu, "foo", null, 1, fooBitSize, fooAbiAlignment, 0, diFieldA, diFieldB );
                    fooOpaque.ReplaceAllUsesWith( context, fooConcrete );

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
