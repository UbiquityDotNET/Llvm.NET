namespace Ubiquity.NET.Llvm.Interop.ABI.libllvm_c
{

    public static partial class TargetRegistration
    {
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LibLLVMRegisterTarget(CodeGenTarget target, Ubiquity.NET.Llvm.Interop.TargetRegistration registrations);
        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial Int32 LibLLVMGetNumTargets();

        [LibraryImport( LibraryName )]
        [UnmanagedCallConv( CallConvs = [ typeof( CallConvCdecl ) ] )]
        public static unsafe partial LLVMErrorRef LibLLVMGetRuntimeTargets([In, Out] CodeGenTarget[] targets, Int32 lengthOfArray);

    }
}
