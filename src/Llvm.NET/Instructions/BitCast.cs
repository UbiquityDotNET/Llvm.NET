namespace Llvm.NET.Instructions
{
    public class BitCast
        : Cast
    {
        internal BitCast( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
