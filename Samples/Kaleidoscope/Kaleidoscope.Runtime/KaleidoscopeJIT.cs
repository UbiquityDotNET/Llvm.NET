// <copyright file="KaleidoscopeJIT.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using Llvm.NET;
using Llvm.NET.JIT;

namespace Kaleidoscope.Runtime
{
    /// <summary>JIT engine for Kaledoscope language</summary>
    public sealed class KaleidoscopeJIT
        : IDisposable
    {
        public KaleidoscopeJIT( )
        {
            TargetMachine = Target.FromTriple( Triple.HostTriple.ToString( ) )
                                  .CreateTargetMachine(Triple.HostTriple.ToString(), null, null, CodeGenOpt.Default, Reloc.Default, CodeModel.JitDefault );

            ExecutionEngine = new OrcJit( TargetMachine );
            ExecutionEngine.AddInteropCallback( "putchard", new CallbackHandler1( PutChard ) );
            ExecutionEngine.AddInteropCallback( "printd", new CallbackHandler1( Printd ) );
        }

        public TargetMachine TargetMachine { get; }

        public IJitModuleHandle AddModule( BitcodeModule module ) => ExecutionEngine.AddModule( module, ExecutionEngine.DefaultSymbolResolver );

        public void RemoveModule( IJitModuleHandle moduleHandle ) => ExecutionEngine.RemoveModule( moduleHandle );

        public T GetDelegateForFunction<T>( string name )
        {
            return ExecutionEngine.GetFunctionDelegate<T>( name );
        }

        public void Dispose( )
        {
            ExecutionEngine.Dispose( );
        }

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler0( );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler1( double arg1 );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler2( double arg1, double arg2 );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler3( double arg1, double arg2, double arg3 );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        public delegate double CallbackHandler4( double arg1, double arg2, double arg3, double arg4 );

        private double Printd( double x )
        {
            // STOP ALL EXCEPTIONS from bubbling out to JIT'ed code
            try
            {
                Console.WriteLine( x );
                return 0.0F;
            }
            catch
            {
                return 0.0;
            }
        }

        private double PutChard( double x )
        {
            // STOP ALL EXCEPTIONS from bubbling out to JIT'ed code
            try
            {
                Console.Write( ( char )x );
                return 0.0F;
            }
            catch
            {
                return 0.0;
            }
        }

        private readonly OrcJit ExecutionEngine;
    }
}
