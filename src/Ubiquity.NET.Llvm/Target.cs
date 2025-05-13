// -----------------------------------------------------------------------
// <copyright file="Target.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.TargetMachine;

namespace Ubiquity.NET.Llvm
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
    public enum CodeGenFileKind
    {
        /// <summary>Generate assembly source file</summary>
        AssemblySource = LLVMCodeGenFileType.LLVMAssemblyFile,

        /// <summary>Generate target object file</summary>
        ObjectFile = LLVMCodeGenFileType.LLVMObjectFile
    }

    /// <summary>Abort behavior when global instruction selection fails to lower/select and instruction</summary>
    public enum GlobalISelAbortMode
    {
        /// <summary>Enabled abort mode</summary>
        Enable = LLVMGlobalISelAbortMode.LLVMGlobalISelAbortEnable,

        /// <summary>Disabled abort mode</summary>
        Disable= LLVMGlobalISelAbortMode.LLVMGlobalISelAbortDisable,

        /// <summary>Disabled abort mode with diagnostic</summary>
        DisableWithDiag = LLVMGlobalISelAbortMode.LLVMGlobalISelAbortDisableWithDiag,
    }

    /// <summary>LLVM Target Instruction Set Architecture</summary>
    public class Target
        : IEquatable<Target>
    {
        #region Equality
        /// <inheritdoc/>
        public bool Equals( Target? other ) => other is not null && Handle.Equals(other.Handle);

        /// <inheritdoc/>
        public override bool Equals( object? obj ) => obj is Target t && Equals(t);

        /// <inheritdoc/>
        public override int GetHashCode( )
        {
            return HashCode.Combine(
                Name,
                Description,
                HasJIT,
                HasTargetMachine,
                HasAsmBackEnd
            );
        }
        #endregion

        /// <summary>Gets the name of this target</summary>
        public LazyEncodedString Name => LLVMGetTargetName( Handle ) ?? LazyEncodedString.Empty;

        /// <summary>Gets the description of this target</summary>
        public LazyEncodedString Description => LLVMGetTargetDescription( Handle ) ?? LazyEncodedString.Empty;

        /// <summary>Gets a value indicating whether this target has JIT support</summary>
        public bool HasJIT => LLVMTargetHasJIT( Handle );

        /// <summary>Gets a value indicating whether this target has a TargetMachine initialized</summary>
        public bool HasTargetMachine => LLVMTargetHasTargetMachine( Handle );

        /// <summary>Gets a value indicating whether this target has an Assembly code generating back end initialized</summary>
        public bool HasAsmBackEnd => LLVMTargetHasAsmBackend( Handle );

        /// <summary>Creates a <see cref="TargetMachine"/> for the target and specified parameters</summary>
        /// <param name="triple">Target triple for this machine (e.g. -mtriple)</param>
        /// <param name="cpu">CPU for this machine (e.g. -mcpu)</param>
        /// <param name="features">Features for this machine (e.g. -mattr...)</param>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns><see cref="TargetMachine"/> based on the specified parameters</returns>
        public TargetMachine CreateTargetMachine( Triple triple
                                                , LazyEncodedString? cpu = null
                                                , LazyEncodedString? features = null
                                                , CodeGenOpt optLevel = CodeGenOpt.Default
                                                , RelocationMode relocationMode = RelocationMode.Default
                                                , CodeModel codeModel = CodeModel.Default
                                                )
        {
            using LLVMTargetMachineRef targetMachineHandle = InternalCreateTargetMachine( triple, cpu, features, optLevel, relocationMode, codeModel );
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
        public TargetMachine CreateTargetMachine( LazyEncodedString triple
                                                , LazyEncodedString? cpu = null
                                                , LazyEncodedString? features = null
                                                , CodeGenOpt optLevel = CodeGenOpt.Default
                                                , RelocationMode relocationMode = RelocationMode.Default
                                                , CodeModel codeModel = CodeModel.Default
                                                )
        {
            using LLVMTargetMachineRef targetMachineHandle = InternalCreateTargetMachine( triple, cpu, features, optLevel, relocationMode, codeModel );
            return new TargetMachine( targetMachineHandle );
        }

        /// <summary>Gets an enumerable collection of the available targets built into this library</summary>
        public static IEnumerable<Target> RegisteredTargets
        {
            get
            {
                var current = LLVMGetFirstTarget( );
                while( current != default )
                {
                    yield return new( current );
                    current = LLVMGetNextTarget( current );
                }
            }
        }

        /// <summary>Gets the target for a given target "triple" value</summary>
        /// <param name="triple">Target <see cref="Triple"/> describing the target</param>
        /// <returns>Target for the given triple</returns>
        public static Target FromTriple( Triple triple ) => FromTriple( triple.ThrowIfNull().ToString( ).ThrowIfNull() );

        /// <summary>Gets the target for a given target "triple" value</summary>
        /// <param name="targetTriple">Target triple string describing the target</param>
        /// <returns>Target for the given triple</returns>
        public static Target FromTriple( LazyEncodedString targetTriple )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( targetTriple );

            return LLVMGetTargetFromTriple( targetTriple, out LLVMTargetRef targetHandle, out string errorMsg ).Failed
                ? throw new InternalCodeGeneratorException( errorMsg ?? "Failed to get target from triple and no message provided from LLVM!" )
                : new( targetHandle );
        }

        internal Target( LLVMTargetRef targetHandle )
        {
            targetHandle.ThrowIfInvalid();
            Handle = targetHandle;
        }

        internal LLVMTargetRef Handle { get; }

        internal LLVMTargetMachineRef InternalCreateTargetMachine(
            LazyEncodedString triple,
            LazyEncodedString? cpu,
            LazyEncodedString? features,
            CodeGenOpt optLevel,
            RelocationMode relocationMode,
            CodeModel codeModel
            )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace( triple );
            optLevel.ThrowIfNotDefined();
            relocationMode.ThrowIfNotDefined();
            codeModel.ThrowIfNotDefined();

            var targetMachineHandle = LLVMCreateTargetMachine( Handle
                                                             , triple
                                                             , cpu ?? LazyEncodedString.Empty
                                                             , features ?? LazyEncodedString.Empty
                                                             , ( LLVMCodeGenOptLevel )optLevel
                                                             , ( LLVMRelocMode )relocationMode
                                                             , ( LLVMCodeModel )codeModel
                                                             );
            return targetMachineHandle;
        }
    }
}
