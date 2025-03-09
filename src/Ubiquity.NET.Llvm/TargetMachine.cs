// -----------------------------------------------------------------------
// <copyright file="TargetMachine.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.NET.Llvm
{
    /// <summary>Target specific code generation information</summary>
    public sealed class TargetMachine
    {
        /// <summary>Initializes a new instance of the <see cref="TargetMachine"/> class.</summary>
        /// <param name="triple">Triple for the target machine</param>
        /// <param name="cpu">CPU options for the machine</param>
        /// <param name="features">CPU features for the machine</param>
        /// <param name="optLevel">General optimization level for machine code generation</param>
        /// <param name="relocationMode">Relocation mode for machine code generation</param>
        /// <param name="codeModel">Code model for machine code generation</param>
        public TargetMachine( Triple triple
                            , string? cpu = null
                            , string? features = null
                            , CodeGenOpt optLevel = CodeGenOpt.Default
                            , RelocationMode relocationMode = RelocationMode.Default
                            , CodeModel codeModel = CodeModel.Default
                            )
            : this( Target.InternalCreateTargetMachine( Target.FromTriple( triple ), triple, cpu, features, optLevel, relocationMode, codeModel ) )
        {
        }

        /// <summary>Gets the target that owns this <see cref="TargetMachine"/></summary>
        public Target Target => Target.FromHandle( LLVMGetTargetMachineTarget( TargetMachineHandle ) );

        /// <summary>Gets the target triple describing this machine</summary>
        public string Triple => LLVMGetTargetMachineTriple( TargetMachineHandle ).ToString() ?? string.Empty;

        /// <summary>Gets the CPU Type for this machine</summary>
        public string Cpu => LLVMGetTargetMachineCPU( TargetMachineHandle ).ToString() ?? string.Empty;

        /// <summary>Gets the CPU specific features for this machine</summary>
        public string Features => LLVMGetTargetMachineFeatureString( TargetMachineHandle ).ToString() ?? string.Empty;

        /// <summary>Gets Layout information for this machine</summary>
        public DataLayout TargetData
        {
            get
            {
                var handle = LLVMCreateTargetDataLayout( TargetMachineHandle );
                return DataLayout.FromHandle( handle.ThrowIfInvalid( ) )!;
            }
        }

        /// <summary>Generate code for the target machine from a module</summary>
        /// <param name="module"><see cref="BitcodeModule"/> to generate the code from</param>
        /// <param name="path">Path to the output file</param>
        /// <param name="fileType">Type of file to emit</param>
        public void EmitToFile( BitcodeModule module, string path, CodeGenFileKind fileType )
        {
            ArgumentNullException.ThrowIfNull( module );
            ArgumentNullException.ThrowIfNull( module.ModuleHandle );
            ArgumentException.ThrowIfNullOrWhiteSpace( path );
            fileType.ThrowIfNotDefined();

            if( module.TargetTriple != null && Triple != module.TargetTriple )
            {
                throw new ArgumentException( Resources.Triple_specified_for_the_module_doesn_t_match_target_machine, nameof( module ) );
            }

            DisposeMessageString? errTxt = null;
            try
            {
                var status = LLVMTargetMachineEmitToFile( TargetMachineHandle
                                                        , module.ModuleHandle
                                                        , path
                                                        , ( LLVMCodeGenFileType )fileType
                                                        , out errTxt
                                                        );
                if( status.Failed )
                {
                    throw new InternalCodeGeneratorException( errTxt?.ToString() ?? string.Empty);
                }
            }
            finally
            {
                errTxt?.Dispose();
            }
        }

        /// <summary>Emits the module for the target machine to a <see cref="MemoryBuffer"/></summary>
        /// <param name="module">Module to emit to the buffer</param>
        /// <param name="fileType">Type of file to generate into the buffer</param>
        /// <returns><see cref="MemoryBuffer"/> containing the generated code</returns>
        /// <remarks>
        /// The <see cref="BitcodeModule.TargetTriple"/> must match the <see cref="Triple"/> for this
        /// target.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "bufferHandle ownership is 'Moved' to the returned MemoryBuffer")]
        public MemoryBuffer EmitToBuffer( BitcodeModule module, CodeGenFileKind fileType )
        {
            ArgumentNullException.ThrowIfNull( module );
            ArgumentNullException.ThrowIfNull( module.ModuleHandle );
            fileType.ThrowIfNotDefined();

            if( module.TargetTriple != null && Triple != module.TargetTriple )
            {
                throw new ArgumentException( Resources.Triple_specified_for_the_module_doesn_t_match_target_machine, nameof( module ) );
            }

            DisposeMessageString? errTxt = null;
            try
            {
                var status = LLVMTargetMachineEmitToMemoryBuffer( TargetMachineHandle
                                                                , module.ModuleHandle
                                                                , ( LLVMCodeGenFileType )fileType
                                                                , out errTxt
                                                                , out var bufferHandle
                                                                );

                return status.Failed
                     ? throw new InternalCodeGeneratorException( errTxt?.ToString() ?? string.Empty )
                     : new MemoryBuffer( bufferHandle );
            }
            finally
            {
                errTxt?.Dispose();
            }
        }

        /// <summary>Creates a <see cref="TargetMachine"/> for the triple and specified parameters</summary>
        /// <param name="triple">Target triple for this machine (e.g. -mtriple)</param>
        /// <param name="cpu">CPU for this machine (e.g. -mcpu)</param>
        /// <param name="features">Features for this machine (e.g. -mattr...)</param>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns><see cref="TargetMachine"/> based on the specified parameters</returns>
        public static TargetMachine FromTriple( Triple triple
                                              , string? cpu = null
                                              , string? features = null
                                              , CodeGenOpt optLevel = CodeGenOpt.Default
                                              , RelocationMode relocationMode = RelocationMode.Default
                                              , CodeModel codeModel = CodeModel.Default
                                              )
        {
            var target = Target.FromTriple( triple );
            return target.CreateTargetMachine( triple, cpu, features, optLevel, relocationMode, codeModel );
        }

        internal TargetMachine( LLVMTargetMachineRef targetMachineHandle )
        {
            ArgumentNullException.ThrowIfNull(targetMachineHandle);
            targetMachineHandle.ThrowIfInvalid();

            TargetMachineHandle = targetMachineHandle;
        }

        internal LLVMTargetMachineRef TargetMachineHandle { get; }
    }
}
