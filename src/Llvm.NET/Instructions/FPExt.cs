using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class FPExt : Cast
    {
        internal FPExt( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
