namespace Llvm.NET.Instructions
{
    public class Terminator
        : Instruction
    {
        internal Terminator( LLVMValueRef valueRef )
            : base( ValidateConversion( valueRef, LLVMNative.IsATerminatorInst ) )
        {
        }
    }
}
