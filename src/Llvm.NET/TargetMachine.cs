// <copyright file="TargetMachine.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
// </copyright>

using System;
using Llvm.NET.Native;
using Llvm.NET.Native.Handles;
using Ubiquity.ArgValidators;

using static Llvm.NET.Native.NativeMethods;

namespace Llvm.NET
{
    /// <summary>Target specific code generation information</summary>
    public sealed class TargetMachine
        : DisposableObject
    {
        public override bool IsDisposed => ( TargetMachineHandle is null ) || TargetMachineHandle.IsClosed || TargetMachineHandle.IsInvalid;

        /// <summary>Gets the target that owns this <see cref="TargetMachine"/></summary>
        public Target Target => Target.FromHandle( LLVMGetTargetMachineTarget( TargetMachineHandle ) );

        /// <summary>Gets the target triple describing this machine</summary>
        public string Triple => LLVMGetTargetMachineTriple( TargetMachineHandle );

        /// <summary>Gets the CPU Type for this machine</summary>
        public string Cpu => LLVMGetTargetMachineCPU( TargetMachineHandle );

        /// <summary>Gets the CPU specific features for this machine</summary>
        public string Features => LLVMGetTargetMachineFeatureString( TargetMachineHandle );

        /// <summary>Gets Layout information for this machine</summary>
        public DataLayout TargetData
        {
            get
            {
                var handle = LLVMCreateTargetDataLayout( TargetMachineHandle );
                if( handle == default )
                {
                    return null;
                }

                return DataLayout.FromHandle( handle, isDisposable: false );
            }
        }

        /// <summary>Generate code for the target machine from a module</summary>
        /// <param name="module"><see cref="BitcodeModule"/> to generate the code from</param>
        /// <param name="path">Path to the output file</param>
        /// <param name="fileType">Type of file to emit</param>
        public void EmitToFile( BitcodeModule module, string path, CodeGenFileType fileType )
        {
            module.ValidateNotNull( nameof( module ) );
            path.ValidateNotNullOrWhiteSpace( nameof( path ) );
            fileType.ValidateDefined( nameof( path ) );

            if( module.TargetTriple != null && Triple != module.TargetTriple )
            {
                throw new ArgumentException( "Triple specified for the module doesn't match target machine", nameof( module ) );
            }

            var status = LLVMTargetMachineEmitToFile( TargetMachineHandle
                                                    , module.ModuleHandle
                                                    , path
                                                    , ( LLVMCodeGenFileType )fileType
                                                    , out string errTxt
                                                    );
            if( status.Failed )
            {
                throw new InternalCodeGeneratorException( errTxt );
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
        public MemoryBuffer EmitToBuffer( BitcodeModule module, CodeGenFileType fileType )
        {
            module.ValidateNotNull( nameof( module ) );
            fileType.ValidateDefined( nameof( fileType ) );

            if( module.TargetTriple != null && Triple != module.TargetTriple )
            {
                throw new ArgumentException( "Triple specified for the module doesn't match target machine", nameof( module ) );
            }

            var status = LLVMTargetMachineEmitToMemoryBuffer( TargetMachineHandle
                                                            , module.ModuleHandle
                                                            , ( LLVMCodeGenFileType )fileType
                                                            , out string errTxt
                                                            , out LLVMMemoryBufferRef bufferHandle
                                                            );

            if( status.Failed )
            {
                throw new InternalCodeGeneratorException( errTxt );
            }

            return new MemoryBuffer( bufferHandle );
        }

        internal TargetMachine( LLVMTargetMachineRef targetMachineHandle )
        {
            targetMachineHandle.ValidateNotDefault( nameof( targetMachineHandle ) );

            TargetMachineHandle = targetMachineHandle;
        }

        internal LLVMTargetMachineRef TargetMachineHandle { get; private set; }

        protected override void InternalDispose( bool disposing )
        {
            if( disposing )
            {
                TargetMachineHandle.Dispose( );
            }
        }
    }
}
