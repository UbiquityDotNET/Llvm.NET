// Copyright (c) Ubiquity.NET Contributors Contributors. All rights reserved.
// Licensed under the Apache-2.0 WITH LLVM-exception license. See the LICENSE.md file in the project root for full license information.
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.Orc;

namespace Ubiquity.NET.Llvm.OrcJITv2
{
    /// <summary>Target machine builder for ORC JIT v2</summary>
    public sealed class TargetMachineBuilder
        : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose( ) => Handle.Dispose();

        /// <summary>Initializes a new instance of the <see cref="TargetMachineBuilder"/> class</summary>
        /// <param name="template"><see cref="TargetMachine"/> to use as a template</param>
        /// <remarks>
        /// Ownership of the <paramref name="template"/> is transferred to native code by this constructor.
        /// It is no longer usable (<see cref="TargetMachine.Dispose"/> is a NOP) after this call.
        /// </remarks>
        public TargetMachineBuilder( TargetMachine template )
        {
            ArgumentNullException.ThrowIfNull( template );
            Handle = LLVMOrcJITTargetMachineBuilderCreateFromTargetMachine( template.Handle );

            // transfer complete mark it as invalid now
            template.Handle.SetHandleAsInvalid();
        }

        /// <summary>Gets or sets the Triple for this builder as a string</summary>
        [SuppressMessage( "Style", "IDE0025:Use expression body for property", Justification = "Temporary, as setter is plausible, though seems dubious" )]
        public LazyEncodedString Triple
        {
            get => LLVMOrcJITTargetMachineBuilderGetTargetTriple( Handle );
            set => LLVMOrcJITTargetMachineBuilderSetTargetTriple( Handle, value );
        }

        /// <summary>Creates a <see cref="TargetMachineBuilder"/> for the current host system</summary>
        /// <param name="optLevel">Optimization level</param>
        /// <param name="relocationMode">Relocation mode for generated code</param>
        /// <param name="codeModel"><see cref="CodeModel"/> to use for generated code</param>
        /// <returns>Builder using this host as the template</returns>
        public static TargetMachineBuilder FromHost(
            CodeGenOpt optLevel = CodeGenOpt.Default,
            RelocationMode relocationMode = RelocationMode.Default,
            CodeModel codeModel = CodeModel.Default
        )
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            // Ownership transfered to return value.
            return new( TargetMachine.HostMachine( optLevel, relocationMode, codeModel ) );
#pragma warning restore CA2000 // Dispose objects before losing scope
            /*
            // While this API does exist, it does NOT provide any means to set the options; they are ALL defaults
            //using LLVMErrorRef errorRef = LLVMOrcJITTargetMachineBuilderDetectHost(out LLVMOrcJITTargetMachineBuilderRef handle);
            //errorRef.ThrowIfFailed();
            //using(handle)
            //{
            //    return new(handle);
            //}
            */
        }

        internal TargetMachineBuilder( LLVMOrcJITTargetMachineBuilderRef h )
        {
            Handle = h.Move();
        }

        internal LLVMOrcJITTargetMachineBuilderRef Handle { get; }
    }
}
