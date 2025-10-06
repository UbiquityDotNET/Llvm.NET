// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.

using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.TargetMachineBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.TargetMachine;

namespace Ubiquity.NET.Llvm
{
    // TODO: ITargetMachine and internal TargetMachineAlias
    // While nothing in the core OO model has one, the JIT is another story...

    /// <summary>Target specific code generation information</summary>
    public sealed class TargetMachine
        : IDisposable
    {
        /// <inheritdoc/>
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected", Justification = "Ownership transferred in constructor")]
        public void Dispose( )
        {
            if(!Handle.IsNull)
            {
                Handle.Dispose();
                InvalidateAfterMove();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="TargetMachine"/> class.</summary>
        /// <param name="triple">Triple for the target machine</param>
        /// <param name="cpu">CPU options for the machine</param>
        /// <param name="features">CPU features for the machine</param>
        /// <param name="optLevel">General optimization level for machine code generation</param>
        /// <param name="relocationMode">Relocation mode for machine code generation</param>
        /// <param name="codeModel">Code model for machine code generation</param>
        public TargetMachine( Triple triple
                            , LazyEncodedString? cpu = null
                            , LazyEncodedString? features = null
                            , CodeGenOpt optLevel = CodeGenOpt.Default
                            , RelocationMode relocationMode = RelocationMode.Default
                            , CodeModel codeModel = CodeModel.Default
                            )
            : this( Target.FromTriple( triple ).InternalCreateTargetMachine( triple, cpu, features, optLevel, relocationMode, codeModel ) )
        {
        }

        /// <summary>Gets the target that owns this <see cref="TargetMachine"/></summary>
        public Target Target => new( LLVMGetTargetMachineTarget( Handle ) );

        /// <summary>Gets the target triple describing this machine</summary>
        public LazyEncodedString Triple => LLVMGetTargetMachineTriple( Handle );

        /// <summary>Gets the CPU Type for this machine</summary>
        public LazyEncodedString Cpu => LLVMGetTargetMachineCPU( Handle );

        /// <summary>Gets the CPU specific features for this machine</summary>
        public LazyEncodedString Features => LLVMGetTargetMachineFeatureString( Handle );

        /// <summary>Creates Data Layout information for this machine</summary>
        /// <returns>Created data layout</returns>
        public DataLayout CreateTargetData( )
        {
            return new( LLVMCreateTargetDataLayout( Handle ) );
        }

        /// <summary>Gets or Sets a value indicating whether this machine uses verbose assembly</summary>
        public bool AsmVerbosity
        {
            get => LibLLVMGetTargetMachineAsmVerbosity( Handle );
            set => LLVMSetTargetMachineAsmVerbosity( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether this machine enables the fast-path instruction selection</summary>
        public bool FastISel
        {
            get => LibLLVMGetTargetMachineFastISel( Handle );
            set => LLVMSetTargetMachineFastISel( Handle, value );
        }

        /// <summary>Gets or Sets a value indicating whether this machine enables global instruction selection</summary>
        public bool GlobalISel
        {
            get => LibLLVMGetTargetMachineGlobalISel( Handle );
            set => LLVMSetTargetMachineGlobalISel( Handle, value );
        }

        /// <summary>Gets or Sets the abort mode for Global ISel</summary>
        public GlobalISelAbortMode GlobalISelAbortMode
        {
            get => (GlobalISelAbortMode)LibLLVMGetTargetMachineGlobalISelAbort( Handle );
            set => LLVMSetTargetMachineGlobalISelAbort( Handle, (LLVMGlobalISelAbortMode)value );
        }

        /// <summary>Gets or Sets a value indicating whether this machine uses the MachineOutliner pass</summary>
        public bool MachineOutliner
        {
            get => LibLLVMGetTargetMachineMachineOutliner( Handle );
            set => LLVMSetTargetMachineMachineOutliner( Handle, value );
        }

        /// <summary>Generate code for the target machine from a module</summary>
        /// <param name="module"><see cref="Module"/> to generate the code from</param>
        /// <param name="path">Path to the output file</param>
        /// <param name="fileType">Type of file to emit</param>
        /// <param name="skipVerify">Skips verification [Default: false; verification of module performed]</param>
        public void EmitToFile( IModule module, string path, CodeGenFileKind fileType, bool skipVerify = false )
        {
            ArgumentNullException.ThrowIfNull( module );
            ArgumentException.ThrowIfNullOrWhiteSpace( path );
            fileType.ThrowIfNotDefined();

            if(!skipVerify && !module.Verify( out string errMessage ))
            {
                throw new InvalidOperationException( errMessage.NormalizeLineEndings( LineEndingKind.LineFeed, StringNormalizer.SystemLineEndings ) );
            }

            if(module.TargetTriple != null && Triple != module.TargetTriple)
            {
                throw new ArgumentException( Resources.Triple_specified_for_the_module_doesn_t_match_target_machine, nameof( module ) );
            }

            var status = LLVMTargetMachineEmitToFile( Handle
                                                    , module.GetUnownedHandle()
                                                    , path
                                                    , ( LLVMCodeGenFileType )fileType
                                                    , out string errTxt
                                                    );
            if(status.Failed)
            {
                throw new InternalCodeGeneratorException( errTxt ?? "Error emitting to file, but LLVM provided no error message!" );
            }
        }

        /// <summary>Emits the module for the target machine to a <see cref="MemoryBuffer"/></summary>
        /// <param name="module">ModuleHandle to emit to the buffer</param>
        /// <param name="fileType">Type of file to generate into the buffer</param>
        /// <returns><see cref="MemoryBuffer"/> containing the generated code</returns>
        /// <remarks>
        /// The <see cref="Module.TargetTriple"/> must match the <see cref="Triple"/> for this
        /// target.
        /// </remarks>
        [SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "bufferHandle ownership is 'Moved' to the returned MemoryBuffer" )]
        public MemoryBuffer EmitToBuffer( IModule module, CodeGenFileKind fileType )
        {
            ArgumentNullException.ThrowIfNull( module );
            fileType.ThrowIfNotDefined();

            if(module.TargetTriple != null && Triple != module.TargetTriple)
            {
                throw new ArgumentException( Resources.Triple_specified_for_the_module_doesn_t_match_target_machine, nameof( module ) );
            }

            var status = LLVMTargetMachineEmitToMemoryBuffer( Handle
                                                            , module.GetUnownedHandle()
                                                            , ( LLVMCodeGenFileType )fileType
                                                            , out string errTxt
                                                            , out var bufferHandle
                                                            );

            return status.Failed
                    ? throw new InternalCodeGeneratorException( errTxt ?? "Error emitting to buffer, but LLVM provided no error message!" )
                    : new MemoryBuffer( bufferHandle );
        }

        /// <summary>Gets a target machine for the current host</summary>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns>Host <see cref="TargetMachine"/></returns>
        /// <remarks>
        /// This is normally only used with JIT support to get a <see cref="TargetMachine"/>
        /// for the current host.
        /// </remarks>
        public static TargetMachine HostMachine(
            CodeGenOpt optLevel = CodeGenOpt.Default,
            RelocationMode relocationMode = RelocationMode.Default,
            CodeModel codeModel = CodeModel.Default
            )
        {
            return FromTriple(
                Llvm.Triple.GetHostTriple(),
                HostCPUName,
                HostCPUFeatures,
                optLevel,
                relocationMode,
                codeModel
            );
        }

        /// <summary>Gets the CPU name for the current host</summary>
        public static LazyEncodedString HostCPUName => LLVMGetHostCPUName();

        /// <summary>Gets the CPU features for the current host</summary>
        public static LazyEncodedString HostCPUFeatures => LLVMGetHostCPUFeatures();

        /// <summary>Creates a <see cref="TargetMachine"/> for the triple and specified parameters</summary>
        /// <param name="triple">Target triple for this machine (e.g. -mtriple)</param>
        /// <param name="cpu">CPU for this machine (e.g. -mcpu)</param>
        /// <param name="features">Features for this machine (e.g. -mattr...)</param>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns><see cref="TargetMachine"/> based on the specified parameters</returns>
        public static TargetMachine FromTriple( Triple triple
                                              , LazyEncodedString? cpu = null
                                              , LazyEncodedString? features = null
                                              , CodeGenOpt optLevel = CodeGenOpt.Default
                                              , RelocationMode relocationMode = RelocationMode.Default
                                              , CodeModel codeModel = CodeModel.Default
                                              )
        {
            var target = Target.FromTriple( triple );
            return target.CreateTargetMachine( triple, cpu, features, optLevel, relocationMode, codeModel );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InvalidateAfterMove( )
        {
            Handle = default;
        }

        internal TargetMachine( LLVMTargetMachineRef targetMachineHandle )
        {
            targetMachineHandle.ThrowIfInvalid();

            Handle = targetMachineHandle;
        }

        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP008:Don't assign member with injected and created disposables", Justification = "Move Semantics in constructor")]
        internal LLVMTargetMachineRef Handle { get; private set; }
    }
}
