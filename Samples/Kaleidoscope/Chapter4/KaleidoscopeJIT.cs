// <copyright file="KaleidoscopeJIT.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>
using Llvm.NET;
using Llvm.NET.JIT;

namespace Kaleidoscope
{
#if USE_ORCJIT
    internal class KaleidoscopeJIT
    {
        public KaleidoscopeJIT( )
        {
            TargetMachine = Target.FromTriple( Triple.HostTriple.ToString( ) )
                                  .CreateTargetMachine(Triple.HostTriple.ToString(), null, null, CodeGenOpt.Default, Reloc.Default, CodeModel.JitDefault );

            ExecutionEngine = new OrcJit( TargetMachine );
        }

        public TargetMachine TargetMachine { get; }

        public OrcJit.OrcJitHandle AddModule( BitcodeModule module ) => ExecutionEngine.AddModule( module, ExecutionEngine.DefaultSymbolResolver );

        public void RemoveModule( OrcJit.OrcJitHandle moduleHandle ) => ExecutionEngine.RemoveModule( moduleHandle );

        public T GetDelegateForFunction<T>( string name )
        {
            return ExecutionEngine.GetFunctionDelegate<T>( name );
        }

        private readonly OrcJit ExecutionEngine;
    }

#else
    internal class KaleidoscopeJIT
    {
        public TargetMachine TargetMachine => ExecutionEngine.TargetMachine;

        public int AddModule( BitcodeModule module ) => ExecutionEngine.AddModule( module );

        public void RemoveModule( int moduleHandle ) => ExecutionEngine.RemoveModule( moduleHandle );

        public T GetDelegateForFunction<T>( string name )
        {
            return ExecutionEngine.GetFunctionDelegate<T>( name );
        }

        internal KaleidoscopeJIT( )
        {
            ExecutionEngine = new LegacyExecutionEngine( EngineKind.Jit, CodeGenOpt.Aggressive );
        }

        private readonly IExecutionEngine<int> ExecutionEngine;
    }
#endif
}
