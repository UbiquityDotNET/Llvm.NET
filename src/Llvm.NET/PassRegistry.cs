// <copyright file="PassRegistry.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Threading;
using Llvm.NET.Native;

namespace Llvm.NET
{
    public sealed class PassRegistry
        : IDisposable
    {
        public PassRegistry( )
        {
            PassRegistryHandle = NativeMethods.CreatePassRegistry( );
        }

        ~PassRegistry( )
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose( false );
        }

        /// <inheritdocs/>
        public void Dispose( )
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        public void InitializeAll( )
        {
            InitializeCore( );
            InitializeTransformUtils( );
            InitializeScalarOpts( );
            InitializeObjCARCOpts( );
            InitializeVectorization( );
            InitializeIPO( );
            InitializeAnalysis( );
            InitializeTransformUtils( );
            InitializeInstCombine( );
            InitializeInstrumentation( );
            InitializeIPA( );
            InitializeTarget( );
            InitializeCodeGenForOpt( );
        }

        public void InitializeCodeGenForOpt()
        {
            NativeMethods.InitializeCodeGenForOpt( PassRegistryHandle );
        }

        public void InitializeCore( )
        {
            NativeMethods.InitializeCore( PassRegistryHandle );
        }

        public void InitializeTransformUtils( )
        {
            NativeMethods.InitializeTransformUtils( PassRegistryHandle );
        }

        public void InitializeScalarOpts( )
        {
            NativeMethods.InitializeScalarOpts( PassRegistryHandle );
        }

        public void InitializeObjCARCOpts( )
        {
            NativeMethods.InitializeObjCARCOpts( PassRegistryHandle );
        }

        public void InitializeVectorization( )
        {
            NativeMethods.InitializeVectorization( PassRegistryHandle );
        }

        public void InitializeInstCombine( )
        {
            NativeMethods.InitializeInstCombine( PassRegistryHandle );
        }

        public void InitializeIPO( )
        {
            NativeMethods.InitializeIPO( PassRegistryHandle );
        }

        public void InitializeInstrumentation( )
        {
            NativeMethods.InitializeInstrumentation( PassRegistryHandle );
        }

        public void InitializeAnalysis( )
        {
            NativeMethods.InitializeAnalysis( PassRegistryHandle );
        }

        public void InitializeIPA( )
        {
            NativeMethods.InitializeIPA( PassRegistryHandle );
        }

        public void InitializeCodeGen( )
        {
            NativeMethods.InitializeCodeGen( PassRegistryHandle );
        }

        public void InitializeTarget( )
        {
            NativeMethods.InitializeTarget( PassRegistryHandle );
        }

        public static PassRegistry GlobalRegistry => LazyGlobalPassRegistry.Value;

        private PassRegistry( LLVMPassRegistryRef hRegistry )
        {
            PassRegistryHandle = hRegistry;
        }

        private void Dispose( bool disposing )
        {
            if( !PassRegistryHandle.IsClosed )
            {
                if( disposing )
                {
                    // TODO: dispose managed state (managed objects).
                }

                PassRegistryHandle.Dispose( );
            }
        }

        private LLVMPassRegistryRef PassRegistryHandle;

        private static readonly Lazy<PassRegistry> LazyGlobalPassRegistry
            = new Lazy<PassRegistry>( () => new PassRegistry( NativeMethods.GetGlobalPassRegistry() )
                                    , LazyThreadSafetyMode.ExecutionAndPublication
                                    );
    }
}
