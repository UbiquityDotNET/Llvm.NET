using Llvm.NET.Native;

namespace Llvm.NET.Instructions
{
    public class FPToUI : Cast
    {
        internal FPToUI( LLVMValueRef valueRef )
            : base( valueRef )
        {
        }
    }
}
