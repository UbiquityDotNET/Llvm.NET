// -----------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ubiquity.NET.Llvm.Interop;

using static Ubiquity.NET.Llvm.Interop.NativeMethods;

[assembly: CLSCompliant( false )]

#pragma warning disable SA1600
#pragma warning disable CA1801 // externally defined signature

namespace InteropTests
{
    [TestClass]
    public class OrcJitTests
    {
        [TestMethod]
        public void TestLazyIRCompilation( )
        {
            using var libLLVm = Library.InitializeLLVM( );
            libLLVm.RegisterTarget( CodeGenTarget.Native );

            using LLVMContextRef context = LLVMContextCreate( );
            string nativeTriple = LLVMGetDefaultTargetTriple();
            Assert.IsFalse( string.IsNullOrWhiteSpace( nativeTriple ) );
            Assert.IsTrue( LLVMGetTargetFromTriple( nativeTriple, out LLVMTargetRef targetHandle, out string errorMessag ).Succeeded );

            using LLVMTargetMachineRef machine = LLVMCreateTargetMachine(
                                                          targetHandle,
                                                          nativeTriple,
                                                          string.Empty,
                                                          string.Empty,
                                                          LLVMCodeGenOptLevel.LLVMCodeGenLevelDefault,
                                                          LLVMRelocMode.LLVMRelocDefault,
                                                          LLVMCodeModel.LLVMCodeModelJITDefault );

            using LLVMOrcJITStackRef orcJit = LLVMOrcCreateInstance( machine );

            // try several different modules with the same function name replacing the previous
            AddAndExecuteTestModule( orcJit, context, machine, 42 );
            AddAndExecuteTestModule( orcJit, context, machine, 12345678 );
            AddAndExecuteTestModule( orcJit, context, machine, 87654321 );
        }

        private static void AddAndExecuteTestModule( LLVMOrcJITStackRef orcJit, LLVMContextRef context, LLVMTargetMachineRef machine, int expectedResult )
        {
            using LLVMModuleRef module = CreateModule( context, machine, expectedResult );
            LLVMErrorRef err = LLVMOrcAddEagerlyCompiledIR( orcJit, out ulong jitHandle, module, SymbolResolver, IntPtr.Zero );
            Assert.IsTrue( err.IsInvalid );

            // ORC now owns the module, so it must never be released
            module.SetHandleAsInvalid( );
            LLVMOrcGetMangledSymbol( orcJit, out string mangledName, "main" );
            err = LibLLVMOrcGetSymbolAddress( orcJit, out ulong funcAddress, mangledName, false );
            Assert.IsTrue( err.IsInvalid );
            Assert.AreNotEqual( 0ul, funcAddress );
            var callableMain = Marshal.GetDelegateForFunctionPointer<TestMain>( ( IntPtr )funcAddress );
            Assert.AreEqual( expectedResult, callableMain( ) );

            LLVMOrcRemoveModule( orcJit, jitHandle );
        }

        private static ulong SymbolResolver( string Name, IntPtr LookupCtx )
        {
            // Should never get here as the test won't include any resolution of functions not generated.
            return 0;
        }

        private static LLVMModuleRef CreateModule( LLVMContextRef context, LLVMTargetMachineRef machine, int magicNumber )
        {
            var module = LLVMModuleCreateWithNameInContext( "test", context );
            var layout = LLVMCreateTargetDataLayout(machine);
            LLVMSetModuleDataLayout( module, layout );

            LLVMTypeRef int32T = LLVMInt32TypeInContext( context );
            LLVMTypeRef signature = LLVMFunctionType( int32T, null, 0, false );
            LLVMValueRef main = LLVMAddFunction( module, "main", signature );
            LLVMBasicBlockRef entryBlock = LLVMAppendBasicBlock( main, "entry" );

            LLVMBuilderRef builder = LLVMCreateBuilder( );
            LLVMPositionBuilderAtEnd( builder, entryBlock );

            LLVMValueRef constNum = LLVMConstInt( int32T, ( ulong )magicNumber, true );
            LLVMBuildRet( builder, constNum );
            Debug.WriteLine( LLVMPrintModuleToString( module ) );
            return module;
        }

        private delegate int TestMain( );
    }
}
