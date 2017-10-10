// <copyright file="PassManagerBuilder.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>Provides a wrapper around the LLVM PassManagerBuilder</summary>
    /// <remarks>This class is still in the experimental stage as there is a lack of full support from the C API</remarks>
    public sealed class PassManagerBuilder
        : IDisposable
    {
        public PassManagerBuilder( )
        {
            PassManagerBuilderHandle = NativeMethods.PassManagerBuilderCreate( );
        }

        public void SetOptLevel( uint optLevel )
        {
            NativeMethods.PassManagerBuilderSetOptLevel( PassManagerBuilderHandle, optLevel );
        }

        public void SetSizeLevel( uint sizeLevel )
        {
            NativeMethods.PassManagerBuilderSetSizeLevel( PassManagerBuilderHandle, sizeLevel );
        }

        public void SetDisableUnitAtATime( bool value )
        {
            NativeMethods.PassManagerBuilderSetDisableUnitAtATime( PassManagerBuilderHandle, value );
        }

        public void SetDisableUnrollLoops( bool value )
        {
            NativeMethods.PassManagerBuilderSetDisableUnrollLoops( PassManagerBuilderHandle, value );
        }

        public void SetDisableSimplifyLibCalls( bool value )
        {
            NativeMethods.PassManagerBuilderSetDisableSimplifyLibCalls( PassManagerBuilderHandle, value );
        }

        /*
        public void PopulateFunctionPassManager( PassManager passManager )
        {
            NativeMethods.PassManagerBuilderPopulateFunctionPassManager( PassManagerBuilderHandle, passManager.PassManagerHandle );
        }

        public void PopulateModulePassManager( PassManager passManager )
        {
            NativeMethods.PassManagerBuilderPopulateModulePassManager( PassManagerBuilderHandle, passManager.PassManagerHandle );
        }

        public void PopulateLTOPassManager( PassManager passManager, bool internalize, bool runInliner )
        {
            NativeMethods.PassManagerBuilderPopulateLTOPassManager( PassManagerBuilderHandle
                                                                  , passManager.PassManagerHandle
                                                                  , internalize
                                                                  , runInliner
                                                                  );
        }
        */
        public void Dispose( )
        {
            if( PassManagerBuilderHandle.Pointer != IntPtr.Zero )
            {
                NativeMethods.PassManagerBuilderDispose( PassManagerBuilderHandle );
                PassManagerBuilderHandle = default( LLVMPassManagerBuilderRef );
            }
        }

        private LLVMPassManagerBuilderRef PassManagerBuilderHandle;
    }
}
