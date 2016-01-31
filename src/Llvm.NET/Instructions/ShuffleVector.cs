namespace Llvm.NET.Instructions
{
    public class ShuffleVector
        : Instruction
    {
        internal ShuffleVector( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
