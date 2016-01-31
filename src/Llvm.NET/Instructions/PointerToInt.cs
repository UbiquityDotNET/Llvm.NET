namespace Llvm.NET.Instructions
{
    public class PointerToInt
        : Cast
    {
        internal PointerToInt( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
