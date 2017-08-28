using System;
using System.Collections.Generic;
using Llvm.NET.Native;

namespace Llvm.NET
{
    /// <summary>LLVM Target Instruction Set Architecture</summary>
    public class Target
    {
        /// <summary>Name of this target</summary>
        public string Name => NativeMethods.GetTargetName( TargetHandle );

        /// <summary>Description of this target</summary>
        public string Description => NativeMethods.GetTargetDescription( TargetHandle );

        /// <summary>Flag indicating if this target has JIT support</summary>
        public bool HasJIT => NativeMethods.TargetHasJIT( TargetHandle );

        /// <summary>Flag indicating if this target has a TargetMachine initialized</summary>
        public bool HasTargetMachine => NativeMethods.TargetHasTargetMachine( TargetHandle );

        /// <summary>Flag indicating if this target has an Assembly code generating back end initialized</summary>
        public bool HasAsmBackEnd => NativeMethods.TargetHasAsmBackend( TargetHandle );

        /// <summary>Creates a <see cref="TargetMachine"/> for the target and specified parameters</summary>
        /// <param name="context">Context to use for LLVM objects created by this machine</param>
        /// <param name="triple">Target triple for this machine (e.g. -mtriple)</param>
        /// <param name="cpu">CPU for this machine (e.g. -mcpu)</param>
        /// <param name="features">Features for this machine (e.g. -mattr...)</param>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns><see cref="TargetMachine"/> based on the specified parameters</returns>
        public TargetMachine CreateTargetMachine( Context context
                                                , string triple
                                                , string cpu = null
                                                , string features = null
                                                , CodeGenOpt optLevel = CodeGenOpt.Default
                                                , Reloc relocationMode = Reloc.Default
                                                , CodeModel codeModel = CodeModel.Default
                                                )
        {
            var targetMachineHandle = NativeMethods.CreateTargetMachine( TargetHandle
                                                                       , triple
                                                                       , cpu ?? string.Empty
                                                                       , features ?? string.Empty
                                                                       , ( LLVMCodeGenOptLevel )optLevel
                                                                       , ( LLVMRelocMode )relocationMode
                                                                       , ( LLVMCodeModel )codeModel
                                                                       );
            return new TargetMachine( context, targetMachineHandle );
        }

        /// <summary>Retrieves an enumerable collection of the available targets built into this library</summary>
        public static IEnumerable<Target> AvailableTargets
        {
            get
            {
                var current = NativeMethods.GetFirstTarget( );
                while( current.Pointer != IntPtr.Zero )
                {
                    yield return FromHandle( current );
                    current = NativeMethods.GetNextTarget( current );
                }
            }
        }

        /// <summary>Gets the target for a given target "triple" value</summary>
        /// <param name="targetTriple">Target triple string describing the target</param>
        /// <returns>Target for the given triple</returns>
        public static Target FromTriple( string targetTriple )
        {
            if( !NativeMethods.GetTargetFromTriple( targetTriple, out LLVMTargetRef targetHandle, out string errorMessag ) )
            {
                throw new InternalCodeGeneratorException( errorMessag );
            }

            return FromHandle( targetHandle );
        }

        internal Target( LLVMTargetRef targetHandle )
        {
            TargetHandle = targetHandle;
        }

        internal LLVMTargetRef TargetHandle { get; }

        internal static Target FromHandle( LLVMTargetRef targetHandle )
        {
            lock( TargetMap )
            {
                if( TargetMap.TryGetValue( targetHandle.Pointer, out Target retVal ) )
                {
                    return retVal;
                }

                retVal = new Target( targetHandle );
                TargetMap.Add( targetHandle.Pointer, retVal );
                return retVal;
            }
        }

        private static readonly Dictionary<IntPtr, Target> TargetMap = new Dictionary<IntPtr, Target>();
    }
}
