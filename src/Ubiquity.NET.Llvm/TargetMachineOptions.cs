using static Ubiquity.NET.Llvm.Interop.ABI.libllvm_c.TargetMachineBindings;
using static Ubiquity.NET.Llvm.Interop.ABI.llvm_c.TargetMachine;

namespace Ubiquity.NET.Llvm
{
    /// <summary>Options for creating a <see cref="TargetMachine"/></summary>
    public class TargetMachineOptions
    {
        /// <summary>Gets or sets the CPU name for the machine</summary>
        public LazyEncodedString Cpu
        {
            get => LibLLVMTargetMachineOptionsGetCPU(Handle) ?? LazyEncodedString.Empty;
            set => LLVMTargetMachineOptionsSetCPU(Handle, value.ThrowIfNull());
        }

        /// <summary>Gets or sets the CPU Features for the machine</summary>
        public LazyEncodedString Features
        {
            get => LibLLVMTargetMachineOptionsGetFeatures(Handle) ?? LazyEncodedString.Empty;
            set => LLVMTargetMachineOptionsSetFeatures(Handle, value.ThrowIfNull());
        }

        /// <summary>Gets or sets the Code generation options for the machine</summary>
        public CodeGenOpt CodeGenOpt
        {
            get => (CodeGenOpt)LibLLVMTargetMachineOptionsGetCodeGenOptLevel(Handle);
            set => LLVMTargetMachineOptionsSetCodeGenOptLevel(Handle, (LLVMCodeGenOptLevel)value);
        }

        /// <summary>Gets or sets the relocation mode for the machine</summary>
        public RelocationMode RelocationMode
        {
            get => (RelocationMode)LibLLVMTargetMachineOptionsGetRelocMode(Handle);
            set => LLVMTargetMachineOptionsSetRelocMode(Handle, (LLVMRelocMode)value);
        }

        /// <summary>Gets or sets the code model for the machine</summary>
        public CodeModel CodeModel
        {
            get => (CodeModel)LibLLVMTargetMachineOptionsGetCodeModel(Handle);
            set => LLVMTargetMachineOptionsSetCodeModel(Handle, (LLVMCodeModel)value);
        }

        /// <summary>Gets or sets the ABI for the machine</summary>
        public LazyEncodedString ABI
        {
            get => LibLLVMTargetMachineOptionsGetABI(Handle) ?? LazyEncodedString.Empty;
            set => LLVMTargetMachineOptionsSetABI(Handle, value.ThrowIfNull());
        }
        internal TargetMachineOptions(LLVMTargetMachineOptionsRef h)
        {
            Handle = h.Move();
        }
        internal LLVMTargetMachineOptionsRef Handle {get;}
    }
}
