// -----------------------------------------------------------------------
// <copyright file="Target.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Llvm.NET.Interop;
using Ubiquity.ArgValidators;

using static Llvm.NET.Interop.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Optimization level for target code generation</summary>
    public enum CodeGenOpt
    {
        /// <summary>No optimization</summary>
        None = LLVMCodeGenOptLevel.LLVMCodeGenLevelNone,

        /// <summary>Minimal optimization</summary>
        Less = LLVMCodeGenOptLevel.LLVMCodeGenLevelLess,

        /// <summary>Default optimization</summary>
        Default = LLVMCodeGenOptLevel.LLVMCodeGenLevelDefault,

        /// <summary>Aggressive optimizations</summary>
        Aggressive = LLVMCodeGenOptLevel.LLVMCodeGenLevelAggressive
    }

    /// <summary>Optimization Size preference</summary>
    public enum OptimizationSizeLevel
    {
        /// <summary>Default optimization</summary>
        None = 0,

        /// <summary>Equivalent to -Os</summary>
        Os = 1,

        /// <summary>Equivalent to -Oz</summary>
        Oz = 2
    }

    /// <summary>Relocation type for target code generation</summary>
    public enum RelocationMode
    {
        /// <summary>Default relocation model for the target</summary>
        Default = LLVMRelocMode.LLVMRelocDefault,

        /// <summary>Static relocation model</summary>
        Static = LLVMRelocMode.LLVMRelocStatic,

        /// <summary>Position independent relocation model</summary>
        PositionIndependent = LLVMRelocMode.LLVMRelocPIC,

        /// <summary>Dynamic relocation model</summary>
        Dynamic = LLVMRelocMode.LLVMRelocDynamicNoPic
    }

    /// <summary>Code model to use for target code generation</summary>
    public enum CodeModel
    {
        /// <summary>Default code model for the target</summary>
        Default = LLVMCodeModel.LLVMCodeModelDefault,

        /// <summary>Default code model for JIT to the target</summary>
        JitDefault = LLVMCodeModel.LLVMCodeModelJITDefault,

        /// <summary>Tiny code model</summary>
        Tiny = LLVMCodeModel.LLVMCodeModelTiny,

        /// <summary>Small code model</summary>
        Small = LLVMCodeModel.LLVMCodeModelSmall,

        /// <summary>Kernel code model</summary>
        Kernel = LLVMCodeModel.LLVMCodeModelKernel,

        /// <summary>Medium code model</summary>
        Medium = LLVMCodeModel.LLVMCodeModelMedium,

        /// <summary>Large code model</summary>
        Large = LLVMCodeModel.LLVMCodeModelLarge
    }

    /// <summary>Output file type for target code generation</summary>
    public enum CodeGenFileType
    {
        /// <summary>Generate assembly source file</summary>
        AssemblySource = LLVMCodeGenFileType.LLVMAssemblyFile,

        /// <summary>Generate target object file</summary>
        ObjectFile = LLVMCodeGenFileType.LLVMObjectFile
    }

    /// <summary>LLVM Target Instruction Set Architecture</summary>
    public class Target
    {
        /// <summary>Gets the name of this target</summary>
        public string Name => LLVMGetTargetName( TargetHandle );

        /// <summary>Gets the description of this target</summary>
        public string Description => LLVMGetTargetDescription( TargetHandle );

        /// <summary>Gets a value indicating whether this target has JIT support</summary>
        public bool HasJIT => LLVMTargetHasJIT( TargetHandle );

        /// <summary>Gets a value indicating whether this target has a TargetMachine initialized</summary>
        public bool HasTargetMachine => LLVMTargetHasTargetMachine( TargetHandle );

        /// <summary>Gets a value indicating whether this target has an Assembly code generating back end initialized</summary>
        public bool HasAsmBackEnd => LLVMTargetHasAsmBackend( TargetHandle );

        /// <summary>Creates a <see cref="TargetMachine"/> for the target and specified parameters</summary>
        /// <param name="triple">Target triple for this machine (e.g. -mtriple)</param>
        /// <param name="cpu">CPU for this machine (e.g. -mcpu)</param>
        /// <param name="features">Features for this machine (e.g. -mattr...)</param>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns><see cref="TargetMachine"/> based on the specified parameters</returns>
        public TargetMachine CreateTargetMachine( Triple triple
                                                , string cpu = null
                                                , string features = null
                                                , CodeGenOpt optLevel = CodeGenOpt.Default
                                                , RelocationMode relocationMode = RelocationMode.Default
                                                , CodeModel codeModel = CodeModel.Default
                                                )
        {
            LLVMTargetMachineRef targetMachineHandle = InternalCreateTargetMachine( this, triple, cpu, features, optLevel, relocationMode, codeModel );
            return new TargetMachine( targetMachineHandle );
        }

        /// <summary>Creates a <see cref="TargetMachine"/> for the target and specified parameters</summary>
        /// <param name="triple">Target triple for this machine (e.g. -mtriple)</param>
        /// <param name="cpu">CPU for this machine (e.g. -mcpu)</param>
        /// <param name="features">Features for this machine (e.g. -mattr...)</param>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns><see cref="TargetMachine"/> based on the specified parameters</returns>
        public TargetMachine CreateTargetMachine( string triple
                                                , string cpu = null
                                                , string features = null
                                                , CodeGenOpt optLevel = CodeGenOpt.Default
                                                , RelocationMode relocationMode = RelocationMode.Default
                                                , CodeModel codeModel = CodeModel.Default
                                                )
        {
            LLVMTargetMachineRef targetMachineHandle = InternalCreateTargetMachine( this, triple, cpu, features, optLevel, relocationMode, codeModel );
            return new TargetMachine( targetMachineHandle );
        }

        /// <summary>Gets an enumerable collection of the available targets built into this library</summary>
        public static IEnumerable<Target> AvailableTargets
        {
            get
            {
                var current = LLVMGetFirstTarget( );
                while( current != default )
                {
                    yield return FromHandle( current );
                    current = LLVMGetNextTarget( current );
                }
            }
        }

        /// <summary>Gets the target for a given target "triple" value</summary>
        /// <param name="triple">Target <see cref="Triple"/> describing the target</param>
        /// <returns>Target for the given triple</returns>
        public static Target FromTriple( Triple triple ) => FromTriple( triple.ToString( ) );

        /// <summary>Gets the target for a given target "triple" value</summary>
        /// <param name="targetTriple">Target triple string describing the target</param>
        /// <returns>Target for the given triple</returns>
        public static Target FromTriple( string targetTriple )
        {
            targetTriple.ValidateNotNullOrWhiteSpace( nameof( targetTriple ) );

            if( LLVMGetTargetFromTriple( targetTriple, out LLVMTargetRef targetHandle, out string errorMessag ).Failed )
            {
                throw new InternalCodeGeneratorException( errorMessag );
            }

            return FromHandle( targetHandle );
        }

        internal Target( LLVMTargetRef targetHandle )
        {
            targetHandle.ValidateNotDefault( nameof( targetHandle ) );

            TargetHandle = targetHandle;
        }

        internal LLVMTargetRef TargetHandle { get; }

        internal static LLVMTargetMachineRef InternalCreateTargetMachine( Target target, string triple, string cpu, string features, CodeGenOpt optLevel, RelocationMode relocationMode, CodeModel codeModel )
        {
            triple.ValidateNotNullOrWhiteSpace( nameof( triple ) );
            optLevel.ValidateDefined( nameof( optLevel ) );
            relocationMode.ValidateDefined( nameof( relocationMode ) );
            codeModel.ValidateDefined( nameof( codeModel ) );

            var targetMachineHandle = LLVMCreateTargetMachine( target.TargetHandle
                                                             , triple
                                                             , cpu ?? string.Empty
                                                             , features ?? string.Empty
                                                             , ( LLVMCodeGenOptLevel )optLevel
                                                             , ( LLVMRelocMode )relocationMode
                                                             , ( LLVMCodeModel )codeModel
                                                             );
            return targetMachineHandle;
        }

        internal static Target FromHandle( LLVMTargetRef targetHandle )
        {
            targetHandle.ValidateNotDefault( nameof( targetHandle ) );
            lock( TargetMap )
            {
                if( TargetMap.TryGetValue( targetHandle, out Target retVal ) )
                {
                    return retVal;
                }

                retVal = new Target( targetHandle );
                TargetMap.Add( targetHandle, retVal );
                return retVal;
            }
        }

        private static readonly Dictionary<LLVMTargetRef, Target> TargetMap = new Dictionary<LLVMTargetRef, Target>();
    }
}
