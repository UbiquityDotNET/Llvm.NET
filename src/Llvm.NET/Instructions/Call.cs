namespace Llvm.NET.Instructions
{
    public class CallInstruction
        : Instruction
    {
        internal CallInstruction( LLVMValueRef valueRef )
            : this( valueRef, false )
        {
        }

        internal CallInstruction( LLVMValueRef valueRef, bool preValidated )
            : base( preValidated ? valueRef : ValidateConversion( valueRef, NativeMethods.IsACallInst ) )
        {
        }
    }
}
