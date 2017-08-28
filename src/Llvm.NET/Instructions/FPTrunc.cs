using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class FPTrunc : Cast
    {
        internal FPTrunc( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
