namespace Llvm.NET.Instructions
{
    public class ExtractValue
        : UnaryInstruction
    {
        internal ExtractValue( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
